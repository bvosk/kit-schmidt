using Autofac;
using KitSchmidt.App_Start;
using KitSchmidt.Common.DAL;
using KitSchmidt.Common.DAL.Models;
using KitSchmidt.DAL;
using KitSchmidt.DAL.Models;
using KitSchmidt.Forms;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace KitSchmidt.Dialogs
{
    [Serializable]
    public class GreetUserDialog : IDialog<User>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var userDataService = new UserDataService(new KitContext());
            var user = userDataService.GetUser(context.Activity.From.Id);
            User newUser = null;

            if (user == null)
            {
                // Register the new user
                newUser = new User
                {
                    UserId = context.Activity.From.Id,
                    Name = context.Activity.From.Name,
                    ChannelId = context.Activity.ChannelId
                };
                await context.PostAsync($"Registering you as {newUser.Name}...");
                await userDataService.AddUser(newUser);
            }
            else
            {
                await context.PostAsync($"Welcome back {user.Name}!");
            }

            context.Done(user ?? newUser);
        }

        public static async Task AfterGreetUser(IDialogContext context, IAwaitable<User> result)
        {
            var user = await result;
            context.UserData.SetValue("user", user);

            context.Call(new MainMenuDialog(), MainMenuDialog.ResumeAfter);
        }
    }
}
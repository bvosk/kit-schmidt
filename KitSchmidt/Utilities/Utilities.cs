using KitSchmidt.Common;
using KitSchmidt.Common.DAL.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KitSchmidt.Utilitites
{
    public class Utilities
    {
        public static HeroCard EventHeroCard(Event @event)
        {
            return new HeroCard()
            {
                Title = $"{@event.Name}",
                Subtitle = $"{@event.Date.ToString("g")}",
                Images = new List<CardImage>
                {
                    new CardImage()
                    {
                        Url = Constants.EventImageUrl
                    }
                }
            };
        }
    }
}
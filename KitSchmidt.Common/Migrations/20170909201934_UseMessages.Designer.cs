using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using KitSchmidt.DAL;

namespace KitSchmidt.Common.Migrations
{
    [DbContext(typeof(KitContext))]
    [Migration("20170909201934_UseMessages")]
    partial class UseMessages
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KitSchmidt.Common.DAL.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CoordinatorId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CoordinatorId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("KitSchmidt.Common.DAL.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChannelId");

                    b.Property<string>("Name");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KitSchmidt.DAL.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("From");

                    b.Property<DateTime>("InputDate");

                    b.Property<string>("Text");

                    b.Property<string>("To");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("KitSchmidt.Common.DAL.Models.Event", b =>
                {
                    b.HasOne("KitSchmidt.Common.DAL.Models.User", "Coordinator")
                        .WithMany("Events")
                        .HasForeignKey("CoordinatorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}

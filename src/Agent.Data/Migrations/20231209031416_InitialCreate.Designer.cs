﻿// <auto-generated />
using Agent.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Agent.Data.Migrations
{
    [DbContext(typeof(AgentDataContext))]
    [Migration("20231209031416_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("Agent.Data.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Agent.Data.Models.WorldInstance", b =>
                {
                    b.Property<string>("WorldInstanceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("WorldId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("WorldInstanceId");

                    b.HasIndex("WorldId");

                    b.ToTable("WorldInstances");
                });
#pragma warning restore 612, 618
        }
    }
}

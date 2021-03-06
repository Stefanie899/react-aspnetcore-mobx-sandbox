﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sandbox.Infrastructure.Data.SqlServer;

namespace Sandbox.Infrastructure.Data.SqlServer.Migrations
{
    [DbContext(typeof(SandboxContext))]
    [Migration("20200124160802_CreateDatabase")]
    partial class CreateDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sandbox.Business.Core.Models.Topics.Topic", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body");

                    b.Property<long?>("CreatedById");

                    b.Property<DateTimeOffset?>("CreatedOn");

                    b.Property<long?>("DeletedById");

                    b.Property<DateTimeOffset?>("DeletedOn");

                    b.Property<int>("Downdoots");

                    b.Property<string>("Title");

                    b.Property<long?>("UpdatedById");

                    b.Property<DateTimeOffset?>("UpdatedOn");

                    b.Property<int>("Updoots");

                    b.HasKey("Id");

                    b.ToTable("Topics");
                });
#pragma warning restore 612, 618
        }
    }
}

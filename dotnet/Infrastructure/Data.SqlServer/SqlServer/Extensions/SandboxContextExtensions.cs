using Microsoft.AspNetCore.Hosting;
using Sandbox.Business.Core.Models.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sandbox.Infrastructure.Data.SqlServer.Extensions
{
    public static class SandboxContextExtensions
    {
        public static void EnsureSeedData(this SandboxContext context, IHostingEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                return;
            }

            context.SeedTopics();
        }

        private static void SeedTopics(this SandboxContext context)
        {
            var topics = new List<Topic>()
            {
                new Topic() { Body = "This is the body of a topic to test the thing. Plz tho.",        Title = "Test the thing",   Downdoots = 0, Updoots = 0 },
                new Topic() { Body = "This is the body of a second topic to test the thing. Plz tho.", Title = "Test the thing 2", Downdoots = 0, Updoots = 0 },
                new Topic() { Body = "This is the body of a third topic to test the thing. Plz tho.",  Title = "Test the thing 3", Downdoots = 0, Updoots = 0 },
            };

            var existing = context.Topics.ToList();

            var newTopics = new List<Topic>();
            newTopics.AddRange(topics.Where(t => existing.All(e => e.Body != t.Body)));

            context.Topics.AddRange(newTopics);

            context.SaveChanges();
        }
    }
}

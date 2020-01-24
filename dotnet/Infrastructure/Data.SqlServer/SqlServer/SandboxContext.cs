using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sandbox.Business.Core.Interfaces;
using Sandbox.Business.Core.Models.Topics;
using Sandbox.Business.Core.Models.Users;

namespace Sandbox.Infrastructure.Data.SqlServer
{
    public class SandboxContext : Context, ISandboxContext
    {
        public DbSet<Topic>     Topics     { get; set; }
        public DbSet<TopicDoot> TopicDoots { get; set; }
        public DbSet<User>      Users      { get; set; }

        IQueryable<Topic>     ISandboxContext.Topics     => Topics;
        IQueryable<TopicDoot> ISandboxContext.TopicDoots => TopicDoots;
        IQueryable<User>      ISandboxContext.Users      => Users;

        public SandboxContext()
            : base("Data Source=.\\; Database=ArchitectureSandbox; user id=sandbox_user; password=Passw0rd!;")
        {
        }
    }
}

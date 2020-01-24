using Sandbox.Business.Core.Models.Topics;
using Sandbox.Business.Core.Models.Users;
using System.Linq;

namespace Sandbox.Business.Core.Interfaces
{
    public interface ISandboxContext
    {
        IQueryable<Topic>     Topics     { get; }
        IQueryable<TopicDoot> TopicDoots { get; }
        IQueryable<User>      Users      { get; }
    }
}

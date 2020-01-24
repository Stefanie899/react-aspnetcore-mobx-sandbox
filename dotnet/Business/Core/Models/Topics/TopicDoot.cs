using AndcultureCode.CSharp.Core.Models;
using Sandbox.Business.Core.Enums;
using Sandbox.Business.Core.Models.Users;

namespace Sandbox.Business.Core.Models.Topics
{
    public class TopicDoot : Auditable
    {
        public DootType DootType { get; set; }
        public long     TopicId  { get; set; }
        public long     UserId   { get; set; }

        public virtual Topic Topic { get; set; }
        public virtual User  User  { get; set; }
    }
}

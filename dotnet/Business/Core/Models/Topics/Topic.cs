using AndcultureCode.CSharp.Core.Models;

namespace Sandbox.Business.Core.Models.Topics
{
    public class Topic : Auditable
    {
        public string Title     { get; set; }
        public string Body      { get; set; }
        public int    Updoots   { get; set; }
        public int    Downdoots { get; set; }
    }
}

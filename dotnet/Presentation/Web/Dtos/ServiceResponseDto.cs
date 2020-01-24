using AndcultureCode.CSharp.Core.Interfaces;
using System.Collections.Generic;

namespace Web.Dtos
{
    public class ServiceResponseDto<T> : IResult<T>
    {
        public Dictionary<string, string> NextLinkParams { get; set; }
        public List<IError>               Errors         { get; set; }
        public T                          ResultObject   { get; set; }

        public int ErrorCount => Errors?.Count ?? 0;

        public bool HasErrors => (Errors?.Count ?? 0) > 0;
    }
}

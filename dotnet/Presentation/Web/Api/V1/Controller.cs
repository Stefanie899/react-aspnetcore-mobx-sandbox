using AndcultureCode.CSharp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Web.Dtos;

namespace Web.Api.V1
{
    public class Controller : ControllerBase
    {
        /// <summary>
        /// Create a result object given the value and errors list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public ServiceResponseDto<T> CreateResult<T>(T value, IEnumerable<IError> errors)
        {
            var result = new ServiceResponseDto<T>()
            {
                Errors       = errors?.ToList(),
                ResultObject = value
            };
            return result;
        }

        public BadRequestObjectResult BadRequest<T>(T value, IEnumerable<IError> errors = null)
        {
            return base.BadRequest(CreateResult(value, errors));
        }

        public BadRequestObjectResult BadRequest<T>(T value, IError error)
        {
            return base.BadRequest(CreateResult(value, new List<IError> { error }));
        }

        public OkObjectResult Ok<T>(T value, IEnumerable<IError> errors = null)
        {
            return base.Ok(CreateResult(value, errors));
        }

        public OkObjectResult Ok<T>(T value, IError error)
        {
            return base.Ok(CreateResult(value, new List<IError> { error }));
        }
    }
}

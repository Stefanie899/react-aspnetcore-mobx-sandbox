using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Presentation.Web.Dtos.Topics;
using AndcultureCode.CSharp.Core.Interfaces.Conductors;
using Sandbox.Business.Core.Models.Topics;

namespace Sandbox.Presentation.Web.Api.V1.Topics
{
    [ApiController]
    [Route("api/v1/topics")]
    public class TopicsController : Controller
    {
        private IRepositoryConductor<Topic> _topicConductor;

        public TopicsController(IRepositoryConductor<Topic> topicConductor)
        {
            _topicConductor = topicConductor;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = _topicConductor.FindAll();

            var topicDtos = new List<TopicDto>();

            foreach (var topic in result.ResultObject.ToList())
            {
                topicDtos.Add(new TopicDto()
                {
                    Id        = topic.Id,
                    Title     = topic.Title,
                    Body      = topic.Body,
                    Updoots   = topic.Updoots,
                    Downdoots = topic.Downdoots
                });
            }

            return Ok(topicDtos);
        }

        [HttpGet("{id:long}")]
        public IActionResult Get(long id)
        {
            var result = _topicConductor.FindById(id);

            if (result.ResultObject == null)
            {
                return NotFound();
            }

            var topic = result.ResultObject;

            return Ok(new TopicDto()
            {
                Id        = topic.Id,
                Title     = topic.Title,
                Body      = topic.Body,
                Updoots   = topic.Updoots,
                Downdoots = topic.Downdoots
            });
        }
    }
}

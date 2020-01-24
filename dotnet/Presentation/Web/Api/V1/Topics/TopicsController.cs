using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Web.Dtos.Topics;

namespace Web.Api.V1.Topics
{
    [ApiController]
    [Route("api/v1/topics")]
    public class TopicsController : Controller
    {
        private List<TopicDto> topics = new List<TopicDto>()
        {
            new TopicDto() { Id = 1, Body = "This is the body of a topic to test the thing. Plz tho.",        Title = "Test the thing",   Downdoots = 0,     Updoots = 0 },
            new TopicDto() { Id = 2, Body = "This is the body of a second topic to test the thing. Plz tho.", Title = "Test the thing 2", Downdoots = 0,     Updoots = 5 },
            new TopicDto() { Id = 3, Body = "This is the body of a third topic to test the thing. Plz tho.",  Title = "Test the thing 3", Downdoots = 15000, Updoots = 0 },
        };

        [Authorize]
        [HttpPost]
        public IActionResult Post()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(topics);
        }

        [HttpGet("{id:long}")]
        public IActionResult Get(long id)
        {
            var topic = topics.FirstOrDefault(e => e.Id == id);

            if (topic == null)
            {
                return NotFound();
            }

            return Ok(topic);
        }
    }
}

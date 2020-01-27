using AndcultureCode.CSharp.Core.Interfaces.Conductors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sandbox.Business.Core.Enums;
using Sandbox.Business.Core.Models.Topics;
using Sandbox.Presentation.Web.Dtos.Topics;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox.Presentation.Web.Api.V1.Topics.Doots
{
    [Authorize]
    [ApiController]
    [Route("api/v1/topics/doot")]
    public class TopicDootsController : Controller
    {
        private IRepositoryConductor<Topic>     _topicsConductor;
        private IRepositoryConductor<TopicDoot> _topicDootsConductor;

        public TopicDootsController(
            IRepositoryConductor<Topic>     topicsConductor,
            IRepositoryConductor<TopicDoot> topicDootsConductor
        )
        {
            _topicsConductor     = topicsConductor;
            _topicDootsConductor = topicDootsConductor;
        }

        [HttpGet]
        public IActionResult Index(long userId)
        {
            var result = _topicDootsConductor.FindAll(e => e.UserId == userId);

            var topicDootDtos = new List<TopicDootDto>();

            foreach (var topicDoot in result.ResultObject.ToList())
            {
                topicDootDtos.Add(new TopicDootDto()
                {
                    Id       = topicDoot.Id,
                    TopicId  = topicDoot.TopicId,
                    UserId   = topicDoot.UserId,
                    DootType = topicDoot.DootType
                });
            }

            return Ok(topicDootDtos);
        }

        [HttpGet("{id:long}")]
        public IActionResult Get(long id)
        {
            var result = _topicDootsConductor.FindById(id);

            if (result.ResultObject == null)
            {
                return NotFound();
            }

            var topicDoot = result.ResultObject;

            return Ok(new TopicDootDto()
            {
                Id       = topicDoot.Id,
                TopicId  = topicDoot.TopicId,
                UserId   = topicDoot.UserId,
                DootType = topicDoot.DootType
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] TopicDootDto dto)
        {
            var topicResult = _topicsConductor.FindById(dto.TopicId);

            if (topicResult.ResultObject == null)
            {
                return NotFound();
            }

            var topic = topicResult.ResultObject;

            var existingDoot = _topicDootsConductor.FindAll(e => e.TopicId == topic.Id && e.UserId == dto.UserId);

            if (existingDoot.ResultObject != null && existingDoot.ResultObject.FirstOrDefault() != null)
            {
                return BadRequest();
            }

            var topicDoot = new TopicDoot
            {
                TopicId  = topic.Id,
                UserId   = dto.UserId,
                DootType = dto.DootType,
            };

            var createResult = _topicDootsConductor.Create(topicDoot);

            var updootCount   = _topicDootsConductor.FindAll(e => e.TopicId == topic.Id && e.DootType == DootType.UpDoot);
            var downdootCount = _topicDootsConductor.FindAll(e => e.TopicId == topic.Id && e.DootType == DootType.DownDoot);

            topic.Updoots   = updootCount.ResultObject.Count();
            topic.Downdoots = downdootCount.ResultObject.Count();

            var updateResult = _topicsConductor.Update(topic);

            return Ok(new TopicDootDto()
            {
                Id       = topicDoot.Id,
                TopicId  = topicDoot.TopicId,
                UserId   = topicDoot.UserId,
                DootType = topicDoot.DootType
            });
        }

        [HttpPut]
        public IActionResult Put([FromBody] TopicDootDto dto)
        {
            var topicResult = _topicsConductor.FindById(dto.TopicId);

            if (topicResult.ResultObject == null)
            {
                return NotFound();
            }

            var topic = topicResult.ResultObject;

            var existingDootResult = _topicDootsConductor.FindAll(e => e.Id == dto.Id);

            if (existingDootResult.ResultObject == null)
            {
                return BadRequest();
            }

            var existingDoot = existingDootResult.ResultObject.FirstOrDefault();

            if (existingDoot == null)
            {
                return NotFound();
            }

            existingDoot.DootType = dto.DootType;

            var createResult = _topicDootsConductor.Update(existingDoot);

            var updootCount   = _topicDootsConductor.FindAll(e => e.TopicId == topic.Id && e.DootType == DootType.UpDoot);
            var downdootCount = _topicDootsConductor.FindAll(e => e.TopicId == topic.Id && e.DootType == DootType.DownDoot);

            topic.Updoots   = updootCount.ResultObject.Count();
            topic.Downdoots = downdootCount.ResultObject.Count();

            var updateResult = _topicsConductor.Update(topic);

            return Ok(new TopicDootDto()
            {
                Id       = existingDoot.Id,
                TopicId  = existingDoot.TopicId,
                UserId   = existingDoot.UserId,
                DootType = existingDoot.DootType
            });
        }
    }
}

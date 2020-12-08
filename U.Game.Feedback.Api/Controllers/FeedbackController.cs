using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using U.Game.Feedback.Api.Models;
using U.Game.Feedback.Domain.Contracts;
using U.Game.Feedback.Domain.Entities;

namespace U.Game.Feedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedbackController : Controller
    {
        readonly IMapper mapper;
        readonly IRepositoryBase<UserFeedback> userFeedbackRepository;
        readonly IRepositoryBase<User> userRepository;

        public FeedbackController(IMapper mapper, IRepositoryBase<UserFeedback> userFeedbackRepository, IRepositoryBase<User> userRepository)
        {
            this.mapper = mapper;
            this.userFeedbackRepository = userFeedbackRepository;
            this.userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromQuery] string sessionId,
            [FromHeader(Name = "Ubi-UserId")] Guid userId,
            [FromBody] FeedbackModel model)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                return BadRequest("The parameter Session Id needs to be supply it in the QueryString.");

            if (userId == Guid.Empty)
                return BadRequest("The parameter User Id needs to be supply it in the Headers.");

            var user = this.userRepository.Get(userId);
            if (user == null)
                return NotFound($"A user with the ID: {userId} doesn't exists in our database.");

            var actionResultMessage = await this.userFeedbackRepository.AddAsync(
                new UserFeedback(Guid.Empty, user, sessionId, model.Rating, model.Comments)
            );

            return Ok(actionResultMessage);
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] int? rating, [FromQuery] int? totalRecords = 15)
        {
            var feedbacks = await this.userFeedbackRepository.GetFilteredListAsync(uf => (rating != null ? uf.Rating == rating.Value: uf.Rating >= 1), totalRecords);
            var mappingResult = this.mapper.Map<IEnumerable<FeedbackModel>>(feedbacks);
            return Ok(mappingResult);
        }
    }
}

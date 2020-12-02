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
    public class UserController : Controller
    {
        readonly IMapper mapper;
        readonly IRepositoryBase<User> userRepository;

        public UserController(IMapper mapper, IRepositoryBase<User> userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserModel model)
        {            
            var actionResultMessage = await this.userRepository.AddAsync(
                new User(Guid.Empty, model.NickName, model.Name, model.Email)
            );

            return Ok(actionResultMessage);
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            var users = await this.userRepository.GetListAsync();
            return Ok(this.mapper.Map<IEnumerable<UserModel>>(users));
        }
    }
}

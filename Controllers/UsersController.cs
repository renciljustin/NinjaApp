using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NinjaApp.Core;
using NinjaApp.Persistence.Dtos;
using NinjaApp.Shared.Enums;

namespace NinjaApp.Controllers {

    [ApiController]
    [Route (RoutePrefix.API)]
    public class UsersController : ControllerBase {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController (IUserRepository userRepository, IMapper mapper) {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers() {
            var usersFromDb = await _userRepository.GetUsersAsync ();

            var usersToReturn = _mapper.Map<List<UserListDto>>(usersFromDb);

            return Ok(usersToReturn);
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            var userFromDb = await _userRepository.GetUserByUserNameAsync(userName);

            if ( userFromDb == null )
                return BadRequest($"Cannot found user with username '{userName}'.");

            var userToReturn = _mapper.Map<UserDetailDto>(userFromDb);

            return Ok(userToReturn);
        }
    }
}
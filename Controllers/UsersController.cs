using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NinjaApp.Core;
using NinjaApp.Core.Models;
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

        [HttpPut("{userName}")]
        public async Task<IActionResult> UpdateUser(string userName, UserDetailDto user)
        {
            if ( user == null || user.UserName == null )
                return BadRequest("Invalid account details.");

            if ( userName != user.UserName )
                return Unauthorized("You are not authorized to edit someone's account.");

            var userFromDb = await _userRepository.GetUserByUserNameAsync( user.UserName );

            if ( userFromDb == null )
                return BadRequest("Account not exists.");

            var userFromRequest = _mapper.Map<User>(user);

            var updatedUser = await _userRepository.UpdateUserAsync( userFromDb, userFromRequest );

            return Ok( _mapper.Map<UserDetailDto>(updatedUser) );
        }
    }
}
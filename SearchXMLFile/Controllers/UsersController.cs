using Microsoft.AspNetCore.Mvc;
using SearchXMLFile.DTOs;
using SearchXMLFile.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SearchXMLFile.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDTO userDto)
        {
            if (userDto == null)
                return BadRequest("User data is null");

            var userFullName = $"{userDto.Surname}{userDto.Name}{userDto.FatherName}";

            if (!_userService.IsUserNameUnique(userFullName))
                return StatusCode(400, "User Name must be unique");

            if (string.IsNullOrWhiteSpace(userDto.Name) ||
                string.IsNullOrWhiteSpace(userDto.Surname) ||
                string.IsNullOrWhiteSpace(userDto.FatherName))
            {
                return BadRequest("Name, Surname, and FatherName cannot be null or empty");
            }
            if (!_userService.IsValidBirthday(userDto.Birthday))
            {
                return BadRequest("Birthday is not valid");
            }
            await _userService.AddUserAsync(userDto);
            return Ok();
        }
    }
}
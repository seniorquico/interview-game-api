using GameAPI.Services;
using GameAPI.Services.DTOs;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GameAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult PostUser()
        {
            var user = _service.CreateUser();
            var uri = $"{Request.GetDisplayUrl()}/{user.UserId}";
            return Created(uri, user);
        } 

        [HttpGet("{id}")]
        public async Task<UserDTO> GetUser(int id)
        {
            return await _service.GetUser(id);
        }

        [HttpPost("{userId}/games")]
        public async Task<IActionResult> PostUserGame(int userId, 
            [FromBody] PostGameRequest gameRequest)
        {
            await _service.AddGame(userId, gameRequest.gameId.Value);

            return NoContent();
        }

        [HttpDelete("{userId}/games/{gameId}")]
        public IActionResult DeleteUserGame(int userId, int gameId)
        {
            _service.DeleteGame(userId, gameId);

            return NoContent();
        }

        [HttpPost("{userId}/comparison")]
        public async Task<UserComparisonDTO> PostUserComparison(int userId, 
            [FromBody] PostUserComparisonRequest comparisonRequest)
        {
            return await _service.GetComparison(userId, comparisonRequest.otherUserId.Value, comparisonRequest.comparison);
        } 

        public class PostGameRequest
        {
            [Required]
            public int? gameId { get; set; }
        }

        public class PostUserComparisonRequest
        {
            [Required]
            public int? otherUserId { get; set; }
            [Required]
            public string comparison { get; set; }
        }
    }
}
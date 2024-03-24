using EKtu.Persistence;
using EKtu.Repository.Dtos;
using EKtu.Repository.IService.EmailService;
using EKtu.Repository.IService.TokenService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace EKtu.WEBAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ResponseBase
    {
        private readonly ITokenService _tokenService;
        private readonly IOptions<Configuration> options;
        public TokenController(ITokenService tokenService, IEmail email, IOptions<Configuration> options)
        {
            _tokenService = tokenService;
            this.options = options;
        }
        [HttpPost] //[FromBody] string GenerateToken
        public async Task<IActionResult> GenerateToken() //token'de bir sıkıntı var 
        {
            //GenerateTokenRequestDto? generateTokenRequestDto = JsonSerializer.Deserialize<GenerateTokenRequestDto>(GenerateToken);
            GenerateTokenRequestDto generateTokenRequestDto = new GenerateTokenRequestDto()
            {
                userId = 2
            };

            var generateTokenResponseDto = await _tokenService.GeneratedTokenService(generateTokenRequestDto!);

            return ResponseData(generateTokenResponseDto);
        }
        [HttpGet]
        public async Task<IActionResult> CheckToken([FromHeader] string accessToken)
        {
            var response = await _tokenService.CheckToken(accessToken);

            return ResponseData(response);
        }
    }

}

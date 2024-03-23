using EKtu.Repository.Dtos;
using EKtu.Repository.IService.EmailService;
using EKtu.Repository.IService.TokenService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EKtu.WEBAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ResponseBase 
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService, IEmail email)
        {
            _tokenService = tokenService;
        }
        [HttpPost]
        public async Task<IActionResult> GenerateToken([FromBody] string GenerateToken) 
        {
            GenerateTokenRequestDto? generateTokenRequestDto = JsonSerializer.Deserialize<GenerateTokenRequestDto>(GenerateToken);
            var generateTokenResponseDto=   await _tokenService.GeneratedTokenService(generateTokenRequestDto!);

            return ResponseData(generateTokenResponseDto);
        }
    }
}

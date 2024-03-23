using EKtu.Repository.Dtos;
using EKtu.Repository.IService.TokenService;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Infrastructure.TokenServices
{
    public class TokenService : ITokenService
    {
        public Task<Response<GenerateTokenResponseDto>> GeneratedTokenService(GenerateTokenRequestDto generateTokenRequestDto)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("keykeykeykeykeykeykeykeykeykeykeykeykeykeykeykeykeykey"));//18 key 
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature);
            var claim = new Claim("userId", generateTokenRequestDto.userId.ToString());



            var SecToken = new JwtSecurityToken(
                issuer: "issuer1",
                audience:"issuer2",
                claims: new[] {claim},
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(SecToken);

            GenerateTokenResponseDto response = new GenerateTokenResponseDto()
            {
                AccessToken = token,
                AccessTokenLifeTime = DateTime.Now.AddMinutes(1)

            };
           return Task.FromResult(Response<GenerateTokenResponseDto>.Success(response, 200));
        }
    }
}

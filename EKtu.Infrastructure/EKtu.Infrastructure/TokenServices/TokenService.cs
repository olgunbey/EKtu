using EKtu.Repository.Dtos;
using EKtu.Repository.IService.TokenService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Infrastructure.TokenServices
{
    public class TokenService : ITokenService
    {
        private IOptions<Configuration> _options;
        public TokenService(IOptions<Configuration> options)
        {
            _options = options;
        }

        public async Task<Response<bool>> CheckToken(string accessToken)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

              var y = await   handler.ValidateTokenAsync(accessToken, new TokenValidationParameters()
                {
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.tokenKey)),
                  ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _options.Value.tokenIssuer,
                    ValidAudience = _options.Value.tokenAudience,
                    LifetimeValidator= CustomLifeTimeValidator

              });
                
            if(y.IsValid)
            {
                return Response<bool>.Success(200);
            }
            else
            {
                if (y.Exception.Message == "IDX10230: Lifetime validation failed. Delegate returned false, securitytoken: '[PII of type 'System.IdentityModel.Tokens.Jwt.JwtSecurityToken' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'.")
                    return Response<bool>.Fail("süresi geçmiş", 401);

                if (y.Exception.Message != null)
                    return Response<bool>.Fail("geçersiz sayfa", 401);
            }
                return Response<bool>.Success(200);
               

            
        
        }
        public bool CustomLifeTimeValidator(DateTime? notBefore,DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
        {
            if(expires != null && notBefore !=null)
            {
                var expiresNow = expires.Value.ToLocalTime();
                var beforeNow = notBefore.Value.ToLocalTime();

               if(expiresNow > DateTime.Now)
                {
                    return true;
                }
                return false;
            }
            return false;

        }

        public Task<Response<GenerateTokenResponseDto>> GeneratedTokenService(GenerateTokenRequestDto generateTokenRequestDto)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.tokenKey));
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature);
            var claim = new Claim("userId", generateTokenRequestDto.userId.ToString());

            var dateTimeNow = DateTime.Now;

            var newTimes = dateTimeNow.Add(TimeSpan.FromMinutes(1));
            var SecToken = new JwtSecurityToken(
                issuer: _options.Value.tokenIssuer,
                audience:_options.Value.tokenAudience,
                claims: new[] {claim},
                expires: newTimes,
                notBefore: dateTimeNow,
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(SecToken);

            GenerateTokenResponseDto response = new GenerateTokenResponseDto()
            {
                AccessToken = token,
                AccessTokenLifeTime = newTimes

            };
           return Task.FromResult(Response<GenerateTokenResponseDto>.Success(response, 200));
        }
    }
}

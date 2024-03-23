using EKtu.Repository.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IService.TokenService
{
    public interface ITokenService
    {
        Task<Response<GenerateTokenResponseDto>> GeneratedTokenService(GenerateTokenRequestDto generateTokenRequestDto);

    }
}

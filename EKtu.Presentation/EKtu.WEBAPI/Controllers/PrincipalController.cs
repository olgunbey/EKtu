using EKtu.Domain.Entities;
using EKtu.Persistence.Builder.IBuilder;
using EKtu.Repository.Dtos;
using EKtu.Repository.IService.AddPersonService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EKtu.WEBAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PrincipalController : ResponseBase
    {
        private readonly IAddPersonService<Principal> addPrincipalService;
        private readonly IPrincipalBuilder principalBuilder;
        public PrincipalController(IAddPersonService<Principal> addPrincipalService, IPrincipalBuilder principalBuilder)
        {
            this.addPrincipalService = addPrincipalService;
            this.principalBuilder = principalBuilder;
        }
        [HttpPost]
        [Authorize(Policy = "ClientCredentials")]
        public async Task<IActionResult> AddPrincipal([FromBody]AddPrincipalRequestDto addPrincipalRequestDto)
        {
          Principal Principal= principalBuilder.
                FirstName(addPrincipalRequestDto.FirstName)
                .LastName(addPrincipalRequestDto.LastName)
                .Email(addPrincipalRequestDto.Email)
                .Password(addPrincipalRequestDto.Password)
                .TckNo(addPrincipalRequestDto.TckNo)
                .GetPerson();

            return ResponseData(await addPrincipalService.AddAsync(Principal));
        }
    }
}

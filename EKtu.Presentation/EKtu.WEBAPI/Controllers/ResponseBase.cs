using EKtu.Repository.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EKtu.WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseBase : ControllerBase
    {
        protected IActionResult ResponseData<T>(Response<T> response)
        {
            if (response.StatusCode == 204)
                return new ObjectResult(null);
            return new ObjectResult(response);

        }
    }
}

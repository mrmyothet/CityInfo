using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class CitiesController : ControllerBase
{
    [HttpGet]
    public JsonResult GetCities()
    {
        return new JsonResult(
            new List<object> { new { id = 1, Name = "New York" }, new { id = 2, Name = "Queens" } }
        );
    }
}

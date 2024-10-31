using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

public class MvcController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

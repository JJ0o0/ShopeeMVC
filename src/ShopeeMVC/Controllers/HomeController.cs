using Microsoft.AspNetCore.Mvc;
namespace ShopeeMVC.Controllers;
public class HomeController : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() { Response.StatusCode = 500; return View(); }
}

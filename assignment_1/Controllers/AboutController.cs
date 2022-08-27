using Microsoft.AspNetCore.Mvc;

namespace assignment_1.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult index()
        {
            return View();
        } 
        
    }
}
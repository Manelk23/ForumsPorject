using ForumsPorject.Models;
using ForumsPorject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Security.Claims;

namespace ForumsPorject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CategoryService _categoryService;
       

        public HomeController(ILogger<HomeController> logger, CategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
            
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAll();
            var categoryModels = new List<CategoryModel>();

            foreach (var cat in categories)
            {
                var categoryMod = new CategoryModel
                {
                    Id = cat.CategorieId,
                    Name = cat.TitreCategorie,
                    Discription = cat.DescriptionCategorie,

                };
                categoryModels.Add(categoryMod);
            }

            return View(categoryModels);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }


        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Home/Error/{message?}")]
        public IActionResult Error(string? message = null)
        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            if (message != null)
            {
               
                    ViewData["Title"] = "Error";
                    ViewData["ErrorMessage"] = message;
                    return View("Error");
               
            }

            

            return View("Error");
        }

             
    }
}
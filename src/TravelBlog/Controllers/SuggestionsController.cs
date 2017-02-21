using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelBlog.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TravelBlog.Controllers
{
    public class SuggestionsController : Controller
    {
        private ISuggestionRepository suggestionRepo;
        public SuggestionsController(ISuggestionRepository thisRepo = null)
        {
            if (thisRepo == null)
            {
                suggestionRepo = new EFSuggestionRepository();
            }
            else
            {
                suggestionRepo = thisRepo;
            }
        }

        public IActionResult Index()
        {
            return View(suggestionRepo.Suggestions.ToList());
        }

        public IActionResult Create()
        {
         
            return View();
        }
        [HttpPost]
        public IActionResult Create(Suggestion suggestion, int locationId)
        {
            Location location = suggestionRepo.Locations.FirstOrDefault(l => l.LocationId == locationId);
            //string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //ApplicationUser user = suggestionRepo.Users.FirstOrDefault(u => u.Id == userId);
            suggestion.Location = location;
            //suggestion.AppUser = user;
            suggestionRepo.Save(suggestion);
            return RedirectToAction("Details", "Locations", new { id = suggestion.Location.LocationId });
        }
    }
}

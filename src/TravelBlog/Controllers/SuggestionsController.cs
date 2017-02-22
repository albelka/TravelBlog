using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelBlog.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Immutable;

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
            //Location location = suggestionRepo.Locations.FirstOrDefault(l => l.LocationId == locationId);
            //string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //ApplicationUser user = suggestionRepo.Users.FirstOrDefault(u => u.Id == userId);
            //suggestion.Location = location;
            //suggestion.AppUser = user;
            suggestionRepo.Save(suggestion);
            return RedirectToAction("Index","Locations");
        }

        public IActionResult Delete(int id)
        {
            var thisSuggestion = suggestionRepo.Suggestions.FirstOrDefault(suggestion => suggestion.Id == id);
            return View(thisSuggestion);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var thisSuggestion = suggestionRepo.Suggestions.FirstOrDefault(suggestion => suggestion.Id == id);
            suggestionRepo.Remove(thisSuggestion);
            return RedirectToAction("Index","Locations");
        }
        [HttpPost]
        public IActionResult Upvote(int Upvote)
        {
            var thisSuggestion = suggestionRepo.Suggestions.FirstOrDefault(suggestion => suggestion.Id == Upvote);
            thisSuggestion.Votes += 1;
            suggestionRepo.Edit(thisSuggestion);
            
            return Json(thisSuggestion);

        }
    }
}

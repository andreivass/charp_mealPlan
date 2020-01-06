using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Recipes
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;
        
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        
        public string? SortName { get; set; }
        public string? SortDescription { get; set; }
        public string? SortTime { get; set; }
        public string? SortServings { get; set; }

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Recipe> Recipe { get;set; } = default!;

        public async Task OnGetAsync(string? onReset, string? sortOrder)
        {
            SortName = string.IsNullOrEmpty(sortOrder) ? "name_sort" : "";
            SortDescription = sortOrder == "desc_sort" ? "desc_desc" : "desc_sort";
            SortTime = sortOrder == "time_sort" ? "time_desc" : "time_sort";

            switch (@sortOrder)
            {
                case "desc_sort":
                    Recipe = await _context.RecipesType
                        .OrderBy(s => s.RecipeDescription).ToListAsync();
                    break;
                case "desc_desc":
                    Recipe = await _context.RecipesType
                        .OrderByDescending(s => s.RecipeDescription).ToListAsync();
                    break;
                case "time_sort":
                    Recipe = await _context.RecipesType
                        .OrderBy(s => s.RecipeTime).ToListAsync();
                    break;
                case "time_desc":
                    Recipe = await _context.RecipesType
                        .OrderByDescending(s => s.RecipeTime).ToListAsync();
                    break;
                case "name_sort":
                    Recipe = await _context.RecipesType
                        .OrderByDescending(s => s.RecipeName).ToListAsync();
                    break;
                default:
                    Recipe = await _context.RecipesType
                        .OrderBy(s => s.RecipeName).ToListAsync();
                    break;
            }
            
            
            if (onReset == "Reset")
            {
                SearchString = "";
            }
            
            
            if (!string.IsNullOrEmpty(SearchString))
            {
                var filterCars = Recipe.Where(s => s.RecipeName.ToLower().Contains(SearchString.ToLower()));
                Recipe = filterCars.ToList();
            }
        }
    }
}

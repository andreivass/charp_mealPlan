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

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Recipe> Recipe { get;set; } = default!;

        public async Task OnGetAsync(string? onReset)
        {
            Recipe = await _context.RecipesType.ToListAsync();
            
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Ingredients
{
    public class IndexModel : PageModel
    
    {
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Ingredient> Ingredient { get; set; } = default!;

        public async Task OnGetAsync(string? onReset)
        {
            Ingredient = await _context.Ingredients
                .Include(i => i.IngredientCategory)
                .Include(i => i.IngredientLocation)
                .Include(i => i.IngredientUnit).ToListAsync();
            
            if (onReset == "Reset")
            {
                SearchString = "";
            }
            
            
            if (!string.IsNullOrEmpty(SearchString))
            {
                var filterCars = Ingredient.Where(s => s.IngredientName.ToLower().Contains(SearchString.ToLower()));
                Ingredient = filterCars.ToList();
            }
        }
    }
}

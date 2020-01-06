using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_IngredientInRecipes
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IngredientInRecipe IngredientInRecipe { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IngredientInRecipe = await _context.IngredientInRecipes
                .Include(i => i.Ingredient)
                .Include(i => i.Recipe).FirstOrDefaultAsync(m => m.IngredientInRecipeId == id);

            if (IngredientInRecipe == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

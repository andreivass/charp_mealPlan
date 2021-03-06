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
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public Recipe Recipe { get; set; } = default!;
        
        public IList<IngredientInRecipe> Ingredients { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await _context.RecipesType.FirstOrDefaultAsync(m => m.RecipeId == id);

            

            if (Recipe == null)
            {
                return NotFound();
            }
            
            Ingredients = await _context
                .IngredientInRecipes.Where(ir => ir.RecipeId == id)
                .Include(i => i.Ingredient)
                .ThenInclude(il => il.IngredientUnit)
                .ToListAsync();
            
            return Page();
        }
    }
}

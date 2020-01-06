using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Meals
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }
        
        public IList<IngredientInRecipe> Ingredients { get;set; } = default!;

        public Recipe Recipe { get; set; } = default!;
        public decimal Servings { get; set; }
        
        public int? Time { get; set; }

        public string? SearchString { get; set; }
        

        public async Task<IActionResult> OnGetAsync(int? id, int time, decimal servings, string searchString)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            //Servings = servings > 0 ? servings : 0;
            Servings = servings;
            Time = time;
            SearchString = searchString;

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
            
            foreach (var ingredientInRecipe in Ingredients)
            {
                ingredientInRecipe.IngredientInRecipeAmount =
                    ingredientInRecipe.IngredientInRecipeAmount *  Servings / Recipe.RecipeServings;
            }
            
            return Page();
        }
    }
}
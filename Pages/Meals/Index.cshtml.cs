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
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        [BindProperty(SupportsGet = true)] public int? Time { get; set; }

        public IList<Ingredient> AllIngredients { get; set; } = default!;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Recipe> Recipes { get; set; } = default!;
        public IList<Recipe> PossibleRecipes { get; set; } = new List<Recipe>();

        public async Task<IActionResult> OnGetAsync(int? time)
        {
            Time = time > 0 ? time : 0;

            // TODO: check ingredients available
            Recipes = await _context.RecipesType.Where(s => s.RecipeTime <= Time)
                .ToListAsync();

            AllIngredients = await _context.Ingredients.ToListAsync();

            foreach (var recipe in Recipes)
            {
                var recipeIngredients = await _context
                    .IngredientInRecipes.Where(ir => ir.RecipeId == recipe.RecipeId)
                    .ToListAsync();
                foreach (var ingredientInRecipe in recipeIngredients)
                {
                    if (AllIngredients.Contains(ingredientInRecipe.Ingredient))
                    {
                        var x = AllIngredients
                            .FirstOrDefault(i => i.IngredientId == ingredientInRecipe.Ingredient.IngredientId)
                            .IngredientAmount;
                        if (ingredientInRecipe.IngredientInRecipeAmount <= x)
                        {
                            PossibleRecipes.Add(recipe);
                        }
                    }
                }
            }

            return Page();
        }
    }
}
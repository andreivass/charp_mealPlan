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
        [BindProperty(SupportsGet = true)] public decimal? Servings { get; set; }

        public IList<Ingredient> AllIngredients { get; set; } = default!;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Recipe> Recipes { get; set; } = default!;
        public IList<Recipe> PossibleRecipes { get; set; } = new List<Recipe>();
        public IList<Recipe> FilteredRecipesYes { get; set; } = new List<Recipe>();
        public IList<Recipe> FilteredRecipesNo { get; set; } = new List<Recipe>();
        
        [BindProperty(SupportsGet = true)]
        public string? SearchStringYes { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchStringNo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? time, decimal? servings)
        {
            Time = time > 0 ? time : 0;
            Servings = servings > 0 ? servings : 0;
            
            
            Recipes = await _context.RecipesType.Where(s => s.RecipeTime <= Time)
                .ToListAsync();

            AllIngredients = await _context.Ingredients.ToListAsync();

            foreach (var recipe in Recipes)
            {
                var recipeIngredients = await _context
                    .IngredientInRecipes.Where(ir => ir.RecipeId == recipe.RecipeId)
                    .ToListAsync();
                var boolCheck = true;
                foreach (var ingredientInRecipe in recipeIngredients)
                {
                    
                    if (AllIngredients.Contains(ingredientInRecipe.Ingredient))
                    {
                        var x = AllIngredients
                            .FirstOrDefault(i => i.IngredientId == ingredientInRecipe.Ingredient.IngredientId)
                            .IngredientAmount;
                        if (ingredientInRecipe.IngredientInRecipeAmount * Servings / recipe.RecipeServings > x)
                        {
                            //PossibleRecipes.Add(recipe);
                            boolCheck = false;
                        }
                    }
                }

                if (boolCheck)
                {
                    PossibleRecipes.Add(recipe);
                }
            }
            /*
            if (onReset == "Reset")
            {
                SearchString = "";
            }*/
            
            
            if (!string.IsNullOrEmpty(SearchStringYes))
            {
                foreach (var possibleRecipe in PossibleRecipes)
                {
                    foreach (var ingredientInRecipe in possibleRecipe.IngredientInRecipe)
                    {
                        if (ingredientInRecipe.Ingredient.IngredientName.ToLower().Contains(SearchStringYes.ToLower()))
                        {
                            if (!FilteredRecipesYes.Contains(possibleRecipe))
                            {
                                FilteredRecipesYes.Add(possibleRecipe);
                            }
                                
                        }
                    }
                }

                PossibleRecipes = FilteredRecipesYes;
            }
            
            if (!string.IsNullOrEmpty(SearchStringNo))
            {
                foreach (var possibleRecipe in PossibleRecipes)
                {
                    var decisionBool = true;
                    
                    foreach (var ingredientInRecipe in possibleRecipe.IngredientInRecipe)
                    {
                        if (ingredientInRecipe.Ingredient.IngredientName.ToLower().Contains(SearchStringNo.ToLower()))
                        {
                            decisionBool = false;
                        }
                    }
                    
                    if (decisionBool)
                    {
                        FilteredRecipesNo.Add(possibleRecipe);
                    }
                }

                PossibleRecipes = FilteredRecipesNo;
            }

            return Page();
        }
    }
}
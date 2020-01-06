using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WebApp.Pages_IngredientInRecipes
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;
        [BindProperty] public int RecipeId { get; set; } = default!;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int recipeId)
        {
            RecipeId = recipeId;
        ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientName");
        ViewData["RecipeId"] = new SelectList(_context.RecipesType, "RecipeId", "RecipeName");
            return Page();
        }

        [BindProperty]
        public IngredientInRecipe IngredientInRecipe { get; set; } = default!;

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            IngredientInRecipe.RecipeId = RecipeId;

            _context.IngredientInRecipes.Add(IngredientInRecipe);
            await _context.SaveChangesAsync();
            
            if (RecipeId > 0)
            {
                return RedirectToPage("../Recipes/Details", new {id = RecipeId});
            }
            return RedirectToPage("./Index");
        }
    }
}

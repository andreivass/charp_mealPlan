using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            /*if (!string.IsNullOrEmpty(SearchString))
            {
                return RedirectToPage("./Meals/Meals?SearchString" + Recipe.RecipeTime);
            }*/
            return Page();
        }

        [BindProperty]
        public Recipe Recipe { get; set; } = default!;
        
        
        [Range(0.1, 100, ErrorMessage = "Be reasonable, you can´t cook for more than 100 or 0.")]
        [BindProperty]
        public decimal Servings { get; set; } = default!;
        
        [Range(1, 600, ErrorMessage = "Be reasonable, you won´t cook for more than 10 h or less than 1 min.")]
        [BindProperty] public int Time { get; set; } = default!;


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.RecipesType.Add(Recipe);
            await _context.SaveChangesAsync();

            return RedirectToPage(".Index");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WumpWeb.Models;

namespace WumpWeb.Pages.Scores
{
    public class DetailsModel : PageModel
    {
        private readonly WumpWeb.Models.ScoreContext _context;

        public DetailsModel(WumpWeb.Models.ScoreContext context)
        {
            _context = context;
        }

        public Score Score { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Score = await _context.ScoreTable.FirstOrDefaultAsync(m => m.ID == id);

            if (Score == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

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
    public class IndexModel : PageModel
    {
        private readonly WumpWeb.Models.ScoreContext _context;

        public IndexModel(WumpWeb.Models.ScoreContext context)
        {
            _context = context;
        }

        public IList<Score> Score { get;set; }

        public async Task OnGetAsync()
        {
            Score = await _context.ScoreTable.ToListAsync();
        }
    }
}

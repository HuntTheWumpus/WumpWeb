using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WumpWeb.Models;
using Newtonsoft.Json.Linq;


namespace WumpWeb.Pages.Scores
{
        public class Question
    {
        public string id {get; }
        public string category {get; set;}
        public string name {get; set; }
        public string detail {get; set; }
        private static int _count = 0;

        public Question(JToken obj)
        {
            id = String.Format("Q{0}", obj["id"]);
            category = obj["category"].ToString();
            name = obj["name"].ToString();
            try {
                detail = obj["detail"].ToString();
            } catch (NullReferenceException e)
            {
                detail = "";
            } 
        }
    }
    public class AnswerChoice 
    {
        public string score {get; set; }
        public string type {get; set; }
        public string criteria {get; set; }
        public AnswerChoice(JToken obj)
        {
            score = obj["score"].ToString();
            type = obj["type"].ToString();
            try {
                criteria = obj["criteria"].ToString();
            } catch(NullReferenceException e)
            {
                criteria = "";
            }
        }
    }

    public class QuestionAndAnswers
    {
        public Question question {get; set; }
        public List<AnswerChoice> answers { get; set; }
        public QuestionAndAnswers(JToken obj)
        {
            question = new Question(obj);
            answers = new List<AnswerChoice>();
            foreach (var item in obj["rubric"])
                answers.Add(new AnswerChoice(item));
        }
    }
    public class EditModel : PageModel
    {
        private readonly WumpWeb.Models.ScoreContext _context;
        public List<string> judges = new List<string>();
        public List<string> teams = new List<string>();
        public Dictionary<string, List<QuestionAndAnswers>> category_qna 
            = new Dictionary<string, List<QuestionAndAnswers>>();

        public EditModel(WumpWeb.Models.ScoreContext context)
        {
            _context = context;
            string fileContent;
            using (var reader = System.IO.File.OpenText("scoring_rubric.json")) {
                fileContent = reader.ReadToEnd();
            }
            var config = JObject.Parse(fileContent);
            foreach (var item in config["judges"])
                judges.Add(item["name"].ToString());
            foreach (var item in config["teams"])
                teams.Add(item["name"].ToString());
            foreach (var item in config["judging_criteria"]) {
                var qna = new QuestionAndAnswers(item);
                if (!category_qna.ContainsKey(qna.question.category))
                    category_qna.Add(qna.question.category, new List<QuestionAndAnswers>());
                category_qna[qna.question.category].Add(qna);
            }
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Score).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScoreExists(Score.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ScoreExists(int id)
        {
            return _context.ScoreTable.Any(e => e.ID == id);
        }
    }
}

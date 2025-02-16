using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication5.Models;

namespace WebApplication5.Pages
{
    [Authorize]
    public class LkModel : PageModel
    {
        ApplicationContext db;
        public Person thisUser;
        public List<Book> thisUserBooks;
        public BooksParser bp;
        private readonly ILogger<LkModel> _logger;
        public LkModel(ApplicationContext db, ILogger<LkModel> logger) 
        { 
            this.db = db; 
            _logger = logger;
            bp = new(db);
        }
        public void OnGet()
        {
            List<Person> people = new List<Person>();
            people = db.Persons.ToList();
            thisUser = people.FirstOrDefault(p => p.Name == HttpContext.User.Identity.Name);
            thisUserBooks = new BooksParser(db).Deparse(thisUser.ClaimedBooks);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var form = HttpContext.Request.Form;
            string byname = form["byname"];
            string byrating = form["byrating"];
            string byjenres = form["byjenres"];

            thisUserBooks = bp.Sort(db.Books.ToList(), int.Parse(byrating.ToString()), byjenres, int.Parse(byname));

            return Redirect("/");
        }
    }
}

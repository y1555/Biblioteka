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
        private readonly ILogger<LkModel> _logger;
        public LkModel(ApplicationContext db, ILogger<LkModel> logger) 
        { 
            this.db = db; 
            _logger = logger;
        }
        public void OnGet()
        {
            List<Person> people = new List<Person>();
            people = db.Persons.ToList();
            thisUser = people.FirstOrDefault(p => p.Name == HttpContext.User.Identity.Name);
            thisUserBooks = new BooksParser(db).Deparse(thisUser.ClaimedBooks);
        }
    }
}

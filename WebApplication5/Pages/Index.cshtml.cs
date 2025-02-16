using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebApplication5.Models;

namespace WebApplication5.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        ApplicationContext db;
        public BooksParser bp;
        public List<Book> sortedBooks;
        string byname = "1";
        string byrating = "8";
        string byjenres = "all";

        public IndexModel(ILogger<IndexModel> logger, ApplicationContext db)
        {
            this.db = db;
            _logger = logger;
            bp = new(db);
            sortedBooks = bp.Sort(db.Books.ToList());
        }

        public void OnGet()
        {
            sortedBooks = bp.Sort(db.Books.ToList(), int.Parse(byrating.ToString()), byjenres, int.Parse(byname));
        }
        public async Task<IActionResult> OnPostAsync() 
        {
            var form = HttpContext.Request.Form;
            string byname = form["byname"];
            string byrating = form["byrating"];
            string byjenres = form["byjenres"];

            sortedBooks = bp.Sort(db.Books.ToList(), int.Parse(byrating.ToString()), byjenres, int.Parse(byname));

            return Redirect("/");
        }
    }
}

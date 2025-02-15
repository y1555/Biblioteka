using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text.Json.Nodes;
using WebApplication5.Models;

namespace WebApplication5.Pages
{
    [Authorize(Roles = "0, 1")]
    public class AdminPanelModel : PageModel
    {
        private readonly ILogger<AdminPanelModel> _logger;
        public ApplicationContext db;
        public string output;

        public AdminPanelModel(ILogger<AdminPanelModel> logger, ApplicationContext db)
        {
            this.db = db;
            _logger = logger;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            // получаем из формы email и пароль
            var form = HttpContext.Request.Form;
            //output = form["jenres"];
            // если email и/или пароль не установлены, посылаем статусный код ошибки 400
            if (!form.ContainsKey("name") || !form.ContainsKey("jenres"))
                return BadRequest("Пусто в заголовке");

            string name = form["name"];
            string rating = form["rating"];
            string jenres = form["jenres"];
            string description = form["description"];

            List<Book> books = new List<Book>();
            books = db.Books.ToList();
            Book? book = books.FirstOrDefault(b => b.Name == name && b.Jenres == jenres);
            // если пользователь не найден, отправляем статусный код 401
            if (book is null)
            {
                book = new Book(name, int.Parse(rating), description, jenres);
                db.Books.Add(book);
                await db.SaveChangesAsync();
            }
            else return BadRequest("Эта хня уже есть");

            return Redirect("/AdminPanel");
        }
    }
}

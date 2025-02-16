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
        ApplicationContext db;
        public BooksParser bp;
        public List<Book> sortedBooks;

        public AdminPanelModel(ILogger<AdminPanelModel> logger, ApplicationContext db)
        {
            this.db = db;
            _logger = logger;
            bp = new(db);
            sortedBooks = bp.Sort(db.Books.ToList());
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            // �������� �� ����� email � ������
            var form = HttpContext.Request.Form;
            //output = form["jenres"];
            // ���� email �/��� ������ �� �����������, �������� ��������� ��� ������ 400
            if (!form.ContainsKey("name") || !form.ContainsKey("jenres"))
                return BadRequest("����� � ���������");

            string name = form["name"];
            string rating = form["rating"];
            string jenres = form["jenres"] + ",";
            string description = form["description"];

            List<Book> books = new List<Book>();
            books = db.Books.ToList();
            Book? book = books.FirstOrDefault(b => b.Name == name && b.Jenres == jenres);
            // ���� ������������ �� ������, ���������� ��������� ��� 401
            if (book is null)
            {
                if (rating is null) rating = Ages.twentyone_plus.ToString();
                book = new Book(name, int.Parse(rating), description, jenres);
                db.Books.Add(book);
                await db.SaveChangesAsync();
            }
            else return BadRequest("��� ��� ��� ����");

            return Redirect("/AdminPanel");
        }

        public async Task<IActionResult> OnSortAsync()
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

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
        public ApplicationContext db;

        public IndexModel(ILogger<IndexModel> logger, ApplicationContext db)
        {
            this.db = db;
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}

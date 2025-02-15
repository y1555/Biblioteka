using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebApplication5.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace WebApplication5.Pages
{
    public class LoginModel : PageModel
    {
        ApplicationContext db;

        //Person loginData;

        private readonly ILogger<LoginModel> _logger;
        public LoginModel(ApplicationContext db, ILogger<LoginModel> logger) { this.db = db; _logger = logger; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(string? returnUrl/*, Person loginData*/)
        {/*
            List<Person> people = new List<Person>();
            people = db.Persons.ToList();
            Person? person = people.FirstOrDefault(p => p.Login == loginData.Login && p.Password == loginData.Password);
            // если пользователь не найден, отправляем статусный код 401
            if (person is null) return Unauthorized();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Login) };
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            // формируем ответ
            var response = new
            {
                access_token = encodedJwt,
                username = person.Login
            };

            return new JsonResult(response);
            */
            
            // получаем из формы email и пароль
            var form = HttpContext.Request.Form;
            // если email и/или пароль не установлены, посылаем статусный код ошибки 400
            if (!form.ContainsKey("login") || !form.ContainsKey("password"))
                return BadRequest("Логин и/или пароль не установлены");

            string login = form["login"];
            string password = form["password"];

            // находим пользователя 
            List<Person> people = new List<Person>();
            people = db.Persons.ToList();
            Person? person = people.FirstOrDefault(p => p.Login == login && p.Password == password);
            // если пользователь не найден, отправляем статусный код 401
            if (person is null) return Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, person.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role.ToString())
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            ViewData["AutorisedUser"] = person.Name;
            return Redirect(returnUrl ?? "/Lk");
            
        }
    }
}

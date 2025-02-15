using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebApplication5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace WebApplication5.Pages
{
    [IgnoreAntiforgeryToken]
    public class RegisterModel : PageModel
    {
        ApplicationContext db;

        string? loggedUser;

        public RegisterModel(ApplicationContext db) { this.db = db; }
        public void OnGet()
        {
            if (!(HttpContext.User.Identity is null))
            {
                loggedUser = HttpContext.User.Identity.Name;
                ViewData["AutorisedUser"] = loggedUser;
            }
        }
        public async Task<IActionResult> OnPostAsync(string? returnUrl/*, Person loginData*/)
        {
            /*
            List<Person> people = new List<Person>();
            people = db.Persons.ToList();
            Person? person = people.FirstOrDefault(p => p.Login == loginData.Login && p.Password == loginData.Password);
            // если пользователь не найден, отправляем статусный код 401
            if (person is null)
            {
                person = new Person(loginData.Login, loginData.Password, loginData.Name, loginData.Age);
                people.Add(person);

            }

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
            if (!form.ContainsKey("login") || !form.ContainsKey("password") || !form.ContainsKey("repassword"))
                return BadRequest("Логин и/или пароль не установлены");

            string login = form["login"];
            string username = form["username"];
            string password = form["password"];
            string repassword = form["repassword"];
            int age = int.Parse(form["age"]);

            // находим пользователя 
            List<Person> people = new List<Person>();
            people = db.Persons.ToList();
            Person? person = people.FirstOrDefault(p => p.Login == login && p.Password == password);
            // если пользователь не найден, отправляем статусный код 401
            if (person is null)
            {
                person = new Person(login, password, username, age);
                db.Persons.Add(person);
                await db.SaveChangesAsync();
            }
            else return Unauthorized();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Login) };
            // создаем объект ClaimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            ViewData["AutorisedUser"] = person.Name;
            return Redirect(returnUrl ?? "/Lk");
        }
    }
}

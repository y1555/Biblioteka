namespace WebApplication5.Models
{
    public class Person
    {
        public Person() { }
        public Person(string? login, string? password, string? name, int age)
        {
            Login = login;
            Password = password;
            Name = name;
            Age = age;
        }

        public int Id { set; get; }
        public string? Name { set; get; }
        public int Age { set; get; }
        public string? Login { set; get; }
        public string? Password { set; get; }
        public int? Role { set; get; } = ((int)Roles.User);
        public string? ClaimedBooks { set; get; }
    }
}

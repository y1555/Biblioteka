using System.Text;
using WebApplication5.Pages;

namespace WebApplication5.Models
{
    public class BooksParser
    {
        ApplicationContext db;
        public BooksParser(ApplicationContext db) { this.db = db;}
        public List<Book> Deparse(string input) //распаковать книги из строки с айдишниками после хранения в массив книг (получить выборку)
        {
            if (input is not null)
            {
                List<Book> all = new();
                all = db.Books.ToList();
                List<Book> output = new();


                string number = "";
                foreach (char symbol in input)
                {
                    if (symbol == ' ')
                    {
                        output.Add(all[int.Parse(number)]);
                        number = "";
                        continue;
                    }
                    number += symbol.ToString();
                }
                return output;
            }
            else return null;
        }
        public string Parse(List<Book> input) //запаковать айдишники книг из списка в строку для храниения в бд пользователей 
        {
            string output = "";
            foreach (Book b in input) { output += b.Id.ToString() + " "; }
            return output;
        }
        public List<Book> Filtering(List<Book> input, FiltersType filtertype, List<string> filters)
        {
            List<Book> output = new();

            switch(filtertype)
            {
                case FiltersType.Age:
                    break;
                case FiltersType.Jenre:
                    break;
                default:
                    output = input;
                    break;
            }
            return output;
        }
    }
}

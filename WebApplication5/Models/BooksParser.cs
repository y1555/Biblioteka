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

        public string JenreParse(string input)
        {
            string output = "";
            string number = "";
            foreach (char symbol in input)
            {
                if (symbol == ',')
                {
                    switch(int.Parse(number))
                    {
                        case ((int)Jenres.scifi): output += "Sci-fi "; break;
                        case ((int)Jenres.fanfic): output += "FanFic "; break;
                        case ((int)Jenres.fantasy): output += "Fantasy "; break;
                        case ((int)Jenres.romantic): output += "Romantic "; break;
                        case ((int)Jenres.battle): output += "Battle "; break;
                        default: output += "Undefined "; break;
                    }

                    number = "";
                    continue;
                }
                else
                {
                    number += symbol.ToString();
                }
            }
            return output;
        }
        public string AgeParse(int input)
        {
            string output = "";
            switch (input)
            {
                case ((int)Ages.ziro_plus): output += "0+"; break;
                case ((int)Ages.three_plus): output += "3+"; break;
                case ((int)Ages.six_plus): output += "6+"; break;
                case ((int)Ages.ten_plus): output += "10+"; break;
                case ((int)Ages.twelwe_plus): output += "12+"; break;
                case ((int)Ages.fourteen_plus): output += "14+"; break;
                case ((int)Ages.sixteen_plus): output += "16+"; break;
                case ((int)Ages.eighteen_plus): output += "18+"; break;
                case ((int)Ages.twentyone_plus): output += "21+"; break;
                default: output += "Undefined"; break;
            }
            return output;
        }
        public List<Book> Sort(List<Book> inputBooks, int inputAge = ((int)Ages.twentyone_plus), string inputJenres = "all", int to = 1)
        {
            List<Book> output = new();
            output = SortByName(SortByJenre(SortByAge(inputBooks, inputAge), inputJenres), to);
            return output;
        }

        private List<Book> SortByJenre(List<Book> inputBooks, string inputJenres)
        {
            List<Book> output = new List<Book>();
            string number = "";
            if (inputJenres == "all" || inputJenres is null) return inputBooks;
            foreach (Book book in inputBooks)
            {
                foreach (char symbol in inputJenres)
                {
                    if (symbol == ',')
                    {
                        if(inputJenres.Contains(number))
                        {
                            if(!output.Contains(book))
                                output.Add(book);
                        }
                        number = "";
                    }
                    else
                    {
                        number += symbol.ToString();
                    }
                }
            }
            return output;
        }

        private List<Book> SortByAge(List<Book> inputBooks, int inputAge)
        {
            List<Book> output = new List<Book>();
            foreach (Book book in inputBooks)
            {
                if(book.Rating <= inputAge)
                {
                    output.Add(book);
                }
            }
            return output;
        }

        private List<Book> SortByName(List<Book> inputBooks, int to)
        {
            List<Book> output = new List<Book>();
            List<Book> bufferList = inputBooks;
            bool sorted = false;
            int i = 0;
            if (bufferList.Count >= 2)
            {
                while (!sorted)
                {
                    if (i + 1 == bufferList.Count - 1) { i = 0; }
                    if (String.Compare(bufferList[i].Name, bufferList[i + 1].Name) >= 0)
                    {
                        Book bufferBook = bufferList[i + 1];
                        bufferList[i + 1] = bufferList[i];
                        bufferList[i] = bufferBook;
                    }
                    sorted = true;
                    for (int j = 0; j < bufferList.Count; j++)
                    {
                        if (String.Compare(bufferList[i].Name, bufferList[i + 1].Name) <= 0)
                        {
                            continue;
                        }
                        else
                        {
                            sorted = false;
                            break;
                        }
                    }
                    i++;
                }
            }
            if (to == 1) output = bufferList;
            else for (int j = bufferList.Count - 1; j >= 0; j--) output.Add(bufferList[j]);
            return output;
        }
    }
}

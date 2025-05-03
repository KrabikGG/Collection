using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection
{
    class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int Shell_Number { get; set; }
        public int Rack_Namber { get; set; }
        public int Year { get; set; }

        public Book(int id, string name, string author, int shellNumber, int rackNumber, int year)
        {
            Id = id;
            Name = name;
            Author = author;
            Shell_Number = shellNumber;
            Rack_Namber = rackNumber;
            Year = year;
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace Collection.Tests
{
    public class MockBookFinderService : IBookFinderService
    {
        private readonly List<Book> _mockDatabase;
        public bool SimulateDatabaseError { get; set; } = false;

        public MockBookFinderService(List<Book> predefinedBooks)
        {
            _mockDatabase = predefinedBooks ?? new List<Book>();
        }

        public Book FindBook(string author, string name)
        {
            if (SimulateDatabaseError)
            {
                throw new System.Exception("Симульована помилка бази даних.");
            }
            return _mockDatabase.FirstOrDefault(b =>
                b.Author.Equals(author, System.StringComparison.OrdinalIgnoreCase) &&
                b.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
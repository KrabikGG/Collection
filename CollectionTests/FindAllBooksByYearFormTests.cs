using Microsoft.VisualStudio.TestTools.UnitTesting;
using Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection.Tests
{
    public class MockDataAccessForYearTest : DataAccess
    {
        public MockDataAccessForYearTest(List<Book> predefinedBookList)
        {
            this.bookList = predefinedBookList;
        }
    }

    [TestClass]
    public class FindAllBooksByYearFormTests
    {
        private void AssertBookListsAreEqual(List<Book> expected, List<Book> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count, "Кількість книг не співпадає.");
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id, $"ID книги не співпадає для книги з індексом {i}.");
                Assert.AreEqual(expected[i].Name, actual[i].Name, $"Назва книги не співпадає для книги з індексом {i}.");
                Assert.AreEqual(expected[i].Author, actual[i].Author, $"Автор книги не співпадає для книги з індексом {i}.");
                Assert.AreEqual(expected[i].Year, actual[i].Year, $"Рік книги не співпадає для книги з індексом {i}.");
            }
        }

        [TestMethod]
        public void FindButton_Click_Should_FilterBooksCorrectlyForSelectedYear()
        {
            int targetYear = 2020;
            var expectedBooks = new List<Book>
        {
            new Book(0, "Author A", "Book 1", 1, 10, 2020),
            new Book(2, "Author C", "Book 3", 2, 12, 2020)
        };

            var allBooks = new List<Book>
        {
            new Book(0, "Author A", "Book 1", 1, 10, 2020),
            new Book(1, "Author B", "Book 2", 1, 11, 2021),
            new Book(2, "Author C", "Book 3", 2, 12, 2020),
            new Book(3, "Author D", "Book 4", 2, 13, 2022)
        };

            var mockDataAccess = new MockDataAccessForYearTest(allBooks);
            var targetForm = new FindAllBooksByYearForm(mockDataAccess);
            var yearsListBox = targetForm.GetYearsListBoxForTest();
            if (mockDataAccess.bookList.Select(b => b.Year).Distinct().Any())
            {
                if (yearsListBox.ItemsSource == null)
                {
                    yearsListBox.ItemsSource = mockDataAccess.bookList.Select(b => b.Year).Distinct().OrderBy(y => y).ToList();
                }
            }

            if (yearsListBox.Items.Contains(targetYear))
            {
                yearsListBox.SelectedItem = targetYear;
            }
            else
            {
                Assert.IsTrue(yearsListBox.Items.Contains(targetYear), $"Цільовий рік {targetYear} відсутній у YearsListBox.Items. Перевірте логіку LoadYears або налаштування тесту.");
                yearsListBox.SelectedItem = targetYear;
            }

            targetForm.FindButton_Click(null, null);
            List<Book> actualBooksFound = targetForm.TestHook_BooksFoundForReport;

            Assert.IsNotNull(actualBooksFound, "Список знайдених книг (TestHook_BooksFoundForReport) не повинен бути null.");
            AssertBookListsAreEqual(expectedBooks, actualBooksFound);
        }
    }
}
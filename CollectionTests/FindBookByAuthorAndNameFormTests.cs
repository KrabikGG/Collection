using Microsoft.VisualStudio.TestTools.UnitTesting;
using Collection;
using System.Collections.Generic;

namespace Collection.Tests
{
    [TestClass]
    public class FindBookByAuthorAndNameFormTests
    {
        private void AssertBooksAreEqual(Book expected, Book actual, string messagePrefix = "")
        {
            if (expected == null && actual == null) return;
            Assert.IsNotNull(actual, $"{messagePrefix} Фактично знайдена книга не повинна бути null, якщо очікується книга.");
            Assert.IsNotNull(expected, $"{messagePrefix} Очікувана книга не повинна бути null, якщо фактично знайдена книга не null.");

            Assert.AreEqual(expected.Id, actual.Id, $"{messagePrefix} ID книги не співпадає.");
            Assert.AreEqual(expected.Name, actual.Name, $"{messagePrefix} Назва книги не співпадає.");
            Assert.AreEqual(expected.Author, actual.Author, $"{messagePrefix} Автор книги не співпадає.");
            Assert.AreEqual(expected.Year, actual.Year, $"{messagePrefix} Рік книги не співпадає.");
            Assert.AreEqual(expected.Shell_Number, actual.Shell_Number, $"{messagePrefix} Номер полиці не співпадає.");
            Assert.AreEqual(expected.Rack_Number, actual.Rack_Number, $"{messagePrefix} Номер стелажа не співпадає.");
        }

        [TestMethod]
        public void FindButton_Click_WhenBookExists_ShouldSetTestHookFoundBook()
        {
            string targetAuthor = "Іван Франко";
            string targetName = "Захар Беркут";
            var expectedBook = new Book(1, targetName, targetAuthor, 10, 5, 1883);

            var mockBooks = new List<Book> { expectedBook };
            var mockService = new MockBookFinderService(mockBooks);
            var targetForm = new FindBookByAuthorAndNameForm(mockService);

            targetForm.Test_AuthorInput = targetAuthor;
            targetForm.Test_NameInput = targetName;

            targetForm.FindButton_Click(null, null);

            Assert.IsNotNull(targetForm.TestHook_FoundBook, "TestHook_FoundBook не повинен бути null, якщо книга знайдена.");
            AssertBooksAreEqual(expectedBook, targetForm.TestHook_FoundBook);
            Assert.IsFalse(targetForm.TestHook_InputWarningShown, "Попередження про неправильний ввід не повинно було з'явитися.");
            Assert.IsFalse(targetForm.TestHook_ErrorShown, "Помилка бази даних не повинна була з'явитися.");
            Assert.IsFalse(targetForm.TestHook_BookNotFoundMessageShown, "Повідомлення 'книгу не знайдено' не повинно було з'явитися.");
            Assert.IsNotNull(targetForm.TestHook_SavedFileName, "Ім'я файлу для збереження мало бути встановлене (імітація виклику SaveDialog).");
        }

        [TestMethod]
        public void FindButton_Click_WhenBookDoesNotExist_ShouldSetTestHookFoundBookToNull()
        {
            string targetAuthor = "Неіснуючий Автор";
            string targetName = "Неіснуюча Книга";

            var mockBooks = new List<Book> { new Book(1, "Захар Беркут", "Іван Франко", 10, 5, 1883) };
            var mockService = new MockBookFinderService(mockBooks);
            var targetForm = new FindBookByAuthorAndNameForm(mockService);

            targetForm.Test_AuthorInput = targetAuthor;
            targetForm.Test_NameInput = targetName;

            targetForm.FindButton_Click(null, null);

            Assert.IsNull(targetForm.TestHook_FoundBook, "TestHook_FoundBook має бути null, якщо книга не знайдена.");
            Assert.IsFalse(targetForm.TestHook_InputWarningShown);
            Assert.IsFalse(targetForm.TestHook_ErrorShown);
            Assert.IsTrue(targetForm.TestHook_BookNotFoundMessageShown, "Повідомлення 'книгу не знайдено' повинно було з'явитися.");
        }

        [TestMethod]
        public void FindButton_Click_WhenAuthorIsEmpty_ShouldShowInputWarning()
        {
            var mockService = new MockBookFinderService(new List<Book>());
            var targetForm = new FindBookByAuthorAndNameForm(mockService);

            targetForm.Test_AuthorInput = "";
            targetForm.Test_NameInput = "Будь-яка назва";

            targetForm.FindButton_Click(null, null);

            Assert.IsTrue(targetForm.TestHook_InputWarningShown, "Попередження про неправильний ввід повинно було з'явитися.");
            Assert.IsNull(targetForm.TestHook_FoundBook);
            Assert.IsFalse(targetForm.TestHook_ErrorShown);
            Assert.IsFalse(targetForm.TestHook_BookNotFoundMessageShown);
        }

        [TestMethod]
        public void FindButton_Click_WhenDatabaseErrorOccurs_ShouldShowError()
        {
            string targetAuthor = "Іван Франко";
            string targetName = "Захар Беркут";

            var mockService = new MockBookFinderService(new List<Book>());
            mockService.SimulateDatabaseError = true;
            var targetForm = new FindBookByAuthorAndNameForm(mockService);

            targetForm.Test_AuthorInput = targetAuthor;
            targetForm.Test_NameInput = targetName;

            targetForm.FindButton_Click(null, null);

            Assert.IsTrue(targetForm.TestHook_ErrorShown, "Повідомлення про помилку бази даних повинно було з'явитися.");
            Assert.IsNull(targetForm.TestHook_FoundBook);
            Assert.IsFalse(targetForm.TestHook_InputWarningShown);
            Assert.IsFalse(targetForm.TestHook_BookNotFoundMessageShown);
        }
    }
}
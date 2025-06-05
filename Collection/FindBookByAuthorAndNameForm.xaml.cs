using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Collection
{
    public partial class FindBookByAuthorAndNameForm : Window
    {
        private readonly IBookFinderService _bookFinderService;

        public string Test_AuthorInput { get; set; }
        public string Test_NameInput { get; set; }
        public Book TestHook_FoundBook { get; private set; }
        public bool TestHook_InputWarningShown { get; private set; }
        public bool TestHook_ErrorShown { get; private set; }
        public bool TestHook_BookNotFoundMessageShown { get; private set; }
        public string TestHook_SavedFileName { get; private set; }

        public FindBookByAuthorAndNameForm()
        {
            InitializeComponent();
            this._bookFinderService = new MySqlBookFinderService(new DataAccess().connectionString);
        }

        public FindBookByAuthorAndNameForm(IBookFinderService bookFinderService)
        {
            InitializeComponent();
            this._bookFinderService = bookFinderService;
        }

        public TextBox GetAuthorTextBoxForTest() => AuthorTextBox;
        public TextBox GetNameTextBoxForTest() => NameTextBox;


        public void FindButton_Click(object sender, RoutedEventArgs e)
        {
            string authorInput = Test_AuthorInput ?? AuthorTextBox.Text.Trim();
            string nameInput = Test_NameInput ?? NameTextBox.Text.Trim();

            TestHook_FoundBook = null;
            TestHook_InputWarningShown = false;
            TestHook_ErrorShown = false;
            TestHook_BookNotFoundMessageShown = false;
            TestHook_SavedFileName = null;

            if (string.IsNullOrEmpty(authorInput) || string.IsNullOrEmpty(nameInput))
            {
                TestHook_InputWarningShown = true;
                return;
            }

            try
            {
                TestHook_FoundBook = _bookFinderService.FindBook(authorInput, nameInput);
            }
            catch (Exception)
            {
                TestHook_ErrorShown = true;

                return;
            }

            if (TestHook_FoundBook != null)
            {

#if !TEST_MODE
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                string safeAuthor = TestHook_FoundBook.Author.Replace(" ", "_");
                string safeName = TestHook_FoundBook.Name.Replace(" ", "_");
                saveFileDialog.FileName = $"Книга_{safeAuthor}_{safeName}";
                saveFileDialog.DefaultExt = ".docx";
                saveFileDialog.Filter = "Документ Word (*.docx)|*.docx";

                bool? result = saveFileDialog.ShowDialog();
                if (result == true)
                {
                    TestHook_SavedFileName = saveFileDialog.FileName;
                }
#else
#endif
            }
            else
            {
                TestHook_BookNotFoundMessageShown = true;
            }
        }
    }

    public class MySqlBookFinderService : IBookFinderService
    {
        private readonly string _connectionString;

        public MySqlBookFinderService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Book FindBook(string author, string name)
        {
            Book foundBook = null;

            using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT id, author, name, year, rack_number, shell_number FROM collection WHERE author = @author AND name = @name";
                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@author", author);
                    cmd.Parameters.AddWithValue("@name", name);
                    using (MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            foundBook = new Book(
                                Convert.ToInt32(reader["id"]),
                                reader["name"].ToString(),
                                reader["author"].ToString(),
                                Convert.ToInt32(reader["shell_number"]),
                                Convert.ToInt32(reader["rack_number"]),
                                Convert.ToInt32(reader["year"])
                            );
                        }
                    }
                }
            }
            return foundBook;
        }
    }
}
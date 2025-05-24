using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Collection
{
    public partial class FindBookByAuthorAndNameForm : Window
    {
        public FindBookByAuthorAndNameForm()
        {
            InitializeComponent();
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            string author = AuthorTextBox.Text.Trim();
            string name = NameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(author) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Будь ласка, введіть автора та назву книги.", "Помилка вводу", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataAccess dataAccess = new DataAccess();
            Book foundBook = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(dataAccess.connectionString))
                {
                    conn.Open();
                    string query = "SELECT id, author, name, year, rack_number, shell_number FROM collection WHERE author = @author AND name = @name";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@author", author);
                        cmd.Parameters.AddWithValue("@name", name);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                foundBook = new Book(
                                    Convert.ToInt32(reader["id"]),
                                    reader["author"].ToString(),
                                    reader["name"].ToString(),
                                    Convert.ToInt32(reader["year"]),
                                    Convert.ToInt32(reader["rack_number"]),
                                    Convert.ToInt32(reader["shell_number"])
                                );
                            }
                        }
                    }

                    if (foundBook != null)
                    {
                        string filePath = "found_books.txt";
                        using (StreamWriter sw = new StreamWriter(filePath, true))
                        {
                            sw.WriteLine($"Автор: {foundBook.Author}, Назва: {foundBook.Name}, Рік: {foundBook.Year}, Стелаж: {foundBook.Rack_Number}, Полиця: {foundBook.Shell_Number}");
                        }
                        MessageBox.Show("Інформацію записано до файлу.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Дану книгу не знайдено.", "Результат пошуку", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час пошуку книги: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
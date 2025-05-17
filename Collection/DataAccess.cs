using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Collection
{
    class DataAccess
    {
        public string connectionString;
        public List<Book> bookList = new List<Book>();
        public DataAccess()
        {
            OpenDbFile();
        }

        private void OpenDbFile()
        {
            try
            {
                connectionString = "server=127.0.0.1; port=3306; username=root; password=1234567890; database=collection";

                MySqlConnection connection = new MySqlConnection(connectionString);
                MySqlCommand command = new MySqlCommand();

                string commandString = "select * from collection";
                command.CommandText = commandString;
                command.Connection = connection;

                MySqlDataReader reader;
                command.Connection.Open();
                reader = command.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    Book book = new Book(
                                        reader.GetInt32("id"),
                                        reader.GetString("name"),
                                        reader.GetString("author"),
                                        reader.GetInt32("shell_number"),
                                        reader.GetInt32("rack_number"),
                                        reader.GetInt32("year")
                                    );
                    bookList.Add(book);
                    i += 1;
                }
                reader.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show($"Помилка бази даних: {ex.Message}{Environment.NewLine}{Environment.NewLine}" +
                                "Перевірте підключення до бази даних та правильність запиту.",
                                "Помилка MySQL", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine($"MySqlException: {ex.ToString()}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Загальна помилка: {ex.Message}{Environment.NewLine}{Environment.NewLine}" +
                                "Спробуйте перезапустити програму або зверніться до адміністратора.",
                                "Загальна помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine($"General Exception: {ex.ToString()}");
            }
        }
    }
}

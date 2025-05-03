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
        public int AttemptConnection()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=127.0.0.1; port=3306; username=root; password=1234567890; database=collection"))
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        MySqlCommand cmd = new MySqlCommand("SELECT name, author FROM collection LIMIT 5", connection);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        StringBuilder sb = new StringBuilder("Перші 5 книг у базі:\n\n");
                        while (reader.Read())
                        {
                            sb.AppendLine($"{reader["name"]} — {reader["author"]}");
                        }
                        reader.Close();

                        MessageBox.Show(sb.ToString(), "Успішне з'єднання та читання", MessageBoxButton.OK, MessageBoxImage.Information);

                        connection.Close();
                        return 1;
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося підключитися до бази даних.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine +
                                "Для завантаження файлу виконайте команду Файл-завантажити",
                                "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);

                return 0;
            }
        }
    }
}

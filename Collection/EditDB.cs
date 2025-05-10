using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Collection
{
    class EditDB
    {
        public int bookNum { get; set; }
        public bool bookAdd { get; set; }

        public void ChangeDBRow(Book bookToUpdate) // Приймає об'єкт Book для оновлення
        {
            // Зміна запису БД.
            try
            {
                MessageBox.Show($"SQL-запит: UPDATE collection SET name = '{bookToUpdate.Name}', author = '{bookToUpdate.Author}', "
    + $"shell_number = {bookToUpdate.Shell_Number}, rack_namber = {bookToUpdate.Rack_Number}, year = {bookToUpdate.Year} WHERE id = {bookToUpdate.Id}");

                using (MySqlConnection conn = new MySqlConnection(new DataAccess().connectionString))             // Ваша connectionString
                using (MySqlCommand cmd = new MySqlCommand("UPDATE collection SET author = ?author, name = ?name, " +
                                                         "year = ?year, rack_number = ?rack_number, shell_number = ?shell_number " +
                                                         "WHERE id = ?id", conn)) // SQL-запит

                {
                    // Додавання параметрів з об'єкта Book
                    cmd.Parameters.Add("?author", MySqlDbType.VarChar, 255).Value = bookToUpdate.Author;
                    cmd.Parameters.Add("?name", MySqlDbType.VarChar, 255).Value = bookToUpdate.Name;
                    cmd.Parameters.Add("?year", MySqlDbType.Int32).Value = bookToUpdate.Year;
                    cmd.Parameters.Add("?rack_number", MySqlDbType.Int32).Value = bookToUpdate.Rack_Number;
                    cmd.Parameters.Add("?shell_number", MySqlDbType.Int32).Value = bookToUpdate.Shell_Number;
                    cmd.Parameters.Add("?id", MySqlDbType.Int32).Value = bookToUpdate.Id; // Використовуємо Id книги

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    MessageBox.Show($"Кількість оновлених рядків: {rowsAffected}");
                }
            }
            catch (Exception ex)
            {
                // Кидаємо виняток далі, або обробляємо його тут, або логуємо.
                // У прикладі EditDataForm ми обробляємо його там.
                throw new Exception("Помилка виконання запиту на оновлення книги в БД.", ex);
                // Або просто:
                // MessageBox.Show(...) // Якщо обробка помилок тільки тут
            }
        }
    }
}

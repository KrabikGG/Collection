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

        public void ChangeDBRow(Book bookToUpdate)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(new DataAccess().connectionString))
                using (MySqlCommand cmd = new MySqlCommand("UPDATE collection SET author = ?author, name = ?name, " +
                                                         "year = ?year, rack_number = ?rack_number, shell_number = ?shell_number " +
                                                         "WHERE id = ?id", conn))
                {
                    cmd.Parameters.Add("?author", MySqlDbType.VarChar, 255).Value = bookToUpdate.Author;
                    cmd.Parameters.Add("?name", MySqlDbType.VarChar, 255).Value = bookToUpdate.Name;
                    cmd.Parameters.Add("?year", MySqlDbType.Int32).Value = bookToUpdate.Year;
                    cmd.Parameters.Add("?rack_number", MySqlDbType.Int32).Value = bookToUpdate.Rack_Number;
                    cmd.Parameters.Add("?shell_number", MySqlDbType.Int32).Value = bookToUpdate.Shell_Number;
                    cmd.Parameters.Add("?id", MySqlDbType.Int32).Value = bookToUpdate.Id;

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Помилка виконання запиту на оновлення книги в БД.", ex);
            }
        }

        public void AddDBRow(Book bookToAdd)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(new DataAccess().connectionString))
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO collection (author, name, year, rack_number, shell_number) " +
                                                         "VALUES (?author, ?name, ?year, ?rack_number, ?shell_number)", conn))
                {
                    cmd.Parameters.Add("?author", MySqlDbType.VarChar, 255).Value = bookToAdd.Author;
                    cmd.Parameters.Add("?name", MySqlDbType.VarChar, 255).Value = bookToAdd.Name;
                    cmd.Parameters.Add("?year", MySqlDbType.Int32).Value = bookToAdd.Year;
                    cmd.Parameters.Add("?rack_number", MySqlDbType.Int32).Value = bookToAdd.Rack_Number;
                    cmd.Parameters.Add("?shell_number", MySqlDbType.Int32).Value = bookToAdd.Shell_Number;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Помилка виконання запиту на додавання книги в БД.", ex);
            }
        }
    }
}

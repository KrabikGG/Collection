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
    public partial class FindAllBooksByYearForm : Window
    {
        private DataAccess dataAccess;

        public FindAllBooksByYearForm()
        {
            InitializeComponent();
            dataAccess = new DataAccess();
        }

        private void FindAllBooksByYearForm_Loaded(object sender, RoutedEventArgs e)
        {
            LoadYears();
        }

        private void LoadYears()
        {
            try
            {
                if (dataAccess.bookList == null || !dataAccess.bookList.Any())
                {
                    MessageBox.Show("Список книг порожній або не завантажений. Неможливо завантажити роки.", "Помилка даних", MessageBoxButton.OK, MessageBoxImage.Warning);
                    FindButton.IsEnabled = false;
                    return;
                }

                var years = dataAccess.bookList
                                    .Select(book => book.Year)
                                    .Distinct()
                                    .OrderBy(year => year)
                                    .ToList();

                if (years.Any())
                {
                    YearsListBox.ItemsSource = years;
                }
                else
                {
                    MessageBox.Show("У базі даних немає книг для відображення років", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                    FindButton.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження років: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                FindButton.IsEnabled = false;
            }
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            if (YearsListBox.SelectedItem == null)
            {
                MessageBox.Show("Виберіть рік зі списку", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dataAccess.bookList == null)
            {
                MessageBox.Show("Список книг не завантажений", "Помилка даних", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int selectedYear = (int)YearsListBox.SelectedItem;

            List<Book> booksFound = dataAccess.bookList
                                              .Where(book => book.Year == selectedYear)
                                              .ToList();

            if (booksFound.Any())
            {
                string filePath = $"Test.docx";
                try
                {
                    MessageBox.Show($"Інформацію записано до файлу: \"{System.IO.Path.GetFullPath(filePath)}\"", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при записі до файлу: {ex.Message}", "Помилка запису", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Книг з таким роком не знайдено.", "Результат пошуку", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            this.Close();
        }
    }
}
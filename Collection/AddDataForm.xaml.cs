using System;
using System.Collections.Generic;
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
    public partial class AddDataForm : Window
    {
        DataAccess DataConnection;
        public Book BookToAdd { get; set; }
        
        public AddDataForm()
        {
            InitializeComponent();
        }

        private void ConfirmAddButton_Click(object sender, RoutedEventArgs e)
        {
            int year;
            if (int.TryParse(yearTextBox.Text, out year))
            {
                BookToAdd.Year = year;
            }
            else
            {
                MessageBox.Show("Невірний формат року.", "Помилка вводу", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int rackNum;
            if (int.TryParse(numRackTextBox.Text, out rackNum))
            {
                BookToAdd.Rack_Number = rackNum;
            }
            else
            {
                MessageBox.Show("Невірний формат номера стелажу.", "Помилка вводу", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int shellNum;
            if (int.TryParse(numShellTextBox.Text, out shellNum))
            {
                BookToAdd.Shell_Number = shellNum;
            }
            else
            {
                MessageBox.Show("Невірний формат номера полиці.", "Помилка вводу", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                EditDB dbAccessor = new EditDB();
                dbAccessor.AddDBRow(BookToAdd);

                MessageBox.Show("Рядок успішно додано.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час створення запису у базі даних: {ex.Message}", "Помилка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            BookListForm BookListWnd = new BookListForm();
            BookListWnd.Show();
        }
    }
}

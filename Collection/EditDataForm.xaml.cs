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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Collection
{
    public partial class EditDataForm : Window
    {
        DataAccess DataConnection;
        public Book BookToEdit { get; set; }

        public EditDataForm()
        {
            InitializeComponent();
            this.Loaded += EditDataForm_Loaded;
        }

        private void EditDataForm_Loaded(object sender, RoutedEventArgs e)
        {
                authorTextBox.Text = BookToEdit.Author;
                nameTextBox.Text = BookToEdit.Name;
                yearTextBox.Text = BookToEdit.Year.ToString();
                numRackTextBox.Text = BookToEdit.Rack_Number.ToString();
                numShellTextBox.Text = BookToEdit.Shell_Number.ToString();
        }

        private void ConfirmEditButton_Click(object sender, RoutedEventArgs e)
        {
            BookToEdit.Author = authorTextBox.Text;
            BookToEdit.Name = nameTextBox.Text;

            int year;
            if (int.TryParse(yearTextBox.Text, out year))
            {
                BookToEdit.Year = year;
            }
            else
            {
                MessageBox.Show("Невірний формат року.", "Помилка вводу", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int rackNum;
            if (int.TryParse(numRackTextBox.Text, out rackNum))
            {
                BookToEdit.Rack_Number = rackNum;
            }
            else
            {
                MessageBox.Show("Невірний формат номера стелажу.", "Помилка вводу", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int shellNum;
            if (int.TryParse(numShellTextBox.Text, out shellNum))
            {
                BookToEdit.Shell_Number = shellNum;
            }
            else
            {
                MessageBox.Show("Невірний формат номера полиці.", "Помилка вводу", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                EditDB dbAccessor = new EditDB();
                dbAccessor.ChangeDBRow(BookToEdit);

                MessageBox.Show("Зміни успішно збережено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час збереження змін у базі даних: {ex.Message}", "Помилка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            BookListForm BookListWnd = new BookListForm();
            BookListWnd.Show();
        }
        private void ConfirmDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookToEdit == null)
            {
                MessageBox.Show("Немає книги для видалення.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Ви впевнені, що хочете видалити книгу '{BookToEdit.Name}' автора '{BookToEdit.Author}'?",
                                                      "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    EditDB dbAccessor = new EditDB();
                    dbAccessor.DeleteBook(BookToEdit.Id);

                    MessageBox.Show("Книгу успішно видалено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка під час видалення книги з бази даних: {ex.Message}", "Помилка БД", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                BookListForm BookListWnd = new BookListForm();
                BookListWnd.Show();
            }
        }
    }
}
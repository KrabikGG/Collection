using MySql.Data.MySqlClient;
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
    public partial class BookListForm : Window
    {
        DataAccess DataConnection;
        public BookListForm()
        {
            InitializeComponent();
            DataConnection = new DataAccess();
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Book addedBook = new Book(DataConnection.bookList.Count + 1, "", "", 0, 0, 0);

            try
            {
                AddDataForm AddWnd = new AddDataForm();

                AddWnd.BookToAdd = addedBook;

                bool? result = AddWnd.ShowDialog();

                if (result == true)
                {
                    RefreshBookDataGridDisplay();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при відкритті форми додавання: " + ex.Message, "Помилка");
            }
            this.Close();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Book selectedBook = BookListDG.SelectedItem as Book;

            if (selectedBook == null)
            {
                MessageBox.Show("Будь ласка, виберіть книгу для редагування.", "Попередження");
                return;
            }

            try
            {
                EditDataForm EditWnd = new EditDataForm();


                EditWnd.BookToEdit = selectedBook;

                bool? result = EditWnd.ShowDialog();

                if (result == true)
                {
                    RefreshBookDataGridDisplay();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при відкритті форми редагування: " + ex.Message, "Помилка");
            }
            this.Close();
        }

        private void RefreshBookDataGridDisplay()
        {
            BookListDG.ItemsSource = null;
            BookListDG.ItemsSource = DataConnection.bookList;
        }

        private void BookListForm_Loaded(object sender, RoutedEventArgs e)
        {
            AddButton.Visibility = Visibility.Hidden;
            EditButton.Visibility = Visibility.Hidden;

            if (Authorization.logUser == 2)
            {
                AddButton.Visibility = Visibility.Visible;
                EditButton.Visibility = Visibility.Visible;
            }
            else
            {
                AddButton.Visibility = Visibility.Hidden;
                EditButton.Visibility = Visibility.Hidden;
            }

            try
            {
                BookListDG.ItemsSource = null;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Помилка бази даних: {ex.Message}{Environment.NewLine}{Environment.NewLine}" +
                               "Перевірте підключення до бази даних та правильність запиту.",
                               "Помилка MySQL", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine($"MySqlException: {ex.ToString()}");
            }
            DataAccess DataConnection = new DataAccess();
            BookListDG.ItemsSource = DataConnection.bookList;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

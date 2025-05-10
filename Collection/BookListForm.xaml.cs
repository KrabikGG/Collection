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
            AddDataForm AddWnd = new AddDataForm();
            AddWnd.Show();
            //this.Visibility = Visibility.Collapsed;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Отримати вибраний елемент з DataGrid
            Book selectedBook = BookListDG.SelectedItem as Book;

            if (selectedBook == null)
            {
                MessageBox.Show("Будь ласка, виберіть книгу для редагування.", "Попередження");
                return; // Виходимо, якщо нічого не вибрано
            }

            this.Close();

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
                // Логування помилки
            }
        }

        // Метод для оновлення відображення DataGrid (як обговорювалося раніше)
        private void RefreshBookDataGridDisplay()
        {
            // Припускаємо, DataConnection.BookList - ваш список книг
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

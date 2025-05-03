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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Collection
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Authorization logedUser = new Authorization();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            LogInForm LogWnd = new LogInForm();
            LogWnd.Show();
            //this.Visibility = Visibility.Collapsed;
        }

        private void BookListButton_Click(object sender, RoutedEventArgs e)
        {
            BookListForm BookListWnd = new BookListForm();
            BookListWnd.Show();
            //this.Visibility = Visibility.Collapsed;
        }

        private void FindBookByAuthorAndNameButton_Click(object sender, RoutedEventArgs e)
        {
            FindBookByAuthorAndNameForm NameAuthorWnd = new FindBookByAuthorAndNameForm();
            NameAuthorWnd.Show();
            //this.Visibility = Visibility.Collapsed;
        }

        private void FindBookByYearButton_Click(object sender, RoutedEventArgs e)
        {
            FindAllBooksByYearForm YearWnd = new FindAllBooksByYearForm();
            YearWnd.Show();
            //this.Visibility = Visibility.Collapsed;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

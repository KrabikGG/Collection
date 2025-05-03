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
    /// <summary>
    /// Логика взаимодействия для BookListForm.xaml
    /// </summary>
    public partial class BookListForm : Window
    {
        public BookListForm()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddDataForm AddWnd = new AddDataForm();
            AddWnd.Show();
            //this.Visibility = Visibility.Collapsed;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditDataForm EditWnd = new EditDataForm();
            EditWnd.Show();
            //this.Visibility = Visibility.Collapsed;
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
        }
    }
}

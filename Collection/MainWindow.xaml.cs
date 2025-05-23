﻿using System;
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
    public partial class MainWindow : Window
    {
        public static Authorization logedUser = new Authorization();
        public static bool IsUserLoggedIn = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsUserLoggedIn)
            {
                MessageBox.Show("Ви вже успішно увійшли в акаунт", "Авторизація", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                LogInForm LogWnd = new LogInForm();
                LogWnd.Show();
            }
        }

        private void BookListButton_Click(object sender, RoutedEventArgs e)
        {
            BookListForm BookListWnd = new BookListForm();
            this.Close();
            BookListWnd.Show();
        }

        private void FindBookByAuthorAndNameButton_Click(object sender, RoutedEventArgs e)
        {
            FindBookByAuthorAndNameForm NameAuthorWnd = new FindBookByAuthorAndNameForm();
            NameAuthorWnd.Show();
        }

        private void FindBookByYearButton_Click(object sender, RoutedEventArgs e)
        {
            FindAllBooksByYearForm YearWnd = new FindAllBooksByYearForm();
            YearWnd.Show();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

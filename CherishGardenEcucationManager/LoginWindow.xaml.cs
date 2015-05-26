using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Mode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CherishGardenEducationManager
{

    public partial class LoginWindow : Window
    {

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            TextBox userTB = (TextBox)FindName("operatorName");
            PasswordBox passowrdPB = (PasswordBox)FindName("pwd");
            String operatorName = userTB.Text;
            String password = passowrdPB.Password;
            MemberBasic currentUser = DatabaseHelper.findOperatorUser(operatorName, password);
            if (currentUser != null)
            {
                Application.Current.Properties["currentUser"] = currentUser;
                navigateToMainWindow();
            }
            else {
                //retry login
                userTB.Text = "";
                userTB.Focus();
                passowrdPB.Password = null;
            }
        }


        private void navigateToMainWindow() {
            this.Close();

            MainWindow mainwindow = new MainWindow();
            Application.Current.MainWindow = mainwindow;
            mainwindow.ShowDialog();
        }

        private void operatorName_LostFocus(object sender, RoutedEventArgs e)
        {
            //get the current user's photo
        }
    }
}

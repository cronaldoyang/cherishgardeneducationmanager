using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Entity;
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
            Button loginBtn = (Button)sender;
            String operatorName = userTB.Text;
            String password = passowrdPB.Password;
            OperatorUser operatorUser = DatabaseHelper.findOperatorUser(operatorName);
            if (operatorUser!=null && operatorUser.password == password)
            {
                MessageBox.Show("login success");
            }
            else {
                MessageBox.Show("login failed");
            }
        }

    }
}

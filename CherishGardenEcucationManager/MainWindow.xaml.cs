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
using System.Windows.Shapes;

namespace CherishGardenEducationManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Frame contentPanelFrame;
        public MainWindow()
        {
            InitializeComponent();
            contentPanelFrame = (Frame)FindName("contentPanel");
            contentPanelFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void naviagetToAddStudentInfo(object sender, RoutedEventArgs e)
        {
            contentPanelFrame.Navigate(new StudentsInfoPage());
        }

        private void naviagetToAddTeacherInfo(object sender, RoutedEventArgs e)
        {
            TeacherInfoPage teacherInfoPage = new TeacherInfoPage();
            contentPanelFrame.Navigate(teacherInfoPage);
            teacherInfoPage.updateStausbar += teacherInfoPage_updateStausbar;
        }

        void teacherInfoPage_updateStausbar(object sender, EventArgs e)
        {
            updateStatusbar();
        }

        private void updateStatusbar() {
            TextBlock statsubarTextBlock = (TextBlock)FindName("statsubarText");
            statsubarTextBlock.Visibility = System.Windows.Visibility.Visible;
            statsubarTextBlock.Text = "Start do saving data.";
        }


    }
}

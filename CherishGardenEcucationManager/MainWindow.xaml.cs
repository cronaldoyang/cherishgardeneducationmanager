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
            MemberBasic basic = (MemberBasic)Application.Current.Properties["currentUser"];
            if (basic != null)
            {
                userNameTextBlock.Text = basic.engname;
            }
            contentPanelFrame = (Frame)FindName("contentPanel");
            contentPanelFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void naviagetToAddStudentInfo(object sender, RoutedEventArgs e)
        {
            StudentsInfoPage studentInfoPage = new StudentsInfoPage();
            contentPanelFrame.Navigate(studentInfoPage);
            studentInfoPage.updateStausbar += studentInfoPage_updateStausbar;
        }

        private void naviagetToAddTeacherInfo(object sender, RoutedEventArgs e)
        {
            TeacherInfoPage teacherInfoPage = new TeacherInfoPage();
            contentPanelFrame.Navigate(teacherInfoPage);
            teacherInfoPage.updateStausbar += teacherInfoPage_updateStausbar;
        }

        void studentInfoPage_updateStausbar(object sender, EventArgs e) {
            updateStatusbar();
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

        private void naviagetToClassesManager(object sender, RoutedEventArgs e)
        {
            ClassPage classpage = new ClassPage();
            contentPanelFrame.Navigate(classpage);
        }

        private void navigateToMakeupCourse(object sender, RoutedEventArgs e)
        {
            CourseWeek coursePage = new CourseWeek();
            contentPanelFrame.Navigate(coursePage);
        }

        private void navigateToCourseCards(object sender, RoutedEventArgs e)
        {
            CourseCards coursePage = new CourseCards();
            contentPanelFrame.Navigate(coursePage);
        }


    }
}

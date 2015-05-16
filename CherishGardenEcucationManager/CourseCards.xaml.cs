using CherishGardenEducationManager.Helper;
using CherishGardenEducationManager.Mode;
using CherishGardenEducationManager.ViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CherishGardenEducationManager
{
    /// <summary>
    /// Interaction logic for CourseCards.xaml
    /// </summary>
    public partial class CourseCards : Page
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        public CourseCards()
        {
            InitializeComponent();
            initData();
        }

        private void initData()
        {
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            allCourseCardsListView.ItemsSource = CourseCardsViewModel.getInstance().currentCourseCards;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            CourseCardsViewModel.getInstance().initData();
        }

        private void AddCourseCard_Click(object sender, RoutedEventArgs e)
        {
            CourseCardsViewModel.getInstance().addCourseCard();

            //It's a very smart function. if The user add item.
            allCourseCardsListView.Items.MoveCurrentToLast();
            allCourseCardsListView.ScrollIntoView(allCourseCardsListView.Items.CurrentItem);
        }

        //Do the logic to judge the click is long or short?
        protected async void ListViewItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            bool isLongPress = await HelperJudgeLongClick.MouseDown(e.Source as FrameworkElement, TimeSpan.FromSeconds(1));
           if (isLongPress) {
              // long press
               MessageBoxResult result = MessageBox.Show("你将删除该课程卡？",
                 "确认对话框", MessageBoxButton.YesNo);
               if (result == MessageBoxResult.Yes)
               {
                   CourseCard item = (CourseCard)allCourseCardsListView.SelectedValue;
                   CourseCardsViewModel.getInstance().removeCourseCard(item);
               }
               else if (result == MessageBoxResult.No)
               {
                   //do nothing.
               }
           } else {
            //short press--> do nothing.
           }
        }

        private void SaveCourseCards_Click(object sender, RoutedEventArgs e)
        {
            CourseCardsViewModel.getInstance().saveCurrentCourseCards();
        }


    }
}

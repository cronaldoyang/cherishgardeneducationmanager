using CherishGardenEducationManager.Helper;
using CherishGardenEducationManager.Mode;
using CherishGardenEducationManager.ViewModel;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CherishGardenEducationManager
{
    /// <summary>
    /// Interaction logic for CourseCards.xaml
    /// </summary>
    public partial class CourseCards : Page
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private bool mFirstBindClass = true;
        private bool mFirstBindCourse = true;
        private CourseCard currentCourseCard;
        private bool mHasShowedClassAlertMessage;
        private bool mHasShowedCourseAlertMessage;

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
            ClassesBox.ItemsSource = CourseCardsViewModel.getInstance().classes;
            CoursesBox.ItemsSource = CourseCardsViewModel.getInstance().courseGroups;
            ClassesBox.SelectedIndex = CourseCardsViewModel.getInstance().getClassIndexById();
            CoursesBox.SelectedIndex = 0;
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
            allCourseCardsListView.ItemsSource = CourseCardsViewModel.getInstance().currentCourseCards;
            allCourseCardsListView.Items.MoveCurrentToLast();
            allCourseCardsListView.ScrollIntoView(allCourseCardsListView.Items.CurrentItem);
        }

        //Do the logic to judge the click is long or short?
        protected async void ListViewItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           bool isLongPress = await HelperJudgeLongClick.MouseDown(e.Source as FrameworkElement, TimeSpan.FromSeconds(1));
           if (isLongPress) {
              // long press
               MessageBoxResult result = MessageBox.Show("你将删除如下课程卡：" + "\n" + currentCourseCard.time + "\n" + currentCourseCard.name,
                 "确认对话框", MessageBoxButton.YesNo);
               if (result == MessageBoxResult.Yes)
               {
                   CourseCardsViewModel.getInstance().removeCourseCard(currentCourseCard);
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

        private void ClassesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //If the user has data need to be saved. we should alert the user.
            if (mFirstBindClass)
            {
                mFirstBindClass = false;
            }
            else
            {
                if (CourseCardsViewModel.getInstance().currentCourseCardsContentHasChanged())
                {
                    if (!mHasShowedClassAlertMessage)
                    {
                        showAlertClassMessaageToUser();//don't care the result.
                        ClassesBox.SelectedIndex = CourseCardsViewModel.getInstance().getClassIndexById();
                    }
                }
                else
                {
                    CourseCardsViewModel.getInstance().selectedClassid = ((int)ClassesBox.SelectedValue);
                    CourseCardsViewModel.getInstance().refreshCourseGroupsData();
                    CoursesBox.ItemsSource = CourseCardsViewModel.getInstance().courseGroups;
                    CoursesBox.SelectedIndex = 0;
                    allCourseCardsListView.ItemsSource = CourseCardsViewModel.getInstance().currentCourseCards;
                }
            }
            mHasShowedClassAlertMessage = false;
        }

        private void CoursesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //If the user has data need to be saved. we should alert the user.
            if (mFirstBindCourse)
            {
                mFirstBindCourse = false;
            }
            else
            {
                if (CourseCardsViewModel.getInstance().currentCourseCardsContentHasChanged())
                {
                    if (!mHasShowedCourseAlertMessage)
                    {
                        showAlertCourseMessaageToUser();
                        CoursesBox.SelectedIndex = CourseCardsViewModel.getInstance().getCourseIndexById();
                    }                    
                }
                else
                {
                    CourseCardsViewModel.getInstance().selectedCourseid = ((int)CoursesBox.SelectedValue);
                    CourseCardsViewModel.getInstance().refreshCourseCards();
                    allCourseCardsListView.ItemsSource = CourseCardsViewModel.getInstance().currentCourseCards;
                }
            }
            mHasShowedCourseAlertMessage = false;
        }

        private MessageBoxResult showAlertCourseMessaageToUser()
        {
            mHasShowedCourseAlertMessage = true;
            return  MessageBox.Show("你已经修改了相关内容，请保存数据后再进行操作?",
                "Confirmation", MessageBoxButton.OK);
        }

        private MessageBoxResult showAlertClassMessaageToUser()
        {
            mHasShowedClassAlertMessage = true;
            return MessageBox.Show("你已经修改了相关内容，请保存数据后再进行操作?",
                "Confirmation", MessageBoxButton.OK);
        }

        private void name_textchanged(object sender, TextChangedEventArgs e)
        {
            //current item name conten has changed.
            currentCourseCard.contentChanged = true;
        }

        private void targets_textchanged(object sender, TextChangedEventArgs e)
        {
            currentCourseCard.contentChanged = true;
        }

        private void teachingplan_textchanged(object sender, TextChangedEventArgs e)
        {
            currentCourseCard.contentChanged = true;
        }

        private void materias_textchanged(object sender, TextChangedEventArgs e)
        {
            currentCourseCard.contentChanged = true;
        }

        private void mark_textchanged(object sender, TextChangedEventArgs e)
        {
            currentCourseCard.contentChanged = true;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentCourseCard = (CourseCard)allCourseCardsListView.SelectedItem;
        }
    }
}

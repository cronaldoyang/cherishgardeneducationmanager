using CherishGardenEducationManager.Mode;
using CherishGardenEducationManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CherishGardenEducationManager
{
    /// <summary>
    /// Interaction logic for CurriculumSchedule.xaml
    /// </summary>
    public partial class CourseWeek : Page
    {
        private readonly BackgroundWorker workerForInitData = new BackgroundWorker();

        private TextBox mCurTextBox;
        private int mEditingRow;
        private int mEditingColumn;
        private bool mFirstEnter = true;


        public CourseWeek()
        {
            InitializeComponent();
            initData();
        }

        private void initData()
        {
            workerForInitData.DoWork += worker_initData;
            workerForInitData.RunWorkerCompleted += worker_initDataCompleted;
            workerForInitData.RunWorkerAsync();
        }

        public void worker_initData(object sender, DoWorkEventArgs e)
        {
            //Do the logic to judge init from database or newMaker.
            //CourseWeekViewModel.getInstance().worker_initDataFromDatabase();
            CourseWeekViewModel.getInstance().worker_initDataFromDatabase(false);
        }

        void worker_initDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Bind Data to UI.
            weeknoTextBlock.Text = "第" + CourseWeekViewModel.getInstance().courseWeekNoForDisplay + "周课程表";
            GradesBox.ItemsSource = ClassViewModel.getInstance().allGrades;
            GradesBox.SelectedIndex = ClassViewModel.getInstance().getGradeIndexById(CourseWeekViewModel.getInstance().selectedGradeId);
            bindDataToUI();
        }

        void bindDataToUI()
        {
            int rowCount = CourseWeekGrid.RowDefinitions.Count;
            int columnCount = CourseWeekGrid.ColumnDefinitions.Count;
            for (int row = 1; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    string controlName = "TextBox" + row + column;
                    TextBox curTextBox = (TextBox)FindName(controlName);
                    curTextBox.Text = getJieCiDescription(row, column);
                }
            }
        }

        void refreshData()
        {
            //TODO should be run in thread.
            CourseWeekViewModel.getInstance().worker_reloadOneWeekCourseDataFromDatabase();
            bindDataToUI();
        }

       
        //The user start to edit current cell's content.
        private void showPopupWindow(object sender, MouseButtonEventArgs e)
        {

            mCurTextBox = (TextBox)sender;
            mEditingRow = Grid.GetRow(e.Source as UIElement);
            mEditingColumn = Grid.GetColumn(e.Source as UIElement);
            Application.Current.Properties["OneWeekCourseEditingColumn"] = mEditingColumn;
            Application.Current.Properties["OneWeekCourseEditingRow"] = mEditingRow;
            ChooseCourseDetailWindow popUp = new ChooseCourseDetailWindow();
            popUp.Closed += popUp_Closed;
            popUp.ShowDialog();
        }

        void popUp_Closed(object sender, EventArgs e)
        {
            //update the UI.
            mCurTextBox.Text = getJieCiDescription(mEditingRow,mEditingColumn);
        }

        private string getJieCiDescription(int row, int column)
        {

            CourseWeekItem item = CourseWeekViewModel.getInstance().getCourseWeekItemByJieCi(row, column);
            if (item != null)
            {
                return CourseWeekViewModel.getInstance().getCourseGroupById(item.coursegroupid).courseName + "\n" +
                    item.contentdesc + "\n" +
                    ClassViewModel.getInstance().getTeacherById(item.teacherid).name + "\n" +
                    CourseWeekViewModel.getInstance().getLocationById(item.locationid).location + "\n";
            }
            else
            {
                return "";
            }
        }

        private void saveDataToDBBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseWeekViewModel.getInstance().saveOneWeekCourseData();
        }

        private void GradesBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //If the user has data need to be saved. we should alert the user.
            if (mFirstEnter)
            {
                mFirstEnter = false;
            }
            else
            {
                if (CourseWeekViewModel.getInstance().contentHasChanged)
                {
                    showAlertMessaageToUser();
                }
                else
                {
                    CourseWeekViewModel.getInstance().selectedGradeId = ((int)GradesBox.SelectedValue);
                    CourseWeekViewModel.getInstance().clealrOneWeekCourse();
                    refreshData();
                }
            }

        }

        private void showAlertMessaageToUser()
        {
            MessageBoxResult result = MessageBox.Show("你已经修改了相关内容，请确认是否保存数据?",
                "Confirmation", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                // Yes code here
                CourseWeekViewModel.getInstance().selectedGradeId = ((int)GradesBox.SelectedValue);
                CourseWeekViewModel.getInstance().saveOneWeekCourseData();//TODO should be in thread.
                CourseWeekViewModel.getInstance().clealrOneWeekCourse();
                refreshData();
            }
            else if (result == MessageBoxResult.No)
            {
                // No code here
                CourseWeekViewModel.getInstance().selectedGradeId  = ((int)GradesBox.SelectedValue);
                CourseWeekViewModel.getInstance().clealrOneWeekCourse();
                refreshData();
            }
            else
            {
                // Cancel code here, do nothing.
                GradesBox.SelectedIndex = ClassViewModel.getInstance().getGradeIndexById(CourseWeekViewModel.getInstance().selectedGradeId);
            }
        }

    }
}

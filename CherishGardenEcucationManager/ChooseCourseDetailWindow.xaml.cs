using CherishGardenEducationManager.Entity;
using CherishGardenEducationManager.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace CherishGardenEducationManager
{
    /// <summary>
    /// Interaction logic for ChooseCourseDetailWindow.xaml
    /// </summary>
    public partial class ChooseCourseDetailWindow : Window
    {
        CourseWeekItem mCourseWeekItem;
        int mEditingColumn = -1;
        int mEditingRow = -1;
        ObservableCollection<MemberBasic> mAllTeachers;
        ObservableCollection<CourseGroup> mAllCourseGroups;
        ObservableCollection<CourseLocation> mAllCourseLocations;

        public ChooseCourseDetailWindow()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            //Fetch data from memory.
            mEditingColumn = (int)Application.Current.Properties["OneWeekCourseEditingColumn"];
            mEditingRow = (int)Application.Current.Properties["OneWeekCourseEditingRow"];
            mCourseWeekItem = CourseWeekViewModel.getInstance().getCourseWeekItemByJieCi(mEditingRow,mEditingColumn);

            //Start to give related value to collection.
            mAllTeachers = CourseWeekViewModel.getInstance().allTeachers;
            mAllCourseGroups = CourseWeekViewModel.getInstance().allCourseGroup;
            mAllCourseLocations = CourseWeekViewModel.getInstance().allCourseLocations;
            courseTeacherComboBox.ItemsSource = mAllTeachers;
            courseGroupComboBox.ItemsSource = mAllCourseGroups;
            courseLocationComboBox.ItemsSource = mAllCourseLocations;

            if (mCourseWeekItem != null)
            {
                //Means modify data.
                courseTeacherComboBox.SelectedIndex = CourseWeekViewModel.getInstance().getTeacherIndexById(mCourseWeekItem.teacherid);
                courseGroupComboBox.SelectedIndex = CourseWeekViewModel.getInstance().getCourseGroupIndexById(mCourseWeekItem.coursegroupid);
                courseLocationComboBox.SelectedIndex = CourseWeekViewModel.getInstance().getCourseLocationIndexById(mCourseWeekItem.locationid);
                contentDescTextBox.Text = mCourseWeekItem.contentdesc;
            }
            else
            {
                //It means we should create the new one.
                mCourseWeekItem = new CourseWeekItem();
                mCourseWeekItem.gradeid = CourseWeekViewModel.getInstance().selectedGradeId;
                mCourseWeekItem.weekno = CourseWeekViewModel.getInstance().CourseWeekNoForDB;
                mCourseWeekItem.weekday = mEditingColumn + 1;
                mCourseWeekItem.jieci = (mEditingColumn + 1) * 10 + mEditingRow;
            }
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            //string resultStr = "科  目：" + courseGroupComboBox.SelectedValue + "\n任课教师：" + courseTeacherComboBox.SelectedValue + "\n上课地点：" + courseLocationComboBox.SelectedValue;
            
            //bind the new data.
            mCourseWeekItem.coursegroupid =(int)courseGroupComboBox.SelectedValue;
            mCourseWeekItem.teacherid = (int)courseTeacherComboBox.SelectedValue;
            mCourseWeekItem.locationid = (int)courseLocationComboBox.SelectedValue;
            mCourseWeekItem.contentdesc = contentDescTextBox.Text;

            //Save data into memory.
            bool isNeedToSaveInMemory = CourseWeekViewModel.getInstance().isNeedTOSaveInMemoryByJieCi(mEditingRow, mEditingColumn);
            if (mCourseWeekItem.id==-1 && isNeedToSaveInMemory)
            {
                CourseWeekViewModel.getInstance().saveCourseWeekItemInMemory(mCourseWeekItem);
            }

            //Close the window.
            Close();
        }

       
    }
}

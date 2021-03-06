﻿using CherishGardenEducationManager.Mode;
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
            courseTeacherComboBox.ItemsSource = ClassViewModel.getInstance().allTeachers;
;
            courseGroupComboBox.ItemsSource = CourseWeekViewModel.getInstance().allCourseGroup;
            courseLocationComboBox.ItemsSource = CourseWeekViewModel.getInstance().allCourseLocations;

            if (mCourseWeekItem != null)
            {
                //Means modify data.
                courseTeacherComboBox.SelectedIndex = ClassViewModel.getInstance().getTeacherIndexById(mCourseWeekItem.teacherid);
                courseGroupComboBox.SelectedIndex = CourseWeekViewModel.getInstance().getCourseGroupIndexById(mCourseWeekItem.coursegroupid);
                courseLocationComboBox.SelectedIndex = CourseWeekViewModel.getInstance().getCourseLocationIndexById(mCourseWeekItem.locationid);
                contentDescTextBox.Text = mCourseWeekItem.contentdesc;
            }
            else
            {
                //It means we should create the new one.
                mCourseWeekItem = new CourseWeekItem();
                mCourseWeekItem.gradeid = CourseWeekViewModel.getInstance().selectedClassId;
                mCourseWeekItem.weekno = CourseWeekViewModel.getInstance().courseWeekNoForDB;
                mCourseWeekItem.weekday = mEditingColumn + 1;
                mCourseWeekItem.jieci = (mEditingColumn + 1) * 10 + mEditingRow;

                //Bind the initial comboBox value.
                courseTeacherComboBox.SelectedIndex = ClassViewModel.getInstance().getTeacherIndexById(CourseWeekViewModel.getInstance().defaultTeacherId) ;
                courseLocationComboBox.SelectedIndex = CourseWeekViewModel.getInstance().getCourseLocationIndexById(CourseWeekViewModel.getInstance().defaultLocationId);
            }
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            int oldCourseGroupId = mCourseWeekItem.coursegroupid;
            int oldCourseTeacherId = mCourseWeekItem.teacherid;
            int oldCourseLocationId = mCourseWeekItem.locationid;
            string oldCouseContentDesc = mCourseWeekItem.contentdesc;
            
            //bind the new data.
            if (courseGroupComboBox.SelectedValue != null)
            {
                mCourseWeekItem.coursegroupid = (int)courseGroupComboBox.SelectedValue;
            }
            mCourseWeekItem.teacherid = (int)courseTeacherComboBox.SelectedValue;
            mCourseWeekItem.locationid = (int)courseLocationComboBox.SelectedValue;
            mCourseWeekItem.contentdesc = contentDescTextBox.Text;

            if((mCourseWeekItem.id!=-1 &&( oldCourseGroupId!= mCourseWeekItem.coursegroupid || 
                oldCourseTeacherId!=mCourseWeekItem.teacherid || 
                oldCourseLocationId!=mCourseWeekItem.locationid ||
                !oldCouseContentDesc.Equals(mCourseWeekItem.contentdesc))))
            {
                //This means has modified the original data.
                CourseWeekViewModel.getInstance().contentHasChanged = true;
            }

            //Save new data into memory.
            if (mCourseWeekItem.id==-1 && CourseWeekViewModel.getInstance().isNeedTOSaveInMemoryByJieCi(mEditingRow, mEditingColumn))
            {
                //this means we have added new data.
                CourseWeekViewModel.getInstance().saveCourseWeekItemInMemory(mCourseWeekItem);
                CourseWeekViewModel.getInstance().contentHasChanged = true;
            }


            //Close the window.
            Close();
        }
       
    }
}

using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Mode;
using CherishGardenEducationManager.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CherishGardenEducationManager.ViewModel
{
    public class CourseWeekViewModel
    {
        private static volatile CourseWeekViewModel instance;
        public int selectedGradeId = -1;
        public int courseWeekNoForDisplay = 0;
        public int courseWeekNoForDB = -1;
        public bool contentHasChanged = false;

        //Helper for Thread Safety
        private static object m_lock = new object();
        public bool mIsInitialized = false;
        public bool mIsInitializedCourseGroupsAndLocations = false;
        public ObservableCollection<CourseWeekItem> oneWeekCourseWeekItems { get; set; }
        public Hashtable mHasSavedInMemoryHashTable;
        public ObservableCollection<CourseGroup> allCourseGroup { get; set; }
        public ObservableCollection<CourseLocation> allCourseLocations { get; set; }

        public static CourseWeekViewModel getInstance()
        {
            // DoubleLock
            if (instance == null)
            {
                lock (m_lock)
                {
                    if (instance == null)
                    {
                        instance = new CourseWeekViewModel();
                    }
                }
            }
            return instance;
        }

        public void worker_initDataFromDatabase(bool newMakeCourseSchedule)
        {
            initWeekNo();

            //Judge ClassViewModel is initilized to get all teachers, all grades and all classes.
            if (!ClassViewModel.getInstance().mIsInitialized)
            {
                ClassViewModel.getInstance().worker_initData();
            }
            // load course groups and locations.
            justInitCourseGroupsAndLocations();

            //init selected grade id.
            MemberBasic currentUser = (MemberBasic)Application.Current.Properties["currentUser"];
            if (currentUser != null)
            {
                //set selectedGradeId;
                int basicId = currentUser.id;
                selectedGradeId = DatabaseHelper.getGradeIdByTeacherId(basicId);
            }
            if (selectedGradeId == -1)
            {
                selectedGradeId = ClassViewModel.getInstance().allGrades[0].id;
            }
            //1415211  14~15代表学年，2代表第二学期，11代表第十一周
            if (newMakeCourseSchedule)
            {
                oneWeekCourseWeekItems = new ObservableCollection<CourseWeekItem>();
            }
            else
            {
                oneWeekCourseWeekItems = DatabaseHelper.getOneWeekCourseWeekItems(selectedGradeId ,courseWeekNoForDB);
            }
            mHasSavedInMemoryHashTable = new Hashtable();
            mIsInitialized = true;
        }

        public void justInitCourseGroupsAndLocations()
        {
            if (!mIsInitializedCourseGroupsAndLocations)
            {
                allCourseLocations = DatabaseHelper.getallCourseLocations();
                allCourseGroup = DatabaseHelper.getallCourseGroups();
                mIsInitializedCourseGroupsAndLocations = true;
            }
        }

        public void worker_reloadOneWeekCourseDataFromDatabase()
        {
            oneWeekCourseWeekItems = DatabaseHelper.getOneWeekCourseWeekItems(selectedGradeId, courseWeekNoForDB);
        }

        private void initWeekNo()
        {
            string currentYearStr = DateTime.Now.ToString("yyyy");
            string monthStr = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");

            int currentTerm = 1;
            int termStartYear = Int32.Parse(currentYearStr);
            int termStartDay = 1;
            int termStartMonth = 1;
            int currentYear = Int32.Parse(currentYearStr.Substring(2, 2));//2014-->14
            int schoolYear = -1;
            int month = -1;
            if (monthStr.StartsWith("0"))
            {
                int index = monthStr.IndexOf("0");
                string realmonth = monthStr.Substring(1, 1);
                month = System.Int32.Parse(realmonth);
            }
            else
            {
                month = System.Int32.Parse(monthStr);
            }

            if (month >= 3 && month < 7)
            {
                currentTerm = 2;
                schoolYear = (currentYear - 1) * 100 + currentYear;
                termStartMonth = 3;
            }
            else if (month >= 7 && month < 9)
            {
                currentTerm = 3;
                schoolYear = (currentYear - 1) * 100 + currentYear;
                termStartMonth = 7;
            }
            else
            {
                currentTerm = 1;
                schoolYear = currentYear * 100 + (currentYear + 1);
                termStartMonth = 9;
            }

            schoolYear = schoolYear * 10 + currentTerm;


            DateTime oldDate = new DateTime(termStartYear, termStartMonth, termStartDay);
            DateTime newDate = DateTime.Now;
            // Difference in days, hours, and minutes. 
            TimeSpan ts = newDate - oldDate;
            // Difference in days. 
            int differenceInDays = ts.Days;
            courseWeekNoForDisplay = differenceInDays / 7 + 1;
            courseWeekNoForDB = schoolYear * 100 + courseWeekNoForDisplay;
        }


        public void saveCourseWeekItemInMemory(CourseWeekItem newItem)
        {
            bool isFound = false;
            foreach (CourseWeekItem item in oneWeekCourseWeekItems)
            {
                if (item.jieci == newItem.jieci)
                {
                    //we should replace with the new item.
                    isFound = true;
                    int position = oneWeekCourseWeekItems.IndexOf(item);
                    oneWeekCourseWeekItems.RemoveAt(position);
                    oneWeekCourseWeekItems.Add(newItem);
                }
            }

            if (!isFound)
            {
                oneWeekCourseWeekItems.Add(newItem);
                mHasSavedInMemoryHashTable.Add(newItem.jieci, newItem.jieci);
            }
        }

        //save all oneweekCourseItems.
        public void saveOneWeekCourseData()
        {
            ObservableCollection<CourseWeekItem> newOneWeekCourseItems = new ObservableCollection<CourseWeekItem>();
            ObservableCollection<CourseWeekItem> OldOneWeekCourseItems = new ObservableCollection<CourseWeekItem>();
            foreach (CourseWeekItem coureWeekItem in oneWeekCourseWeekItems)
            {
                if (coureWeekItem.id == -1)
                {
                    newOneWeekCourseItems.Add(coureWeekItem);
                }
                else
                {
                    OldOneWeekCourseItems.Add(coureWeekItem);
                }
            }
            DatabaseHelper.saveOneWeekCourseItems(OldOneWeekCourseItems, newOneWeekCourseItems);
            //TODO do we need refresh the list?
        }
        public CourseLocation getLocationByName(string name)
        {
            foreach (CourseLocation tempLocation in allCourseLocations)
            {
                if (tempLocation.location.Equals(name))
                {
                    return tempLocation;
                }
            }
            return null;
        }

       

        public CourseLocation getLocationById(int id)
        {
            foreach (CourseLocation tempLocation in allCourseLocations)
            {
                if (tempLocation.id == id)
                {
                    return tempLocation;
                }
            }
            return null;
        }

        public CourseGroup getCourseGroupByName(string name)
        {
            foreach (CourseGroup courseGroupTemp in allCourseGroup)
            {
                if (courseGroupTemp.courseName.Equals(name))
                {
                    return courseGroupTemp;
                }
            }
            return null;
        }

        public CourseGroup getCourseGroupById(int id)
        {
            foreach (CourseGroup courseGroupTemp in allCourseGroup)
            {
                if (courseGroupTemp.id == id)
                {
                    return courseGroupTemp;
                }
            }
            return null;
        }

        public int getCourseGroupIndexById(int id)
        {
            int index = 0;
            foreach (CourseGroup courseGroup in allCourseGroup)
            {
                if (courseGroup.id == id) { return index; }
                index++;
            }
            return index;
        }


        public CourseWeekItem getCourseWeekItemByJieCi(int row, int column)
        {
            int jieci = getJieCiByRowAndColumn(row, column);
            foreach (CourseWeekItem item in oneWeekCourseWeekItems)
            {
                if (item.jieci == jieci) return item;
            }
            return null;
        }

        public int getJieCiByRowAndColumn(int row, int column)
        {
            return (column + 1) * 10 + row;
        }

       

        public int getCourseLocationIndexById(int id)
        {
            int index = 0;
            foreach (CourseLocation location in allCourseLocations)
            {
                if (location.id == id) { return index; }
                index++;
            }
            return index;
        }

        public bool isNeedTOSaveInMemoryByJieCi(int row, int column)
        {
            int jieci = getJieCiByRowAndColumn(row, column);
            return !mHasSavedInMemoryHashTable.ContainsKey(jieci);
        }

        public void clealrOneWeekCourse()
        {
            contentHasChanged = false;
            oneWeekCourseWeekItems.Clear();
            mHasSavedInMemoryHashTable.Clear();
        }
    }
}

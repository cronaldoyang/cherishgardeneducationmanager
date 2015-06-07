using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Mode;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CherishGardenEducationManager.ViewModel
{
    class WeeklyReportViewModel
    {
        private static object m_lock = new object();
        private static volatile WeeklyReportViewModel instance;

        public string schoolYearForDisplay;
        public int weekNoForDisplay;
        public int weekNoForDB;
        public int selectedClassId = -1;

        public ObservableCollection<CourseGroup> allCourseGroups = new ObservableCollection<CourseGroup>();
        public ObservableCollection<WeeklyReportItem> weeklyReportLiveItems = new ObservableCollection<WeeklyReportItem>();
        public ObservableCollection<WeeklyReportItem> weeklyReportDuoYuanItems = new ObservableCollection<WeeklyReportItem>();
        public ObservableCollection<WeeklyReportItem> weeklyReportEvaluationItems = new ObservableCollection<WeeklyReportItem>();
        public ObservableCollection<WeeklyReportItem> weeklyReportRiskItems = new ObservableCollection<WeeklyReportItem>();
        public ObservableCollection<WeeklyReportItem> weeklyReportFamilyAndSchollEduItems = new ObservableCollection<WeeklyReportItem>();

        public static WeeklyReportViewModel getInstance()
        {
            // DoubleLock
            if (instance == null)
            {
                lock (m_lock)
                {
                    if (instance == null)
                    {
                        instance = new WeeklyReportViewModel();
                    }
                }
            }
            return instance;
        }

        public void initData()
        {
            initWeekNo();
            //Judge ClassViewModel is initilized to get all teachers, all grades and all classes.
            if (!ClassViewModel.getInstance().mIsInitialized)
            {
                ClassViewModel.getInstance().worker_initData();
            }
            //init selected grade id.
            MemberBasic currentUser = (MemberBasic)Application.Current.Properties["currentUser"];
            if (currentUser != null)
            {
                //set selectedGradeId;
                int basicId = currentUser.id;
                selectedClassId = DatabaseHelper.getClassIdByTeacherId(basicId);
            }
            if (selectedClassId == -1)
            {
                selectedClassId = ClassViewModel.getInstance().allClasses[0].id;
            }

            //init courseGroups.
            allCourseGroups = DatabaseHelper.getallCourseGroups();
            readWeeklyReportItemsFromDataBase(-1);
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
                schoolYear = (currentYear - 1) * 100 + currentYear;//1415
                schoolYearForDisplay = (termStartYear -1) + "~" + termStartYear;
                termStartMonth = 3;
            }
            else if (month >= 7 && month < 9)
            {
                currentTerm = 3;
                schoolYear = (currentYear - 1) * 100 + currentYear;
                schoolYearForDisplay = (termStartYear - 1) + "~" + termStartYear;
                termStartMonth = 7;
            }
            else
            {
                currentTerm = 1;
                schoolYear = currentYear * 100 + (currentYear + 1);
                schoolYearForDisplay = termStartYear + "~" + (termStartYear + 1);
                termStartMonth = 9;
            }

            schoolYear = schoolYear * 10 + currentTerm;
            schoolYearForDisplay += "第" + currentTerm + "学期";


            DateTime oldDate = new DateTime(termStartYear, termStartMonth, termStartDay);
            DateTime newDate = DateTime.Now;
            // Difference in days, hours, and minutes. 
            TimeSpan ts = newDate - oldDate;
            // Difference in days. 
            int differenceInDays = ts.Days;
            weekNoForDisplay = differenceInDays / 7 + 1;
            weekNoForDB = schoolYear * 100 + weekNoForDisplay;
        }

        public void readWeeklyReportItemsFromDataBase(int stuid)
        {
             ObservableCollection<WeeklyReportItem> weeklyReportItems = DatabaseHelper.getWeeklyReportItems(weekNoForDB, selectedClassId, stuid);
            CourseGroup cg = null;
            foreach (WeeklyReportItem item in weeklyReportItems)
            {
                cg = findCourseGroupById(item.courseid);
                if (cg != null)
                {
                    item.category = cg.category;
                    item.coursename = cg.courseName;

                }
                if (item.category.Equals("多元智能"))
                {
                    weeklyReportDuoYuanItems.Add(item);
                }
                else if (item.category.Equals("生活"))
                {
                    weeklyReportLiveItems.Add(item);
                }
                else if (item.category.Equals("风险事故"))
                {
                    weeklyReportRiskItems.Add(item);
                }
                else if (item.category.Equals("一周综合评价"))
                {
                    weeklyReportEvaluationItems.Add(item);
                }
                else if (item.category.Equals("家园合作建议"))
                {
                    weeklyReportFamilyAndSchollEduItems.Add(item);
                }
            }
        }

        public CourseGroup findCourseGroupById(int courseid)
        {
            foreach (CourseGroup cg in allCourseGroups)
            {
                if (cg.id == courseid) return cg;
            }
            return null;
        }
    }
}

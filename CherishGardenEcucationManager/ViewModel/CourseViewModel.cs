using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CherishGardenEducationManager.ViewModel
{
    class CourseViewModel
    {
        private static volatile CourseViewModel instance;

        public static CourseViewModel getInstance()
        {
            // DoubleLock
            if (instance == null)
            {
                lock (m_lock)
                {
                    if (instance == null)
                    {
                        instance = new CourseViewModel();
                    }
                }
            }
            return instance;
        }

        //Helper for Thread Safety
        private static object m_lock = new object();

        public ObservableCollection<MemberBasic> allTeachers { get; set; }
        public ObservableCollection<Class> allClasses { get; set; }
        public ObservableCollection<Course> weekCourses { get; set; }

        public void worker_initData()
        {
            allTeachers = DatabaseHelper.getAllTeachers();
            allClasses = DatabaseHelper.getAllClasses();
            weekCourses = DatabaseHelper.getWeekCoursesByDate();
        }
    }
}

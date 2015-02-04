using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CherishGardenEducationManager.ViewModel
{
    class ClassViewModel
    {
        private static volatile ClassViewModel instance;

        public static ClassViewModel getInstance()
        {
            // DoubleLock
            if (instance == null)
            {
                lock (m_lock)
                {
                    if (instance == null)
                    {
                        instance = new ClassViewModel();
                    }
                }
            }
            return instance;
        }

        //Helper for Thread Safety
        private static object m_lock = new object();

        public ObservableCollection<MemberBasic> allTeachers { get; set; }
        public ObservableCollection<Grade> allGrades { get; set; }
        public ObservableCollection<Class> allClasses { get; set; }

        public  void worker_initData()
        {
            allGrades = DatabaseHelper.getAllGrades();
            allTeachers = DatabaseHelper.getAllTeachers();
            allClasses = DatabaseHelper.getAllClasses();
        }

        public  string getTeacherNameByid(int id)
        {
            string teachername = "";
            foreach (MemberBasic teacher in allTeachers)
            {
                if (teacher.id == id)
                {
                    teachername = teacher.name;
                    break;
                }
            }
            return teachername;
        }

        public  string getGradeNameByid(int id)
        {
            string gradename = "";
            foreach (Grade grade in allGrades)
            {
                if (grade.id == id)
                {
                    gradename = grade.name;
                    break;
                }
            }
            return gradename;
        }

        public  int getTeacherIdByName(string  name)
        {
            int teacherid = -1;
            foreach (MemberBasic teacher in allTeachers)
            {
                if (teacher.name.Equals(name))
                {
                    teacherid = teacher.id;
                    break;
                }
            }
            return teacherid;
        }

        public void addNewClassRecord()
        {
            Class newClass = new Class();
            newClass.candidateGrades = allGrades;
            newClass.candidateTeachers = allTeachers;
            allClasses.Add(newClass);
        }

        public void addNewGradeRecord()
        {
            allGrades.Add(new Grade());
        }

        public void saveAllClassesData()
        {
            ObservableCollection<Class> newAddedClasses = new ObservableCollection<Class>();
            ObservableCollection<Class> OldClasses = new ObservableCollection<Class>();
            foreach (Class _class in allClasses)
            {
                if (_class.name.Equals("")) continue;
                if (_class.id == -1)
                {
                    newAddedClasses.Add(_class);
                }
                else
                {
                    OldClasses.Add(_class);
                }
            }
            DatabaseHelper.SaveClassesInfo(OldClasses, newAddedClasses);
            
            //refresh all classes data from databaseHelper.
            allClasses = DatabaseHelper.getAllClasses();
        }

        public void saveAllGradesData()
        {
            ObservableCollection<Grade> newAddedGrades = new ObservableCollection<Grade>();
            ObservableCollection<Grade> oldGrades = new ObservableCollection<Grade>();
            foreach (Grade grade in allGrades)
            {
                if (grade.name.Equals("")) continue;
                if (grade.id == -1)
                {
                    //this means new add grade.
                    newAddedGrades.Add(grade);
                }
                else
                {
                    oldGrades.Add(grade);
                }
            }
            DatabaseHelper.SaveGradesInfo(oldGrades, newAddedGrades);
            //reload gradesData from database;
            allGrades = DatabaseHelper.getAllGrades();
            //refresh all grades.
            foreach (Class _class in allClasses)
            {
                _class.candidateGrades = allGrades;
            }
        }
    }
}

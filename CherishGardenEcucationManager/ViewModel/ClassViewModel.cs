using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Mode;
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
        public bool mIsInitialized = false;

        public ObservableCollection<MemberBasic> allTeachers { get; set; }
        public ObservableCollection<Grade> allGrades { get; set; }
        public ObservableCollection<Class> allClasses { get; set; }

        public  void worker_initData()
        {
            allGrades = DatabaseHelper.getAllGrades();
            allTeachers = DatabaseHelper.getAllTeachers();
            allClasses = DatabaseHelper.getAllClasses();
            mIsInitialized = true;
        }

        public Class getClassById(int id)
        {
            foreach(Class item in allClasses) 
            {
                if (item.id == id) return item;
            }
            return null;
        }

        public int getClassIndexById(int id)
        {
            int index = 0;
            foreach (Class  cs in allClasses)
            {
                if (cs.id == id) { return index; }
                index++;
            }
            return index;
        }

        public int getClassDefaultLocationByClassId(int classid)
        {
            int locationid = 0;
            foreach (Class cs in allClasses)
            {
                if (cs.id == classid) { return cs.defaultlocationid; }
            }
            return locationid;
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

        public MemberBasic getTeacherById(int id)
        {
            foreach (MemberBasic basic in allTeachers)
            {
                if (basic.id == id)
                {
                    return basic;
                }
            }
            return null;
        }

        public int getTeacherIndexById(int id)
        {
            int index = 0;
            foreach (MemberBasic basic in allTeachers)
            {
                if (basic.id == id) { return index; }
                index++;
            }
            return index;
        }

        public int getGradeIndexById(int gradeId)
        {
            int index = 0;
            foreach (Grade grade in allGrades)
            {
                if (grade.id == gradeId) return index;
                index++;
            }
            return index;
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

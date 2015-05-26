using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Mode;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace CherishGardenEducationManager.ViewModel
{
    class CourseCardsViewModel
    {
        private static volatile CourseCardsViewModel instance;

        public static CourseCardsViewModel getInstance()
        {
            // DoubleLock
            if (instance == null)
            {
                lock (m_lock)
                {
                    if (instance == null)
                    {
                        instance = new CourseCardsViewModel();
                    }
                }
            }
            return instance;
        }

        //Helper for Thread Safety
        private static object m_lock = new object();
        public int selectedClassid = 0;
        public int selectedCourseid = 0;

        public ObservableCollection<CourseCard> currentCourseCards;
        public ObservableCollection<CourseCard> deletedCourseCards;
        public ObservableCollection<ClassCourse> classCourses;
        public ObservableCollection<Class> classes;
        public ObservableCollection<CourseGroup> courseGroups;


        public CourseCardsViewModel()
        {
            currentCourseCards = new ObservableCollection<CourseCard>();
            deletedCourseCards = new ObservableCollection<CourseCard>();
            classCourses = new ObservableCollection<ClassCourse>();
            classes = new ObservableCollection<Class>();
            courseGroups = new ObservableCollection<CourseGroup>();
        }

        public void initData()
        {
            //Confirmation all teachers, grades and classes have loaded.
            if(!ClassViewModel.getInstance().mIsInitialized) 
            {
                ClassViewModel.getInstance().worker_initData();
            }
            //confirmation all course groups and locations has loaded.
            if (!CourseWeekViewModel.getInstance().mIsInitialized)
            {
                CourseWeekViewModel.getInstance().worker_initDataFromDatabase(false);
            }

            //Get the current user's id.
            MemberBasic currentUser = (MemberBasic)Application.Current.Properties["currentUser"];
            int basicid = 0;
            if (currentUser != null)
            {
                basicid = currentUser.id;
            }
            selectedClassid = DatabaseHelper.getClassIdByHeadTeacherId(basicid);
            classCourses = DatabaseHelper.getClassCoursesByTeacherId(basicid);

            //find the class which the teacher has teached.
            foreach (ClassCourse cc in classCourses)
            {
               classes.Add(ClassViewModel.getInstance().getClassById(cc.classid));
            }
            courseGroups = getCourseGroupsByClassId(selectedClassid);
            selectedCourseid = courseGroups[0].id;

            //start load  course cards.
            currentCourseCards = DatabaseHelper.getAllCourseCards(selectedClassid, selectedCourseid);
        }

        public ObservableCollection<CourseGroup> getCourseGroupsByClassId(int classid)
        {
            ObservableCollection<CourseGroup> courseGroupsByClassid = new ObservableCollection<CourseGroup>();
            string coursegroupsids = "";
            foreach (ClassCourse cc in classCourses)
            {
                if (cc.classid == classid) { coursegroupsids = cc.coursesid; break; }
            }

            if (!coursegroupsids.Equals(""))
            {
                // start to handle the string.
                string[] ids = coursegroupsids.Split('|');
                foreach(string id in ids) 
                {
                    int coursegroupid = Int32.Parse(id);
                    courseGroupsByClassid.Add(CourseWeekViewModel.getInstance().getCourseGroupById(coursegroupid));
                }
            }

            return courseGroupsByClassid;
        }

        public void addCourseCard()
        {
            currentCourseCards.Add(new CourseCard() {
                classid = selectedClassid,
                courseid = selectedCourseid
            });
        }

        public void removeCourseCard(CourseCard item)
        {
            currentCourseCards.Remove(item);
            if (item.id == -1)
            {
                //means new added course card, but the user delete it again.
            }
            else
            {
                //otherwise, we should delete it in database.
                deletedCourseCards.Add(item);
            }
        }

        public void saveCurrentCourseCards()
        {
            ObservableCollection<CourseCard> oldCourseCardsHasChanged = new ObservableCollection<CourseCard>();
            ObservableCollection<CourseCard> newCourseCards = new ObservableCollection<CourseCard>();
            foreach (CourseCard card in currentCourseCards)
            {
                if (card.id == -1)
                {
                    //means new added;
                    newCourseCards.Add(card);
                }
                else
                {
                    //means the old data maybe changed.
                    //TODO need to confirm the coursecard data has changed, then persist it on.
                    if (card.contentChanged)
                    {
                        oldCourseCardsHasChanged.Add(card);
                    }
                }
            }
            bool saveSuccess = DatabaseHelper.saveCourseCards(oldCourseCardsHasChanged, newCourseCards, deletedCourseCards);
            if (saveSuccess)
            {
                deletedCourseCards.Clear();
                currentCourseCards.Clear();
            }
        }

        public int getClassIndexById()
        {
            int index = 0;
            int size = classes.Count;
            for (int i = 0; i < size; i++)
            {
                Class cs = classes[i];
                if (cs.id == selectedClassid)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public int getCourseIndexById()
        {
            int index = 0;
            int size = courseGroups.Count;
            for (int i = 0; i < size; i++)
            {
                CourseGroup cg = courseGroups[i];
                if (cg.id == selectedCourseid)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }


        public void refreshCourseGroupsData()
        {
            deletedCourseCards.Clear();
            currentCourseCards.Clear();
            courseGroups = getCourseGroupsByClassId(selectedClassid);

        }

        public bool currentCourseCardsContentHasChanged()
        {
            if (deletedCourseCards.Count > 0)
            {
                return true;
            }
            foreach (CourseCard card in currentCourseCards)
            {
                if (card.id == -1)
                {
                    return true;
                }
                else
                {
                    if (card.contentChanged)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void refreshCourseCards()
        {
            currentCourseCards.Clear();
            //Maybe need to reload coursecards from database.
            currentCourseCards = DatabaseHelper.getAllCourseCards(selectedClassid, selectedCourseid);
        } 
    }
}

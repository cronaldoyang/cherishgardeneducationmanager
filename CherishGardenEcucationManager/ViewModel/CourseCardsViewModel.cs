using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Mode;
using System.Collections.ObjectModel;

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
        private int currentClassid = 11;
        private int currentCourseid = 1;

        public ObservableCollection<CourseCard> currentCourseCards;
        public ObservableCollection<CourseCard> deletedCourseCards;


        public CourseCardsViewModel()
        {
            currentCourseCards = new ObservableCollection<CourseCard>();
            deletedCourseCards = new ObservableCollection<CourseCard>();
        }

        public void initData()
        {
            currentCourseCards =  DatabaseHelper.getAllCourseCards(currentClassid, currentCourseid);
        }

        public void addCourseCard()
        {
            currentCourseCards.Add(new CourseCard() { 
                classid = currentClassid,
                courseid = currentCourseid
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
            ObservableCollection<CourseCard> oldCourseCards = new ObservableCollection<CourseCard>();
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
                    oldCourseCards.Add(card);
                }
            }
            DatabaseHelper.saveCourseCards(oldCourseCards, newCourseCards, deletedCourseCards);
        }
    }
}

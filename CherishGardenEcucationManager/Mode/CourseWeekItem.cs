using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CherishGardenEducationManager.Mode
{
    public class CourseWeekItem
    {
        int _id = -1;
        int _gradeid;
        int _weekno;//1415211  14~15代表学年，2代表第二学期，11代表第十一周
        int _weekday;
        string _contentdesc;
        int _jieci;//值为31代表星期三的第一节课
        int _coursegroupid;
        int _teacherid;
        int _locationid;

        public CourseWeekItem() { }

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int jieci
        {
            get { return _jieci; }
            set { _jieci = value; }
        }

        public int coursegroupid
        {
            get { return _coursegroupid; }
            set { _coursegroupid = value; }
        }

        public int teacherid
        {
            get { return _teacherid; }
            set { _teacherid = value; }
        }

        public int locationid
        {
            get { return _locationid; }
            set { _locationid = value; }
        }

        public int gradeid
        {
            get { return _gradeid; }
            set { _gradeid = value; }
        }

        public int weekno
        {
            get { return _weekno; }
            set { _weekno = value; }
        }

        public int weekday
        {
            get { return _weekday; }
            set { _weekday = value; }
        }

        public string contentdesc
        {
            get { return _contentdesc; }
            set { _contentdesc = value; }
        }

    }
}

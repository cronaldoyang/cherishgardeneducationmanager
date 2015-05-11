using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CherishGardenEducationManager.Entity
{
    class Course
    {
        int _id;
        DateTime coursedateField;
        int coursenoField;
        string coursenameField;
        string coursetargetField;
        int teacheridField;

        public Course() { }

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime coursedate
        {
            get { return coursedateField; }
            set { coursedateField = value; }
        }

        public int courseno
        {
            get { return coursenoField; }
            set { coursenoField = value; }
        }

        public string coursetarget
        {
            get { return coursetargetField; }
            set { coursetargetField = value; }
        }

        public int teacherid
        {
            get { return teacheridField; }
            set { teacheridField = value; }
        }

        public ObservableCollection<MemberBasic> candidateTeachers { get; set; }
    }
}

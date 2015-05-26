using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CherishGardenEducationManager.Mode
{
    class Class
    {
        int _id = -1;
        string nameField = "";
        int teacheridField;
        int groupidField;

        public ObservableCollection<Grade> candidateGrades { get; set; }
        public ObservableCollection<MemberBasic> candidateTeachers { get; set; }

        public Class() { }

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string name {
            get { return nameField; }
            set { nameField = value; }
        }

        public int teacherid {
            get { return teacheridField; }
            set { teacheridField = value; }
        }

        public int gradeid
        {
            get { return groupidField; }
            set { groupidField = value; }
        }
    }
}

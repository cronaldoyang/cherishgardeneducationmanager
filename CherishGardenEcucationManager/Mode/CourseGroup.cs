using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CherishGardenEducationManager.Mode
{
    public class CourseGroup
    {
        public CourseGroup() { }

        private int _id;
        private string _category;
        private string _courseName;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string category
        {
            get { return _category; }
            set { _category = value; }
        }

        public string courseName
        {
            get { return _courseName; }
            set { _courseName = value; }
        }

        public override bool Equals(object obj) 
        { 
            if (obj == null || !(obj is CourseGroup)) 
                return false; 
  
            return ((CourseGroup)obj).id == this.id; 
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CherishGardenEducationManager.Entity
{
    //This entity is for course week location.
    public class CourseLocation
    {
        public CourseLocation() { }

        private int _id;
        private string _location;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string location
        {
            get { return _location; }
            set { _location = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is CourseLocation))
                return false;

            return ((CourseLocation)obj).id == this.id;
        }
    }
}

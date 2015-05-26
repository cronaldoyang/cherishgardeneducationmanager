using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CherishGardenEducationManager.Mode
{
    public class Grade : ICloneable 
    {
        int _id = -1;
        string nameField = "";

        public Grade() { }

        public int id {
            get { return _id; }
            set { _id = value; }
        }

        public string name {
            get { return nameField; }
            set { nameField = value; }
        }

        public Object Clone()
        {
            return MemberwiseClone();
        }
    }
}

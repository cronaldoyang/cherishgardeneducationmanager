using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CherishGardenEducationManager.Entity
{
    class Grade
    {
        int _id;
        string nameField;

        public Grade() { }

        public int id {
            get { return _id; }
            set { _id = value; }
        }

        public string name {
            get { return nameField; }
            set { nameField = value; }
        }
    }
}

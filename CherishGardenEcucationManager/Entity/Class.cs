using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CherishGardenEducationManager.Entity
{
    class Class
    {
        string nameField;
        int teacheridField;
        int groupidField;

        public Class() { }

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

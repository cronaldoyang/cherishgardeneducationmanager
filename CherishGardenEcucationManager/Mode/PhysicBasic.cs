using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CherishGardenEducationManager.Entity
{
    class PhysicBasic
    {
        float heightField;
        float weightField;
        DateTime dateField;

        public PhysicBasic() { }

        public float height
        {
            get { return heightField; }
            set { heightField = value; }
        }

        public float weight
        {
            get { return weightField; }
            set { weightField = value; }
        }

        public DateTime date
        {
            get { return dateField; }
            set { dateField = value; }
        }
    }
}

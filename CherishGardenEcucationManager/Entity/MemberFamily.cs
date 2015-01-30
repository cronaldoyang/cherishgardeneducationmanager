using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CherishGardenEducationManager.Entity
{
    class MemberFamily
    {
        string nameField;
        string realtionshipField;
        string phoneField;
        string idcardnoField;
        Boolean  pickupField;
        Boolean  emergencycontactField;

        public MemberFamily() { }

        public string name
        {
            get { return nameField; }
            set { nameField = value; }
        }

        public string relationship
        {
            get { return realtionshipField; }
            set { realtionshipField = value; }
        }

        public string phone
        {
            get { return phoneField; }
            set { phoneField = value; }
        }

        public string idcardno
        {
            get { return idcardnoField; }
            set { idcardnoField = value; }
        }

        public Boolean  pickup
        {
            get { return pickupField; }
            set { pickupField = value; }
        }

        public Boolean  emergencycontact
        {
            get { return emergencycontactField; }
            set { emergencycontactField = value; }
        }
    }
}

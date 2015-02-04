using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CherishGardenEducationManager.Entity
{
    class MemberFamily
    {
        int _id;
        string nameField;
        string realtionshipField;
        string phoneField;
        string idcardnoField;
        Boolean  pickupField;
        Boolean  emergencycontactField;
        string addressField;


        public MemberFamily() { }

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

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

        public string address
        {
            get { return addressField; }
            set { idcardnoField = value; }
        }
    }
}

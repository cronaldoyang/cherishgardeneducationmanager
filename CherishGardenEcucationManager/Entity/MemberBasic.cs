using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CherishGardenEducationManager.Entity
{
    class MemberBasic
    {
        string nameField;
        string engnameField;
        string idcardnoField;
        string genderField;
        Boolean isteacherField;

        public string name { 
            get { return nameField;}
            set { nameField = value;}
        }
        public string engname
        {
            get { return engnameField; }
            set { engnameField = value; }
        }

        public String gender
        {
            get { return genderField; }
            set { genderField = value; }
        }

        public string idcardno
        {
            get { return idcardnoField; }
            set { idcardnoField = value; }
        }

        public Boolean isteacher
        {
            get { return isteacherField; }
            set { isteacherField = value; }
        }

        public MemberBasic(String namev, String engnamev, string gender, String idcardnov, Boolean isteacherv) {
             nameField = namev;
             engnameField = engnamev;
             genderField = gender;
             idcardnoField = idcardnov;
             isteacherField = isteacherv;
        }

        public override String ToString() {
            return "name:" + nameField + "|engName:" + engnameField + "|gender:" + genderField + "|idcardno:" + idcardnoField + "|isteacher:" + isteacherField;
        }

    }
}

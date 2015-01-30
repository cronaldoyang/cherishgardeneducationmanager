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

        string name { 
            get { return nameField;}
            set { nameField = value;}
        }
        string engname {
            get { return engnameField; }
            set { engnameField = value; }
        }

        String male {
            get { return genderField; }
            set { genderField = value; }
        }

        string idcardno {
            get { return idcardnoField; }
            set { idcardnoField = value; }
        }

        Boolean isteacher {
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

        public String ToString() {
            return "name:" + nameField + "|engName:" + engnameField + "|gender:" + genderField + "|idcardno:" + idcardnoField + "|isteacher:" + isteacherField;
        }

    }
}

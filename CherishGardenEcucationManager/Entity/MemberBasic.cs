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
        Boolean maleField;
        Boolean isteacherField;

        string name { 
            get { return nameField;}
            set { nameField = value;}
        }
        string engname {
            get { return engnameField; }
            set { engnameField = value; }
        }

        Boolean male {
            get { return maleField; }
            set { maleField = value; }
        }

        string idcardno {
            get { return idcardnoField; }
            set { idcardnoField = value; }
        }

        Boolean isteacher {
            get { return isteacherField; }
            set { isteacherField = value; }
        }

        public MemberBasic(String namev, String engnamev, Boolean malev, String idcardnov, Boolean isteacherv) {
             nameField = namev;
             engnameField = engnamev;
             maleField = malev;
             idcardnoField = idcardnov;
             isteacherField = isteacherv;
        }

    }
}

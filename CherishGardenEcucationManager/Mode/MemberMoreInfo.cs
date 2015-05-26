using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace CherishGardenEducationManager.Mode
{
    class MemberMoreInfo
    {
        int _id;
        DateTime birthdayYangliField;
        DateTime birthdayNongliField;
        string minzuField;
        string birthPlaceField;
        string nowaddressField;
        string residenceaddressField;
        string phoneField;
        string qqField;
        DateTime graduateddateField;
        string professionField;
        string forteField;
        string educationbackgroundField;
        string graduatedschoolField;
        string putonghualevelField;
        string computerlevelField;
        string selfevaluationField;
        string photopathField;
        int _mbid;

        public MemberMoreInfo() { }

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int mbid
        {
            get { return _mbid; }
            set { _mbid = value; }
        }

        public DateTime birthdayyangli
        {
            get { return birthdayYangliField; }
            set { birthdayYangliField = value; }
        }

        public DateTime birthdaynongli
        {
            get { return birthdayNongliField; }
            set { birthdayNongliField = value; }
        }

        public string minzu
        {
            get { return minzuField; }
            set { minzuField = value; }
        }

        public string birthplace
        {
            get { return birthPlaceField; }
            set { birthPlaceField = value; }
        }

        public string nowaddress
        {
            get { return nowaddressField; }
            set { nowaddressField = value; }
        }

        public string residenceaddress
        {
            get { return residenceaddressField; }
            set { residenceaddressField = value; }
        }

        public string phone
        {
            get { return phoneField; }
            set { phoneField = value; }
        }

        public string qq
        {
            get { return qqField; }
            set { qqField = value; }
        }


        public DateTime graduateddate
        {
            get { return graduateddateField; }
            set { graduateddateField = value; }
        }


        public string profession
        {
            get { return professionField; }
            set { professionField = value; }
        }


        public string forte
        {
            get { return forteField; }
            set { forteField = value; }
        }

        public string graduatedschool
        {
            get { return graduatedschoolField; }
            set { graduatedschoolField = value; }
        }

        public string educationbackground
        {
            get { return educationbackgroundField; }
            set { educationbackgroundField = value; }
        }

        public string putonghualevel
        {
            get { return putonghualevelField; }
            set { putonghualevelField = value; }
        }

        public string computerlevel
        {
            get { return computerlevelField; }
            set { computerlevelField = value; }
        }

        public string selfevaluation
        {
            get { return selfevaluationField; }
            set { selfevaluationField = value; }
        }

        public string  photopath
        {
            get { return photopathField; }
            set { photopathField = value; }
        }
    }
}

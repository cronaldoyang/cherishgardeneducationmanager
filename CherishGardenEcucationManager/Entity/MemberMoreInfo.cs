using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace CherishGardenEducationManager.Entity
{
    class MemberMoreInfo
    {
        DateTime birthdayYangliField;
        DateTime birthdayNongliField;
        string minzuField;
        string birthdayPlaceField;
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
        BitmapImage photoField;

        public MemberMoreInfo() { }

        public DateTime birthdayYangli
        {
            get { return birthdayYangliField; }
            set { birthdayYangliField = value; }
        }

        public DateTime birthdayNongli
        {
            get { return birthdayNongliField; }
            set { birthdayNongliField = value; }
        }

        public string minzu
        {
            get { return minzuField; }
            set { minzuField = value; }
        }

        public string birthdayPlace
        {
            get { return birthdayPlaceField; }
            set { birthdayPlaceField = value; }
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

        public BitmapImage photo
        {
            get { return photoField; }
            set { photoField = value; }
        }
    }
}

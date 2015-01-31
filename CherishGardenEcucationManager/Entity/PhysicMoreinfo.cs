using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CherishGardenEducationManager.Entity
{
    class PhysicMoreinfo
    {
        Boolean haveFoodallergyField;
        string foodallergyinfoField;
        Boolean haveConvulsionsepilepsyField;
        Boolean haveBraindiseasesField;
        Boolean haveAcutechronicinfectiousField;
        Boolean haveheartdiseasesField;
        Boolean haverenaldiseaseField;
        Boolean havedrugallergyField;
        string drugallergyFieldField;
        string markField;

        public PhysicMoreinfo() { }

        /**有无食物过敏史*/
        public Boolean haveFoodallergy {
            get { return haveFoodallergyField; }
            set { haveFoodallergyField = value; }
        }

        /**有无抽风、癫痫史*/
        public Boolean haveConvulsionsepilepsy
        {
            get { return haveConvulsionsepilepsyField; }
            set { haveConvulsionsepilepsyField = value; }
        }

        /**有无脑部疾病*/
        public Boolean haveBraindiseases
        {
            get { return haveBraindiseasesField; }
            set { haveBraindiseasesField = value; }
        }

        /**有无急慢性传染病史*/
        public Boolean haveAcutechronicinfectious
        {
            get { return haveAcutechronicinfectiousField; }
            set { haveAcutechronicinfectiousField = value; }
        }

        /**有无心脏病史*/
        public Boolean haveheartdiseases
        {
            get { return haveheartdiseasesField; }
            set { haveheartdiseasesField = value; }
        }

        /**有无肾脏病史*/
        public Boolean haverenaldisease
        {
            get { return haverenaldiseaseField; }
            set { haverenaldiseaseField = value; }
        }

        /**有无食物过敏史*/
        public Boolean havedrugallergy
        {
            get { return havedrugallergyField; }
            set { havedrugallergyField = value; }
        }

        public string drugallergy
        {
            get { return drugallergyFieldField; }
            set { drugallergyFieldField = value; }
        }

        public string foodallergyinfo
        {
            get { return foodallergyinfoField; }
            set { foodallergyinfoField = value; }
        }

        public string mark
        {
            get { return markField; }
            set { markField = value; }
        }
    }
}

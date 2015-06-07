using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CherishGardenEducationManager.Mode
{
    class WeeklyReportItem
    {
        public WeeklyReportItem() 
        { 
        }

        public int id { get; set; }
        public int weekno { get; set; }
        public int classid { get; set; }
        public int courseid { get; set; }
        public string coursename { get; set; }
        public string category { get; set; }//Get from courseGroup field.
        public string target { get; set; }
        public int stuid { get; set; }
        public bool contentChanged { get; set; }//This field's target as mark the object content has changed.
    }
}

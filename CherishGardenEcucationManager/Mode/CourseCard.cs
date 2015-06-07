using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CherishGardenEducationManager.Mode
{
    //This class will represent one piece of course card content;
    class CourseCard
    {
        public CourseCard() 
        { 
            time = DateTime.Now;
            updatetime = DateTime.Now;
            id = -1; 
        }

        public int id { get; set; }
        public DateTime time { get; set; }
        public DateTime updatetime { get; set; }
        public string name { get; set; }
        public string targets { get; set; }
        public string teachingplan { get; set; }
        public string materias { get; set; }
        public string mark { get; set; }
        public int classid { get; set; }
        public int courseid { get; set; }
        public bool contentChanged { get; set; }//This field's target as mark the object content has changed.
    }
}

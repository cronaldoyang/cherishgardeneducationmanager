using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CherishGardenEducationManager.Mode
{
    class AwardOrPunishment
    {
        int _id;
        DateTime dateField = DateTime.Now;
        string contentField;
        string organizationField;

        public AwardOrPunishment() { }

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime date {
            get { return dateField; }
            set { dateField = value; }
        }

        public string content {
            get { return contentField; }
            set { contentField = value; }
        }

        public string organization
        {
            get { return organizationField; }
            set { organizationField = value; }
        }
    }
}

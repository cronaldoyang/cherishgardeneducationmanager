using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CherishGardenEducationManager.Entity
{
    class AwardOrPunishment
    {
        DateTime dateField;
        string contentField;
        string organizationField;

        public AwardOrPunishment() { }

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

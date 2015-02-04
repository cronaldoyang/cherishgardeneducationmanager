using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CherishGardenEducationManager.Entity
{
    class Exprience
    {
        int _id;
        DateTime fromField = new DateTime(1990,1,1);
        DateTime toField = new DateTime(1990, 1, 1);
        string addressField;
        string positionsField;
        string responsibilityField;

        public Exprience() { }

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime from {
            get { return fromField; }
            set { fromField = value; }
        }

        public DateTime to
        {
            get { return toField; }
            set { toField = value; }
        }

        public string address
        {
            get { return addressField; }
            set { addressField = value; }
        }

        public string  positions
        {
            get { return positionsField; }
            set { positionsField = value; }
        }

        public string  responsibility
        {
            get { return responsibilityField; }
            set { responsibilityField = value; }
        }

    }
}

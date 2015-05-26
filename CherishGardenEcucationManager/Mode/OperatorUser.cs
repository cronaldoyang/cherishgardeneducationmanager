using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CherishGardenEducationManager.Mode
{
    class OperatorUser
    {
        string passwordField;
        int operatoridField;

        public string password {
            get { return passwordField; }
            set { passwordField = value; }
        }

        public int operatorid {
            get { return operatoridField; }
            set { operatoridField = value; }
        }

        public OperatorUser(string passwordv, int operatoridv) {
            passwordField = passwordv;
            operatoridField = operatoridv;
        }

        public OperatorUser()
        {
        }
    }
}

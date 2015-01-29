using System;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace CherishGardenEducationManager
{
    public class NumberTextBox : TextBox
    {
        #region Constructors
            
        public NumberTextBox()
        {
            TextChanged += new TextChangedEventHandler(OnTextChanged);
            KeyDown += new KeyEventHandler(OnKeyDown);
        }
        #endregion

        #region Properties
        new public String Text
        {
            get { return base.Text; }
            set
            {
                base.Text = LeaveOnlyNumbers(value);
            }
        }

        #endregion

        #region Functions
        private bool IsNumberKey(Key inKey)
        {
            if (inKey < Key.D0 || inKey > Key.D9)
            {
                if (inKey < Key.NumPad0 || inKey > Key.NumPad9)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsDelOrBackspaceOrTabKey(Key inKey)
        {
            return inKey == Key.Delete || inKey == Key.Back || inKey == Key.Tab;
        }

        private string LeaveOnlyNumbers(String inString)
        {
            String tmp = inString;
            foreach (char c in inString.ToCharArray())
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(c.ToString(), "^[0-9]*$"))
                {
                    tmp = tmp.Replace(c.ToString(), "");
                }
            }
            return tmp;
        }
        #endregion

        #region Event Functions
        protected void OnKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = !IsNumberKey(e.Key) && !IsDelOrBackspaceOrTabKey(e.Key);
        }

        protected void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            base.Text = LeaveOnlyNumbers(Text);
        }
        #endregion
    }
}

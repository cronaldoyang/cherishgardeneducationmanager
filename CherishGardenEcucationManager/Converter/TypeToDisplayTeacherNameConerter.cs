using CherishGardenEducationManager.Mode;
using CherishGardenEducationManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CherishGardenEducationManager
{
    class TypeToDisplayTeacherNameConerter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int  teacherid = (int)value;
            return ClassViewModel.getInstance().getTeacherNameByid(teacherid);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}

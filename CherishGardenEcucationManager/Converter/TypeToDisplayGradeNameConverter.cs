using CherishGardenEducationManager.Mode;
using CherishGardenEducationManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CherishGardenEducationManager
{
    class TypeToDisplayGradeNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int  gradeid = (int)value;
            return ClassViewModel.getInstance().getGradeNameByid(gradeid);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}

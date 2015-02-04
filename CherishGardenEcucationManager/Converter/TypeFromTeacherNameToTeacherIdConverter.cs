using CherishGardenEducationManager.Entity;
using CherishGardenEducationManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CherishGardenEducationManager
{
    class TypeFromTeacherNameToTeacherIdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string   name = (string)value;
            return ClassViewModel.getInstance().getTeacherIdByName(name);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}

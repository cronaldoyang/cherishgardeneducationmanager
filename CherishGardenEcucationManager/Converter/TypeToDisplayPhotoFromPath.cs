using CherishGardenEducationManager.Entity;
using CherishGardenEducationManager.Helper;
using CherishGardenEducationManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CherishGardenEducationManager
{
    class TypeToDisplayPhotoFromPath : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string   photopath = (string)value;
            return ChoosePhotoHelper.getRealPhoto(photopath);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}

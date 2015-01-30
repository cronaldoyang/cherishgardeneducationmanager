using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CherishGardenEducationManager.Helper
{
    class ChoosePhotoHelper
    {
        public static String getFilenameFromUserChoose()
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                return dlg.FileName;
            }

            return "";
        }

        public static BitmapImage getRealPhoto(String fileName)
        {

            BitmapImage photobmp = null;
            // Open document 
            try
            {
                photobmp = new BitmapImage(new Uri(fileName, UriKind.Absolute));
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid image file.", "Browse", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            return photobmp;
        }
    }
}

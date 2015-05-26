using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Mode;
using CherishGardenEducationManager.Helper;
using CherishGardenEducationManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CherishGardenEducationManager
{
    /// <summary>
    /// Interaction logic for StudentsInfo.xaml
    /// </summary>
    public partial class StudentsInfoPage : Page
    {
        private readonly BackgroundWorker workerForInitData = new BackgroundWorker();
        private readonly BackgroundWorker workerForSaveData = new BackgroundWorker();

        public event EventHandler updateStausbar;
        string realPhotoPathName;
        DataGrid memberFamilyDataGrid;
        TextBox markTextBox;
        public StudentsInfoPage()
        {
            InitializeComponent();
            initData();
        }

        void initData()
        {
            workerForInitData.DoWork += workerForInitData_DoWork;
            workerForInitData.RunWorkerCompleted += workerForInitData_RunWorkerCompleted;
            workerForInitData.RunWorkerAsync();
        }

        private void workerForInitData_DoWork(object sender, DoWorkEventArgs e)
        {
            MemberAllInfoViewModel.getInstance().initData(true);
        }

        private void workerForInitData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            markTextBox = (TextBox)FindName("mark");
            memberFamilyDataGrid = (DataGrid)FindName("MemberFamilyDataGrid");
            Button choosePhtotoBtn = (Button)FindName("choosePhotoBtn");
            Image photo = (Image)FindName("photo"); 
            
            MemberMoreInfo membermoreinfo = MemberAllInfoViewModel.getInstance().moreInfo;
            studentBasicInfoGrid.DataContext = MemberAllInfoViewModel.getInstance().basic;
            studentMoreInfoGrid.DataContext = membermoreinfo;
            if ( !membermoreinfo.photopath.Equals(""))
            {
                choosePhotoBtn.Visibility = System.Windows.Visibility.Collapsed;
                photo.Visibility = System.Windows.Visibility.Visible;
            }
            memberFamilyDataGrid.ItemsSource = MemberAllInfoViewModel.getInstance().allMemberFamily;
            PhysicMoreinfo physicmoreinfo = MemberAllInfoViewModel.getInstance().physicMoreInfo;
            physicMoreInfoGrid.DataContext = physicmoreinfo;
            markTextBox.DataContext = physicmoreinfo;
        }

  


        private void choosePhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            realPhotoPathName = ChoosePhotoHelper.getFilenameFromUserChoose();
            Button choosePhotoPBtn = (Button)sender;
            Image photo = (Image)FindName("photo");
            choosePhotoPBtn.Visibility = System.Windows.Visibility.Collapsed;
            photo.Visibility = System.Windows.Visibility.Visible;
            if (!realPhotoPathName.Equals(""))
            {
                photo.Source = ChoosePhotoHelper.getRealPhoto(realPhotoPathName);
            }
        }

        private void dynamicAddMemberFamilyBtn_Click(object sender, RoutedEventArgs e)
        {
            addMemberFamily(new MemberFamily());
        }

        private void StudentInfoPageSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            //0.
            updateStausbar(this, new EventArgs());

            //1. Generate the memory objects.
            workerForSaveData.DoWork += workerForSaveData_DoWork;
            workerForSaveData.RunWorkerCompleted += workerForSaveData_RunWorkerCompleted;
            workerForSaveData.RunWorkerAsync();

            //2.notify save memberinfo into database result.
            updateStausbar(this, new EventArgs());
        }

        private void workerForSaveData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Notify the user save data completed.
        }

        private void workerForSaveData_DoWork(object sender, DoWorkEventArgs e)
        {
            MemberAllInfoViewModel.getInstance().saveMemberInfo(true);
        }

        void addMemberFamily(MemberFamily obj)
        {
            MemberAllInfoViewModel.getInstance().addMemberFamily();
        }

        private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);
            }
        }

    }
}

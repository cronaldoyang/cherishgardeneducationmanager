using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Entity;
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
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for TeacherInfoPage.xaml
    /// </summary>
    public partial class TeacherInfoPage : Page
    {
        private readonly BackgroundWorker workerForInitData = new BackgroundWorker();
        private readonly BackgroundWorker workerForSaveData = new BackgroundWorker();

        public event EventHandler updateStausbar;
        string realPhotoPathName;


        //delcaration global variable;
        DataGrid memberFamilyDataGrid;
        DataGrid exprienceDataGrid;
        DataGrid awardsDataGrid;
        public TeacherInfoPage()
        {
            InitializeComponent();
            initData();
        }
        private void initData()
        {
            workerForInitData.DoWork += worker_initData;
            workerForInitData.RunWorkerCompleted += worker_initDataCompleted;
            workerForInitData.RunWorkerAsync();
        }

        public void worker_initData(object sender, DoWorkEventArgs e)
        {
            MemberAllInfoViewModel.getInstance().initData(false);
        }

        void worker_initDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            memberFamilyDataGrid = (DataGrid)FindName("MemberFamilyDataGrid");
            exprienceDataGrid = (DataGrid)FindName("ExprienceDataGrid");
            awardsDataGrid = (DataGrid)FindName("AwardsDataGrid");
            Button choosePhtotoBtn = (Button)FindName("choosePhotoBtn");
            Image photo = (Image)FindName("photo");

            basicInfoGrid.DataContext = MemberAllInfoViewModel.getInstance().basic;
            MemberMoreInfo moreinfo = MemberAllInfoViewModel.getInstance().moreInfo;
            moreinfoGrid.DataContext = moreinfo;
            if(!moreinfo.photopath.Equals("")) {
                choosePhotoBtn.Visibility = System.Windows.Visibility.Collapsed;
                photo.Visibility = System.Windows.Visibility.Visible;
            }
            memberFamilyDataGrid.ItemsSource = MemberAllInfoViewModel.getInstance().allMemberFamily;
            exprienceDataGrid.ItemsSource = MemberAllInfoViewModel.getInstance().allExpriences;
            awardsDataGrid.ItemsSource = MemberAllInfoViewModel.getInstance().allAwards;
        }

        private void TeacherInfoPageSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            //We should do the following things when the user click save button.
            //0. update the statsbar state: start save.
            //1. check all input is valide; update statsbar "input is ok"
            //2. update statusbar "Preperation for writing database "-->Generate the memory object
            //3. update statusbar "start write database"-->Open database connection and commit memory data to database.
            //4. get database feedback is ok and update statusbar "save done".
            //5. otherwise, redo from zero to four.

            //0.
            updateStausbar(this, new EventArgs());

            //1. Generate the memory objects.

            workerForSaveData.DoWork += worker_saveMemberAllInfo;
            workerForSaveData.RunWorkerCompleted += worker_saveMemberAllInfoCompleted;
            workerForSaveData.RunWorkerAsync();
        }

        public void worker_saveMemberAllInfo(object sender, DoWorkEventArgs e)
        {
            MemberAllInfoViewModel.getInstance().saveMemberInfo(false);
        }

        void worker_saveMemberAllInfoCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //do notify user .
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
            MemberAllInfoViewModel.getInstance().addMemberFamily();
        }

        private void dynamicAddAwardBtn_Click(object sender, RoutedEventArgs e)
        {
            MemberAllInfoViewModel.getInstance().addAward();
        }

        private void dynamicAddExprienceBtn_Click(object sender, RoutedEventArgs e)
        {
            MemberAllInfoViewModel.getInstance().addExprience();
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

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
            

            //2.notify save memberinfo into database result.
            updateStausbar(this, new EventArgs());
        }

        void addMemberFamily(MemberFamily obj)
        {
            
        }

        private MemberBasic generateMemberBasicFromUser()
        {
            String teacherName = ((TextBox)FindName("studentName")).Text;
            String teacherEngName = ((TextBox)FindName("studentEngName")).Text;
            String gender = ((TextBox)FindName("genderTextBox")).Text;
            String idcardno = ((TextBox)FindName("studentIdCardNo")).Text;
            return new MemberBasic(teacherName, teacherEngName, gender, idcardno, false);
        }

        private MemberMoreInfo generateMemberMoreInfoFromUser()
        {
            DateTime birthdayYangli = (DateTime)((DatePicker)FindName("birthdayYangli")).SelectedDate;
            DateTime birthdayNongli = (DateTime)((DatePicker)FindName("birthdayNongli")).SelectedDate;
            string minzu = ((TextBox)FindName("minzu")).Text;
            string birthdayPlace = ((TextBox)FindName("birthdayplace")).Text;
            string nowaddress = ((TextBox)FindName("nowaddress")).Text;
            string residenceaddress = ((TextBox)FindName("residenceaddress")).Text;


            MemberMoreInfo moreInfoObj = new MemberMoreInfo();
            moreInfoObj.birthdaynongli = birthdayNongli;
            moreInfoObj.birthdayyangli = birthdayYangli;
            moreInfoObj.minzu = minzu;
            moreInfoObj.birthplace = birthdayPlace;
            moreInfoObj.nowaddress = nowaddress;
            moreInfoObj.residenceaddress = residenceaddress;
            moreInfoObj.photopath = realPhotoPathName;
            return moreInfoObj;
        }

        private PhysicMoreinfo generatePhysicMoreInfoFromUser()
        {
            Boolean haveFoodallergy = (Boolean)((CheckBox)FindName("haveFoodallergy")).IsChecked;
            Boolean haveConvulsionsepilepsy = (Boolean)((CheckBox)FindName("haveConvulsionsepilepsy")).IsChecked;
            Boolean haveBraindiseases = (Boolean)((CheckBox)FindName("haveBraindiseases")).IsChecked;
            Boolean haveAcutechronicinfectious = (Boolean)((CheckBox)FindName("haveAcutechronicinfectious")).IsChecked;
            Boolean haveheartdiseases = (Boolean)((CheckBox)FindName("haveheartdiseases")).IsChecked;
            Boolean haverenaldisease = (Boolean)((CheckBox)FindName("haverenaldisease")).IsChecked;
            Boolean havedrugallergy = (Boolean)((CheckBox)FindName("havedrugallergy")).IsChecked;
            string foodallergyinfo = ((TextBox)FindName("foodallergyTextBox")).Text;
            string drugallergy = ((TextBox)FindName("drugallergy")).Text;
            string mark = ((TextBox)FindName("mark")).Text;

            PhysicMoreinfo physicMoreInfo = new PhysicMoreinfo();
            physicMoreInfo.haveFoodallergy = haveFoodallergy;
            physicMoreInfo.haveConvulsionsepilepsy = haveConvulsionsepilepsy;
            physicMoreInfo.haveBraindiseases = haveBraindiseases;
            physicMoreInfo.haveAcutechronicinfectious = haveAcutechronicinfectious;
            physicMoreInfo.haveheartdiseases = haveheartdiseases;
            physicMoreInfo.haverenaldisease = haverenaldisease;
            physicMoreInfo.havedrugallergy = havedrugallergy;
            physicMoreInfo.foodallergyinfo = foodallergyinfo;
            physicMoreInfo.drugallergy = drugallergy;
            physicMoreInfo.mark = mark;
            return physicMoreInfo;
        }
    }
}

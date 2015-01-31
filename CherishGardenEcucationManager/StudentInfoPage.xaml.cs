using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Entity;
using CherishGardenEducationManager.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public event EventHandler updateStausbar;
        ObservableCollection<MemberFamily> memberFamilyCollection = new ObservableCollection<MemberFamily>();

        string realPhotoPathName;
        DataGrid memberFamilyDataGrid;


        public StudentsInfoPage()
        {
            InitializeComponent();
            initUI();
        }

        void initUI()
        {
            memberFamilyDataGrid = (DataGrid)FindName("MemberFamilyDataGrid");
            memberFamilyDataGrid.ItemsSource = memberFamilyCollection;
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
            Boolean result = DatabaseHelper.SaveMemberAllInfo(generateMemberBasicFromUser(), generateMemberMoreInfoFromUser() , 
                memberFamilyCollection, new ObservableCollection<EducationAndEmployeeExprience>(), 
                new ObservableCollection<AwardOrPunishment>(), generatePhysicMoreInfoFromUser());

            //2.notify save memberinfo into database result.
            updateStausbar(this, new EventArgs());
        }

        void addMemberFamily(MemberFamily obj)
        {
            memberFamilyCollection.Add(obj);
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
            moreInfoObj.birthdayNongli = birthdayNongli;
            moreInfoObj.birthdayYangli = birthdayYangli;
            moreInfoObj.minzu = minzu;
            moreInfoObj.birthPlace = birthdayPlace;
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

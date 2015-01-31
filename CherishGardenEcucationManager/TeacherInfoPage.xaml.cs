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
        public event EventHandler updateStausbar;

        ObservableCollection<MemberFamily> memberFamilyCollection = new ObservableCollection<MemberFamily>();
        ObservableCollection<EducationAndEmployeeExprience> exprienceCollection = new ObservableCollection<EducationAndEmployeeExprience>();
        ObservableCollection<AwardOrPunishment> awardsCollection = new ObservableCollection<AwardOrPunishment>();

        string realPhotoPathName;

        public void initTestData()
        {
            //construct member family;
            memberFamilyCollection.Add(new MemberFamily
            {
                name = "Yangxiao",
                relationship = "Father",
                phone = "18286736596",
                idcardno = "522425198501283918",
                pickup = true,
                emergencycontact = false
            });
            memberFamilyCollection.Add(new MemberFamily
            {
                name = "LiuMingHui",
                relationship = "Mother",
                phone = "18286736596",
                idcardno = "522425198501283918",
                pickup = true,
                emergencycontact = false
            });
            memberFamilyCollection.Add(new MemberFamily
            {
                name = "Xiaohui",
                relationship = "Mother",
                phone = "18286736596",
                idcardno = "522425198501283918",
                pickup = true,
                emergencycontact = false
            });
            //construct exprience;
            exprienceCollection.Add(new EducationAndEmployeeExprience()
            {
                from = new DateTime(2013, 1, 2),
                to = new DateTime(2014, 3, 2),
                address = "TapasDianxin",
                positions = "Engineer",
                responsibility = "Develop IM"
            });
            exprienceCollection.Add(new EducationAndEmployeeExprience()
            {
                from = new DateTime(2010, 10, 2),
                to = new DateTime(2011, 9, 19),
                address = "SonyEricssion",
                positions = "Engineer",
                responsibility = "Develop IM"
            });
            //construct award
            awardsCollection.Add(new AwardOrPunishment()
            {
                date = new DateTime(2015, 2, 1),
                content = "Excellect for education",
                organization = "CherishGarden"
            });
        }

        void addMemberFamily(MemberFamily obj)
        {
            memberFamilyCollection.Add(obj);
        }

        void addExprience(EducationAndEmployeeExprience obj)
        {
            exprienceCollection.Add(obj);
        }

        void addAward(AwardOrPunishment obj)
        {
            awardsCollection.Add(obj);
        }

        //delcaration global variable;
        DataGrid memberFamilyDataGrid;
        DataGrid exprienceDataGrid;
        DataGrid awardsDataGrid;
        public TeacherInfoPage()
        {
            InitializeComponent();
            initTestData();
            initUI();
        }

        void initUI()
        {
            memberFamilyDataGrid = (DataGrid)FindName("MemberFamilyDataGrid");
            exprienceDataGrid = (DataGrid)FindName("ExprienceDataGrid");
            awardsDataGrid = (DataGrid)FindName("AwardsDataGrid");

            memberFamilyDataGrid.ItemsSource = memberFamilyCollection;
            exprienceDataGrid.ItemsSource = exprienceCollection;
            awardsDataGrid.ItemsSource = awardsCollection;
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
            MemberBasic basicObj = generateMemberBasicFromUser();
            MemberMoreInfo moreInfoObj = generateMemberMoreInfoFromUser();

            Boolean? result = DatabaseHelper.SaveMemberAllInfo(basicObj, moreInfoObj, memberFamilyCollection, exprienceCollection, awardsCollection);
            //DatabaseHelper.SaveExprienceAwardsFamilyInfo(memberFamilyCollection, exprienceCollection, awardsCollection);
            //2.notify save memberinfo into database result.
            updateStausbar(this, new EventArgs());
        }

        private MemberBasic generateMemberBasicFromUser()
        {
            String teacherName = ((TextBox)FindName("teacherName")).Text;
            String teacherEngName = ((TextBox)FindName("teacherEngName")).Text;
            String gender = ((TextBox)FindName("genderTextBox")).Text;
            String idcardno = ((TextBox)FindName("teacherIdCardNo")).Text;
            return new MemberBasic(teacherName, teacherEngName, gender, idcardno, true);
        }

        private MemberMoreInfo generateMemberMoreInfoFromUser()
        {
            DateTime birthdayYangli = (DateTime)((DatePicker)FindName("birthdayYangli")).SelectedDate;
            DateTime birthdayNongli = (DateTime)((DatePicker)FindName("birthdayNongli")).SelectedDate;
            string minzu = ((TextBox)FindName("minzu")).Text;
            string birthdayPlace = ((TextBox)FindName("birthdayplace")).Text;
            string nowaddress = ((TextBox)FindName("nowaddress")).Text;
            string residenceaddress = ((TextBox)FindName("residenceaddress")).Text;
            string phone = ((TextBox)FindName("phone")).Text;
            string qq = ((TextBox)FindName("qq")).Text;
            DateTime graduateddate = (DateTime)((DatePicker)FindName("graducatedDate")).SelectedDate;
            string profession = ((TextBox)FindName("profession")).Text;
            string forte = ((TextBox)FindName("forte")).Text;
            string graduatedschool = ((TextBox)FindName("graduatedschool")).Text;
            string putonghualevel = ((TextBox)FindName("putonghuaLevelTextBox")).Text;
            string computerlevel = ((TextBox)FindName("computerlevel")).Text;
            string selfevaluation = ((TextBox)FindName("selfvaluation")).Text;
            string educationbackground = ((TextBox)FindName("educationbackgroundTextBox")).Text;


            MemberMoreInfo moreInfoObj = new MemberMoreInfo();
            moreInfoObj.birthdayNongli = birthdayNongli;
            moreInfoObj.birthdayYangli = birthdayYangli;
            moreInfoObj.minzu = minzu;
            moreInfoObj.birthPlace = birthdayPlace;
            moreInfoObj.nowaddress = nowaddress;
            moreInfoObj.residenceaddress = residenceaddress;
            moreInfoObj.phone = phone;
            moreInfoObj.qq = qq;
            moreInfoObj.graduateddate = graduateddate;
            moreInfoObj.profession = profession;
            moreInfoObj.forte = forte;
            moreInfoObj.graduatedschool = graduatedschool;
            moreInfoObj.educationbackground = educationbackground;
            moreInfoObj.putonghualevel = putonghualevel;
            moreInfoObj.computerlevel = computerlevel;
            moreInfoObj.selfevaluation = selfevaluation;
            moreInfoObj.photopath = realPhotoPathName;
            return moreInfoObj;
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
            int familycount = memberFamilyCollection.Count;
            //MemberFamily tempfamily = null;
            //for (int i = 0; i < familycount; i++)
            //{
            //    tempfamily = memberFamilyCollection[i];
            //    string insertMemberFamilySql = "";
            //    insertMemberFamilySql += "(" + tempfamily.name + "," + tempfamily.relationship + "," + tempfamily.phone + "," + tempfamily.idcardno + "," + tempfamily.pickup + "," + tempfamily.emergencycontact + ")";

            //}
        }

        private void dynamicAddAwardBtn_Click(object sender, RoutedEventArgs e)
        {
            addAward(new AwardOrPunishment());
        }

        private void dynamicAddExprienceBtn_Click(object sender, RoutedEventArgs e)
        {
            addExprience(new EducationAndEmployeeExprience());
        }

    }
}

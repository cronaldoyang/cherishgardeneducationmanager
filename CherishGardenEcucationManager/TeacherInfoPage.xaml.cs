using CherishGardenEducationManager.Entity;
using CherishGardenEducationManager.Helper;
using System;
using System.Collections.Generic;
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

        public TeacherInfoPage()
        {
            InitializeComponent();
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

            //2. Generate the memory objects.
            MemberBasic basicObj = generateMemberBasicFromUser();
            Console.Write(basicObj.ToString());
            MemberMoreInfo moreInfoObj = generateMemberMoreInfoFromUser();


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
            BitmapImage photo = (BitmapImage)((Image)FindName("photo")).Source;

            MemberMoreInfo moreInfoObj = new MemberMoreInfo();
            moreInfoObj.birthdayNongli = birthdayNongli;
            moreInfoObj.birthdayYangli = birthdayYangli;
            moreInfoObj.minzu = minzu;
            moreInfoObj.birthdayPlace = birthdayPlace;
            moreInfoObj.nowaddress = nowaddress;
            moreInfoObj.residenceaddress = residenceaddress;
            moreInfoObj.phone = phone;
            moreInfoObj.qq = qq;
            moreInfoObj.graduateddate = graduateddate;
            moreInfoObj.profession = profession;
            moreInfoObj.forte = forte;
            moreInfoObj.graduatedschool = graduatedschool;
            moreInfoObj.putonghualevel = putonghualevel;
            moreInfoObj.computerlevel = computerlevel;
            moreInfoObj.selfevaluation = selfevaluation;
            moreInfoObj.photo = photo;
            return moreInfoObj;
        }

        private void choosePhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            string  realPhotoName = ChoosePhotoHelper.getFilenameFromUserChoose();
            Button choosePhotoPBtn = (Button)sender;
            Image photo = (Image)FindName("photo");
            choosePhotoPBtn.Visibility = System.Windows.Visibility.Collapsed;
            photo.Visibility = System.Windows.Visibility.Visible;
            if (!realPhotoName.Equals(""))
            {
                photo.Source = ChoosePhotoHelper.getRealPhoto(realPhotoName);
            }
        }

        private void gridaddrowBtn_Click(object sender, RoutedEventArgs e)
        {
            Grid memberFamilyGrid = (Grid)FindName("memberFamilyGrid");
            memberFamilyGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
        }

    }
}

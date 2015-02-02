using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Entity;
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
    /// Interaction logic for ClassPage.xaml
    /// </summary>
    public partial class ClassPage : Page
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();


        ObservableCollection<MemberBasic> allTeachersCollection;
        ObservableCollection<Grade> allGradesCollection;
        ObservableCollection<Class> allClassesCollection;


        DataGrid allClassesDataGrid;
        DataGrid allGradesDataGrid;
        DataGridComboBoxColumn dgClassColumn;
        DataGridComboBoxColumn dgClassGroupColumn;

        public ClassPage()
        {
            InitializeComponent();
            allClassesDataGrid = (DataGrid)FindName("AllClassesDataGrid");
            allGradesDataGrid = (DataGrid)FindName("AllGradesDataGrid");
            dgClassColumn = (DataGridComboBoxColumn)FindName("headTeacherColumn");
            dgClassGroupColumn = (DataGridComboBoxColumn)FindName("classGroupColumn");
            worker.DoWork += worker_initData;
            worker.RunWorkerCompleted += worker_initDataCompleted;
            worker.RunWorkerAsync();
        }

        public void worker_initData(object sender, DoWorkEventArgs e)
        {
            allTeachersCollection = DatabaseHelper.getAllTeachers();
            allGradesCollection = DatabaseHelper.getAllGrades();
        }

        public void worker_initDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            allClassesCollection = new ObservableCollection<Class>();
            dgClassColumn.ItemsSource = allTeachersCollection;
            dgClassGroupColumn.ItemsSource = allGradesCollection;
            allClassesDataGrid.ItemsSource = allClassesCollection;
            AllGradesDataGrid.ItemsSource = allGradesCollection;

        }
        private void dynamicAddClassBtn_Click(object sender, RoutedEventArgs e)
        {
            //allClaasesCollection.Add(new Class());
            allClassesCollection.Add(new Class());
        }

        private void ClassPageSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHelper.SaveClassesInfo(allClassesCollection);
        }

        private void dynamicAddGradeBtn_Click(object sender, RoutedEventArgs e)
        {
            allGradesCollection.Add(new Grade());
        }

        private void saveGradesBtn_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHelper.SaveGradesInfo(allGradesCollection);
            //reload gradesData;
            allGradesCollection = DatabaseHelper.getAllGrades();
        }
    }
}

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
    /// Interaction logic for ClassPage.xaml
    /// </summary>
    public partial class ClassPage : Page
    {
        private readonly BackgroundWorker workerForInitData = new BackgroundWorker();
        private readonly BackgroundWorker workerForSaveAndUpdateGradesData = new BackgroundWorker();
        private readonly BackgroundWorker workerForSaveAndUpdateClassesData = new BackgroundWorker();


        public delegate void UpdateAllGradesDataCallback(string message);



        ObservableCollection<MemberBasic> allTeachersCollection{ get; set; }
        ObservableCollection<Grade> allGradesForDataGridCollection{ get; set; }
        ObservableCollection<Grade> allGradesForClassDataGridGradeColumnCollection{ get; set; }
        ObservableCollection<Class> allClassesCollection{ get; set; }


        DataGrid allClassesDataGrid;
        DataGrid allGradesDataGrid;
        DataGridComboBoxColumn dgHeadTeacherColumn;
        DataGridComboBoxColumn dgGradeColumn;

        public ClassPage()
        {
            InitializeComponent();
            allClassesDataGrid = (DataGrid)FindName("AllClassesDataGrid");
            allGradesDataGrid = (DataGrid)FindName("AllGradesDataGrid");
            dgHeadTeacherColumn = (DataGridComboBoxColumn)FindName("headTeacherColumn");
            dgGradeColumn = (DataGridComboBoxColumn)FindName("greadColumn");
            workerForInitData.DoWork += worker_initData;
            workerForInitData.RunWorkerCompleted += worker_initDataCompleted;
            workerForInitData.RunWorkerAsync();
        }

        public void worker_initData(object sender, DoWorkEventArgs e)
        {
            allGradesForClassDataGridGradeColumnCollection =DatabaseHelper.getAllGrades();
            allTeachersCollection = DatabaseHelper.getAllTeachers();
            allGradesForDataGridCollection = new ObservableCollection<Grade>(allGradesForClassDataGridGradeColumnCollection);
            allClassesCollection = DatabaseHelper.getAllClasses();
        }

        public void worker_initDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgGradeColumn.ItemsSource = allGradesForClassDataGridGradeColumnCollection;
            dgHeadTeacherColumn.ItemsSource = allTeachersCollection;
            allClassesDataGrid.ItemsSource = allClassesCollection;
            allGradesDataGrid.ItemsSource = allGradesForDataGridCollection;

        }

        private void dynamicAddClassBtn_Click(object sender, RoutedEventArgs e)
        {
            allClassesCollection.Add(new Class());
        }

        private void ClassPageSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            workerForSaveAndUpdateClassesData.DoWork += worker_saveClassesData;
            workerForSaveAndUpdateClassesData.RunWorkerCompleted += worker_saveClassesDataCompleted;
            workerForSaveAndUpdateClassesData.RunWorkerAsync();

        }

        private void worker_saveClassesData(object sender, DoWorkEventArgs e)
        {
            ObservableCollection<Class> newAddedClasses = new ObservableCollection<Class>();
            ObservableCollection<Class> OldClasses = new ObservableCollection<Class>();
            foreach (Class _class in allClassesCollection)
            {
                if (_class.name.Equals("")) continue;
                if (_class.id == -1)
                {
                    newAddedClasses.Add(_class);
                }
                else
                {
                    OldClasses.Add(_class);
                }
            }
            DatabaseHelper.SaveClassesInfo(OldClasses, newAddedClasses);
            allClassesCollection = DatabaseHelper.getAllClasses();
        }

        private void worker_saveClassesDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            allClassesDataGrid.ItemsSource = allClassesCollection;
        }

        private void dynamicAddGradeBtn_Click(object sender, RoutedEventArgs e)
        {
            allGradesForDataGridCollection.Add(new Grade());
        }

        private void saveGradesBtn_Click(object sender, RoutedEventArgs e)
        {
            workerForSaveAndUpdateGradesData.DoWork += worker_saveGradesData;
            workerForSaveAndUpdateGradesData.RunWorkerCompleted += worker_saveGradesDataCompleted;
            workerForSaveAndUpdateGradesData.RunWorkerAsync();
        }

        private void worker_saveGradesDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //allGradesDataGrid.ItemsSource = allGradesForDataGridCollection;
        }

        private void worker_saveGradesData(object sender, DoWorkEventArgs e)
        {
            ObservableCollection<Grade> newAddedGrades = new ObservableCollection<Grade>();
            ObservableCollection<Grade> oldGrades = new ObservableCollection<Grade>();
            foreach (Grade grade in allGradesForDataGridCollection)
            {
                if (grade.name.Equals("")) continue;
                if (grade.id == -1)
                {
                    //this means new add grade.
                    newAddedGrades.Add(grade);
                }
                else
                {
                    oldGrades.Add(grade);
                }
            }
            DatabaseHelper.SaveGradesInfo(oldGrades, newAddedGrades);
            //reload gradesData from database;
            allGradesForDataGridCollection = DatabaseHelper.getAllGrades();
        }

    }
}

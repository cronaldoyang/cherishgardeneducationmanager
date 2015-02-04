using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Entity;
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
    /// Interaction logic for ClassPage.xaml
    /// </summary>
    public partial class ClassPage : Page
    {
        private readonly BackgroundWorker workerForInitData = new BackgroundWorker();
        private readonly BackgroundWorker workerForSaveAndUpdateGradesData = new BackgroundWorker();
        private readonly BackgroundWorker workerForSaveAndUpdateClassesData = new BackgroundWorker();

        DataGrid allClassesDataGrid;
        DataGrid allGradesDataGrid;
        
        public ClassPage()
        {
            InitializeComponent();
            initData();
        }

        public  void initData()
        {
            workerForInitData.DoWork += worker_initData;
            workerForInitData.RunWorkerCompleted += worker_initDataCompleted;
            workerForInitData.RunWorkerAsync();
        }

        public void worker_initData(object sender, DoWorkEventArgs e)
        {
            ClassViewModel.getInstance().worker_initData();
        }

        public void worker_initDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            allClassesDataGrid = (DataGrid)FindName("AllClassesDataGrid");
            allGradesDataGrid = (DataGrid)FindName("AllGradesDataGrid");
            allClassesDataGrid.ItemsSource = ClassViewModel.getInstance().allClasses;
            allGradesDataGrid.ItemsSource = ClassViewModel.getInstance().allGrades;
        }

        private void dynamicAddClassBtn_Click(object sender, RoutedEventArgs e)
        {
            ClassViewModel.getInstance().addNewClassRecord();
        }

        private void ClassPageSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            workerForSaveAndUpdateClassesData.DoWork += worker_saveClassesData;
            workerForSaveAndUpdateClassesData.RunWorkerCompleted += worker_saveClassesDataCompleted;
            workerForSaveAndUpdateClassesData.RunWorkerAsync();
        }

        private void worker_saveClassesData(object sender, DoWorkEventArgs e)
        {
            ClassViewModel.getInstance().saveAllClassesData();
        }

        private void worker_saveClassesDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            allClassesDataGrid.ItemsSource = ClassViewModel.getInstance().allClasses;
        }

        private void dynamicAddGradeBtn_Click(object sender, RoutedEventArgs e)
        {
            ClassViewModel.getInstance().addNewGradeRecord();
        }

        private void saveGradesBtn_Click(object sender, RoutedEventArgs e)
        {
            workerForSaveAndUpdateGradesData.DoWork += worker_saveGradesData;
            workerForSaveAndUpdateGradesData.RunWorkerCompleted += worker_saveGradesDataCompleted;
            workerForSaveAndUpdateGradesData.RunWorkerAsync();
        }

        private void worker_saveGradesData(object sender, DoWorkEventArgs e)
        {
            ClassViewModel.getInstance().saveAllGradesData();
        }

        private void worker_saveGradesDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            allGradesDataGrid.ItemsSource = ClassViewModel.getInstance().allGrades;
        }
    }
}

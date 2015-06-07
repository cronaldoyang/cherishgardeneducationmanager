using CherishGardenEducationManager.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for WeeklyReport.xaml
    /// </summary>
    public partial class WeeklyReport : Page
    {
        private readonly BackgroundWorker workerForInitData = new BackgroundWorker();

        public WeeklyReport()
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
            WeeklyReportViewModel.getInstance().initData();
        }

        void worker_initDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SchoolYearTextBlock.Text = WeeklyReportViewModel.getInstance().schoolYearForDisplay;
            ClassesBox.ItemsSource = ClassViewModel.getInstance().allClasses;
            ClassesBox.SelectedIndex = ClassViewModel.getInstance().getClassIndexById(WeeklyReportViewModel.getInstance().selectedClassId);
            WeekNoTextBlock.Text = "第" + WeeklyReportViewModel.getInstance().weekNoForDisplay + "周";

            ListCollectionView collectionLive = new ListCollectionView(WeeklyReportViewModel.getInstance().weeklyReportLiveItems);
            collectionLive.GroupDescriptions.Add(new PropertyGroupDescription("coursename"));
            weeklyReportLiveDataGrid.ItemsSource = collectionLive;

            ListCollectionView collectionDuoYuan = new ListCollectionView(WeeklyReportViewModel.getInstance().weeklyReportDuoYuanItems);
            collectionDuoYuan.GroupDescriptions.Add(new PropertyGroupDescription("coursename"));
            weeklyReportDuoyuanDataGrid.ItemsSource = collectionDuoYuan;

            ListCollectionView collectionRisk = new ListCollectionView(WeeklyReportViewModel.getInstance().weeklyReportRiskItems);
            collectionRisk.GroupDescriptions.Add(new PropertyGroupDescription("coursename"));
            weeklyReportRiskDataGrid.ItemsSource = collectionRisk;

            ListCollectionView collectionEvaluation = new ListCollectionView(WeeklyReportViewModel.getInstance().weeklyReportEvaluationItems);
            collectionEvaluation.GroupDescriptions.Add(new PropertyGroupDescription("coursename"));
            weeklyReportEvaluationDataGrid.ItemsSource = collectionEvaluation;

            ListCollectionView collectionFamilyAndSchollEdu = new ListCollectionView(WeeklyReportViewModel.getInstance().weeklyReportFamilyAndSchollEduItems);
            collectionFamilyAndSchollEdu.GroupDescriptions.Add(new PropertyGroupDescription("coursename"));
            weeklyReportFamilyAndSchoolEduDataGrid.ItemsSource = collectionFamilyAndSchollEdu;
        }
    }
}

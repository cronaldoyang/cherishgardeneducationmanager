using CherishGardenEducationManager.Database;
using CherishGardenEducationManager.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CherishGardenEducationManager.ViewModel
{
    class MemberAllInfoViewModel
    {
        private static volatile MemberAllInfoViewModel instance;
        private static object m_lock = new object();

        public MemberBasic basic{ get; set; }
        public MemberMoreInfo moreInfo { get; set; }
        public ObservableCollection<Exprience> allExpriences { get; set; }
        public ObservableCollection<MemberFamily> allMemberFamily { get; set; }
        public ObservableCollection<AwardOrPunishment> allAwards { get; set; }

        public static MemberAllInfoViewModel getInstance()
        {
            // DoubleLock
            if (instance == null)
            {
                lock (m_lock)
                {
                    if (instance == null)
                    {
                        instance = new MemberAllInfoViewModel();
                    }
                }
            }
            return instance;
        }

        public void initData()
        {
            basic = DatabaseHelper.getMemberBasicInfoFromDB("cronaldo");
            moreInfo = DatabaseHelper.getMemberMoreInfoFromDB(basic.id);
            allExpriences = DatabaseHelper.getExpriencesByMemberBasicIdFromDB(basic.id);
            allMemberFamily = DatabaseHelper.getMemberFamilyByMemberBasicIdFromDB(basic.id);
            allAwards = DatabaseHelper.getAwardsByMemberBasicIdFromDB(basic.id);
        }

        public void addMemberFamily()
        {
            allMemberFamily.Add(new MemberFamily());
        }

        public void addExprience()
        {
            allExpriences.Add(new Exprience());
        }

        public void addAward()
        {
            allAwards.Add(new AwardOrPunishment());
        }

        public void saveMemberInfo()
        {
            //TODO this should be save new data and updata
            DatabaseHelper.SaveMemberAllInfo(basic, moreInfo, allMemberFamily, allExpriences, allAwards, null);
        }
    }
}

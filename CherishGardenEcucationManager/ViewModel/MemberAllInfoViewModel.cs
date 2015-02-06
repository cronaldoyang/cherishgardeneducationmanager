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
        public PhysicMoreinfo physicMoreInfo { get; set; }
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

        public void initData(Boolean fromStudentPage)
        {
            reset();
            string engname = fromStudentPage ? "hellen" :"cronaldo";
            basic = DatabaseHelper.getMemberBasicInfoFromDB(engname);
            moreInfo = DatabaseHelper.getMemberMoreInfoFromDB(basic.id, basic.isteacher);
            allExpriences = DatabaseHelper.getExpriencesByMemberBasicIdFromDB(basic.id);
            allMemberFamily = DatabaseHelper.getMemberFamilyByMemberBasicIdFromDB(basic.id);
            allAwards = DatabaseHelper.getAwardsByMemberBasicIdFromDB(basic.id);
            if (fromStudentPage) physicMoreInfo = DatabaseHelper.getPhysicMoreInfo(basic.id);
        }

        public void reset()
        {
            basic = null;
            moreInfo = null;
            allExpriences = null;
            allMemberFamily = null;
            allAwards = null;
            physicMoreInfo = null;
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

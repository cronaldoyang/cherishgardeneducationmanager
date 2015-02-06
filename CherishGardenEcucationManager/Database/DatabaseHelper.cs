using CherishGardenEducationManager.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CherishGardenEducationManager.Database
{
    class DatabaseHelper
    {
        private static string CONNECTIONSTR = ConfigurationManager.ConnectionStrings["cgemConnectionstring"].ConnectionString;
        private static string SAVEMEMBERINFO_SP = "savememberinfo_sp";

        //Judge the user login success?
        public static OperatorUser findOperatorUser(string loginuser)
        {
            OperatorUser user = null;
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTR);
            conn.Open();
            MySqlTransaction trans = conn.BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                string query = "select password, mbid from operatorinfo where mbid in (select _id from memberbasic where engname=@username)";
                cmd.Parameters.AddWithValue("@username", loginuser);
                cmd.CommandText = query;
                MySqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    Console.WriteLine("no data!");
                }
                else
                {
                    while (reader.Read())
                    {
                        string password = reader[0].ToString();
                        int mbid = (int)reader[1];
                        user = new OperatorUser(password, mbid);
                    }
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return user;
        }

        //Save memberinfo;
        public static Boolean SaveMemberAllInfo(MemberBasic basicobj, MemberMoreInfo moreobj,
            ObservableCollection<MemberFamily> familyCollection,
            ObservableCollection<Exprience> exprienceCollection,
            ObservableCollection<AwardOrPunishment> awardsCollection,
            PhysicMoreinfo physicMoreInfoObj)
        {
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTR);
            conn.Open();
            MySqlTransaction trans = conn.BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand(SAVEMEMBERINFO_SP, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Transaction = trans;

                /**turn off autocommit */
                //cmd.CommandType = CommandType.Text;
                //cmd.CommandText = "SET autocommit = 0";
                //cmd.ExecuteNonQuery();

                //fill the memberbasic parameters.
                cmd.Parameters.AddWithValue("@basicid", basicobj.id);
                cmd.Parameters.AddWithValue("@basicname", basicobj.name);
                cmd.Parameters.AddWithValue("@basicengname", basicobj.engname);
                cmd.Parameters.AddWithValue("@basicgender", basicobj.gender);
                cmd.Parameters.AddWithValue("@basicidcardno", basicobj.idcardno);
                cmd.Parameters.AddWithValue("@basicisteacher", basicobj.isteacher ? 1 : 0);
                //fill the membermoreinfo parameters
                cmd.Parameters.AddWithValue("@birthdayyangli", moreobj.birthdayyangli);
                cmd.Parameters.AddWithValue("@birthdaynongli", moreobj.birthdaynongli);
                cmd.Parameters.AddWithValue("@minzu", moreobj.minzu);
                cmd.Parameters.AddWithValue("@birthplace", moreobj.birthplace);
                cmd.Parameters.AddWithValue("@nowaddress", moreobj.nowaddress);
                cmd.Parameters.AddWithValue("@residenceaddress", moreobj.residenceaddress);
                cmd.Parameters.AddWithValue("@photopath", moreobj.photopath);
                cmd.Parameters.AddWithValue("@phone", moreobj.phone);
                cmd.Parameters.AddWithValue("@qq", moreobj.qq);
                cmd.Parameters.AddWithValue("@graduated", moreobj.graduateddate);
                cmd.Parameters.AddWithValue("@profession", moreobj.profession);
                cmd.Parameters.AddWithValue("@forte", moreobj.forte);
                cmd.Parameters.AddWithValue("@educationbackground", moreobj.educationbackground);
                cmd.Parameters.AddWithValue("@graduatedschool", moreobj.graduatedschool);
                cmd.Parameters.AddWithValue("@putonghualevel", moreobj.putonghualevel);
                cmd.Parameters.AddWithValue("@computerlevel", moreobj.computerlevel);
                cmd.Parameters.AddWithValue("@selfevaluation", moreobj.selfevaluation);
                //fill the output parameter
                MySqlParameter resultParameter = new MySqlParameter("@result", MySqlDbType.Int16);
                resultParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(resultParameter);
                MySqlParameter basicRecordIdParameter = new MySqlParameter("@basicRecordId", MySqlDbType.Int16);
                basicRecordIdParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(basicRecordIdParameter);


                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.Text;

                //
                int result = Convert.ToInt16(resultParameter.Value);
                int basicId = Convert.ToInt16(basicRecordIdParameter.Value);
                if (result > 0 && basicId > 0)
                {
                    //save member family records;
                    //save member family records;
                    //save exprience records;
                    ObservableCollection<MemberFamily> updateFamilyCollection = new ObservableCollection<MemberFamily>();
                    ObservableCollection<MemberFamily> insertFamilyCollection = new ObservableCollection<MemberFamily>();
                    foreach (MemberFamily family in familyCollection)
                    {
                        if (family.id == 0)
                        {
                            insertFamilyCollection.Add(family);
                        }
                        else
                        {
                            updateFamilyCollection.Add(family);
                        }
                    }
                    int insertfamilycount = insertFamilyCollection.Count;
                    if (insertfamilycount > 0)
                    {
                        string insertMemberFamilySql = "insert into memberfamily(name, relation,mobilephone,idcardno,address, pickup,emergencycontact,mbid) values ";
                        MemberFamily tempfamily = null;
                        for (int i = 0; i < insertfamilycount; i++)
                        {
                            tempfamily = insertFamilyCollection[i];
                            insertMemberFamilySql += "('" + tempfamily.name + "','" + tempfamily.relationship + "','" + tempfamily.phone + "','" + tempfamily.idcardno + "','" + tempfamily.address +  "'," + (tempfamily.pickup ? 1 : 0) + "," + (tempfamily.emergencycontact ? 1 : 0) + "," + basicId + ")";
                            if (i < insertfamilycount - 1)
                            {
                                insertMemberFamilySql += ",";
                            }
                            else
                            {
                                insertMemberFamilySql += ";";
                            }

                        }
                        cmd.CommandText = insertMemberFamilySql;
                        cmd.ExecuteNonQuery();
                    }

                    //update family record;
                    int updatefamilycount = updateFamilyCollection.Count;
                     if (updatefamilycount > 0)
                     {
                         string updateExprienceSql = "update memberfamily set ";
                         string nameUpdateSql = " name=case _id ";
                         string relationUpdateSql = " relation=case _id ";
                         string mobilephoneUpdateSql = " mobilephone=case _id ";
                         string idcardnoUpdateSql = " idcardno=case _id ";
                         string pickupUpdateSql = " pickup=case _id ";
                         string emergencycontactUpdateSql = " emergencycontact=case _id ";
                         string addressUpdateSql = " address=case _id ";
                         string wheresql = "where _id in (";
                         MemberFamily tempFamily = null;
                         for (int i = 0; i < updatefamilycount; i++)
                         {
                             tempFamily = updateFamilyCollection[i];
                             nameUpdateSql += " when " + tempFamily.id + " then '" + tempFamily.name + "'";
                             relationUpdateSql += " when " + tempFamily.id + " then '" + tempFamily.relationship + "'";
                             mobilephoneUpdateSql += " when " + tempFamily.id + " then '" + tempFamily.phone + "'";
                             idcardnoUpdateSql += " when " + tempFamily.id + " then '" + tempFamily.idcardno + "'";
                             pickupUpdateSql += " when " + tempFamily.id + " then " + (tempFamily.pickup==true ? 1 :0) + "";
                             emergencycontactUpdateSql += " when " + tempFamily.id + " then " + (tempFamily.emergencycontact==true ? 1 : 0) + "";
                             addressUpdateSql += " when " + tempFamily.id + " then '" + tempFamily.address + "'";

                             wheresql += tempFamily.id;
                             if (i < updatefamilycount - 1)
                             {
                                 wheresql += ",";
                             }
                             else
                             {
                                 nameUpdateSql += " end, ";
                                 relationUpdateSql += " end, ";
                                 mobilephoneUpdateSql += " end, ";
                                 idcardnoUpdateSql += " end, ";
                                 pickupUpdateSql += " end, ";
                                 emergencycontactUpdateSql += " end, ";
                                 addressUpdateSql += " end ";
                                 wheresql += ");";
                             }
                         }
                         cmd.CommandText = updateExprienceSql + nameUpdateSql + relationUpdateSql + mobilephoneUpdateSql + idcardnoUpdateSql + pickupUpdateSql + emergencycontactUpdateSql + addressUpdateSql + wheresql;
                         cmd.ExecuteNonQuery();
                     }


                    //save exprience records;
                    ObservableCollection<Exprience> updateExpriencesCollection = new ObservableCollection<Exprience>();
                    ObservableCollection<Exprience> insertexpriencesCollection = new ObservableCollection<Exprience>();
                    foreach (Exprience exprience in exprienceCollection)
                    {
                        if (exprience.id == 0)
                        {
                            insertexpriencesCollection.Add(exprience);
                        }
                        else
                        {
                            updateExpriencesCollection.Add(exprience);
                        }
                    }

                    int insertExprienceCount = insertexpriencesCollection.Count;
                    if (insertExprienceCount > 0)
                    {
                        string insetrExprienceSql = "insert into exprience(fromdate, todate,address,positions,responsibility,mbid) values ";
                        Exprience tempExprience = null;
                        for (int i = 0; i < insertExprienceCount; i++)
                        {
                            tempExprience = insertexpriencesCollection[i];
                            insetrExprienceSql += "('" + tempExprience.from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "','" + tempExprience.to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "','" + tempExprience.address + "','" + tempExprience.positions + "','" + tempExprience.responsibility + "'," + basicId + ")";
                            if (i < insertExprienceCount - 1)
                            {
                                insetrExprienceSql += ",";
                            }
                            else
                            {
                                insetrExprienceSql += ";";
                            }
                        }
                        cmd.CommandText = insetrExprienceSql;
                        cmd.ExecuteNonQuery();
                    }

                    //update all expriences
                    int updateExprienceCount = updateExpriencesCollection.Count;
                    if (updateExprienceCount > 0)
                    {
                        string updateExprienceSql = "update exprience set ";
                        string fromdateUpdateSql = " fromdate=case _id ";
                        string todateUpdateSql = " todate=case _id ";
                        string addressUpdateSql = " address=case _id ";
                        string positionsUpdateSql = " positions=case _id ";
                        string responsibilityUpdateSql = " responsibility=case _id ";
                        string wheresql = "where _id in (";
                        Exprience tempExprience = null;
                        for (int i = 0; i < updateExprienceCount; i++)
                        {
                            tempExprience = updateExpriencesCollection[i];
                            fromdateUpdateSql += " when " + tempExprience.id + " then '" + tempExprience.from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'";
                            todateUpdateSql += " when " + tempExprience.id + " then '" + tempExprience.to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'";
                            addressUpdateSql += " when " + tempExprience.id + " then '" + tempExprience.address + "'";
                            positionsUpdateSql += " when " + tempExprience.id + " then '" + tempExprience.positions + "'";
                            responsibilityUpdateSql += " when " + tempExprience.id + " then '" + tempExprience.responsibility + "'";
                            wheresql += tempExprience.id;
                            if (i < updateExprienceCount - 1)
                            {
                                wheresql += ",";
                            }
                            else
                            {
                                fromdateUpdateSql += " end, ";
                                todateUpdateSql += " end, ";
                                addressUpdateSql += " end, ";
                                positionsUpdateSql += " end, ";
                                responsibilityUpdateSql += " end ";
                                wheresql += ");";
                            }
                        }
                        cmd.CommandText = updateExprienceSql + fromdateUpdateSql + todateUpdateSql + addressUpdateSql + positionsUpdateSql + responsibilityUpdateSql + wheresql;
                        cmd.ExecuteNonQuery();
                    }


                    //save award records;
                    ObservableCollection<AwardOrPunishment> updateAwardsCollection = new ObservableCollection<AwardOrPunishment>();
                    ObservableCollection<AwardOrPunishment> insertAwardsCollection = new ObservableCollection<AwardOrPunishment>();

                    foreach (AwardOrPunishment award in awardsCollection)
                    {
                        if (award.id == 0)
                        {
                            insertAwardsCollection.Add(award);
                        }
                        else
                        {
                            updateAwardsCollection.Add(award);
                        }
                    }

                    int insertAwardsCount = insertAwardsCollection.Count;
                    if (insertAwardsCount > 0)
                    {
                        string insertAwardsSql = "insert into awardspunishmentsinfo(date, content,organization,mbid) values ";
                        AwardOrPunishment tempAward = null;
                        for (int i = 0; i < insertAwardsCount; i++)
                        {
                            tempAward = insertAwardsCollection[i];
                            insertAwardsSql += "('" + tempAward.date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "','" + tempAward.content + "','" + tempAward.organization + "'," + basicId + ")";
                            if (i < insertAwardsCount - 1)
                            {
                                insertAwardsSql += ",";
                            }
                            else
                            {
                                insertAwardsSql += ";";
                            }
                        }
                        cmd.CommandText = insertAwardsSql;
                        cmd.ExecuteNonQuery();
                    }

                    int updateAwardsCount = updateAwardsCollection.Count;
                    if (updateAwardsCount > 0)
                    {
                        //update related awards.
                        string updateAwardsSql = "update awardspunishmentsinfo set ";
                        string dateUpdateSql = " date=case _id ";
                        string contentUpdateSql = " content=case _id ";
                        string organizationUpdateSql = " organization=case _id ";
                        string wheresql = "where _id in (";
                        AwardOrPunishment tempAward = null;
                        for (int i = 0; i < updateAwardsCount; i++)
                        {
                            tempAward = updateAwardsCollection[i];
                            dateUpdateSql += " when " + tempAward.id + " then '" + tempAward.date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'";
                            contentUpdateSql += " when " + tempAward.id + " then '" + tempAward.content + "'";
                            organizationUpdateSql += " when " + tempAward.id + " then '" + tempAward.organization + "'";
                            wheresql += tempAward.id;
                            if (i < updateAwardsCount - 1)
                            {
                                wheresql += ",";
                            }
                            else
                            {
                                dateUpdateSql += " end, ";
                                contentUpdateSql += " end, ";
                                organizationUpdateSql += " end ";
                                wheresql += ");";
                            }
                        }
                        cmd.CommandText = updateAwardsSql + dateUpdateSql + contentUpdateSql + organizationUpdateSql + wheresql;
                        cmd.ExecuteNonQuery();
                    }

                    //save physic more info obj;
                    if (physicMoreInfoObj != null)
                    {
                        string insertphysicMoreInfoSql = "insert into physicmoreinfo(haveFoodallergy, foodallergyinfo, haveConvulsionsepilepsy, haveBraindiseases, haveAcutechronicinfectious, haveheartdiseases, haverenaldisease, havedrugallergy, drugallergy, mark, mbid) values " +
                            "(@haveFoodallergy, @foodallergyinfo, @haveConvulsionsepilepsy, @haveBraindiseases, @haveAcutechronicinfectious, @haveheartdiseases, @haverenaldisease, @havedrugallergy, @drugallergy, @mark, @mbid); ";
                        cmd.Parameters.AddWithValue("@haveFoodallergy", physicMoreInfoObj.haveFoodallergy ? 1 : 0);
                        cmd.Parameters.AddWithValue("@foodallergyinfo", physicMoreInfoObj.foodallergyinfo);
                        cmd.Parameters.AddWithValue("@haveConvulsionsepilepsy", physicMoreInfoObj.haveConvulsionsepilepsy ? 1 : 0);
                        cmd.Parameters.AddWithValue("@haveBraindiseases", physicMoreInfoObj.haveBraindiseases ? 1 : 0);
                        cmd.Parameters.AddWithValue("@haveAcutechronicinfectious", physicMoreInfoObj.haveAcutechronicinfectious ? 1 : 0);
                        cmd.Parameters.AddWithValue("@haveheartdiseases", physicMoreInfoObj.haveheartdiseases ? 1 : 0);
                        cmd.Parameters.AddWithValue("@haverenaldisease", physicMoreInfoObj.haverenaldisease ? 1 : 0);
                        cmd.Parameters.AddWithValue("@havedrugallergy", physicMoreInfoObj.havedrugallergy ? 1 : 0);
                        cmd.Parameters.AddWithValue("@drugallergy", physicMoreInfoObj.drugallergy);
                        cmd.Parameters.AddWithValue("@mark", physicMoreInfoObj.mark);
                        cmd.Parameters.AddWithValue("@mbid", basicId);
                        cmd.CommandText = insertphysicMoreInfoSql;
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {

                trans.Rollback();
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        //Save classes info;
        public static Boolean SaveClassesInfo(ObservableCollection<Class> oldClasses, ObservableCollection<Class> newClasses)
        {
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTR);
            conn.Open();
            MySqlTransaction trans = conn.BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = trans;

                //update old classes
                int oldGradesCount = oldClasses.Count;
                if (oldGradesCount > 0)
                {
                    // the last final string like this :
                    //update class 
                    //set name=case _id when 3 then 'shabi' when 4 then 'niubi' end,
                    //headerteacherid=case _id when 3 then 6 when 4 then 7' end, 
                    //gradeid=case _id when 3 then 5 when 4 then 6 end where _id in (3,4);
                    string updateClassesSql = "update class set ";
                    string nameUpdateSql = " classname=case _id ";
                    string headteacheridUpdateSql = " headteacherid=case _id ";
                    string gradeidUpdateSql = " gradeid=case _id ";
                    string wheresql = "where _id in (";
                    Class tempClass = null;
                    for (int i = 0; i < oldGradesCount; i++)
                    {
                        tempClass = oldClasses[i];
                        nameUpdateSql += " when " + tempClass.id + " then  '" + tempClass.name + "'";
                        headteacheridUpdateSql += " when " + tempClass.id + " then  " + tempClass.teacherid;
                        gradeidUpdateSql += " when " + tempClass.id + " then  " + tempClass.gradeid;
                        wheresql += tempClass.id;
                        if (i < oldGradesCount - 1)
                        {
                            wheresql += ",";
                        }
                        else
                        {
                            nameUpdateSql += " end, ";
                            headteacheridUpdateSql += " end, ";
                            gradeidUpdateSql += " end ";
                            wheresql += ");";
                        }
                    }
                    cmd.CommandText = updateClassesSql + nameUpdateSql + headteacheridUpdateSql + gradeidUpdateSql + wheresql;
                    cmd.ExecuteNonQuery();
                }

                //save physic more info obj;
                int newClassesCount = newClasses.Count;
                if (newClassesCount > 0)
                {
                    string insertClassesSql = "insert into class(classname, headteacherid, gradeid) values ";
                    Class tempClass = null;
                    for (int i = 0; i < newClassesCount; i++)
                    {
                        tempClass = newClasses[i];
                        insertClassesSql += "('" + tempClass.name + "'," + tempClass.teacherid + "," + tempClass.gradeid + ")";
                        if (i < newClassesCount - 1)
                        {
                            insertClassesSql += ",";
                        }
                        else
                        {
                            insertClassesSql += ";";
                        }
                    }
                    cmd.CommandText = insertClassesSql;
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        //Save Grades info;
        public static Boolean SaveGradesInfo(ObservableCollection<Grade> oldGrades, ObservableCollection<Grade> newGrades)
        {
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTR);
            conn.Open();
            MySqlTransaction trans = conn.BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = trans;

                int oldGradesCount = oldGrades.Count;
                if (oldGradesCount > 0)
                {
                    // the last final string like this :
                    //update grade set name=case _id when 3 then 'shabi' when 4 then 'niubi' end where _id in (3,4);
                    string updateClassesSql = "update grade set name=case _id ";
                    string wheresql = "where _id in (";
                    Grade tempGrade = null;
                    for (int i = 0; i < oldGradesCount; i++)
                    {
                        tempGrade = oldGrades[i];
                        updateClassesSql += " when " + tempGrade.id + " then  '" + tempGrade.name + "'";
                        wheresql += tempGrade.id;
                        if (i < oldGradesCount - 1)
                        {
                            wheresql += ",";
                        }
                        else
                        {
                            updateClassesSql += " end ";
                            wheresql += ");";
                        }
                    }
                    cmd.CommandText = updateClassesSql + wheresql;
                    cmd.ExecuteNonQuery();
                }

                //save physic more info obj;
                int newgradesCount = newGrades.Count;
                if (newgradesCount > 0)
                {
                    string insertClassesSql = "insert into grade(name) values ";
                    Grade tempGrade = null;
                    for (int i = 0; i < newgradesCount; i++)
                    {
                        tempGrade = newGrades[i];
                        insertClassesSql += "('" + tempGrade.name + "')";
                        if (i < newgradesCount - 1)
                        {
                            insertClassesSql += ",";
                        }
                        else
                        {
                            insertClassesSql += ";";
                        }
                    }
                    cmd.CommandText = insertClassesSql;
                    cmd.ExecuteNonQuery();
                }

                //update old grades
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public static ObservableCollection<MemberBasic> getAllTeachers()
        {
            ObservableCollection<MemberBasic> allTeachers = new ObservableCollection<MemberBasic>();
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTR);
            conn.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                string queryAllTeachers = "select * from memberbasic where isteacher=@isteacher;";
                cmd.CommandText = queryAllTeachers;
                //if it's teacher, isteacher is 1.
                cmd.Parameters.AddWithValue("@isteacher", 1);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    Console.WriteLine("no data!");
                }
                else
                {
                    while (reader.Read())
                    {
                        int _id = (int)reader[0];
                        string nameValue = (string)reader[1];
                        string engnameValue = (string)reader[2];
                        string genderValue = (string)reader[3];
                        string idcardnoValue = (string)reader[4];
                        int isteacherValue = (int)reader[5];

                        allTeachers.Add(new MemberBasic()
                        {
                            id = _id,
                            name = nameValue,
                            engname = engnameValue,
                            gender = genderValue,
                            idcardno = idcardnoValue,
                            isteacher = isteacherValue == 1 ? true : false
                        });
                    }
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return allTeachers;
        }

        public static ObservableCollection<Grade> getAllGrades()
        {
            ObservableCollection<Grade> allGradesGroups = new ObservableCollection<Grade>();
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTR);
            conn.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                string queryAllGradesSql = "select * from grade;";
                cmd.CommandText = queryAllGradesSql;
                MySqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    Console.WriteLine("no data!");
                }
                else
                {
                    while (reader.Read())
                    {
                        int _id = (int)reader[0];
                        string nameValue = (string)reader[1];

                        allGradesGroups.Add(new Grade()
                        {
                            id = _id,
                            name = nameValue
                        });
                    }
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return allGradesGroups;
        }

        public static ObservableCollection<Class> getAllClasses()
        {
            ObservableCollection<Class> allClasses = new ObservableCollection<Class>();
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTR);
            conn.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                //get all grades.
                ObservableCollection<Grade> candidateGradesFromDB = new ObservableCollection<Grade>();
                string queryAllGradesSql = "select * from grade;";
                cmd.CommandText = queryAllGradesSql;
                MySqlDataReader readerGrades = cmd.ExecuteReader();
                if (!readerGrades.HasRows)
                {
                    Console.WriteLine("no data!");
                }
                else
                {
                    while (readerGrades.Read())
                    {
                        int _id = (int)readerGrades[0];
                        string nameValue = (string)readerGrades[1];

                        candidateGradesFromDB.Add(new Grade()
                        {
                            id = _id,
                            name = nameValue
                        });
                    }
                }
                readerGrades.Close();

                //get all teachers
                ObservableCollection<MemberBasic> candidateTeachersFromDB = new ObservableCollection<MemberBasic>();
                string queryAllTeachers = "select * from memberbasic where isteacher=@isteacher;";
                cmd.CommandText = queryAllTeachers;
                //if it's teacher, isteacher is 1.
                cmd.Parameters.AddWithValue("@isteacher", 1);
                MySqlDataReader readerTeachers = cmd.ExecuteReader();
                if (!readerTeachers.HasRows)
                {
                    Console.WriteLine("no data!");
                }
                else
                {
                    while (readerTeachers.Read())
                    {
                        int _id = (int)readerTeachers[0];
                        string nameValue = (string)readerTeachers[1];
                        string engnameValue = (string)readerTeachers[2];
                        string genderValue = (string)readerTeachers[3];
                        string idcardnoValue = (string)readerTeachers[4];
                        int isteacherValue = (int)readerTeachers[5];

                        candidateTeachersFromDB.Add(new MemberBasic()
                        {
                            id = _id,
                            name = nameValue,
                            engname = engnameValue,
                            gender = genderValue,
                            idcardno = idcardnoValue,
                            isteacher = isteacherValue == 1 ? true : false
                        });
                    }
                }
                readerTeachers.Close();

                string queryAllClassGroups = "select * from class;";
                cmd.CommandText = queryAllClassGroups;
                MySqlDataReader readerClasses = cmd.ExecuteReader();
                if (!readerClasses.HasRows)
                {
                    Console.WriteLine("no data!");
                }
                else
                {
                    while (readerClasses.Read())
                    {
                        int _id = (int)readerClasses[0];
                        string _name = (string)readerClasses[1];
                        int _headteacherid = (int)readerClasses[2];
                        int _gradeid = (int)readerClasses[3];

                        allClasses.Add(new Class()
                        {
                            id = _id,
                            name = _name,
                            teacherid = _headteacherid,
                            gradeid = _gradeid,
                            candidateGrades = candidateGradesFromDB,
                            candidateTeachers = candidateTeachersFromDB
                        });
                    }
                }
                readerClasses.Close();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return allClasses;
        }

        public static MemberBasic getMemberBasicInfoFromDB(string name)
        {
            MemberBasic basicobj = null;
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTR);
            conn.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                //get all grades.
                string queryMemberBasicSql = "select * from memberbasic where engname=@engname;";
                cmd.CommandText = queryMemberBasicSql;
                //if it's teacher, isteacher is 1.
                cmd.Parameters.AddWithValue("@engname", name);
                MySqlDataReader readerMemberInfo = cmd.ExecuteReader();
                if (!readerMemberInfo.HasRows)
                {
                    Console.WriteLine("no data!");
                }
                else
                {
                    while (readerMemberInfo.Read())
                    {
                        int _id = (int)readerMemberInfo[0];
                        string nameValue = (string)readerMemberInfo[1];
                        string engnameValue = (string)readerMemberInfo[2];
                        string genderValue = (string)readerMemberInfo[3];
                        string idcardnoValue = (string)readerMemberInfo[4];
                        int isteacherValue = (int)readerMemberInfo[5];

                        basicobj = new MemberBasic()
                        {
                            id = _id,
                            name = nameValue,
                            engname = engnameValue,
                            gender = genderValue,
                            idcardno = idcardnoValue,
                            isteacher = isteacherValue == 1 ? true : false
                        };
                    }
                }
                readerMemberInfo.Close();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }

            return basicobj;
        }

        public static MemberMoreInfo getMemberMoreInfoFromDB(int basicid, Boolean isteacher)
        {
            MemberMoreInfo moreinfoobj = null;
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTR);
            conn.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                //get all grades.
                string queryMemberMoreInfoSql = "select * from membermoreinfo where mbid=@mbid;";
                cmd.CommandText = queryMemberMoreInfoSql;
                //if it's teacher, isteacher is 1.
                cmd.Parameters.AddWithValue("@mbid", basicid);
                MySqlDataReader readerMemberMoreInfo = cmd.ExecuteReader();
                if (!readerMemberMoreInfo.HasRows)
                {
                    Console.WriteLine("no data!");
                }
                else
                {
                    while (readerMemberMoreInfo.Read())
                    {
                        int _id = (int)readerMemberMoreInfo[0];
                        DateTime _birthdayyangli = (DateTime)readerMemberMoreInfo[1];
                        DateTime _birthdaynongli = (DateTime)readerMemberMoreInfo[2];
                        string _minzu = (string)readerMemberMoreInfo[3];
                        string _birthplace = (string)readerMemberMoreInfo[4];
                        string _nowaddress = (string)readerMemberMoreInfo[5];
                        string _residenceaddress = (string)readerMemberMoreInfo[6];
                        string _photopath = (string)readerMemberMoreInfo[7];
                        string _phone = "";
                        string _qq = "";
                        DateTime _graduated = new DateTime();
                        string _profession = "";
                        string _forte = "";
                        string _educationbackground = "";
                        string _graduatedschool = "";
                        string _putonghualevel = "";
                        string _computerlevel = "";
                        string _selfevaluation = "";
                        if (isteacher)
                        {
                             _phone = (string)readerMemberMoreInfo[8];
                             _qq =  (string)readerMemberMoreInfo[9];
                             _graduated =  (DateTime)readerMemberMoreInfo[10];
                             _profession = (string)readerMemberMoreInfo[11];
                             _forte = (string)readerMemberMoreInfo[12];
                             _educationbackground = (string)readerMemberMoreInfo[13];
                             _graduatedschool = (string)readerMemberMoreInfo[14];
                             _putonghualevel =  (string)readerMemberMoreInfo[15];
                             _computerlevel = (string)readerMemberMoreInfo[16];
                             _selfevaluation = (string)readerMemberMoreInfo[17];
                        }
                        
                        int _mbid = (int)readerMemberMoreInfo[18];
                        moreinfoobj = new MemberMoreInfo()
                        {
                            id = _id,
                            birthdaynongli = _birthdaynongli,
                            birthdayyangli = _birthdayyangli,
                            minzu = _minzu,
                            birthplace = _birthplace,
                            nowaddress = _nowaddress,
                            residenceaddress = _residenceaddress,
                            photopath = _photopath,
                            phone = _phone,
                            qq = _qq,
                            graduateddate = _graduated,
                            profession = _profession,
                            forte = _forte,
                            educationbackground = _educationbackground,
                            graduatedschool = _graduatedschool,
                            putonghualevel = _putonghualevel,
                            computerlevel = _computerlevel,
                            selfevaluation = _selfevaluation,
                            mbid = _mbid
                        };
                    }
                }
                readerMemberMoreInfo.Close();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return moreinfoobj;
        }

        public static ObservableCollection<Exprience> getExpriencesByMemberBasicIdFromDB(int basicid)
        {
            ObservableCollection<Exprience> allExpriences = new ObservableCollection<Exprience>();
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTR);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                //get all grades.
                string queryAllExpriencesSql = "select * from exprience where mbid=@mbid;";
                cmd.CommandText = queryAllExpriencesSql;
                //if it's teacher, isteacher is 1.
                cmd.Parameters.AddWithValue("@mbid", basicid);
                MySqlDataReader readerAllExpriencesInfo = cmd.ExecuteReader();
                if (!readerAllExpriencesInfo.HasRows)
                {
                    Console.WriteLine("no data!");
                }
                else
                {
                    while (readerAllExpriencesInfo.Read())
                    {
                        int _id = (int)readerAllExpriencesInfo[0];
                        DateTime _fromdate = (DateTime)readerAllExpriencesInfo[1];
                        DateTime _todate = (DateTime)readerAllExpriencesInfo[2];
                        string _address = (string)readerAllExpriencesInfo[3];
                        string _positions = (string)readerAllExpriencesInfo[4];
                        string _responsibility = (string)readerAllExpriencesInfo[5];
                        int _mbid = (int)readerAllExpriencesInfo[6];
                        allExpriences.Add(new Exprience()
                        {
                            id = _id,
                            from = _fromdate,
                            to = _todate,
                            address = _address,
                            positions = _positions,
                            responsibility = _responsibility
                        });
                    }
                }
                readerAllExpriencesInfo.Close();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return allExpriences;
        }

        public static ObservableCollection<MemberFamily> getMemberFamilyByMemberBasicIdFromDB(int basicid)
        {
            ObservableCollection<MemberFamily> allMemberFamily = new ObservableCollection<MemberFamily>();
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTR);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                //get all grades.
                string queryAllMemberFamilySql = "select * from memberfamily where mbid=@mbid;";
                cmd.CommandText = queryAllMemberFamilySql;
                //if it's teacher, isteacher is 1.
                cmd.Parameters.AddWithValue("@mbid", basicid);
                MySqlDataReader readerAllMemberFamilyInfo = cmd.ExecuteReader();
                if (!readerAllMemberFamilyInfo.HasRows)
                {
                    Console.WriteLine("no data!");
                }
                else
                {
                    while (readerAllMemberFamilyInfo.Read())
                    {
                        int _id = (int)readerAllMemberFamilyInfo[0];
                        string _name = (string)readerAllMemberFamilyInfo[1];
                        string _relation = (string)readerAllMemberFamilyInfo[2];
                        string _phone = (string)readerAllMemberFamilyInfo[3];
                        string _idcardno = (string)readerAllMemberFamilyInfo[4];
                        Boolean _pickup = (Boolean)readerAllMemberFamilyInfo[5];
                        Boolean _emergency = (Boolean)readerAllMemberFamilyInfo[6];
                        string _address = (string)readerAllMemberFamilyInfo[7];
                        allMemberFamily.Add(new MemberFamily()
                        {
                            id = _id,
                            name = _name,
                            relationship = _relation,
                            phone = _phone,
                            idcardno = _idcardno,
                            pickup = _pickup,
                            emergencycontact = _emergency,
                            address = _address
                        });
                    }
                }
                readerAllMemberFamilyInfo.Close();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return allMemberFamily;
        }

        public static ObservableCollection<AwardOrPunishment> getAwardsByMemberBasicIdFromDB(int basicid)
        {
            ObservableCollection<AwardOrPunishment> allAwards = new ObservableCollection<AwardOrPunishment>();
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTR);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                //get all grades.
                string queryAllAwardsSql = "select * from awardspunishmentsinfo where mbid=@mbid;";
                cmd.CommandText = queryAllAwardsSql;
                cmd.Parameters.AddWithValue("@mbid", basicid);
                MySqlDataReader readerAllAwards = cmd.ExecuteReader();
                if (!readerAllAwards.HasRows)
                {
                    Console.WriteLine("no data!");
                }
                else
                {
                    while (readerAllAwards.Read())
                    {
                        int _id = (int)readerAllAwards[0];
                        DateTime _date = (DateTime)readerAllAwards[1];
                        string _content = (string)readerAllAwards[2];
                        string _organization = (string)readerAllAwards[3];
                        allAwards.Add(new AwardOrPunishment()
                        {
                            id = _id,
                            date = _date,
                            content = _content,
                            organization = _organization
                        });
                    }
                }
                readerAllAwards.Close();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return allAwards;
        }

        public static PhysicMoreinfo getPhysicMoreInfo(int basicid)
        {
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTR);
            PhysicMoreinfo physicMoreInfo = null;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                //get all grades.
                string queryAllAwardsSql = "select * from physicmoreinfo where mbid=@mbid;";
                cmd.CommandText = queryAllAwardsSql;
                cmd.Parameters.AddWithValue("@mbid", basicid);
                MySqlDataReader readerPhysicMoreInfo = cmd.ExecuteReader();
                if (!readerPhysicMoreInfo.HasRows)
                {
                    Console.WriteLine("no data!");
                }
                else
                {
                    while (readerPhysicMoreInfo.Read())
                    {
                        int _id = (int)readerPhysicMoreInfo[0];
                        Boolean _haveFoodallergy = (Boolean)readerPhysicMoreInfo[1];
                        string _foodallergyinfo = (string)readerPhysicMoreInfo[2];
                        Boolean _haveConvulsionsepilepsy = (Boolean)readerPhysicMoreInfo[3];
                        Boolean _haveBraindiseases = (Boolean)readerPhysicMoreInfo[4];
                        Boolean _haveAcutechronicinfectious = (Boolean)readerPhysicMoreInfo[5];
                        Boolean _haveheartdiseases = (Boolean)readerPhysicMoreInfo[6];
                        Boolean _haverenaldisease = (Boolean)readerPhysicMoreInfo[7];
                        Boolean _havedrugallergy = (Boolean)readerPhysicMoreInfo[8];
                        string _drugallergy = (string)readerPhysicMoreInfo[9];
                        string _mark = (string)readerPhysicMoreInfo[10];
                        physicMoreInfo = new PhysicMoreinfo()
                        {
                            id = _id,
                            haveFoodallergy= _haveFoodallergy,
                            foodallergyinfo = _foodallergyinfo,
                            haveConvulsionsepilepsy = _haveConvulsionsepilepsy,
                            haveBraindiseases = _haveBraindiseases,
                            haveAcutechronicinfectious = _haveAcutechronicinfectious,
                            haveheartdiseases = _haveheartdiseases,
                            haverenaldisease = _haverenaldisease,
                            havedrugallergy = _havedrugallergy,
                            drugallergy = _drugallergy,
                            mark = _mark
                        };
                    }
                }
                readerPhysicMoreInfo.Close();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return physicMoreInfo;
        }
    }
}

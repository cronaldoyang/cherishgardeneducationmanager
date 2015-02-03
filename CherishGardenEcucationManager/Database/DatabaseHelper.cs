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
        private static string CONNECTIONSTR = ConfigurationManager.ConnectionStrings["cgemConnectionString"].ConnectionString;
        private static string SAVEMEMBERINFO_SP = "savememberinfo_sp";

        //Judge the user login success?
        public static OperatorUser findOperatorUser(string loginuser)
        {
            OperatorUser user = null;
            MySqlConnection conn;
            MySqlCommand cmd;
            conn = new MySqlConnection();
            conn.ConnectionString = CONNECTIONSTR;
            string query = "select password, mbid from operatorinfo where mbid in (select _id from memberbasic where engname=@username)";

            try
            {

                cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@username", loginuser);
                conn.Open();
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
            ObservableCollection<EducationAndEmployeeExprience> exprienceCollection,
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
                cmd.Parameters.AddWithValue("@basicname", basicobj.name);
                cmd.Parameters.AddWithValue("@basicengname", basicobj.engname);
                cmd.Parameters.AddWithValue("@basicgender", basicobj.gender);
                cmd.Parameters.AddWithValue("@basicidcardno", basicobj.idcardno);
                cmd.Parameters.AddWithValue("@basicisteacher", basicobj.isteacher ? 1 : 0);
                //fill the membermoreinfo parameters
                cmd.Parameters.AddWithValue("@birthdayyangli", moreobj.birthdayYangli);
                cmd.Parameters.AddWithValue("@birthdaynongli", moreobj.birthdayNongli);
                cmd.Parameters.AddWithValue("@minzu", moreobj.minzu);
                cmd.Parameters.AddWithValue("@birthplace", moreobj.birthPlace);
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
                //
                int result = Convert.ToInt16(resultParameter.Value);
                int basicId = Convert.ToInt16(basicRecordIdParameter.Value);
                if (result > 0 && basicId > 0)
                {
                    //save member family records;
                    //save member family records;
                    int familycount = familyCollection.Count;
                    if (familycount > 0)
                    {
                        string insertMemberFamilySql = "insert into memberfamily(name, relation,mobilephone,idcardno,pickup,emergencycontact,mbid) values ";
                        MemberFamily tempfamily = null;
                        for (int i = 0; i < familycount; i++)
                        {
                            tempfamily = familyCollection[i];
                            insertMemberFamilySql += "('" + tempfamily.name + "','" + tempfamily.relationship + "','" + tempfamily.phone + "','" + tempfamily.idcardno + "'," + (tempfamily.pickup ? 1 : 0) + "," + (tempfamily.emergencycontact ? 1 : 0) + "," + basicId + ")";
                            if (i < familycount - 1)
                            {
                                insertMemberFamilySql += ",";
                            }
                            else
                            {
                                insertMemberFamilySql += ";";
                            }

                        }
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = insertMemberFamilySql;
                        cmd.ExecuteNonQuery();
                    }


                    //save exprience records;
                    int expriencecount = exprienceCollection.Count;
                    if (expriencecount > 0)
                    {
                        string insetrExprienceSql = "insert into experience(fromdate, todate,address,positions,responsibility,mbid) values ";
                        EducationAndEmployeeExprience tempExprience = null;
                        for (int i = 0; i < expriencecount; i++)
                        {
                            tempExprience = exprienceCollection[i];
                            insetrExprienceSql += "('" + tempExprience.from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "','" + tempExprience.to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "','" + tempExprience.address + "','" + tempExprience.positions + "','" + tempExprience.responsibility + "'," + basicId + ")";
                            if (i < expriencecount - 1)
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


                    //save exprience records;
                    int awardscount = awardsCollection.Count;
                    if (awardscount > 0)
                    {
                        string insertAwardsSql = "insert into awardspunishmentsinfo(date, content,organization,mbid) values ";
                        AwardOrPunishment tempAward = null;
                        for (int i = 0; i < awardscount; i++)
                        {
                            tempAward = awardsCollection[i];
                            insertAwardsSql += "('" + tempAward.date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "','" + tempAward.content + "','" + tempAward.organization + "'," + basicId + ")";
                            if (i < awardscount - 1)
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
                    cmd.CommandText = updateClassesSql + nameUpdateSql + headteacheridUpdateSql + gradeidUpdateSql + wheresql ;
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
                        if(i < oldGradesCount-1)
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
                        string nameValue = (String)reader[1];
                        string engnameValue = (String)reader[2];
                        string genderValue = (String)reader[3];
                        string idcardnoValue = (String)reader[4];
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
            ObservableCollection<Grade> allClassGroups = new ObservableCollection<Grade>();
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTR);
            conn.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                string queryAllClassGroups = "select * from grade";
                cmd.CommandText = queryAllClassGroups;
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
                        string nameValue = (String)reader[1];

                        allClassGroups.Add(new Grade()
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

            return allClassGroups;
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

                string queryAllClassGroups = "select * from class";
                cmd.CommandText = queryAllClassGroups;
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
                        string _name = (String)reader[1];
                        int _headteacherid = (int)reader[2];
                        int _gradeid = (int)reader[3];

                        allClasses.Add(new Class() {
                            id = _id,
                            name = _name,
                            teacherid = _headteacherid,
                            gradeid = _id
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

            return allClasses;
        }
    }
}

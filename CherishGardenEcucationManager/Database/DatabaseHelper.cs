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
                        Exprience tempExprience = null;
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
                        string nameValue = (String)reader[1];

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
                        string nameValue = (String)readerGrades[1];

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
                        string nameValue = (String)readerTeachers[1];
                        string engnameValue = (String)readerTeachers[2];
                        string genderValue = (String)readerTeachers[3];
                        string idcardnoValue = (String)readerTeachers[4];
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
                        string _name = (String)readerClasses[1];
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
                        string nameValue = (String)readerMemberInfo[1];
                        string engnameValue = (String)readerMemberInfo[2];
                        string genderValue = (String)readerMemberInfo[3];
                        string idcardnoValue = (String)readerMemberInfo[4];
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

        public static MemberMoreInfo getMemberMoreInfoFromDB(int basicid)
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
                        string _minzu = (String)readerMemberMoreInfo[3];
                        string _birthplace = (String)readerMemberMoreInfo[4];
                        string _nowaddress = (String)readerMemberMoreInfo[5];
                        string _residenceaddress = (String)readerMemberMoreInfo[6];
                        string _photopath = (string)readerMemberMoreInfo[7];
                        string _phone = (string)readerMemberMoreInfo[8];
                        string _qq = (string)readerMemberMoreInfo[9];
                        DateTime _graduated = (DateTime)readerMemberMoreInfo[10];
                        string _profession = (string)readerMemberMoreInfo[11];
                        string _forte = (string)readerMemberMoreInfo[12];
                        string _educationbackground = (string)readerMemberMoreInfo[13];
                        string _graduatedschool = (string)readerMemberMoreInfo[14];
                        string _putonghualevel = (string)readerMemberMoreInfo[15];
                        string _computerlevel = (string)readerMemberMoreInfo[16];
                        string _selfevaluation = (string)readerMemberMoreInfo[17];
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
                string queryAllExpriencesSql = "select * from experience where mbid=@mbid;";
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
                        string _address = (String)readerAllExpriencesInfo[3];
                        string _positions = (String)readerAllExpriencesInfo[4];
                        string _responsibility = (String)readerAllExpriencesInfo[5];
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
                        string _phone = (String)readerAllMemberFamilyInfo[3];
                        string _idcardno = (String)readerAllMemberFamilyInfo[4];
                        Boolean _pickup = (Boolean)readerAllMemberFamilyInfo[5] ;
                        Boolean _emergency = (Boolean )readerAllMemberFamilyInfo[6];
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
                        string _organization = (String)readerAllAwards[3];
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
    }
}

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

    }
}

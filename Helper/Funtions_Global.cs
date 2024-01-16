using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using QLHN.Models.Systems;
using QLHN.ViewModels.Manages;
using System.Net.Http;

namespace QLHN.Helper
{
    public class Funtions_Global
    {
        public static string GetMenuMinimize(ISession session)
        {
            if (!string.IsNullOrEmpty(session.GetString("MenuMinimize")))
            {
                return session.GetString("MenuMinimize");
            }
            else
            {
                return "False";
            }
        }

        public static string GetSsAdmin(ISession session, string key)
        {
            if (!string.IsNullOrEmpty(session.GetString("SsAdmin")))
            {
                string ssadmin = session.GetString("SsAdmin");
                dynamic sessionInfo = JsonConvert.DeserializeObject(ssadmin);
                string value = sessionInfo[key];
                return string.IsNullOrEmpty(value) ? "" :value;
            }
            else
            {
                return "";
            }
        }
        public static string GetThongTinDonVi(ISession session, string key)
        {
            if (!string.IsNullOrEmpty(session.GetString("ThongTinDonVi")))
            {
                string thongtindonvi = session.GetString("ThongTinDonVi");
                dynamic thongtindonviInfo = JsonConvert.DeserializeObject(thongtindonvi);
                string value = thongtindonviInfo[key];
                return value;
            }
            else
            {
                return "";
            }
        }

        public static string GetThongTinUsers(ISession session)
        {
            string ssadmin = session.GetString("SsAdmin");
            dynamic sessionInfo = JsonConvert.DeserializeObject(ssadmin);
            string value = sessionInfo["Name"] + " (" + sessionInfo["Username"] + ")";
            return value;
        }

        public static bool CheckPermission(ISession session, string roles, string key)
        {
            if (!string.IsNullOrEmpty(session.GetString("SsAdmin")))
            {
                string ssadmin = session.GetString("SsAdmin");
                dynamic sessionInfo = JsonConvert.DeserializeObject(ssadmin);
                bool ssa = sessionInfo["Sadmin"];
                if (ssa)
                {
                    return true;
                }
                else
                {
                    string per = session.GetString("Permission");
                    if (!string.IsNullOrEmpty(per))
                    {
                        dynamic info = JsonConvert.DeserializeObject(per);

                        foreach (var item in info)
                        {
                            if (item["Roles"] == roles && item[key] == true)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static bool CheckPermissionAction(ISession session, string madonvi)
        {
            if (!string.IsNullOrEmpty(session.GetString("SsAdmin")))
            {
                string ssadmin = session.GetString("SsAdmin");
                dynamic sessionInfo = JsonConvert.DeserializeObject(ssadmin);
                bool ssa = sessionInfo["Sadmin"];
                if (ssa)
                {
                    return true;
                }
                else
                {
                    string Level = sessionInfo["Level"];
                    string madonviss = "";
                    if (Level == "T")
                    {
                        return true;
                    }
                    else
                    {
                        if (Level == "H")
                        {
                            madonviss = sessionInfo["MaHuyen"];
                        }
                        else
                        {
                            madonviss = sessionInfo["MaXa"];
                        }
                        if (madonvi == madonviss)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string[] GetRolesList()
        {
            string[] roles = new string[]
            {
                "manages",

                "manages.quanlythutuchanhchinh",
                "manages.quanlythutuchanhchinh.bothutuc",
                "manages.quanlythutuchanhchinh.danhsach",


                "manages.quanlyhoso",
                "manages.quanlyhoso.ncc",
                "manages.quanlyhoso.ncc.dichuyen",
                "manages.quanlyhoso.ncc.baotu",
                "manages.quanlyhoso.ncc.giayto",

                "manages.quanlyhoso.thannhan",
                "manages.quanlyhoso.thannhan.dichuyen",
                "manages.quanlyhoso.thannhan.baotu",
                "manages.quanlyhoso.thannhan.giayto",

                //"manages.quanlyhoso.qd1312021",
                "manages.quanlyhoso.duthaoquyetdinh",

                "manages.quanlychitrahangthang",
                "manages.quanlychitrahangthang.thongtinhuong",
                "manages.quanlychitrahangthang.ncc",
                "manages.quanlychitrahangthang.ncc.danhsach",
                "manages.quanlychitrahangthang.ncc.xetduyet",
                "manages.quanlychitrahangthang.ncc.chitra",
                "manages.quanlychitrahangthang.thannhan",
                "manages.quanlychitrahangthang.thannhan.danhsach",
                "manages.quanlychitrahangthang.thannhan.xetduyet",
                "manages.quanlychitrahangthang.thannhan.chitra",
                "manages.quanlychitrahangthang.baohiemyte",
                "manages.quanlychitrahangthang.baohiemyte.danhsach",
                "manages.quanlychitrahangthang.baohiemyte.xetduyet",

                "manages.quanlychitragiaoducdaotao",
                "manages.quanlychitragiaoducdaotao.danhsachdenghihuong",
                "manages.quanlychitragiaoducdaotao.danhsachdenghihuong.lapdanhsach",
                "manages.quanlychitragiaoducdaotao.danhsachdenghihuong.xetduyet",
                "manages.quanlychitragiaoducdaotao.danhsachhuong",
                "manages.quanlychitragiaoducdaotao.danhsachhuong.lapdanhsach",
                "manages.quanlychitragiaoducdaotao.danhsachhuong.xetduyet",
                "manages.quanlychitragiaoducdaotao.danhsachhuong.chitra",

                "manages.quanlychitrahangnam",
                "manages.quanlychitrahangnam.danhsach",
                "manages.quanlychitrahangnam.xetduyet",

                "manages.quanlychitramotlan",
                "manages.quanlychitramotlan.danhsach",
                "manages.quanlychitramotlan.xetduyet",

                "manages.quanlyqualetet",
                "manages.quanlyqualetet.danhsach",
                "manages.quanlyqualetet.xetduyet",

                "manages.quanlydieuduong",
                "manages.quanlydieuduong.dutoankinhphi",
                "manages.quanlydieuduong.dutoankinhphi.lapdutoan",
                "manages.quanlydieuduong.dutoankinhphi.xetduyet",
                "manages.quanlydieuduong.danhsachdenghidieuduong",
                "manages.quanlydieuduong.danhsachdenghidieuduong.lapdanhsach",
                "manages.quanlydieuduong.danhsachdenghidieuduong.xetduyet",
                "manages.quanlydieuduong.danhsach",
                "manages.quanlydieuduong.danhsach.lapdanhsach",
                "manages.quanlydieuduong.danhsach.xetduyet",

                "manages.quanlyphuongtientrogiup",
                "manages.quanlyphuongtientrogiup.dutoankinhphi",
                "manages.quanlyphuongtientrogiup.dutoankinhphi.lapdutoan",
                "manages.quanlyphuongtientrogiup.dutoankinhphi.xetduyet",
                "manages.quanlyphuongtientrogiup.danhsachdenghicap",
                "manages.quanlyphuongtientrogiup.danhsachdenghicap.lapdanhsach",
                "manages.quanlyphuongtientrogiup.danhsachdenghicap.xetduyet",
                "manages.quanlyphuongtientrogiup.danhsach",
                "manages.quanlyphuongtientrogiup.danhsach.lapdanhsach",
                "manages.quanlyphuongtientrogiup.danhsach.xetduyet",

                "manages.quanlymolietsy",
                "manages.quanlymolietsy.danhsach",
                "manages.quanlymolietsy.thamvieng",

                "manages.baocaotonghop",
                "manages.baocaotonghop.qd092007",
                "manages.baocaotonghop.ttlt132014",
                "manages.baocaotonghop.nd1312021",
                "manages.baocaotonghop.khac",

                "manages.quanlynghidinh",
                "manages.reportscustom",


                //Settings
                "settings",
                "settings.danhmuchoso",
                "settings.danhmucnghidinh",
                "settings.danhmuchuongqualetet",
                "settings.danhmucnghiatrang",
                "settings.danhmucthutuchanhchinh",
                "settings.danhmucmaubieu",
                "settings.maubieuduthaoquyetdinh",                

                //Systems
                "systems",
                "systems.districts",
                "systems.towns",
                "systems.mucluongcoban",
                "systems.group_permission",
                "systems.users",
                "systems.users.permission",

            };
            return roles;

        }

        public static string ConvertDateToText(DateTime date)
        {

            string date_convert = date.Date.ToString("dd/MM/yyyy");
            if (date_convert == "01/01/0001")
            {
                return " ngày .. tháng .. năm ....";
            }
            else
            {
                string str = "";
                str += " ngày " + date.ToString("dd");
                str += " tháng " + date.ToString("MM");
                str += " năm " + date.ToString("yyyy");
                return str;
            }
        }

        public static string ConvertDateToFormView(DateTime date)
        {
            //string date_convert = date.Date.ToString("dd/MM/yyyy");
            //if (date_convert == "01/01/0001")
            //{
            //    string str = DateTime.Now.Date.ToString("yyyy-MM-dd");
            //    //string str = string.Format("{0:MM/dd/yyyy}");
            //    return str;
            //}
            //else
            //{
            string str = date.Date.ToString("yyyy-MM-dd");
            return str;
            //}
        }

        public static string ConvertDateTimeToFormView(DateTime date)
        {
            string date_convert = date.Date.ToString("dd/MM/yyyy");
            if (date_convert == "01/01/0001")
            {
                string str = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(' ', 'T');
                return str;
            }
            else
            {
                string str = date.ToString("yyyy-MM-dd HH:mm:ss").Replace(' ', 'T');
                return str;
            }
        }

        public static string ConvertDbToStrDecimal(double db)
        {
            if (db == 0)
            {
                return "";
            }
            else
            {
                return db.ToString();
            }
        }

        public static string ConvertDbToStr(double db, bool dinhdang = true)
        {
            if (db == 0)
            {
                return "";
            }
            else
            {
                string str = String.Format("{0:n0}", db);
                if (dinhdang)
                {
                    str = str.Replace(",", ".");
                }
                
                return str;
            }
        }

        public static string ConvertIntToStr(int value)
        {
            if (value == 0)
            {
                return "";
            }
            else
            {
                string str = String.Format("{0:n0}", value);
                return str;
            }
        }

        public static string ConvertDateToStr(DateTime date)
        {

            string str = date.Date.ToString("dd/MM/yyyy");
            if (str == "01/01/0001")
            {
                return "";
            }
            else
            {
                return str;
            }
        }

        public static string ConvertDateTimeToStr(DateTime datetime)
        {
            string str = datetime.Date.ToString("dd/MM/yyyy HH:mm:ss,fff tt");
            return str;
        }

        public static string ConvertDateTimeToText(DateTime datetime)
        {
            string gio = datetime.Hour < 10 ? "0" + datetime.Hour.ToString() : datetime.Hour.ToString();
            string phut = datetime.Minute < 10 ? "0" + datetime.Minute.ToString() : datetime.Minute.ToString();
            string ngay = datetime.Day < 10 ? "0" + datetime.Day.ToString() : datetime.Day.ToString();
            string thang = datetime.Month < 10 ? "0" + datetime.Month.ToString() : datetime.Month.ToString();
            string text = gio + " giờ " + phut + " phút";
            text += ", ngày " + ngay + " tháng " + thang + " năm " + datetime.Year.ToString();
            return text;
        }

        public static double ConvertStrToDb(string str)
        {
            if (str == "")
            {
                return 0;
            }
            else
            {
                double db = double.Parse(str.Replace(".", "").Replace(",", ""));
                return db;
            }
        }

        public static string ConvertYearToStr(int year)
        {
            if (year == 0)
            {
                return "";
            }
            else
            {
                return year.ToString();
            }
        }

        public static string ConvertDbToMoneyText(double inputNumber, bool suffix = true)
        {
            if (inputNumber != 0)
            {
                string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
                bool isNegative = false;

                // -12345678.3445435 => "-12345678"
                string sNumber = inputNumber.ToString("#");
                double number = Convert.ToDouble(sNumber);
                if (number < 0)
                {
                    number = -number;
                    sNumber = number.ToString();
                    isNegative = true;
                }


                int ones, tens, hundreds;

                int positionDigit = sNumber.Length;   // last -> first

                string result = " ";


                if (positionDigit == 0)
                    result = unitNumbers[0] + result;
                else
                {
                    // 0:       ###
                    // 1: nghìn ###,###
                    // 2: triệu ###,###,###
                    // 3: tỷ    ###,###,###,###
                    int placeValue = 0;

                    while (positionDigit > 0)
                    {
                        // Check last 3 digits remain ### (hundreds tens ones)
                        tens = hundreds = -1;
                        ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                            if (positionDigit > 0)
                            {
                                hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                                positionDigit--;
                            }
                        }

                        if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                            result = placeValues[placeValue] + result;

                        placeValue++;
                        if (placeValue > 3) placeValue = 1;

                        if ((ones == 1) && (tens > 1))
                            result = "một " + result;
                        else
                        {
                            if ((ones == 5) && (tens > 0))
                                result = "lăm " + result;
                            else if (ones > 0)
                                result = unitNumbers[ones] + " " + result;
                        }
                        if (tens < 0)
                            break;
                        else
                        {
                            if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                            if (tens == 1) result = "mười " + result;
                            if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                        }
                        if (hundreds < 0) break;
                        else
                        {
                            if ((hundreds > 0) || (tens > 0) || (ones > 0))
                                result = unitNumbers[hundreds] + " trăm " + result;
                        }
                        result = " " + result;
                    }
                }
                result = result.Trim();
                string[] str = result.Split(" ");
                int len_array = str.Count() - 1;
                string new_str = str[0].Substring(0, 1).ToUpper() + str[0].Substring(1);
                for (int i = 1; i <= len_array; i++)
                {
                    new_str += " " + str[i];
                }
                if (isNegative) result = "Âm " + new_str;
                return new_str + (suffix ? " đồng chẵn" : "");
            }
            else
            {
                return "";
            }
        }

        public static string ConvertDbToText(double inputNumber, bool suffix = true)
        {
            if (inputNumber != 0)
            {
                string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
                bool isNegative = false;

                // -12345678.3445435 => "-12345678"
                string sNumber = inputNumber.ToString("#");
                double number = Convert.ToDouble(sNumber);
                if (number < 0)
                {
                    number = -number;
                    sNumber = number.ToString();
                    isNegative = true;
                }


                int ones, tens, hundreds;

                int positionDigit = sNumber.Length;   // last -> first

                string result = " ";


                if (positionDigit == 0)
                    result = unitNumbers[0] + result;
                else
                {
                    // 0:       ###
                    // 1: nghìn ###,###
                    // 2: triệu ###,###,###
                    // 3: tỷ    ###,###,###,###
                    int placeValue = 0;

                    while (positionDigit > 0)
                    {
                        // Check last 3 digits remain ### (hundreds tens ones)
                        tens = hundreds = -1;
                        ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                            if (positionDigit > 0)
                            {
                                hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                                positionDigit--;
                            }
                        }

                        if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                            result = placeValues[placeValue] + result;

                        placeValue++;
                        if (placeValue > 3) placeValue = 1;

                        if ((ones == 1) && (tens > 1))
                            result = "một " + result;
                        else
                        {
                            if ((ones == 5) && (tens > 0))
                                result = "lăm " + result;
                            else if (ones > 0)
                                result = unitNumbers[ones] + " " + result;
                        }
                        if (tens < 0)
                            break;
                        else
                        {
                            if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                            if (tens == 1) result = "mười " + result;
                            if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                        }
                        if (hundreds < 0) break;
                        else
                        {
                            if ((hundreds > 0) || (tens > 0) || (ones > 0))
                                result = unitNumbers[hundreds] + " trăm " + result;
                        }
                        result = " " + result;
                    }
                }
                result = result.Trim();
                string[] str = result.Split(" ");
                int len_array = str.Count() - 1;
                string new_str = str[0].Substring(0, 1).ToUpper() + str[0].Substring(1);
                for (int i = 1; i <= len_array; i++)
                {
                    new_str += " " + str[i];
                }
                if (isNegative) new_str = "Âm " + new_str;
                return new_str;
            }
            else
            {
                return "";
            }
        }

        public static string[] GetListsDays()
        {
            string[] days = new string[]
               {
                    "01", "02", "03", "04", "05", "06", "07", "08", "09", "10",
                    "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
                    "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"
               };
            return days;
        }

        public static string[] GetListsMonths()
        {
            string[] months = new string[]
               {
                    "01", "02", "03", "04", "05", "06", "07", "08", "09", "10",
                    "11", "12"
               };
            return months;
        }

        public static string[] GetListsDanTocs()
        {
            string[] dantocs = new string[]
            {
                "Kinh","Tày","Thái","Hoa","Khơ-me","Mường","Nùng","HMông","Dao","Gia-rai","Ngái","Ê-đê","Ba na","Xơ-Đăng","Sán Chay",
                "Cơ-ho","Chăm","Sán Dìu","Hrê","Mnông","Ra-glai","Xtiêng","Bru-Vân Kiều","Thổ","Giáy","Cơ-tu","Gié Triêng","Mạ","Khơ-mú",
                "Co","Tà-ôi","Chơ-ro","Kháng","Xinh-mun","Hà Nhì","Chu ru","Lào","La Chí","La Ha","Phù Lá","La Hủ","Lự","Lô Lô","Chứt",
                "Mảng","Pà Thẻn","Co Lao","Cống","Bố Y","Si La","Pu Péo","Brâu","Ơ Đu","Rơ măm"
            };
            return dantocs;
        }

        public static string[] GetListsMoiQuanHe()
        {
            string[] moiquanhes = new string[]
            {
                "Cha đẻ", "Mẹ đẻ", "Người nuôi dưỡng", "Vợ","Chồng", "Con đẻ", "Con nuôi", "Cô", "Dì", "Chú", "Bác",
                "Anh", "Chị", "Em","Cháu", "Chắt", "Người phục vụ", "Người thờ cúng"
            };
            return moiquanhes;
        }

        public static string ConvertDayMonthYearToString(string day, string month, string year)
        {
            string str = "";
            if (!string.IsNullOrEmpty(day))
            {
                str += day + "/";
            }
            if (!string.IsNullOrEmpty(month))
            {
                str += month + "/";
            }
            if (!string.IsNullOrEmpty(year))
            {
                str += year;
            }
            return str;
        }

        public static bool CheckFileType(string extension)
        {
            if (extension == ".jpg") return true;
            if (extension == ".jpeg") return true;
            if (extension == ".png") return true;
            if (extension == ".doc") return true;
            if (extension == ".docx") return true;
            if (extension == ".pdf") return true;
            if (extension == ".xls") return true;
            if (extension == ".xlsx") return true;
            return false;
        }

        public static bool CheckFileSize(long size)
        {
            if (size <= 5242880) return true;
            return false;
        }

        public static string[] GetGroupPer()
        {
            string[] group = new string[]
               { "K" , "T", "H", "X"};
            return group;
        }

        public static DateTime ConvertStringToDate(string Ngay, string Thang, string Nam)
        {
            if (string.IsNullOrEmpty(Ngay)) Ngay = "01";
            if (string.IsNullOrEmpty(Thang)) Thang = "01";
            if (string.IsNullOrEmpty(Nam)) Nam = "1900";
            string date = Ngay + "/" + Thang + "/" + Nam;
            DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return dt;
        }

        public static DateTime ConvertIntToDate(int Ngay, int Thang, int Nam)
        {
            string strNgay, strThang, strNam = "";
            strNgay = Ngay < 10 ? "0" + Ngay.ToString() : Ngay.ToString();
            strThang = Thang < 10 ? "0" + Thang.ToString() : Thang.ToString();
            strNam = Nam.ToString();

            string date = strNgay + "/" + strThang + "/" + strNam;
            DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return dt;
        }

        public static string ConvertMonthYearToString(string Thang, string Nam)
        {
            string str = "";
            if (!string.IsNullOrEmpty(Thang))
            {
                str += Thang + " tháng ";
            }
            if (!string.IsNullOrEmpty(Nam))
            {
                str += Nam + " năm";
            }

            return str;
        }

        public static string ConvertIntToRoman(int number)
        {
            switch (number)
            {
                case 1: return "I";
                case 2: return "II";
                case 3: return "III";
                case 4: return "IV";
                case 5: return "V";
                case 6: return "VI";
                case 7: return "VII";
                case 8: return "VIII";
                case 9: return "IX";
                case 10: return "X";
                case 11: return "XI";
                case 12: return "XII";
                case 13: return "XIII";
                case 14: return "XIV";
                case 15: return "XV";
                case 16: return "XVI";
                case 17: return "XVII";
                case 18: return "XVIII";
                case 19: return "XIX";
                case 20: return "XX";
                case 21: return "XXI";
                case 22: return "XXII";
                case 23: return "XXIII";
                case 24: return "XXIV";
                case 25: return "XXV";
                case 26: return "XXVI";
                case 27: return "XXVII";
                case 28: return "XXVIII";
                case 29: return "XXIX";
                case 30: return "XXX";
                default: return "";
            }
        }

        public static string ConvertIntToAlphabet(int number)
        {
            switch (number)
            {
                case 1: return "A";
                case 2: return "B";
                case 3: return "C";
                case 4: return "D";
                case 5: return "E";
                case 6: return "F";
                case 7: return "G";
                case 8: return "H";
                case 9: return "I";
                case 10: return "J";
                case 11: return "K";
                case 12: return "L";
                case 13: return "M";
                case 14: return "N";
                case 15: return "O";
                case 16: return "P";
                case 17: return "Q";
                case 18: return "R";
                case 19: return "S";
                case 20: return "T";
                case 21: return "U";
                case 22: return "V";
                case 23: return "W";
                case 24: return "X";
                case 25: return "Y";
                case 26: return "Z";
                case 27: return "AA";
                case 28: return "AB";
                case 29: return "AC";
                case 30: return "AD";
                case 31: return "AE";
                case 32: return "AF";
                case 33: return "AG";
                case 34: return "AH";
                case 35: return "AI";
                case 36: return "AJ";
                case 37: return "AK";
                case 38: return "AL";
                case 39: return "AM";
                case 40: return "AN";
                case 41: return "AO";
                case 42: return "AP";
                case 43: return "AQ";
                case 44: return "AR";
                case 45: return "AS";
                case 46: return "AT";
                case 47: return "AU";
                case 48: return "AV";
                case 49: return "AW";
                case 50: return "AX";
                case 51: return "AY";
                case 52: return "AZ";
                default: return "";
            }
        }

        public static List<Towns> GetListTownsActive(List<Towns> towns, DateTime NgayKetXuat)
        {
            List<Towns> model = new List<Towns> { };
            return model;
        }

        public static List<VMKeyValue> GetListColExcel()
        {
            List<VMKeyValue> list = new List<VMKeyValue> { };
            list.Add(new VMKeyValue { Key = "A", Value = 1 });
            list.Add(new VMKeyValue { Key = "B", Value = 2 });
            list.Add(new VMKeyValue { Key = "C", Value = 3 });
            list.Add(new VMKeyValue { Key = "D", Value = 4 });
            list.Add(new VMKeyValue { Key = "E", Value = 5 });
            list.Add(new VMKeyValue { Key = "F", Value = 6 });
            list.Add(new VMKeyValue { Key = "G", Value = 7 });
            list.Add(new VMKeyValue { Key = "H", Value = 8 });
            list.Add(new VMKeyValue { Key = "I", Value = 9 });
            list.Add(new VMKeyValue { Key = "J", Value = 10 });
            list.Add(new VMKeyValue { Key = "K", Value = 11 });
            list.Add(new VMKeyValue { Key = "L", Value = 12 });
            list.Add(new VMKeyValue { Key = "M", Value = 13 });
            list.Add(new VMKeyValue { Key = "N", Value = 14 });
            list.Add(new VMKeyValue { Key = "O", Value = 15 });
            list.Add(new VMKeyValue { Key = "P", Value = 16 });
            list.Add(new VMKeyValue { Key = "Q", Value = 17 });
            list.Add(new VMKeyValue { Key = "R", Value = 18 });
            list.Add(new VMKeyValue { Key = "S", Value = 19 });
            list.Add(new VMKeyValue { Key = "T", Value = 20 });
            list.Add(new VMKeyValue { Key = "U", Value = 21 });
            list.Add(new VMKeyValue { Key = "V", Value = 22 });
            list.Add(new VMKeyValue { Key = "W", Value = 23 });
            list.Add(new VMKeyValue { Key = "X", Value = 24 });
            list.Add(new VMKeyValue { Key = "Y", Value = 25 });
            list.Add(new VMKeyValue { Key = "Z", Value = 26 });
            list.Add(new VMKeyValue { Key = "AA", Value = 27 });
            list.Add(new VMKeyValue { Key = "AB", Value = 28 });
            list.Add(new VMKeyValue { Key = "AC", Value = 29 });
            list.Add(new VMKeyValue { Key = "AD", Value = 30 });
            list.Add(new VMKeyValue { Key = "AE", Value = 31 });
            list.Add(new VMKeyValue { Key = "AF", Value = 32 });
            list.Add(new VMKeyValue { Key = "AG", Value = 33 });
            list.Add(new VMKeyValue { Key = "AH", Value = 34 });
            list.Add(new VMKeyValue { Key = "AI", Value = 35 });
            list.Add(new VMKeyValue { Key = "AJ", Value = 36 });
            list.Add(new VMKeyValue { Key = "AK", Value = 37 });
            list.Add(new VMKeyValue { Key = "AL", Value = 38 });
            list.Add(new VMKeyValue { Key = "AM", Value = 39 });
            list.Add(new VMKeyValue { Key = "AN", Value = 40 });
            list.Add(new VMKeyValue { Key = "AO", Value = 41 });
            list.Add(new VMKeyValue { Key = "AP", Value = 42 });
            list.Add(new VMKeyValue { Key = "AQ", Value = 43 });
            list.Add(new VMKeyValue { Key = "AR", Value = 44 });
            list.Add(new VMKeyValue { Key = "AS", Value = 45 });
            list.Add(new VMKeyValue { Key = "AT", Value = 46 });
            list.Add(new VMKeyValue { Key = "AU", Value = 47 });
            list.Add(new VMKeyValue { Key = "AV", Value = 48 });
            list.Add(new VMKeyValue { Key = "AW", Value = 49 });
            list.Add(new VMKeyValue { Key = "AX", Value = 50 });
            list.Add(new VMKeyValue { Key = "AY", Value = 51 });
            list.Add(new VMKeyValue { Key = "AZ", Value = 52 });

            return list;
        }

        public static DateTime ConvertStrToDate(string strDate)
        {
            try
            {
                DateTime dt; dt = DateTime.ParseExact(strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                return dt;
            }
            catch
            {
                DateTime Now = DateTime.Now;
                return new DateTime(Now.Year, Now.Month, 1);
            }
        }

        public static string GetDayOrMonthOrYearFromStr(string strDate, string type)
        {
            if (type == "Day")
            {
                string day = "";
                if (strDate.Length == 10)
                {
                    day = strDate.Split("/")[0];
                }
                if (day.Length > 2)
                {
                    day = "01";
                }
                return day;
            }
            else if (type == "Month")
            {
                string month = "";
                if (strDate.Length >= 7)
                {
                    string[] arr = strDate.Split("/");
                    month = arr[arr.Length - 2];
                }
                if (month.Length > 2)
                {
                    month = "01";
                }
                return month;
            }
            else
            {
                string year = "";
                if (strDate.Length == 4)
                {
                    year = strDate;
                }
                else if (strDate.Length > 4)
                {
                    string[] arr = strDate.Split("/");
                    year = arr[arr.Length - 1];
                }
                if (year.Length > 4)
                {
                    year = "1900";
                }
                return year;
            }
        }

        public static string ConvertStrToStyle(string strStyle)
        {
            if (string.IsNullOrEmpty(strStyle))
            {
                return "";
            }
            else
            {
                string HtmlStyle = "";
                List<string> list_style = strStyle.Split(",").ToList();
                if (list_style.Contains("Chữ in hoa"))
                {
                    HtmlStyle += "text-transform: uppercase;";
                }
                if (list_style.Contains("Chữ in đậm"))
                {
                    HtmlStyle += "font-weight:bold;";
                }
                if (list_style.Contains("Chữ in nghiêng"))
                {
                    HtmlStyle += "font-style:italic;";
                }
                return HtmlStyle;
            }
        }

        public static string SetGoiTinh(string gioitinh)
        {
            gioitinh= string.IsNullOrEmpty(gioitinh)? "Nam":gioitinh.Trim();
            string result = "";
            switch (gioitinh.ToLower())
            {
                case "nam":
                    result = "Nam";
                    break;
                case "nữ":
                    result = "Nữ";
                    break;
                default:
                    result = "Nam";
                    break;
            }
            return result;
        }

        public static string SetDanToc(string dantoc)
        {
            dantoc=string.IsNullOrEmpty(dantoc)? "Kinh":dantoc.Trim();
            string result = "";
            switch (dantoc.ToLower())
            {
                case "kinh":
                    result = "Kinh";
                    break;
                case "tày":
                    result = "Tày";
                    break;
                case "thái":
                    result = "Thái";
                    break;
                case "hoa":
                    result = "Hoa";
                    break;
                case "khơ-me":
                    result = "Khơ-me";
                    break;
                case "mường":
                    result = "Mường";
                    break;
                case "nùng":
                    result = "Nùng";
                    break;
                case "hmông":
                    result = "HMông";
                    break;
                case "dao":
                    result = "Dao";
                    break;
                case "gia-rai":
                    result = "Gia-rai";
                    break;
                case "ngái":
                    result = "Ngái";
                    break;
                case "ê-đê":
                    result = "Ê-đê";
                    break;
                case "ba na":
                    result = "Ba na";
                    break;
                case "xơ-đăng":
                    result = "Xơ-Đăng";
                    break;
                case "sán chay":
                    result = "Sán Chay";
                    break;
                case "cơ-ho":
                    result = "Cơ-ho";
                    break;
                case "chăm":
                    result = "Chăm";
                    break;
                case "sán dìu":
                    result = "Sán Dìu";
                    break;
                case "hrê":
                    result = "Hrê";
                    break;
                case "mnông":
                    result = "Mnông";
                    break;
                case "ra-glai":
                    result = "Ra-glai";
                    break;
                case "xtiêng":
                    result = "Xtiêng";
                    break;
                case "bru-vân kiều":
                    result = "Bru-Vân Kiều";
                    break;
                case "thổ":
                    result = "Thổ";
                    break;
                case "giáy":
                    result = "Giáy";
                    break;
                case "cơ-tu":
                    result = "Cơ-tu";
                    break;
                case "gié triêng":
                    result = "Gié Triêng";
                    break;
                case "mạ":
                    result = "Mạ";
                    break;
                case "khơ-mú":
                    result = "Khơ-mú";
                    break;
                case "co":
                    result = "Co";
                    break;
                case "tà-ôi":
                    result = "Tà-ôi";
                    break;
                case "chơ-ro":
                    result = "Chơ-ro";
                    break;
                case "kháng":
                    result = "Kháng";
                    break;
                case "xinh-mun":
                    result = "Xinh-mun";
                    break;
                case "hà nhì":
                    result = "Hà Nhì";
                    break;
                case "chu ru":
                    result = "Chu ru";
                    break;
                case "lào":
                    result = "Lào";
                    break;
                case "la chí":
                    result = "La Chí";
                    break;
                case "la ha":
                    result = "La Ha";
                    break;
                case "phù lá":
                    result = "Phù Lá";
                    break;
                case "la hủ":
                    result = "La Hủ";
                    break;
                case "lự":
                    result = "Lự";
                    break;
                case "lô lô":
                    result = "Lô Lô";
                    break;
                case "chứt":
                    result = "Chứt";
                    break;
                case "mảng":
                    result = "Mảng";
                    break;
                case "pà thẻn":
                    result = "Pà Thẻn";
                    break;
                case "co lao":
                    result = "Co Lao";
                    break;
                case "cống":
                    result = "Cống";
                    break;
                case "bố y":
                    result = "Bố Y";
                    break;
                case "si la":
                    result = "Si La";
                    break;
                case "pu péo":
                    result = "Pu Péo";
                    break;
                case "brâu":
                    result = "Brâu";
                    break;
                case "ơ đu":
                    result = "Ơ Đu";
                    break;
                case "rơ măm":
                    result = "Rơ măm";
                    break;
                default:
                    result = "Kinh";
                    break;
            }
            return result;
        }

        public static async Task<string> GetDataClient(string baseApiAddress, string path, Dictionary<string, string> parameters)
        {
            string data = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseApiAddress);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromMinutes(5);
                var requestContent = parameters;
                //requestContent.Add("Key", "123456");
                try { 
                var response = await client.PostAsync(path, new FormUrlEncodedContent(requestContent));
                if (response.IsSuccessStatusCode)
                {
                    data = response.Content.ReadAsStringAsync().Result;
                }                
                } catch (Exception ex)
                {
                    data = "error";
                    Console.WriteLine(ex);
                }             
                return data;
            }
        }

    }
}

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ah_2class
{
    class Service
    {
        public  event FrmMain.RefreshInfo refresh;
        public List<User> Users{ get; set; }


        public void Start()
        {
            foreach (User user in Users)
            {
                if (user.Login() && user.Exam())
                {
                    refresh(user);
                }
            }
        }

        /// <summary>
        /// 读取Excel文件的第一个工作表到学生成绩集合
        /// </summary>
        /// <param name="filePath">Excel文件</param>
        /// <returns>学生成绩集合</returns>
        public static List<User> GetUsers(string filePath)
        {
            List<User> Users = new List<User>();
            try
            {
                using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var workBook = new XSSFWorkbook(file);
                    var sheet = workBook.GetSheetAt(0);
                    var rows = sheet.GetRowEnumerator();//获取所有行
                                                        //读取表头，判断科目数量
                    rows.MoveNext();
                    while (rows.MoveNext())
                    {
                        var crow = (IRow)rows.Current;
                        if (crow.GetCell(0)!=null && crow.GetCell(0).ToString().Length>0)
                        {
                            User user = new User(crow.GetCell(0).ToString());
                            if (crow.GetCell(1) != null && crow.GetCell(1).ToString().Length > 0)
                            {
                                user.Password = crow.GetCell(1).ToString();
                            }
                            if (crow.GetCell(2) != null && crow.GetCell(2).ToString().Length > 0)
                            {
                                try
                                {
                                    int p = Convert.ToInt32(crow.GetCell(2).ToString());
                                    user.ExpectPoint = p;
                                }
                                catch (Exception)
                                {
                                }                                
                            }
                            Users.Add(user);
                        }
                    }

                }
                return Users;
            }
            catch (Exception ex)
            {

                throw new Exception($"\n错误描述:{ex.Message}");
            }

        }
    }
}

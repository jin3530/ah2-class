using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ah_2class
{
    class LoginRet
    {
        public string Result { get; set; }
        public string Message { get; set; }
    }
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Point { get; set; }
        public int ExpectPoint { get; set; }
        public string School { get; set; }
        public string Reqtoken { get; set; }
        public string Time { get; set; }


        private HttpHelper httpHelper;

        public User(string userName, string password = "123456", int expectPoint = -1)
        {
            this.UserName = userName;
            this.Password = password;
            this.ExpectPoint = expectPoint;
            this.httpHelper = new HttpHelper();
        }



        public bool Login()
        {
            string ret = this.httpHelper.GetRedirectUrl("https://ah.2-class.com/api/anHui/user/anHuiUser_Login?isAnHuiMobile=0");
            ret = ret.Replace("Auth/open-login.html", "open-api/v1/users/login");
            try
            {
                ret = httpHelper.HttpPost(ret, "", new string[] { $@"account:{this.UserName}", $@"password:{this.Password}" });
                LoginRet rb = JsonConvert.DeserializeObject<LoginRet>(ret);
                ret = httpHelper.HttpGet(rb.Result, "");
                Match m = Regex.Match(ret, "reqtoken:\"(.+?)\"");
                if (m.Success)
                {
                    this.Reqtoken = m.Groups[1].ToString();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Exam()
        {
            if (this.Reqtoken==null || "".Equals(this.Reqtoken) )
            {
                return false;
            }
            QuizData qd = new QuizData();
            Random rand = new Random();
            qd.Time = rand.Next(180, 300);
            qd.Reqtoken = this.Reqtoken;
            int num = this.ExpectPoint;
            if (num==-1)
            {
                num = rand.Next(14, 21);
            }
            else
            {
                num /= 5;
            }
            if (num>20)
            {
                num = 20;
            }
            if (num<0)
            {
                num = 0;
            }
            qd.Questions.AddRange(Config.GoodQuestions.GetRange(0,num));
            qd.Questions.AddRange(Config.BadQuestions.GetRange(num,20-num));
            QuizRet rq = null;
            try
            {
                string ret = httpHelper.HttpPost("https://ah.2-class.com/api/quiz/commit", qd.ToString(), null);
                //Console.WriteLine(ret);
                ret = httpHelper.HttpGet("https://ah.2-class.com/api/quiz/getQuizCertificate", "");
                // Console.WriteLine(ret);
                rq = JsonConvert.DeserializeObject<QuizRet>(ret);
            }
            catch (Exception)
            {
                return false;
            }
            if (rq==null)
            {
                return false;
            }
            this.School = rq.data.schoolName;
            this.Point = rq.data.point;
            this.Time = Str2TimeStr(rq.data.time);
            this.Name = rq.data.userName;
            return true;
        }

        private string Str2TimeStr(string str)
        {
            int x = Convert.ToInt32(str);
            return $"{x/60}:{x%60}";
        }

        public override string ToString()
        {
            return $"学校：{this.School} 姓名：{this.Name}  用时：{this.Time}  得分：{this.Point}";
        }
    }



public class Data
    {
        public string gainDate { get; set; }
        public string award { get; set; }
        public string time { get; set; }
        public string point { get; set; }
        public string userName { get; set; }
        public string num { get; set; }
        public string schoolName { get; set; }
    }

    public class QuizRet
    {
        public Data data { get; set; }
        public string errorCode { get; set; }
        public string errorMsg { get; set; }
        public string remark { get; set; }
        public string success { get; set; }
    }
}

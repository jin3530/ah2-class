using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace ah_2class
{
    class QuizData
    {
        public List<Question> Questions = new List<Question> { };
        public int Time { get; set; }
        public string Reqtoken { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"list\":[");
            sb.Append(string.Join(",",Questions.Select(q=>q.ToString())));
            sb.Append("],\"time\":");
            sb.Append(this.Time.ToString());
            sb.Append(",\"reqtoken\":\"");
            sb.Append(this.Reqtoken);
            sb.Append("\"}");
            return sb.ToString();
        }
    }
}

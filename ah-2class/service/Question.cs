using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ah_2class
{
    class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return "{ \"questionId\": "+this.Id.ToString()+",\"questionContent\": \""+this.Content+"\"}";
        }
    }
}

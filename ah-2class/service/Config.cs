using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ah_2class
{
    class Config
    {
        public static List<Question> GoodQuestions = new List<Question> {
            new Question{Id=2579,Content= "D"},
            new Question{Id=2643,Content= "A"},
            new Question{Id=2646,Content= "A"},
            new Question{Id=2583,Content= "B"},
            new Question{Id=2586,Content= "C"},
            new Question{Id=2650,Content= "B"},
            new Question{Id=2651,Content= "A"},
            new Question{Id=2653,Content= "A"},
            new Question{Id=2654,Content= "A"},
            new Question{Id=2658,Content= "A"},
            new Question{Id=2595,Content= "D"},
            new Question{Id=2659,Content= "A"},
            new Question{Id=2596,Content= "A"},
            new Question{Id=2598,Content= "C"},
            new Question{Id=2599,Content= "D"},
            new Question{Id=2664,Content= "A"},
            new Question{Id=2603,Content= "ABCD"},
            new Question{Id=2667,Content= "B"},
            new Question{Id=2604,Content= "B"},
            new Question{Id=2669,Content= "A"},
        };
        public static List<Question> BadQuestions = new List<Question> {
            new Question{Id=2579,Content= "A"},
            new Question{Id=2643,Content= "B"},
            new Question{Id=2646,Content= "B"},
            new Question{Id=2583,Content= "A"},
            new Question{Id=2586,Content= "A"},
            new Question{Id=2650,Content= "A"},
            new Question{Id=2651,Content= "B"},
            new Question{Id=2653,Content= "B"},
            new Question{Id=2654,Content= "B"},
            new Question{Id=2658,Content= "B"},
            new Question{Id=2595,Content= "C"},
            new Question{Id=2659,Content= "B"},
            new Question{Id=2596,Content= "B"},
            new Question{Id=2598,Content= "D"},
            new Question{Id=2599,Content= "C"},
            new Question{Id=2664,Content= "B"},
            new Question{Id=2603,Content= "CD"},
            new Question{Id=2667,Content= "A"},
            new Question{Id=2604,Content= "A"},
            new Question{Id=2669,Content= "B"},
        };

    }
}

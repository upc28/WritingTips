using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huaLibBase
{  
    [Serializable]
    public class valueWord
    {
        public string word;
        public int value;      
        public valueWord(string a, int b)
        {
            word = a; value = b;
        }
    }
    [Serializable]
    public class locWord
    {
        public string words;
        public int totalTimes;
        public List<valueWord> link = new List<valueWord>();
        public locWord(string a)
        {
            words = a; totalTimes = 0;
        }
    }
}

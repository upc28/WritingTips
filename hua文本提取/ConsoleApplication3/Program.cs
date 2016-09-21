using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using huaLibBase;


namespace ConsoleApplication3
{
    class Program
    {
        public static List<locWord> wordList;
        public static Dictionary<string, int> dictionary;
        static int addWordsToList(locWord a)
        {
            int length = wordList.Count-1;
            if (length == -1)
            {
                wordList.Add(a);
                return 1;
            }
            if (String.Compare(wordList[length].words, a.words) == 0)
                return 0;
            if (String.Compare(wordList[0].words, a.words) == 1)
            {
                wordList.Insert(0, a);
                return 1;
            }
            if(String.Compare(wordList[length].words, a.words) == -1)
            {
                wordList.Add(a);
                return 1;
            }
            int max = length, min = 0,mid;
            
            int count = 0;
            while(true)
            {
                if (count == 8000)
                    return -1;
                mid = (min + max) / 2;
                if (String.Compare(wordList[mid].words, a.words) == -1 && String.Compare(wordList[mid + 1].words, a.words) == -1)
                    min = mid;
                else if (String.Compare(wordList[mid].words, a.words) == 1 && String.Compare(wordList[mid + 1].words, a.words) == 1)
                    max = mid;
                else if (String.Compare(wordList[mid].words, a.words) == 0 || String.Compare(wordList[mid + 1].words, a.words) == 0)
                    return 0;
                else { wordList.Insert(mid + 1, a);
                    return 1;
                }
                count++;
            }
        }
        static int addWordsToLink(List<valueWord> tList,valueWord tWord)
        {
            
            int length = tList.Count - 1;
            if (length == -1)
            {
                tList.Add(tWord);
                return 1;
            }
            if (tWord.value > tList[0].value)
            {
                tList.Insert(0, tWord);
                return 1;
            }
            if (tWord.value <= tList[length].value)
            {
                tList.Add(tWord);
                return 1;
            }
            int max = length, min = 0, mid;

            int count = 0;
            while (true)
            {
                if (count == 800)
                    return -1;
                mid = (min + max) / 2;
                if(tWord.value < tList[mid].value&&tWord.value > tList[mid+1].value)
                {
                    tList.Insert(mid + 1, tWord);
                    return 1;
                }
                else if (tWord.value<tList[mid].value)
                    min = mid;
                else if (tWord.value > tList[mid].value)
                    max = mid;
                
                else
                {
                    tList.Insert(mid , tWord);
                    return 1;
                }
                count++;
            }
        }
        static void Main(string[] args)
        {
            /*******************************/
            /*int totalTimes = 0, recordtimes = 0;
            StreamReader file = new StreamReader("C:\\Users\\zb\\Desktop\\hp2.txt", true);
            string gettxt = file.ReadToEnd();
            
            //非正则表达式  
            char[] splitarray = { '.',':' ,';' ,'?'};
            char[] split = { ' ' };
            
            string[] words = gettxt.Split(splitarray);
            int l = words.Length;
            
            dictionary = new Dictionary<string, int>();
            for (int i=0;i< l;i++)
            {
                //Console.WriteLine(words[i]);
                //Console.Read();

                string[] word = words[i].Split(split);
                int lengthWord = word.Length;
                if (lengthWord == 0)
                    continue;
                //检查单词是否存在多余空格
                int check = words[i].IndexOf(' ');
                if (check == -1 || check == words[i].Length - 1)
                    continue;
                
                for (int j = 0; j < lengthWord - 1; j++)
                {
                    string temp = word[j] + '#' + word[j + 1];
                    
                    if (dictionary.ContainsKey(temp))
                        dictionary[temp] += 1;
                    else {
                        dictionary.Add(temp, 1);
                        recordtimes++;
                    }
                    totalTimes += 1;
                }

            }
            wordList = new List<locWord>();
            foreach(KeyValuePair<string,int>kv in dictionary)
            {
                string[] tempWords = kv.Key.Split('#');
                locWord tlocWord = new locWord(tempWords[0]);
                if (addWordsToList(tlocWord) == 1)
                {
                    foreach (KeyValuePair<string, int> kb in dictionary)
                    {
                        if (kb.Value > 1)
                        {
                            string[] kbWords = kb.Key.Split('#');
                            if (kbWords[0].Equals(tempWords[0]))
                            {
                                valueWord tvalueWord = new valueWord(kbWords[1], kb.Value);
                                tlocWord.totalTimes += kb.Value;
                                if (addWordsToLink(tlocWord.link, tvalueWord) == -1)
                                    Console.WriteLine("error");
                            }
                        }
                    }
                }
                
            }
            IFormatter iformatter = new BinaryFormatter();

            FileStream fStream = new FileStream("C:\\Users\\zb\\Desktop\\WordsData", FileMode.Create);
            iformatter.Serialize(fStream, wordList);
            fStream.Flush();
            fStream.Close();*/
            IFormatter iformatter = new BinaryFormatter();
            FileStream tfStream = new FileStream("C:\\Users\\zb\\Desktop\\WordsData2", FileMode.Open);
            List<locWord> tlocList = (List<locWord>)iformatter.Deserialize(tfStream);
            StreamWriter filewriter = new StreamWriter("C:\\Users\\zb\\Desktop\\dataout2.txt", true);
            
            int listCount = tlocList.Count;
            for(int j=listCount-1;j>=0;j--)
            {
            
                int n = tlocList[j].link.Count;
                
                if (n == 0)
                    continue;
                //if (tlocList[j].words!=null&& (tlocList[j].words[0]<'a'|| tlocList[j].words[0] > 'z'))
                //{
                    //Console.WriteLine("j=" + j);
                    filewriter.Write(tlocList[j].words + '@');
                    //tlocList.RemoveAt(j);
                    //j++;
                    //Console.WriteLine(tlocList[j].words + '@');
                    for (int k = 0; k < n; k++)
                        //Console.Write(tlocList[j].link[k].word + '-' + tlocList[j].link[k].value+'*');
                    filewriter.Write(tlocList[j].link[k].word + '-' + tlocList[j].link[k].value + '*');
                    filewriter.WriteLine();
                    //Console.ReadLine();
               // }
                //Console.ReadLine();
            }


            /*FileStream fStream = new FileStream("C:\\Users\\zb\\Desktop\\WordsData2", FileMode.Create);
            iformatter.Serialize(fStream, tlocList);
            fStream.Flush();
            fStream.Close();*/
            filewriter.Flush();
            filewriter.Close();
        }
       
    }
}

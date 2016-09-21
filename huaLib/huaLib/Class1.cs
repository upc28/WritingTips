using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using huaLibBase;

namespace huaLib
{
    public class DinoComparer : IComparer<valueWord>
    {
        public int Compare(valueWord x, valueWord y)
        {
            if (x.value > y.value)
                return -1;
            else if (x.value < y.value)
                return 1;
            else return 0;
        }
    }
    public class Finding
    {
        List<locWord> wordList, uWordList;
        IFormatter iformatter;
        FileStream ufStream;
        DinoComparer dc;
        float totalT, utotalT;
        List<valueWord> findWord(string goalWord)
        {
            int length = wordList.Count - 1;
            int max = length, min = 0, mid;
            int count = 0;
            while (true)
            {
                if (count == 8000)
                {
                    return null;
                }
                mid = (min + max) / 2;
                if (String.Compare(wordList[mid].words, goalWord) == -1)
                    min = mid;
                else if (String.Compare(wordList[mid].words, goalWord) == 1)
                    max = mid;
                else if (String.Compare(wordList[mid].words, goalWord) == -1 && String.Compare(wordList[mid + 1].words, goalWord) == 1)
                {
                    return null;
                }
                else
                {
                    totalT = (float)wordList[mid].totalTimes;
                    return wordList[mid].link;
                }
                count++;
            }
        }
        List<valueWord> ufindWord(string goalWord)
        {
            int length = uWordList.Count;
            for (int i = 0; i < length; i++)
            {
                if (uWordList[i].words.Equals(goalWord))
                {
                    utotalT = (float)uWordList[i].totalTimes;
                    return uWordList[i].link;
                }
            }
            return null;
        }
        public string[] finds(string theWord)
        {
            theWord = theWord.ToLower();
            List<valueWord> getValueWord = findWord(theWord);
            List<valueWord> ugetValueWord = ufindWord(theWord);
            if (getValueWord == null && ugetValueWord == null)
            {
                return null;
            }
            int length, ulength;
            List<valueWord> reList = new List<valueWord>();
            if (getValueWord == null) length = 0;
            else length = getValueWord.Count;
            if (ugetValueWord == null) ulength = 0;
            else ulength = ugetValueWord.Count;

            for (int i = 0; i < length; i++)
            {
                valueWord t = new valueWord(getValueWord[i].word, getValueWord[i].value);
                reList.Add(t);
                if (reList[i].value > 8)
                    reList[i].value = (int)(reList[i].value / totalT * 1000);
                else reList[i].value = (int)(reList[i].value / totalT * 600);

            }
            for (int i = 0; i < ulength; i++)
            {
                valueWord t = new valueWord(ugetValueWord[i].word, ugetValueWord[i].value);
                reList.Add(t);
                reList[i].value = (int)(reList[i].value / utotalT * 1000);
            }
            reList.Sort(dc);
            for(int i=0;i<reList.Count;i++)
            {
                for (int k = i + 1; k < reList.Count; k++)
                {
                    if (reList[k].word.Equals(reList[i].word))
                    {
                        reList.RemoveAt(k);
                        break;
                    }
                }
            }
            int j = reList.Count;
            if (j > 5) j = 5;
            string[] reString = new string[5] { "0", "0", "0", "0", "0" };
            for (int k = 0; k < j; k++)
                reString[k] = reList[k].word;

            return reString;
        }
        public void init()
        {
            dc = new DinoComparer();
            iformatter = new BinaryFormatter();
            FileStream tfStream = new FileStream("WordsData", FileMode.Open);
            wordList = (List<locWord>)iformatter.Deserialize(tfStream);
            tfStream.Close();
            if (File.Exists("UserData"))
            {
                ufStream = new FileStream("UserData", FileMode.OpenOrCreate);
                uWordList = (List<locWord>)iformatter.Deserialize(ufStream);
                ufStream.Close();
            }
            else uWordList = new List<locWord>();
            
        }
        public void close()
        {
            ufStream = new FileStream("UserData", FileMode.OpenOrCreate);
            iformatter.Serialize(ufStream, uWordList);
            ufStream.Flush();
            ufStream.Close();
        }
        int uaddWordsToList(locWord a, valueWord b)
        {
            a.totalTimes++;
            int length = uWordList.Count - 1;
            if (length == -1)
            {
                uWordList.Add(a);
                uaddWordsToLink(a.link, b);
                return 1;
            }
            if (String.Compare(uWordList[length].words, a.words) == 0)
            {
                uWordList[length].totalTimes++;
                uaddWordsToLink(uWordList[length].link, b);
                uWordList[length].link.Sort(dc);
                return 0;
            }
            if (String.Compare(uWordList[0].words, a.words) == 1)
            {
                uWordList.Insert(0, a);
                uaddWordsToLink(a.link, b);
                return 1;
            }
            if (String.Compare(uWordList[length].words, a.words) == -1)
            {
                uWordList.Add(a);
                uaddWordsToLink(a.link, b);
                return 1;
            }
            int max = length, min = 0, mid;
            int count = 0;
            while (true)
            {
                if (count == 8000)
                    return -1;
                mid = (min + max) / 2;
                if (String.Compare(uWordList[mid].words, a.words) == -1 && String.Compare(uWordList[mid + 1].words, a.words) == -1)
                    min = mid;
                else if (String.Compare(uWordList[mid].words, a.words) == 1 && String.Compare(uWordList[mid + 1].words, a.words) == 1)
                    max = mid;
                else if (String.Compare(uWordList[mid].words, a.words) == 0 || String.Compare(uWordList[mid + 1].words, a.words) == 0)
                {
                    uWordList[mid].totalTimes++;
                    uaddWordsToLink(uWordList[mid].link, b);
                    uWordList[mid].link.Sort(dc);
                }
                else
                {
                    uWordList.Insert(mid + 1, a);
                    uaddWordsToLink(a.link, b);
                    return 1;
                }
                count++;
            }
        }
        int uaddWordsToLink(List<valueWord> tList, valueWord tWord)
        {
            int length = tList.Count;
            for (int i = 0; i < length; i++)
            {
                if (tList[i].word.Equals(tWord.word))
                {
                    tList[i].value++;
                    return 1;
                }

            }
            tList.Insert(0, tWord);
            return 0;

        }
        public void choice(string a, string b)
        {
            locWord userWord = new locWord(a);
            valueWord userValue = new valueWord(b, 1);
            uaddWordsToList(userWord, userValue);
        }
    }
}

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

namespace crossword.engine
{
    [Serializable]
    public class CrosswordDatabaseItem
    {
        public string question;
        public string answer;

        public CrosswordDatabaseItem(string question, string answer)
        {
            this.question = question;
            this.answer = answer;
        }

        public override string ToString()
        {
            return string.Format("[CrosswordDatabaseItem] QUESTION: {0} ~ ANSWER: {1}", question, answer);
        }
    }

    [Serializable]
    public class CrosswordDatabase
    {
        public List<CrosswordDatabaseItem> database;

        public void InitiateDatabase()
        {
            StreamReader reader = new StreamReader("Assets/database.json");
            string s = reader.ReadToEnd();
            database = UnityEngine.JsonUtility.FromJson<CrosswordDatabase>(s).database;
        }

        public void AddItemToDatabase(string question, string answer)
        {
            database.Add(new CrosswordDatabaseItem(question, answer));
        }

        public CrosswordDatabaseItem GetRandomItem()
        {
            return database.GetRandomElement();
        }

        public CrosswordDatabaseItem GetRandomItem(int lessThanCharCount, int equalCharCount, List<Tuple<int, string>> intersectionsTuples)
        {
            var list = GetRandomItems(lessThanCharCount, equalCharCount, intersectionsTuples);
            if (list.Count == 0) return null;

            CrosswordDatabaseItem toReturn = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                if (toReturn.answer.Length < list[i].answer.Length)
                    toReturn = list[i];
            }

            return toReturn;
        }

        public List<CrosswordDatabaseItem> GetRandomItems(int lessThanCharCount, int equalCharCount, List<Tuple<int, string>> intersectionsTuples)
        {
            var list = database.FindAll(x => x.answer.Length < lessThanCharCount || x.answer.Length == equalCharCount);

            for (int i = 0; i < intersectionsTuples.Count; i++)
            {
                list = list.FindAll(delegate (CrosswordDatabaseItem x)
                {
                    if (x.answer.Length <= intersectionsTuples[i].value1)
                        return true;
                    return x.answer[intersectionsTuples[i].value1].ToString() == intersectionsTuples[i].value2;
                });
            }
            return list;
        }
    }
}
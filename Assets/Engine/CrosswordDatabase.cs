using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

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

        public bool AnswerStartsWith(char c)
        {
            if (string.IsNullOrEmpty(answer)) return false;
            return answer[0] == c;
        }

        public bool AnswerCharCount(int n)
        {
            if (string.IsNullOrEmpty(answer)) return false;
            return answer.Length == n;
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

        public void AddItemToDatabase(string question, string answer)
        {
            database.Add(new CrosswordDatabaseItem(question, answer));
        }

        public CrosswordDatabaseItem GetRandomItem()
        {
            return database.GetRandomElement();
        }

        public List<CrosswordDatabaseItem> GetRandomItems(int minCharCount, int maxCharCount, List<Tuple<int, string>> intersectionsTuples)
        {
            var list = database.FindAll(x => x.answer.Length >= minCharCount && x.answer.Length <= maxCharCount);

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

        public CrosswordDatabaseItem GetItem(int charcount, char firstchar)
        {
            return database.FindAll(x => x.AnswerCharCount(charcount)).FindAll(x => x.AnswerStartsWith(firstchar)).GetRandomElement();
        }
    }
}
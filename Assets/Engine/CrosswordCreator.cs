using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace crossword.engine
{
    public struct CrosswordPosition
    {
        public int row;
        public int column;

        public CrosswordPosition(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public override string ToString()
        {
            return string.Format("[Vector2Int] (X: {0}, Y: {1})", row, column);
        }
    }

    public class StartPositionAndOrientation
    {
        public CrosswordPosition position;
        public CrosswordTileItem.Orientation orientation;

        public StartPositionAndOrientation(CrosswordPosition position, CrosswordTileItem.Orientation orientation)
        {
            this.position = position;
            this.orientation = orientation;
        }

        public override string ToString()
        {
            return string.Format("[StartPositionAndOrientation] POSITION: {0}, ORIENTATON: {1}", position.ToString(), orientation.ToString());
        }
    }

    public class CrosswordCreator
    {
        public const string EMPTY_SLOT_CHARACTER = "~";

        private CrosswordsDatabase mDatabase = new CrosswordsDatabase();
        private Crossword mCrossword;
        private int mSizeWidth;
        private int mSizeHeight;

        public void InitDatabase()
        {
            mDatabase.SetupDatabase();
        }

        public Crossword CreateCrossword(int sizeWidth = 9, int sizeHeight = 14)
        {
            mSizeWidth = sizeWidth;
            mSizeHeight = sizeHeight;

            mCrossword = new Crossword();
            mCrossword.SetupTiles(mSizeWidth, mSizeHeight);

            //Setup corner questions
            SetQuestionTile(0, 0, new StartPositionAndOrientation(new CrosswordPosition(0, 1), CrosswordTileItem.Orientation.VERTICAL), mSizeHeight, mSizeHeight);
            SetQuestionTile(2, 0, new StartPositionAndOrientation(new CrosswordPosition(1, 0), CrosswordTileItem.Orientation.HORIZONTAL), mSizeWidth, mSizeWidth);
            SetQuestionTile(mSizeHeight - 2, 0, new StartPositionAndOrientation(new CrosswordPosition(mSizeHeight - 1, 0), CrosswordTileItem.Orientation.HORIZONTAL), mSizeWidth, mSizeWidth);
            SetQuestionTile(0, mSizeWidth - 2, new StartPositionAndOrientation(new CrosswordPosition(0, mSizeWidth - 1), CrosswordTileItem.Orientation.VERTICAL), mSizeHeight, mSizeHeight);

            //Setup the rest of the questions
            SetupQuestions();

            mCrossword.Log();

            return mCrossword;

            //ForceQuestionTile(mCrossword.GetTile(0, 0), new StartPositionAndOrientation());

            //SetupCrosswordTiles();
        }


        private void SetupQuestions()
        {
            #region Rows
            var nList = Enumerable.Range(0, mSizeHeight - 1).ToList();
            nList.Shuffle();

            for (int i = 0; i < nList.Count; i++)
            {
                if (mCrossword.GetTile(nList[i], 0).hasValue) continue;

                FillLine(nList[i], 0, CrosswordTileItem.Orientation.HORIZONTAL);
            }
            #endregion

            nList.Clear();

            #region Columns
            nList = Enumerable.Range(0, mSizeWidth - 1).ToList();
            nList.Shuffle();

            for (int i = 0; i < nList.Count; i++)
            {
                if (mCrossword.GetTile(0, nList[i]).hasValue) continue;

                FillLine(0, nList[i], CrosswordTileItem.Orientation.VERTICAL);
            }
            #endregion
        }

        private void FillLine(int row, int column, CrosswordTileItem.Orientation orientation, bool randomOrientation = false)
        {
            if (row >= mSizeHeight || column >= mSizeWidth) return;

            if(!mCrossword.GetTile(row, column).hasValue){
                var answerStartTile = GetAvailableTilesAround(row, column).GetRandomElement();
                if(!randomOrientation)
                    answerStartTile.orientation = orientation;
                SetQuestionTile(row, column, answerStartTile, 1, GetMaxAnswerLength(answerStartTile) - 1);
            }

            row = orientation == CrosswordTileItem.Orientation.VERTICAL ? row + 1 : row;
            column = orientation == CrosswordTileItem.Orientation.HORIZONTAL ? column + 1 : column;

            FillLine(row, column, orientation, true);
        }

        private void SetQuestionTile(int questionRow, int questionColumn, StartPositionAndOrientation answerStartTile, int minCharCount, int maxCharCount = -1)
        {
            var intersections = GetIntersections(answerStartTile);
            if (maxCharCount == -1)
                maxCharCount = GetMaxAnswerLength(answerStartTile);
            var crossDatabaseItem = mDatabase.GetRandomItems(minCharCount, maxCharCount, GetIntersectionTuples(intersections)).GetRandomElement();

            if (crossDatabaseItem == null)
            {
                Debug.LogWarning(string.Format("Impossible to find word with max characters: {0} and with intersections: {1}", maxCharCount, IntersectionToString(intersections)));
                return;
            }

            var crossItem = new CrosswordTileItem();
            crossItem.orientation = answerStartTile.orientation;
            crossItem.element = crossDatabaseItem.question;
            crossItem.hasValue = true;
            crossItem.questionElement = crossItem;

            Debug.Log(string.Format("Setting item: {0} at {1}", crossItem.ToString(), answerStartTile.ToString()));

            mCrossword.SetTile(questionRow, questionColumn, crossItem);
            SetAnswerTiles(crossItem, crossDatabaseItem, answerStartTile);
        }

        //private void SetQuestionTile(CrosswordTileItem item)
        //{
        //    var answerStartTile = GetAvailableTilesAround(item.row, item.column).GetRandomElement();
        //    if (answerStartTile == null) return;

        //    var maxCharLength = GetMaxAnswerLength(answerStartTile);
        //    if (maxCharLength == 0) return;

        //    var intersections = GetIntersections(answerStartTile);
        //    if (intersections.TrueForAll(x => x != EMPTY_SLOT_CHARACTER)) return;

        //    var crossDatabaseItem = mDatabase.GetRandomItems(1, maxCharLength, GetIntersectionTuples(intersections)).GetRandomElement();

        //    Debug.Log(string.Format("ANSWER TILE: {0}, MAX CHAR: {1}, INTERSECTION: {2}, DATABASE ITEM: {3}", answerStartTile.ToString(), maxCharLength, IntersectionToString(intersections), crossDatabaseItem.ToString()));

        //    var crossItem = new CrosswordTileItem();
        //    crossItem.orientation = answerStartTile.orientation;
        //    crossItem.element = crossDatabaseItem.question;
        //    crossItem.hasValue = true;
        //    crossItem.questionElement = crossItem;

        //    mCrossword.SetTile(item.row, item.column, crossItem);

        //    SetAnswerTiles(crossItem, crossDatabaseItem, answerStartTile);
        //}

        private void SetAnswerTiles(CrosswordTileItem crossTileItem, CrosswordDatabaseItem crossDatabaseItem, StartPositionAndOrientation answerStartTile)
        {
            for (int i = 0; i < crossDatabaseItem.answer.Length; i++)
            {
                int x = answerStartTile.orientation == CrosswordTileItem.Orientation.VERTICAL ? answerStartTile.position.row + i : answerStartTile.position.row;
                int y = answerStartTile.orientation == CrosswordTileItem.Orientation.HORIZONTAL ? answerStartTile.position.column + i : answerStartTile.position.column;

                var crossItem = new CrosswordTileItem();
                crossItem.orientation = answerStartTile.orientation;
                crossItem.element = GetCharacterAtIndex(crossDatabaseItem.answer, i).ToString();
                crossItem.hasValue = true;
                crossItem.questionElement = crossTileItem;

                mCrossword.SetTile(x, y, crossItem);
            }
        }

        private List<StartPositionAndOrientation> GetAvailableTilesAround(int row, int column)
        {
            List<StartPositionAndOrientation> toReturn = new List<StartPositionAndOrientation>();

            if (TileAvailable(row, column + 1))
            {
                //right horizontal
                toReturn.Add(new StartPositionAndOrientation(new CrosswordPosition(row, column + 1), CrosswordTileItem.Orientation.HORIZONTAL));
                //right vertical
                if (row < mSizeWidth)
                    toReturn.Add(new StartPositionAndOrientation(new CrosswordPosition(row, column + 1), CrosswordTileItem.Orientation.VERTICAL));
            }
            if (TileAvailable(row + 1, column))
            {
                //down horizontal
                if (column < mSizeWidth)
                    toReturn.Add(new StartPositionAndOrientation(new CrosswordPosition(row + 1, column), CrosswordTileItem.Orientation.HORIZONTAL));
                //down vertical
                toReturn.Add(new StartPositionAndOrientation(new CrosswordPosition(row + 1, column), CrosswordTileItem.Orientation.VERTICAL));
            }

            //left vertical
            if (TileAvailable(row, column - 1) && row < mSizeHeight)
                toReturn.Add(new StartPositionAndOrientation(new CrosswordPosition(row, column - 1), CrosswordTileItem.Orientation.VERTICAL));

            //up horizontal
            if (TileAvailable(row - 1, column) && column < mSizeWidth)
                toReturn.Add(new StartPositionAndOrientation(new CrosswordPosition(row - 1, column), CrosswordTileItem.Orientation.HORIZONTAL));

            return toReturn;
        }

        private int GetMaxAnswerLength(StartPositionAndOrientation posAndOri)
        {
            int toReturn = 0;
            bool canMove = true;
            while (canMove)
            {
                toReturn++;

                int x = posAndOri.orientation == CrosswordTileItem.Orientation.VERTICAL ? posAndOri.position.row + toReturn : posAndOri.position.row;
                int y = posAndOri.orientation == CrosswordTileItem.Orientation.HORIZONTAL ? posAndOri.position.column + toReturn : posAndOri.position.column;

                canMove = TileAvailable(x, y);
            }

            return toReturn;
        }

        private List<string> GetIntersections(StartPositionAndOrientation posAndOri)
        {
            List<string> toReturn = new List<string>();
            bool canMove = true;
            int iterator = 0;
            while (canMove)
            {
                string character = EMPTY_SLOT_CHARACTER;

                int row = posAndOri.orientation == CrosswordTileItem.Orientation.VERTICAL ? posAndOri.position.row + iterator : posAndOri.position.row;
                int column = posAndOri.orientation == CrosswordTileItem.Orientation.HORIZONTAL ? posAndOri.position.column + iterator : posAndOri.position.column;

                canMove = TileAvailable(row, column);
                if (!canMove)
                    return toReturn;

                if (mCrossword.GetTile(row, column).hasValue && !mCrossword.GetTile(row, column).isQuestion)
                    character = mCrossword.GetTile(row, column).element;

                toReturn.Add(character);
                iterator++;
            }
            return toReturn;
        }

        private bool TileAvailable(int row, int column)
        {
            return row >= 0 && row < mSizeHeight && column >= 0 && column < mSizeWidth && !mCrossword.GetTile(row, column).isQuestion;
        }

        private char GetCharacterAtIndex(string word, int i)
        {
            return word[i];
        }

        private string IntersectionToString(List<string> intersections)
        {
            string log = "";
            intersections.ForEach(x => log += "[" + x + "]");
            return log;
        }

        private List<Tuple<int, string>> GetIntersectionTuples(List<string> intersections)
        {
            List<Tuple<int, string>> tuples = new List<Tuple<int, string>>();
            for (int i = 0; i < intersections.Count; i++)
            {
                if (intersections[i] == EMPTY_SLOT_CHARACTER) continue;
                tuples.Add(new Tuple<int, string>(i, intersections[i]));
            }

            return tuples;
        }
    }
}
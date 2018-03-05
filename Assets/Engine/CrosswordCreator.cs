using System;
using System.Collections.Generic;
using System.IO;
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
            return string.Format("[Vector2Int] (ROW: {0}, COLUMN: {1})", row, column);
        }
    }

    public struct CrosswordPositionAndOrientation
    {
        public CrosswordPosition position;
        public CrosswordOrientation orientation;

        public CrosswordPositionAndOrientation(CrosswordPosition position, CrosswordOrientation orientation)
        {
            this.position = position;
            this.orientation = orientation;
        }

        public override string ToString()
        {
            return string.Format("[CrosswordPositionAndOrientation] POSITION: {0}, ORIENTATON: {1}", position.ToString(), orientation.ToString());
        }
    }

    public class CrosswordCreator
    {
        public const string EMPTY_SLOT_CHARACTER = "~";

        private CrosswordDatabase mDatabase = new CrosswordDatabase();
        private Crossword mCrossword;
        private int mSizeWidth;
        private int mSizeHeight;

        public Crossword CreateCrossword(int sizeWidth = 9, int sizeHeight = 14)
        {
            mSizeWidth = sizeWidth;
            mSizeHeight = sizeHeight;

            mDatabase.InitiateDatabase();

            mCrossword = new Crossword();
            mCrossword.SetupTiles(mSizeWidth, mSizeHeight);

            //Setup corner questions
            SetQuestionTile(new CrosswordPosition(0, 0), new CrosswordPositionAndOrientation(new CrosswordPosition(0, 1), CrosswordOrientation.VERTICAL), new Tuple<int, int>(0, mSizeHeight));
            SetQuestionTile(new CrosswordPosition(2, 0), new CrosswordPositionAndOrientation(new CrosswordPosition(1, 0), CrosswordOrientation.HORIZONTAL), new Tuple<int, int>(0, mSizeWidth));
            SetQuestionTile(new CrosswordPosition(mSizeHeight - 2, 0), new CrosswordPositionAndOrientation(new CrosswordPosition(mSizeHeight - 1, 0), CrosswordOrientation.HORIZONTAL), new Tuple<int, int>(0, mSizeWidth));
            SetQuestionTile(new CrosswordPosition(0, mSizeWidth - 2), new CrosswordPositionAndOrientation(new CrosswordPosition(0, mSizeWidth - 1), CrosswordOrientation.VERTICAL), new Tuple<int, int>(0, mSizeHeight));

            //Setup the rest of the questions
            SetupQuestions();

            mCrossword.Log();

            return mCrossword;
        }


        private void SetupQuestions()
        {
            #region Rows
            List<int> nList = Enumerable.Range(3, mSizeHeight - 5).ToList();
            nList.Shuffle();

            CrosswordPositionAndOrientation answerStartTile;

            for (int i = 0; i < nList.Count; i++)
            {
                if (mCrossword.GetTile(new CrosswordPosition(nList[i], 0)).hasValue) continue;

                answerStartTile = GetAvailableTilesAround(nList[i], 0).GetRandomElement();
                answerStartTile.orientation = CrosswordOrientation.HORIZONTAL;
                FillLine(new CrosswordPosition(nList[i], 0), CrosswordOrientation.HORIZONTAL, answerStartTile);
            }
            #endregion
            return;
            nList.Clear();

            #region Columns
            nList = Enumerable.Range(2, mSizeWidth - 2).ToList();
            nList.Shuffle();

            for (int i = 0; i < nList.Count; i++)
            {
                if (mCrossword.GetTile(new CrosswordPosition(0, nList[i])).hasValue) continue;

                answerStartTile = GetAvailableTilesAround(0, nList[i]).GetRandomElement();
                answerStartTile.orientation = CrosswordOrientation.VERTICAL;
                FillLine(new CrosswordPosition(0, nList[i]), CrosswordOrientation.VERTICAL, answerStartTile);
            }
            #endregion
        }

        private void FillLine(CrosswordPosition questionPos, CrosswordOrientation orientation, CrosswordPositionAndOrientation? answerStartPos = null)
        {
            if (!InBounds(questionPos)) return;

            var answerTile = answerStartPos.HasValue ? answerStartPos.Value : new CrosswordPositionAndOrientation(GetForwardTilePosition(questionPos, orientation), orientation);

            if(!mCrossword.GetTile(questionPos).hasValue){
                var tile = mCrossword.GetTile(answerTile.position);
                if (tile is CrosswordTileAnswerItem && !answerStartPos.HasValue)
                    answerTile.orientation = ((CrosswordTileAnswerItem)tile).oppositeDirection;

                SetQuestionTile(questionPos, answerTile, GetMinMaxAnswerLength(answerTile));
            }

            FillLine(answerTile.position, orientation);
        }

        private void SetQuestionTile(CrosswordPosition questionPos, CrosswordPositionAndOrientation answerStartTile, Tuple<int, int> answerLessOrGreaterThan)
        {
            var intersections = GetIntersections(answerStartTile);
            var crossDatabaseItem = mDatabase.GetRandomItem(answerLessOrGreaterThan.value1, answerLessOrGreaterThan.value2, GetIntersectionTuples(intersections));

            if (crossDatabaseItem == null)
            {
                Debug.LogWarning(string.Format("Impossible to find word with min/max characters: {0} and with intersections: {1}", answerLessOrGreaterThan, IntersectionToString(intersections)));
                return;
            }

            var crossItem = new CrosswordTileQuestionItem();
            crossItem.orientation = answerStartTile.orientation;
            crossItem.element = crossDatabaseItem.question;
            crossItem.startPositionAndOrientation = answerStartTile;

            //Debug.Log(string.Format("Setting item: {0} at {1}", crossItem.ToString(), answerStartTile.ToString()));

            mCrossword.SetTile(questionPos, crossItem);
            SetAnswerTiles(crossItem, crossDatabaseItem, answerStartTile);
        }

        private void SetAnswerTiles(CrosswordTileQuestionItem crossQuestionTileItem, CrosswordDatabaseItem crossDatabaseItem, CrosswordPositionAndOrientation answerStartTile)
        {
            for (int i = 0; i < crossDatabaseItem.answer.Length; i++)
            {
                var newPos = GetForwardTilePosition(answerStartTile.position, answerStartTile.orientation, i);
                var crossItem = new CrosswordTileAnswerItem();
                crossItem.orientation = answerStartTile.orientation;
                crossItem.element = GetCharacterAtIndex(crossDatabaseItem.answer, i).ToString();
                crossItem.questionElement = crossQuestionTileItem;

                mCrossword.SetTile(newPos, crossItem);
            }
        }

        private List<CrosswordPositionAndOrientation> GetAvailableTilesAround(int row, int column)
        {
            List<CrosswordPositionAndOrientation> toReturn = new List<CrosswordPositionAndOrientation>();

            if (TileAvailable(new CrosswordPosition(row, column + 1)))
            {
                //right horizontal
                toReturn.Add(new CrosswordPositionAndOrientation(new CrosswordPosition(row, column + 1), CrosswordOrientation.HORIZONTAL));
                //right vertical
                if (row < mSizeWidth)
                    toReturn.Add(new CrosswordPositionAndOrientation(new CrosswordPosition(row, column + 1), CrosswordOrientation.VERTICAL));
            }
            if (TileAvailable(new CrosswordPosition(row + 1, column)))
            {
                //down horizontal
                if (column < mSizeWidth)
                    toReturn.Add(new CrosswordPositionAndOrientation(new CrosswordPosition(row + 1, column), CrosswordOrientation.HORIZONTAL));
                //down vertical
                toReturn.Add(new CrosswordPositionAndOrientation(new CrosswordPosition(row + 1, column), CrosswordOrientation.VERTICAL));
            }

            //left vertical
                if (TileAvailable(new CrosswordPosition(row, column - 1)) && row < mSizeHeight)
                toReturn.Add(new CrosswordPositionAndOrientation(new CrosswordPosition(row, column - 1), CrosswordOrientation.VERTICAL));

            //up horizontal
            if (TileAvailable(new CrosswordPosition(row - 1, column)) && column < mSizeWidth)
                toReturn.Add(new CrosswordPositionAndOrientation(new CrosswordPosition(row - 1, column), CrosswordOrientation.HORIZONTAL));

            return toReturn;
        }

        private Tuple<int, int> GetMinMaxAnswerLength(CrosswordPositionAndOrientation posAndOri)
        {
            int equal = -1;

            bool canMove = true;
            while (canMove)
            {
                equal++;

                var newPos = GetForwardTilePosition(posAndOri.position, posAndOri.orientation, equal);
                canMove = TileAvailable(newPos);
            }

            int lessThan = MathUtils.Clamp(equal - 1, 0, int.MaxValue);

            return new Tuple<int, int>(lessThan, equal);
        }

        private List<string> GetIntersections(CrosswordPositionAndOrientation posAndOri)
        {
            List<string> toReturn = new List<string>();
            bool canMove = true;
            int iterator = 0;
            while (canMove)
            {
                string character = EMPTY_SLOT_CHARACTER;

                int row = posAndOri.orientation == CrosswordOrientation.VERTICAL ? posAndOri.position.row + iterator : posAndOri.position.row;
                int column = posAndOri.orientation == CrosswordOrientation.HORIZONTAL ? posAndOri.position.column + iterator : posAndOri.position.column;

                canMove = TileAvailable(new CrosswordPosition(row, column));
                if (!canMove)
                    return toReturn;

                if (mCrossword.GetTile(new CrosswordPosition(row, column)).hasValue && !(mCrossword.GetTile(new CrosswordPosition(row, column)) is CrosswordTileQuestionItem))
                    character = mCrossword.GetTile(new CrosswordPosition(row, column)).element;

                toReturn.Add(character);
                iterator++;
            }
            return toReturn;
        }

        private CrosswordPosition GetForwardTilePosition(CrosswordPosition pos, CrosswordOrientation orientation, int tileDistance = 1)
        {
            pos.row = orientation == CrosswordOrientation.VERTICAL ? pos.row + tileDistance : pos.row;
            pos.column = orientation == CrosswordOrientation.HORIZONTAL ? pos.column + tileDistance : pos.column;
            return pos;
        }

        private bool TileAvailable(CrosswordPosition pos)
        {
            return InBounds(pos) && !(mCrossword.GetTile(pos) is CrosswordTileQuestionItem);
        }

        private bool InBounds(CrosswordPosition pos)
        {
            return pos.row >= 0 && pos.row < mSizeHeight && pos.column >= 0 && pos.column < mSizeWidth;
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
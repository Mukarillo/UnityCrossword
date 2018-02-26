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

        public void InitDatabase()
        {
            StreamReader reader = new StreamReader("Assets/database.json");
            string s = reader.ReadToEnd();
            mDatabase = JsonUtility.FromJson<CrosswordDatabase>(s);
        }

        public Crossword CreateCrossword(int sizeWidth = 9, int sizeHeight = 14)
        {
            mSizeWidth = sizeWidth;
            mSizeHeight = sizeHeight;

            mCrossword = new Crossword();
            mCrossword.SetupTiles(mSizeWidth, mSizeHeight);

            //Setup corner questions
            SetQuestionTile(0, 0, new CrosswordPositionAndOrientation(new CrosswordPosition(0, 1), CrosswordOrientation.VERTICAL), new Tuple<int, int>(mSizeHeight, mSizeHeight));
            SetQuestionTile(2, 0, new CrosswordPositionAndOrientation(new CrosswordPosition(1, 0), CrosswordOrientation.HORIZONTAL), new Tuple<int, int>(mSizeWidth, mSizeWidth));
            SetQuestionTile(mSizeHeight - 2, 0, new CrosswordPositionAndOrientation(new CrosswordPosition(mSizeHeight - 1, 0), CrosswordOrientation.HORIZONTAL), new Tuple<int, int>(mSizeWidth, mSizeWidth));
            SetQuestionTile(0, mSizeWidth - 2, new CrosswordPositionAndOrientation(new CrosswordPosition(0, mSizeWidth - 1), CrosswordOrientation.VERTICAL), new Tuple<int, int>(mSizeHeight, mSizeHeight));

            //Setup the rest of the questions
            SetupQuestions();

            mCrossword.Log();

            return mCrossword;
        }


        private void SetupQuestions()
        {
            #region Rows
            var nList = Enumerable.Range(1, mSizeHeight - 1).ToList();
            nList.Shuffle();

            CrosswordPositionAndOrientation tile;

            for (int i = 0; i < nList.Count; i++)
            {
                if (mCrossword.GetTile(new CrosswordPosition(nList[i], 0)).hasValue) continue;

                tile = GetAvailableTilesAround(nList[i], 0).GetRandomElement();
                tile.orientation = CrosswordOrientation.HORIZONTAL;
                FillLine(new CrosswordPosition(nList[i], 0), CrosswordOrientation.HORIZONTAL, tile);
            }
            #endregion

            return;

            nList.Clear();

            #region Columns
            nList = Enumerable.Range(1, mSizeWidth - 1).ToList();
            nList.Shuffle();

            for (int i = 0; i < nList.Count; i++)
            {
                if (mCrossword.GetTile(new CrosswordPosition(0, nList[i])).hasValue) continue;

                tile = GetAvailableTilesAround(0, nList[i]).GetRandomElement();
                tile.orientation = CrosswordOrientation.VERTICAL;
                FillLine(new CrosswordPosition(0, nList[i]), CrosswordOrientation.VERTICAL, tile);
            }
            #endregion
        }

        private void FillLine(CrosswordPosition pos, CrosswordOrientation orientation, CrosswordPositionAndOrientation? posOri = null)
        {
            if (pos.row >= mSizeHeight || pos.column >= mSizeWidth) return;

            var answerStartTile = posOri.HasValue ? posOri.Value : new CrosswordPositionAndOrientation(GetForwardTilePosition(pos, orientation), orientation);

            if(!mCrossword.GetTile(pos).hasValue){
                var tile = mCrossword.GetTile(answerStartTile.position);
                if (tile is CrosswordTileAnswerItem)
                    answerStartTile.orientation = ((CrosswordTileAnswerItem)tile).oppositeDirection;

                SetQuestionTile(pos.row, pos.column, answerStartTile, GetMinMaxAnswerLength(answerStartTile));
            }

            FillLine(answerStartTile.position, orientation);
        }

        private void SetQuestionTile(int questionRow, int questionColumn, CrosswordPositionAndOrientation answerStartTile, Tuple<int, int> answerMinMaxConstrains)
        {
            var intersections = GetIntersections(answerStartTile);
            var crossDatabaseItem = mDatabase.GetRandomItems(answerMinMaxConstrains.value1, answerMinMaxConstrains.value2, GetIntersectionTuples(intersections)).GetRandomElement();

            if (crossDatabaseItem == null)
            {
                Debug.LogWarning(string.Format("Impossible to find word with min/max characters: {0} and with intersections: {1}", answerMinMaxConstrains, IntersectionToString(intersections)));
                return;
            }

            var crossItem = new CrosswordTileQuestionItem();
            crossItem.orientation = answerStartTile.orientation;
            crossItem.element = crossDatabaseItem.question;
            crossItem.startPositionAndOrientation = answerStartTile;

            //Debug.Log(string.Format("Setting item: {0} at {1}", crossItem.ToString(), answerStartTile.ToString()));

            mCrossword.SetTile(new CrosswordPosition(questionRow, questionColumn), crossItem);
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
            int min = 1;
            int max = -1;

            bool canMove = true;
            while (canMove)
            {
                max++;

                var newPos = GetForwardTilePosition(posAndOri.position, posAndOri.orientation, max);
                canMove = TileAvailable(newPos);
            }

            return new Tuple<int, int>(min, max);
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
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

namespace crossword.engine
{
    [Serializable]
    public class Crossword
    {
        public CrosswordTileItem[,] tiles;

        public void SetupTiles(int width, int height)
        {
            tiles = new CrosswordTileItem[height, width];

            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for (int column = 0; column < tiles.GetLength(1); column++)
                {
                    SetTile(new CrosswordPosition(row, column), new CrosswordTileItem());
                }
            }
        }

        public void SetTile(CrosswordPosition pos , CrosswordTileItem tileItem)
        {
            tileItem.row = pos.row;
            tileItem.column = pos.column;
            tiles[pos.row, pos.column] = tileItem;
        }

        public CrosswordTileItem GetTile(CrosswordPosition pos)
        {
            return tiles[pos.row, pos.column];
        }

        public bool HasUnsetTiles()
        {
            bool toReturn = false;
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for (int column = 0; column < tiles.GetLength(1); column++)
                {
                    if (!tiles[row, column].hasValue) toReturn = true;
                }
            }

            return toReturn;
        }

        public List<CrosswordTileItem> GetUnusedTiles()
        {
            List<CrosswordTileItem> unusuedTiles = new List<CrosswordTileItem>();
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for (int column = 0; column < tiles.GetLength(1); column++)
                {
                    if (!tiles[row, column].hasValue)
                        unusuedTiles.Add(tiles[row, column]);
                }
            }

            return unusuedTiles;
        }

        public override string ToString()
        {
            string log = "SIZE(0): " + tiles.GetLength(0) + ", SIZE(1): " + tiles.GetLength(1);
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                log += "\n";
                for (int column = 0; column < tiles.GetLength(1); column++)
                {
                    if (tiles[row, column].hasValue)
                        log += tiles[row, column] is CrosswordTileQuestionItem ? "[~]" : "[" + tiles[row, column].element + "]";
                    else
                        log += "[0]";
                }
            }
            return string.Format("[Crossword]\n{0}", log);
        }

        public void Log()
        {
            Debug.Log(ToString());
        }
    }
}

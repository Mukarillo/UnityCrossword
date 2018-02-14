using System.Collections.Generic;

namespace crossword.engine
{
    public class CrosswordTileItem
    {
        public enum Orientation
        {
            HORIZONTAL,
            VERTICAL
        }

        public CrosswordTileItem questionElement;

        public Orientation orientation;
        public int row;
        public int column;
        public bool hasValue;
        public string element;

        public bool isQuestion
        {
            get
            {
                if (questionElement == null) return false;
                return row == questionElement.row && column == questionElement.column;
            }
        }
    }
}

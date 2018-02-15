using System.Collections.Generic;

namespace crossword.engine
{
    public enum TileOrientation
    {
        HORIZONTAL,
        VERTICAL
    }

    public class CrosswordTileItem
    {
        public TileOrientation orientation;
        public int row;
        public int column;
        public string element;

        public virtual bool hasValue { get; protected set; }
    }
}

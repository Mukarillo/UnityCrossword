using System.Collections.Generic;

namespace crossword.engine
{
    public enum CrosswordOrientation
    {
        HORIZONTAL,
        VERTICAL
    }

    public class CrosswordTileItem
    {
        public CrosswordOrientation orientation;

        public int row;
        public int column;
        public string element;

        public virtual bool hasValue { get; protected set; }

        public override string ToString()
        {
            return string.Format("[CrosswordTileItem: row={0}, column={1}, element={2}]", row, column, element);
        }
    }
}

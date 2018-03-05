namespace crossword.engine
{
    public class CrosswordTileAnswerItem : CrosswordTileItem
    {
        public override bool hasValue { get { return true; } }
        public CrosswordOrientation oppositeDirection{ get { return orientation == CrosswordOrientation.HORIZONTAL ? CrosswordOrientation.VERTICAL : CrosswordOrientation.HORIZONTAL; } } 
        public CrosswordTileQuestionItem questionElement;
    }
}
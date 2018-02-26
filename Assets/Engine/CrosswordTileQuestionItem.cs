namespace crossword.engine
{
    public class CrosswordTileQuestionItem : CrosswordTileItem
    {
        public override bool hasValue { get { return true; } }
        public CrosswordPositionAndOrientation startPositionAndOrientation;
    }
}
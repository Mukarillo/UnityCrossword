namespace crossword.engine
{
    public class CrosswordTileAnswerItem : CrosswordTileItem
    {
        public override bool hasValue { get { return true; } }
        public CrosswordTileQuestionItem questionElement;
    }
}
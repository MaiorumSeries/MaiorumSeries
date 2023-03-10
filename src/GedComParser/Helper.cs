using MaiorumSeries.GedComModel;

namespace MaiorumSeries.GedComParser
{
    public static class Helper
    {
        public static BaseRecord GetRecord (this Token token)
        {
            return new BaseRecord()
            {
                Tag = token.Tag,
                XrefId = token.XrefId,
                Value = token.Value

            };
        }
    }
}

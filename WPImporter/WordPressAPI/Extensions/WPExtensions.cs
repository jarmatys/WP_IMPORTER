using WPImporter.WordPressAPI.Models;

namespace WPImporter.WordPressAPI.Extensions
{
    public static class WPExtensions
    {
        public static void AddMetaData(this List<MetaData> metaDatas, MetaData metaData)
        {
            if (metaDatas != null && !string.IsNullOrEmpty(metaData.MetaValue))
            {
                metaDatas.Add(metaData);
            }
        }

        public static void AddMetaDatas(this List<MetaData> metaDatas, List<MetaData> metaDatasToAdd)
        {
            foreach (var metaData in metaDatasToAdd)
            {
                metaDatas.Add(metaData);
            }
        }
    }
}

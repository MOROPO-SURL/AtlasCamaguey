namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Extensions
{
    public static class StringExtensions
    {
        public static string AsExtensionToMDICssClass(this string target)
        {
            switch (target)
            {
                case string x when x.Contains("pdf"):
                    return "mdi-file-pdf";

                case string x when x.Contains("doc"):
                    return "mdi-file-word";

                case string x when x.Contains("xls") || x.Contains("sheet"):
                    return "mdi-file-excel";

                case string x when x.Contains("ppt"):
                    return "mdi-file-powerpoint";

                case string x when x.Contains("jpg") || x.Contains("png") || x.Contains("bmp") || x.Contains("gif"):
                    return "mdi-file-image";

                case string x when x.Contains("apk"):
                    return "mdi-android-head";

                case string x when x.Contains("zip"):
                    return "mdi-zip-box-outline";

                default:
                    return "mdi-file";
            }
        }
    }
}

namespace BinaryViewer 
{
    static class Extensions 
    {
        public static bool IsBlank(this string str) 
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsNotBlank(this string str) =>
            !str.IsBlank();
    }
}

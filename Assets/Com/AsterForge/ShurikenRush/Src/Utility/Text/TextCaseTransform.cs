namespace Com.AsterForge.ShurikenRush.Utility.Text
{
    public static class TextCaseTransform
    {
        public static string ToTitleCase(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            text = text.Trim();

            // Split by spaces and capitalize each part
            var words = text.ToLower().Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                    words[i] = char.ToUpper(words[i][0]) + words[i][1..];
            }

            return string.Join("", words);
        }
    }
}
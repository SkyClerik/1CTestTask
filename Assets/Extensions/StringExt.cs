public static class StringExt
{
    public static string ToPriceStyle(int value)
    {
        string result = value.ToString();

        for (int count = result.Length, i = 3, t = 3; count >= i; i += 3, t += 4)
        {
            result = result.Insert(result.Length - t, " ");
        }

        return result;
    }
}

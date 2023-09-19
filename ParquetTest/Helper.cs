namespace ParquetTest;

public static class Helper
{
    private static readonly char[] alphabet =
        Enumerable.Range((int)'A', 26).Concat(Enumerable.Range((int)'a', 26))
        .Select(c => (char)c)
        .ToArray();

    public static string GetRandomString(this Random random, int length = 12)
    {
        return new string(
            Enumerable.Range(0, length)
                .Select(_ => alphabet[random.Next(alphabet.Length)])
                .ToArray()
            );
    }

}

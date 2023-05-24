public static class Indenter
{
    private const string IndentationString = "    ";

    public static string GetIdent(int indentLevel)
    {
        string indentation = new string(' ', indentLevel * IndentationString.Length);
        return indentation;
    }
}
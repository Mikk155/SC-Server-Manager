public class CLog
{
    public CLog Write( string message, ConsoleColor? color = null )
    {
        if( color is not null )
            Console.ForegroundColor = (ConsoleColor)color;
        Console.Write( message );
        Console.ResetColor();
        return this;
    }

    public CLog WriteLine( string message, ConsoleColor? color = null )
    {
        return Write(message,color).WriteLine();
    }

    public CLog WriteLine()
    {
        Console.WriteLine();
        return this;
    }

    public CLog Beep()
    {
        Console.Beep();
        return this;
    }

    public CLog Pause()
    {
        WriteLine( "Press any key to continue.", ConsoleColor.Yellow );
        Console.ReadKey();
        return this;
    }
}

public class Context
{
    public readonly string SvenDirectory;

    public Context()
    {
        string dir = Path.GetFullPath( Directory.GetCurrentDirectory() );

        if( !Directory.Exists( Path.Combine( dir, "svencoop" ) ) )
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine( "Could not find the svencoop directory\nmake sure to execute this program in within the \"Sven Co-op\" folder." );
            Console.Beep();
            Console.ResetColor();
            Environment.Exit(1);
        }

        this.SvenDirectory = dir;
    }
}

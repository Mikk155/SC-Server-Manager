public class Context
{
    /// <summary>
    /// Program version, Maps has a version stored in worldspawn as well.
    /// Use this to not apply changes that has been already applied.
    /// </summary>
    public readonly SemanticVersion Version = new SemanticVersion( 1, 0, 0 );

    public MapContext Map = null!;

    public readonly string SvenDirectory;

    public Context()
    {
        string dir = Path.GetFullPath( Directory.GetCurrentDirectory() );

        if( !Directory.Exists( Path.Combine( dir, "svencoop" ) ) )
        {
            Program.Log.Write( "Error: ", ConsoleColor.Red )
                .Write( "Could not find the \"" )
                .Write( "svencoop", ConsoleColor.Green )
                .Write( "\" directory." )
                .WriteLine()
                .Write( "Make sure to execute this program in within the \"" )
                .Write( "Sven Co-op", ConsoleColor.Red )
                .Write( "\" folder." )
                .WriteLine()
                .Beep()
                .Pause();
            Environment.Exit(1);
        }

        this.SvenDirectory = dir;
    }

    public readonly string[] AssetsFolders = [
        "svencoop_addon",
        "svencoop",
        "svencoop_downloads"
    ];

    public bool AssetExists( string asset, out string? path )
    {
        if( asset.Contains( '/' ) )
        {
            string[] paths = asset.Split( '/' );
            asset = string.Empty;
            foreach( string p in paths )
            {
                asset = Path.Combine( asset, p );
            }
//            asset = asset.Replace( '/', Path.PathSeparator );
        }

        foreach( string folder in this.AssetsFolders )
        {
            string assetDirectory = Path.Combine( SvenDirectory, folder, asset );

            if( File.Exists( assetDirectory ) )
            {
                path = assetDirectory;
                return true;
            }
        }

        path = null;
        return false;
    }
}

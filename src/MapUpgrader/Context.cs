public class MapContext( string Name )
{
    public readonly string Name = Name;
    public List<Entity> Entities = new List<Entity>();
}

public class Context
{
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

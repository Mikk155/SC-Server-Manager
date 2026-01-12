public static class Program
{
    private static List<IUpgrade> Upgrades = new List<IUpgrade>();
    public static CLog Log = new CLog();
    private static Context context = null!;

    public static void Main( string[] args )
    {
        List<string> UpgradesCount = new List<string>();
        List<string> UpgradedMaps = new List<string>();

        context = new Context();

        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

        var upgradesList = assembly.GetTypes().Where( t =>
            typeof(IUpgrade).IsAssignableFrom(t)
            && !t.IsInterface
            && t.Namespace != null
            && t.Namespace == "Upgrades"
        );

        foreach( Type? upgradeClass in upgradesList )
        {
            if( Activator.CreateInstance( upgradeClass ) is IUpgrade upgrade )
            {
                string[] maps = upgrade.Maps();

                foreach( string map in maps )
                {
                    string? mapDirectory;

                    if( !Program.context.AssetExists( $"maps/{map}.bsp", out mapDirectory ) || string.IsNullOrWhiteSpace( mapDirectory ) )
                        continue;

                    Program.Log.Write( $"Reading map \"" ).Write( map, ConsoleColor.Green ).WriteLine( "\"" );

                    Sledge.Formats.Bsp.BspFile bsp = null!;

                    Program.context.Map = new MapContext( map );

                    string MapUpgraderVersionName = $"MapUpgrader_{upgradeClass.Name}";

                    using( FileStream stream = File.OpenRead( mapDirectory ))
                    {
                        bsp = new Sledge.Formats.Bsp.BspFile( stream );

                        // Get semantic version
                        Program.context.Map.Version = new SemanticVersion( bsp.Entities[0].Get( MapUpgraderVersionName, "0.0.0" ) );

                        if( !upgrade.ShouldUpgrade( context ) )
                            continue;

                        int i = 0;
                        foreach( Sledge.Formats.Bsp.Objects.Entity entity in bsp.Entities )
                        {
                            Program.context.Map.Entities.Add( new Entity( entity.KeyValues.ToDictionary( kvp => kvp.Key, kvp => kvp.Value ), i++ ) );
                        }

                        Program.Log.Write( $"Upgrading map \"" )
                            .Write( map, ConsoleColor.Green )
                            .Write( "\" with upgrade \"" )
                            .Write( upgradeClass.Name, ConsoleColor.Green )
                            .Write( "\" " )
                            .Write( Program.context.Map.Version.ToString(), ConsoleColor.Green )
                            .Write( " -> " )
                            .Write( Program.context.Version.ToString(), ConsoleColor.Green )
                        .WriteLine();

                        upgrade.Upgrade( context );

                        if( !UpgradesCount.Contains( upgradeClass.Name ) )
                            UpgradesCount.Add( upgradeClass.Name );

                        if( !UpgradedMaps.Contains( map ) )
                            UpgradedMaps.Add( map );

                        bsp.Entities.Clear();
                    }

                    Program.Log.Write( $"Writing map \"" ).Write( map, ConsoleColor.Green ).WriteLine( "\"" );

                    using( FileStream stream = File.Create( mapDirectory ))
                    {
                        // Set new version.
                        Program.context.Map.Entities[0].SetString( MapUpgraderVersionName, Program.context.Version.ToString() );

                        List<Sledge.Formats.Bsp.Objects.Entity> sledge_entities = Program.context.Map.Entities
                            .Where( e => e.IsValid() ).Select( e =>
                            {
                                Sledge.Formats.Bsp.Objects.Entity sledge_entity = new Sledge.Formats.Bsp.Objects.Entity();

                                foreach( KeyValuePair<string, string> kv in e.keyvalues )
                                {
                                    sledge_entity.KeyValues[ kv.Key ] = kv.Value;
                                }

                                return sledge_entity;
                            } ).ToList();

                        foreach( Sledge.Formats.Bsp.Objects.Entity entity in sledge_entities )
                        {
                            bsp.Entities.Add( entity );
                        }

                        bsp.WriteToStream( stream, bsp.Version );
                    }
                }
            }
        }

        Log.Write( "All done!" )
            .WriteLine()
            .Write( "Applied " )
            .Write( UpgradesCount.Count.ToString(), ConsoleColor.Green )
            .Write( " Upgrades to " )
            .Write( UpgradedMaps.Count.ToString(), ConsoleColor.Green )
            .Write( " maps." )
            .WriteLine()
            .Beep()
        .Pause();
    }
}

public static class Program
{
    private static List<IUpgrade> Upgrades = new List<IUpgrade>();
    private static Context context = new Context();

    public static void Main( string[] args )
    {
        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

        var types = assembly.GetTypes().Where( t =>
            typeof(IUpgrade).IsAssignableFrom(t)
            && !t.IsInterface
            && !t.IsAbstract
            && t.Namespace != null
            && t.Namespace == "maps"
        );

        foreach( Type? type in types )
        {
            if( Activator.CreateInstance(type) is IUpgrade upgrade )
            {
                string[] maps = upgrade.Maps();

                foreach( string map in maps )
                {
                    string? mapDirectory;

                    if( !Program.context.AssetExists( $"maps/{map}.bsp", out mapDirectory ) || string.IsNullOrWhiteSpace( mapDirectory ) )
                        continue;

                    Sledge.Formats.Bsp.BspFile bsp = null!;

                    Program.context.Map = new MapContext( map );

                    using( FileStream stream = File.OpenRead( mapDirectory ))
                    {
                        bsp = new Sledge.Formats.Bsp.BspFile( stream );

                        int i = 0;
                        foreach( Sledge.Formats.Bsp.Objects.Entity entity in bsp.Entities )
                        {
                            Program.context.Map.Entities.Add( new Entity( entity.KeyValues.ToDictionary( kvp => kvp.Key, kvp => kvp.Value ), i++ ) );
                        }

                        upgrade.Upgrade( context );

                        bsp.Entities.Clear();
                    }

                    using( FileStream stream = File.Create( mapDirectory ))
                    {
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

        Console.WriteLine( context.SvenDirectory );
    }
}

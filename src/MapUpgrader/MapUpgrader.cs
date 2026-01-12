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
                }
            }
        }

        Console.WriteLine( context.SvenDirectory );
    }
}

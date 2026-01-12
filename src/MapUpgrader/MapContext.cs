public class MapContext( string Name )
{
    public readonly string Name = Name;
    public List<Entity> Entities = new List<Entity>();
    public SemanticVersion Version = null!;
}

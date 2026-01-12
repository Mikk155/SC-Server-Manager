public class SemanticVersion
{
    public uint Major { get; }
    public uint Minor { get; }
    public uint Patch { get; }
    public bool Defined { get; }

    public SemanticVersion( string vec )
    {
        string[] veclist = vec.Split( "." );

        if( veclist.Length == 3 )
        {
            Major = uint.TryParse( veclist[0], out uint majorNumber ) ? majorNumber : 0;
            Minor = uint.TryParse( veclist[1], out uint MinorNumber ) ? MinorNumber : 0;
            Patch = uint.TryParse( veclist[2], out uint PatchNumber ) ? PatchNumber : 0;
        }
    }

    public SemanticVersion( uint major, uint minor, uint patch )
    {
        Major = major;
        Minor = minor;
        Patch = patch;
    }

    public override string ToString() => $"{Major}.{Minor}.{Patch}";

    public bool Zero => ( Major + Minor + Patch == 0 );

    public int CompareTo( SemanticVersion other )
    {
        int result = Major.CompareTo( other.Major );

        if( result != 0 )
            return result;

        result = Minor.CompareTo( other.Minor );

        if( result != 0 )
            return result;

        return Patch.CompareTo( other.Patch );
    }

    public override bool Equals( object? obj )
        => obj is SemanticVersion v && CompareTo(v) == 0;

    public override int GetHashCode()
        => HashCode.Combine( Major, Minor, Patch );

    public static bool operator ==( SemanticVersion left, SemanticVersion right )
        => left.Equals(right);

    public static bool operator !=( SemanticVersion left, SemanticVersion right )
        => !left.Equals(right);

    public static bool operator >( SemanticVersion left, SemanticVersion right )
        => left.CompareTo(right) > 0;

    public static bool operator <( SemanticVersion left, SemanticVersion right )
        => left.CompareTo(right) < 0;

    public static bool operator >=( SemanticVersion left, SemanticVersion right )
        => left.CompareTo(right) >= 0;

    public static bool operator <=( SemanticVersion left, SemanticVersion right )
        => left.CompareTo(right) <= 0;
}

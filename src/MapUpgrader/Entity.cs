public class Entity( Dictionary<string, string> keyvalues, int index )
{
    /// <summary>
    /// Index in the .ent file
    /// </summary>
    public readonly int index = index;

    public readonly Dictionary<string, string> keyvalues = keyvalues;

    /// <summary>
    /// Get a key's value in string form
    /// </summary>
    public string GetString( string key )
    {
        return this.keyvalues.TryGetValue( key, out string? value ) ? value : "";
    }

    /// <summary>
    /// Set a key's value in string form
    /// </summary>
    public void SetString( string key, string value )
    {
        this.keyvalues[ key ] = value;
    }

    /// <summary>
    /// Get a key's value in integer form
    /// </summary>
    public int GetInteger( string key )
    {
        return this.keyvalues.TryGetValue( key, out string? value ) ? int.TryParse( value.ToString(), out int ivalue ) ? ivalue : 0 : 0;
    }

    /// <summary>
    /// Set a key's value in integer form
    /// </summary>
    public void SetInteger( string key, int value )
    {
        this.keyvalues[ key ] = value.ToString();
    }

    /// <summary>
    /// Get a key's value in float form
    /// </summary>
    public float GetFloat( string key )
    {
        return this.keyvalues.TryGetValue( key, out string? value ) ? float.TryParse( value.ToString(), out float ivalue ) ? ivalue : 0.0f : 0.0f;
    }

    /// <summary>
    /// Set a key's value in float form
    /// </summary>
    public void SetFloat( string key, float value )
    {
        this.keyvalues[ key ] = value.ToString();
    }

    /// <summary>
    /// Get a key's value in bool form (0/1)
    /// </summary>
    public bool GetBool( string key )
    {
        return this.keyvalues.TryGetValue( key, out string? value ) ? int.TryParse( value.ToString(), out int ivalue ) ? ivalue != 0 : false : false;
    }

    /// <summary>
    /// Set a key's value in bool form (0/1)
    /// </summary>
    public void SetBool( string key, bool value )
    {
        this.keyvalues[ key ] = value.ToString();
    }

    /// <summary>
    /// Get a key's value in Vector form (0/1)
    /// </summary>
    public Vector GetVector( string key )
    {
        return this.keyvalues.TryGetValue( key, out string? value ) ? new Vector( value ) : Vector.g_VecZero;
    }

    /// <summary>
    /// Set a key's value in Vector form (0/1)
    /// </summary>
    public void SetVector( string key, Vector value )
    {
        this.keyvalues[ key ] = value.ToString();
    }

    /// <summary>
    /// Whatever a bit-type key-value has this flag value
    /// </summary>
    public bool HasFlag( string key, int flag )
    {
        if( this.keyvalues.TryGetValue( key, out string? value ) && int.TryParse( value.ToString(), out int flags ) )
        {
            return ( flags & flag ) != 0;
        }

        return false;
    }

    /// <summary>
    /// Clears a flag from a bit-type key-value
    /// </summary>
    public void ClearFlag( string key, int flag )
    {
        if( this.keyvalues.TryGetValue( key, out string? value ) && int.TryParse( value.ToString(), out int flags ) && ( flags & flag ) != 0 )
        {
            flags &= ~flag;
            keyvalues[ key ] = flags.ToString();
        }
    }

    /// <summary>
    /// Adds a flag on a bit-type key-value
    /// </summary>
    public void SetFlag( string key, int flag )
    {
        if( this.keyvalues.TryGetValue( key, out string? value ) && int.TryParse( value.ToString(), out int flags ) && ( flags & flag ) == 0 )
        {
            flags |= flag;
            keyvalues[ key ] = flags.ToString();
        }
    }

    /// <summary>
    /// Remove the given key and value
    /// </summary>
    public void RemoveKeyValue( string key )
    {
        this.keyvalues.Remove( key );
    }

    /// <summary>
    /// Remove the entity from the entity data
    /// </summary>
    public void RemoveEntity()
    {
        keyvalues.Clear();
    }

    /// <summary>
    /// Returns whatever this is a valid entity with keyvalues and a valid classname
    /// </summary>
    public bool IsValid()
    {
        return keyvalues.Count > 0 && this.keyvalues.TryGetValue( "classname", out string? value ) && !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Return the entity in the .ent format
    /// </summary>
    public override string ToString()
    {
        System.Text.StringBuilder s = new System.Text.StringBuilder();

        s.AppendLine( "{" );

        foreach( KeyValuePair<string, string> keyvalue in this.keyvalues )
        {
            s.AppendLine( $"\"{keyvalue.Key}\" \"{keyvalue.Value}\"" );
        }

        s.AppendLine( "}" );

        return s.ToString();
    }
}

public interface IUpgrade
{
    /// <summary>
    /// List of maps to apply this upgrade on
    /// </summary>
    public string[] Maps();

    /// <summary>
    /// Called when a bsp is to be upgraded
    /// </summary>
    public void Upgrade( Context context );

    /// <summary>
    /// Return true if this map should be upgraded.
    /// </summary>
    public bool ShouldUpgrade( Context context );
}

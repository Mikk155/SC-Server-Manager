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
}

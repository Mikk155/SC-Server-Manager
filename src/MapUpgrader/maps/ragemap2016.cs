namespace Upgrades
{
    class ragemap2016 : IUpgrade
    {
        public string[] Maps()
        {
            return [ "ragemap2016" ];
        }

        public bool ShouldUpgrade( Context context )
        {
            return context.Map.Version.Zero;
        }

        public void Upgrade( Context context )
        {
            Entity pushEntity = context.Map.Entities.FirstOrDefault( e => e.GetString( "model" ) == "*185" )!;
            pushEntity.SetInteger( "tiny_monsters", 0 );
            pushEntity.ClearFlag( "spawnflags", 64 );
        }
    }
}

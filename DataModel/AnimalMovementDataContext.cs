namespace DataModel
{
    public partial class AnimalMovementDataContext
    {
        partial void OnCreated()
        {
            CommandTimeout = Properties.Settings.Default.CommandTimeout;
        }
    }
}

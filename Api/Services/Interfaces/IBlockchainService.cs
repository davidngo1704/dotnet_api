namespace Api.Services.Interfaces
{
    public interface IBlockchainService
    {
        Task TriggerMinute();
        Task TriggerTenMinute();
        Task TriggerHour();
    }
}

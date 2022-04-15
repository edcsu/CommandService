namespace CommandService.Business.Repositories.Interfaces
{
    public interface IBaseRepository
    {
        Task<bool> SaveChangesAsync();
    }
}

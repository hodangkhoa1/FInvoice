namespace DAL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(T entity, string action);
        Task<List<T>> GetAll(T entity, string action);
        Task<bool> ActionEdit(T entity, string action);
        int Count(T? entity, string? action);
    }
}

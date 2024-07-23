namespace HealthTracker.Repositories.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        public Task<T> Add(T entity);
        public Task<T> GetById(K key);
        public Task<List<T>> GetAll();
        public Task<T> Update(T entity);
        public Task<T> Delete(K key);
    }
}

namespace BarberBoss.Domain.Repositories;

public interface IWriteOnlyRepository<T>
{
    void Add(T entity);
    // <summary>
    /// This function returns TRUE if deletion was successfully otherwise returns FALSE
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> Delete(Guid id);
}

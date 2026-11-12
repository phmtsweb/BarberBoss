using BarberBoss.Domain.Repositories;

namespace BarberBoss.Infrastructure.DataAccess;

internal class UnitOfWork(BarberBossDbContext dbContext) : IUnitOfWork
{
    private readonly BarberBossDbContext _dbContext = dbContext;
    public async Task Commit() => await _dbContext.SaveChangesAsync();
}

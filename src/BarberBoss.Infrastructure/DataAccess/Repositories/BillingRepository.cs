using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Records;
using BarberBoss.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

public class BillingRepository(BarberBossDbContext dbContext) : IReadOnlyRepository<Billing>, IUpdateOnlyRepository<Billing>, IWriteOnlyRepository<Billing>
{
    private readonly BarberBossDbContext _dbContext = dbContext;
    public void Add(Billing billing)
    {
        _dbContext.Billings.Add(billing);
    }

    public async Task<bool> Delete(Guid id)
    {
        var billing = await _dbContext.Billings.FirstOrDefaultAsync(b => b.Id == id);
        if (billing is null)
        {
            return false;
        }
        _dbContext.Billings.Remove(billing);
        return true;
    }

    public async Task<(List<Billing> Items, int TotalCount)> GetAll(PaginationSettings settings)
    {
        var query = _dbContext.Billings.AsNoTracking();
        var total = await query.CountAsync();
        if (total == 0)
            return ([], 0);
        query = ApplyOrderBy(query, settings.OrderBy);
        query = ApplyPagination(query, settings);
        var billings = await query
            .ToListAsync();
        return (billings, total);
    }

    public async Task<Billing?> GetById(Guid id)
    {
        return await _dbContext.Billings
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Billing?> GetForUpdateById(Guid id)
    {
        return await _dbContext.Billings
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public void Update(Billing billing)
    {
        _dbContext.Billings.Update(billing);
    }

    public async Task<List<Billing>> GetAllByPeriod(ReportSettings settings)
    {
        return await _dbContext.Billings
            .AsNoTracking()
            .Where(b => b.Date >= settings.Start && b.Date <= settings.End)
            .ToListAsync();
    }

    private IQueryable<Billing> ApplyPagination(IQueryable<Billing> query, PaginationSettings settings)
    {
        int skip = (settings.PageNumber - 1) * settings.PageSize;
        return query.Skip(skip).Take(settings.PageSize);
    }

    private IQueryable<Billing> ApplyOrderBy(IQueryable<Billing> query, string? orderBy)
    {
        if (string.IsNullOrEmpty(orderBy))
        {
            return query;
        }
        return orderBy.ToLower() switch
        {
            "id" => query.OrderBy(b => b.Id),
            "id_desc" => query.OrderByDescending(b => b.Id),
            "date" => query.OrderBy(b => b.Date),
            "date_desc" => query.OrderByDescending(b => b.Date),
            "barber_name" => query.OrderBy(b => b.BarberName),
            "barber_name_desc" => query.OrderByDescending(b => b.BarberName),
            "client_name" => query.OrderBy(b => b.ClientName),
            "client_name_desc" => query.OrderByDescending(b => b.ClientName),
            "service_name" => query.OrderBy(b => b.ServiceName),
            "service_name_desc" => query.OrderByDescending(b => b.ServiceName),
            "amount" => query.OrderBy(b => b.Amount),
            "amount_desc" => query.OrderByDescending(b => b.Amount),
            "payment_method" => query.OrderBy(b => b.PaymentMethod),
            "payment_method_desc" => query.OrderByDescending(b => b.PaymentMethod),
            "status" => query.OrderBy(b => b.Status),
            "status_desc" => query.OrderByDescending(b => b.Status),
            "notes" => query.OrderBy(b => b.Notes),
            "notes_desc" => query.OrderByDescending(b => b.Notes),
            "created_at" => query.OrderBy(b => b.CreatedAt),
            "created_at_desc" => query.OrderByDescending(b => b.CreatedAt),
            "updated_at" => query.OrderBy(b => b.UpdatedAt),
            "updated_at_desc" => query.OrderByDescending(b => b.UpdatedAt),
            _ => query
        };
    }

}

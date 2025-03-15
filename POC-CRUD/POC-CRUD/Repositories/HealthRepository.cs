using POC_CRUD.Data;
using POC_CRUD.Models;

namespace POC_CRUD.Repositories;

public class HealthRepository : IRepository
{
    private readonly AppDbContext _context;

    public HealthRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<HealthStatus> GetAll() => _context.HealthStatuses.ToList();
}
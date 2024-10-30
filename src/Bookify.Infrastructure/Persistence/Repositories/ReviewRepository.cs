using Bookify.Domain.Reviews;

namespace Bookify.Infrastructure.Persistence.Repositories;

internal sealed class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
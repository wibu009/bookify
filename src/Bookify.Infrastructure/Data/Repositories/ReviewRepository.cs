using Bookify.Domain.Reviews;

namespace Bookify.Infrastructure.Data.Repositories;

internal sealed class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Domain.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace MockInterview.Interviews.DataAccess;

public class InterviewsDbContext : DbContext
{
    public const string Schema = "interviews";

    public InterviewsDbContext(DbContextOptions<InterviewsDbContext> options) : base(options)
    {
    }

    public DbSet<Interview> Interviews { get; set; }

    public DbSet<InterviewMember> InterviewMembers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<InterviewQuestionsList> InterviewQuestionsLists { get; set; }
    public DbSet<InterviewQuestion> InterviewQuestions { get; set; }
    public DbSet<InterviewOrder> InterviewOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InterviewsDbContext).Assembly);
    }
}
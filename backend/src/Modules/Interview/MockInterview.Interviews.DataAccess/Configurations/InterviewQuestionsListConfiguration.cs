using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.DataAccess.Configurations;

public class InterviewQuestionsListConfiguration : IEntityTypeConfiguration<InterviewQuestionsList>
{
    public void Configure(EntityTypeBuilder<InterviewQuestionsList> builder)
    {
        builder
            .HasMany(l => l.Questions)
            .WithOne(q => q.InterviewQuestionsList)
            .HasForeignKey(q => q.InterviewQuestionsListId);

        builder
            .HasOne(l => l.InterviewOrder)
            .WithOne(i => i.QuestionsList)
            .HasForeignKey<InterviewQuestionsList>(l => l.InterviewOrderId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
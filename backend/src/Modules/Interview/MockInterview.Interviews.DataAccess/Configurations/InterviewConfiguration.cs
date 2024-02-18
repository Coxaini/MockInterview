using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.DataAccess.Configurations;

public class InterviewConfiguration : IEntityTypeConfiguration<Interview>
{
    public void Configure(EntityTypeBuilder<Interview> builder)
    {
        builder.Property(i => i.Tags);

        builder
            .HasMany(i => i.QuestionsLists)
            .WithOne(l => l.Interview)
            .HasForeignKey(ql => ql.InterviewId)
            .OnDelete(DeleteBehavior.SetNull);

        /*builder.Ignore(i => i.FirstMemberQuestions);
        builder.Ignore(i => i.SecondMemberQuestions);*/
    }
}
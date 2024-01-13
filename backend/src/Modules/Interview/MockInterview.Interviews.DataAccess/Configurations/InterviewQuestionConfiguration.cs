using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.DataAccess.Configurations;

public class InterviewQuestionConfiguration : IEntityTypeConfiguration<InterviewQuestion>
{
    public void Configure(EntityTypeBuilder<InterviewQuestion> builder)
    {
        builder
            .HasOne(iq => iq.Author)
            .WithMany()
            .HasForeignKey(iq => iq.AuthorId);
    }
}
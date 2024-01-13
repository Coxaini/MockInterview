using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.DataAccess.Configurations;

public class InterviewConfiguration : IEntityTypeConfiguration<Interview>
{
    public void Configure(EntityTypeBuilder<Interview> builder)
    {
        builder
            .HasOne(i => i.FirstMember)
            .WithMany()
            .HasForeignKey(i => i.FirstMemberId);

        builder
            .HasOne(i => i.SecondMember)
            .WithMany()
            .HasForeignKey(i => i.SecondMemberId);

        builder
            .HasMany(i => i.Questions)
            .WithOne()
            .HasForeignKey(iq => iq.InterviewId);

        builder.Property(i => i.Tags);

        builder.Ignore(i => i.FirstMemberQuestions);
        builder.Ignore(i => i.SecondMemberQuestions);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.DataAccess.Configurations;

public class InterviewMemberConfiguration : IEntityTypeConfiguration<InterviewMember>
{
    public void Configure(EntityTypeBuilder<InterviewMember> builder)
    {
        builder.HasKey(m => new { m.UserId, m.InterviewId });

        builder
            .HasOne(m => m.Interview)
            .WithMany(i => i.Members)
            .HasForeignKey(m => m.InterviewId);

        builder
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId);

        builder
            .HasOne(m => m.InterviewOrder)
            .WithMany()
            .HasForeignKey(m => m.InterviewOrderId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.DataAccess.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder
            .HasOne(q => q.Author)
            .WithMany()
            .HasForeignKey(q => q.AuthorId);
    }
}
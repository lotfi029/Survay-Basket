namespace Survay_Basket.API.Presistence.EntitiesConfigurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasIndex(p => new { p.PollId, p.Content });

        builder.Property(e => e.Content).HasMaxLength(1000);

        
    }
}
  
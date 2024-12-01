namespace Survay_Basket.API.Presistence.EntitiesConfigurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasIndex(e => new {e.QuestionId, e.Content});

        builder.Property(e => e.Content).HasMaxLength(1000);

    }
}
  
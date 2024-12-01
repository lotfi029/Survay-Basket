﻿namespace Survay_Basket.API.Presistence.EntitiesConfigurations;

public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasIndex(v => new { v.PollId, v.UserId });
    }
}
  
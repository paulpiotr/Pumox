#region using

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pumox.Core.Models;

#endregion

namespace Pumox.Core.Database.Data.EntityTypeConfiguration
{
    internal class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            //builder.Property(p => p.Id).ValueGeneratedNever();

            builder.HasIndex(i => i.Name)
                .HasDatabaseName(
                    string.Format("{0}{1}{2}{3}{4}", "IX", "_", nameof(Company), "_", nameof(Company.Name)))
                .IsUnique(false);

            builder.HasIndex(i => i.EstablishmentYear)
                .HasDatabaseName(string.Format("{0}{1}{2}{3}{4}", "IX", "_", nameof(Company), "_",
                    nameof(Company.EstablishmentYear)))
                .IsUnique(false);
        }
    }
}

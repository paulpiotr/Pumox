#region using

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pumox.Core.Models;

#endregion

namespace Pumox.Core.Database.Data.EntityTypeConfiguration
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            //builder.Property(p => p.Id).ValueGeneratedNever();

            builder.HasIndex(i => i.FirstName)
                .HasDatabaseName(string.Format("{0}{1}{2}{3}{4}", "IX", "_", nameof(Employee), "_",
                    nameof(Employee.FirstName)))
                .IsUnique(false);

            builder.HasIndex(i => i.LastName)
                .HasDatabaseName(string.Format("{0}{1}{2}{3}{4}", "IX", "_", nameof(Employee), "_",
                    nameof(Employee.LastName)))
                .IsUnique(false);

            builder.HasIndex(i => i.JobTitle)
                .HasDatabaseName(string.Format("{0}{1}{2}{3}{4}", "IX", "_", nameof(Employee), "_",
                    nameof(Employee.JobTitle)))
                .IsUnique(false);

            builder.HasIndex(i => i.DateOfBirth)
                .HasDatabaseName(string.Format("{0}{1}{2}{3}{4}", "IX", "_", nameof(Employee), "_",
                    nameof(Employee.DateOfBirth)))
                .IsUnique(false);
        }
    }
}

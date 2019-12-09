using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class ReklamlarMap : EntityTypeConfiguration<Reklamlar>
    {
        public ReklamlarMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.reklamPath)
                .HasMaxLength(150);

            this.Property(t => t.reklamLink)
                .HasMaxLength(250);

            this.Property(t => t.reklamText)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("Reklamlar");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.reklamPath).HasColumnName("reklamPath");
            this.Property(t => t.reklamLink).HasColumnName("reklamLink");
            this.Property(t => t.reklamText).HasColumnName("reklamText");
        }
    }
}

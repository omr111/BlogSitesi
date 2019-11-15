using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class KullaniciBegeniMap : EntityTypeConfiguration<KullaniciBegeni>
    {
        public KullaniciBegeniMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            // Table & Column Mappings
            this.ToTable("KullaniciBegeni");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.KullaniciID).HasColumnName("KullaniciID");
            this.Property(t => t.MakaleID).HasColumnName("MakaleID");

            // Relationships
            this.HasRequired(t => t.Kullanici)
                .WithMany(t => t.KullaniciBegenis)
                .HasForeignKey(d => d.KullaniciID);
            this.HasRequired(t => t.Makale)
                .WithMany(t => t.KullaniciBegenis)
                .HasForeignKey(d => d.MakaleID);

        }
    }
}

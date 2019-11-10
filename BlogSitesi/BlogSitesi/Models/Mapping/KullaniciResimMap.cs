using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class KullaniciResimMap : EntityTypeConfiguration<KullaniciResim>
    {
        public KullaniciResimMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.resimPath)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("KullaniciResim");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.resimPath).HasColumnName("resimPath");
            this.Property(t => t.kullaniciId).HasColumnName("kullaniciId");

            // Relationships
            this.HasRequired(t => t.Kullanici)
                .WithMany(t => t.KullaniciResims)
                .HasForeignKey(d => d.kullaniciId);

        }
    }
}

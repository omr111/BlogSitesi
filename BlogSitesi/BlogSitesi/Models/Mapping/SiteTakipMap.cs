using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class SiteTakipMap : EntityTypeConfiguration<SiteTakip>
    {
        public SiteTakipMap()
        {
            // Primary Key
            this.HasKey(t => t.Mail);

            // Properties
            this.Property(t => t.Mail)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SiteTakip");
            this.Property(t => t.Mail).HasColumnName("Mail");
            this.Property(t => t.YazarID).HasColumnName("YazarID");
            this.Property(t => t.KategoriID).HasColumnName("KategoriID");

            // Relationships
            this.HasOptional(t => t.Kategori)
                .WithMany(t => t.SiteTakips)
                .HasForeignKey(d => d.KategoriID);
            this.HasOptional(t => t.Kullanici)
                .WithMany(t => t.SiteTakips)
                .HasForeignKey(d => d.YazarID);

        }
    }
}

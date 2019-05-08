using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class ResimMap : EntityTypeConfiguration<Resim>
    {
        public ResimMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.Adi)
                .HasMaxLength(50);

            this.Property(t => t.KucukResimYol)
                .HasMaxLength(1000);

            this.Property(t => t.OrtaResimYol)
                .HasMaxLength(1000);

            this.Property(t => t.BuyukResimYol)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("Resim");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.Adi).HasColumnName("Adi");
            this.Property(t => t.KucukResimYol).HasColumnName("KucukResimYol");
            this.Property(t => t.OrtaResimYol).HasColumnName("OrtaResimYol");
            this.Property(t => t.BuyukResimYol).HasColumnName("BuyukResimYol");
            this.Property(t => t.EkleyenID).HasColumnName("EkleyenID");
            this.Property(t => t.EklemeTarihi).HasColumnName("EklemeTarihi");
            this.Property(t => t.Goruntulenme).HasColumnName("Goruntulenme");
            this.Property(t => t.Begeni).HasColumnName("Begeni");

            // Relationships
            this.HasOptional(t => t.Kullanici)
                .WithMany(t => t.Resims)
                .HasForeignKey(d => d.EkleyenID);

        }
    }
}

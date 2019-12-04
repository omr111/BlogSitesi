using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class MakaleMap : EntityTypeConfiguration<Makale>
    {
        public MakaleMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.Baslik)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.icerik)
                .IsRequired();

            this.Property(t => t.BuyukResimYol)
                .HasMaxLength(150);

            this.Property(t => t.kucukResimYol)
                .HasMaxLength(150);

            this.Property(t => t.resimAlt)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Makale");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.Baslik).HasColumnName("Baslik");
            this.Property(t => t.icerik).HasColumnName("icerik");
            this.Property(t => t.YayinTarihi).HasColumnName("YayinTarihi");
            this.Property(t => t.KategoriID).HasColumnName("KategoriID");
            this.Property(t => t.MakaleTipID).HasColumnName("MakaleTipID");
            this.Property(t => t.YazarID).HasColumnName("YazarID");
            this.Property(t => t.Goruntulenme).HasColumnName("Goruntulenme");
            this.Property(t => t.Aktif).HasColumnName("Aktif");
            this.Property(t => t.BuyukResimYol).HasColumnName("BuyukResimYol");
            this.Property(t => t.kucukResimYol).HasColumnName("kucukResimYol");
            this.Property(t => t.resimAlt).HasColumnName("resimAlt");

            // Relationships
            this.HasRequired(t => t.Kategori)
                .WithMany(t => t.Makales)
                .HasForeignKey(d => d.KategoriID);
            this.HasRequired(t => t.Kullanici)
                .WithMany(t => t.Makales)
                .HasForeignKey(d => d.YazarID);
            this.HasOptional(t => t.MakaleTip)
                .WithMany(t => t.Makales)
                .HasForeignKey(d => d.MakaleTipID);

        }
    }
}

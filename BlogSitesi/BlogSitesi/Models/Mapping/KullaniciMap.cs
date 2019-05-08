using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class KullaniciMap : EntityTypeConfiguration<Kullanici>
    {
        public KullaniciMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.Adi)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Soyadi)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Hakkinda)
                .HasMaxLength(1000);

            this.Property(t => t.Mail)
                .IsRequired()
                .HasMaxLength(70);

            this.Property(t => t.Nick)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.KullaniciAdi)
                .HasMaxLength(50);

            this.Property(t => t.parola)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Kullanici");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.Adi).HasColumnName("Adi");
            this.Property(t => t.Soyadi).HasColumnName("Soyadi");
            this.Property(t => t.Hakkinda).HasColumnName("Hakkinda");
            this.Property(t => t.Mail).HasColumnName("Mail");
            this.Property(t => t.KayitTarihi).HasColumnName("KayitTarihi");
            this.Property(t => t.Nick).HasColumnName("Nick");
            this.Property(t => t.YazarMi).HasColumnName("YazarMi");
            this.Property(t => t.ResimID).HasColumnName("ResimID");
            this.Property(t => t.Aktif).HasColumnName("Aktif");
            this.Property(t => t.KullaniciAdi).HasColumnName("KullaniciAdi");
            this.Property(t => t.parola).HasColumnName("parola");

            // Relationships
            this.HasMany(t => t.Makales1)
                .WithMany(t => t.Kullanicis)
                .Map(m =>
                    {
                        m.ToTable("KullaniciBegeni");
                        m.MapLeftKey("KullaniciID");
                        m.MapRightKey("MakaleID");
                    });

            this.HasRequired(t => t.aspnet_Users)
                .WithOptional(t => t.Kullanici);
            this.HasOptional(t => t.Resim)
                .WithMany(t => t.Kullanicis)
                .HasForeignKey(d => d.ResimID);

        }
    }
}

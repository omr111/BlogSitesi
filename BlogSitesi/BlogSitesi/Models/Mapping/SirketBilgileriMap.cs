using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class SirketBilgileriMap : EntityTypeConfiguration<SirketBilgileri>
    {
        public SirketBilgileriMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.siteAdi)
                .HasMaxLength(50);

            this.Property(t => t.telefon)
                .HasMaxLength(11);

            this.Property(t => t.email)
                .HasMaxLength(50);

            this.Property(t => t.emailPassword)
                .HasMaxLength(50);

            this.Property(t => t.logoPath)
                .HasMaxLength(100);

            this.Property(t => t.pinperestUrl)
                .HasMaxLength(50);

            this.Property(t => t.twitterUrl)
                .HasMaxLength(50);

            this.Property(t => t.googleUrl)
                .HasMaxLength(50);

            this.Property(t => t.facebookUrl)
                .HasMaxLength(50);

            this.Property(t => t.hakkimizda)
                .HasMaxLength(150);

            this.Property(t => t.adres)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("SirketBilgileri");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.siteAdi).HasColumnName("siteAdi");
            this.Property(t => t.telefon).HasColumnName("telefon");
            this.Property(t => t.email).HasColumnName("email");
            this.Property(t => t.emailPassword).HasColumnName("emailPassword");
            this.Property(t => t.logoPath).HasColumnName("logoPath");
            this.Property(t => t.pinperestUrl).HasColumnName("pinperestUrl");
            this.Property(t => t.twitterUrl).HasColumnName("twitterUrl");
            this.Property(t => t.googleUrl).HasColumnName("googleUrl");
            this.Property(t => t.facebookUrl).HasColumnName("facebookUrl");
            this.Property(t => t.hakkimizda).HasColumnName("hakkimizda");
            this.Property(t => t.adres).HasColumnName("adres");
        }
    }
}

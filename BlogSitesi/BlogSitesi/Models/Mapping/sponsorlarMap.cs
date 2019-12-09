using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class sponsorlarMap : EntityTypeConfiguration<sponsorlar>
    {
        public sponsorlarMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.sponsorName)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.sponsorPath)
                .HasMaxLength(150);

            this.Property(t => t.sponsorLink)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.soponsorAciklama)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("sponsorlar");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.sponsorName).HasColumnName("sponsorName");
            this.Property(t => t.sponsorPath).HasColumnName("sponsorPath");
            this.Property(t => t.sponsorLink).HasColumnName("sponsorLink");
            this.Property(t => t.soponsorAciklama).HasColumnName("soponsorAciklama");
        }
    }
}

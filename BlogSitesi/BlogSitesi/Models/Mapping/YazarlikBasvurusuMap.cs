using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class YazarlikBasvurusuMap : EntityTypeConfiguration<YazarlikBasvurusu>
    {
        public YazarlikBasvurusuMap()
        {
            // Primary Key
            this.HasKey(t => t.BasvuruID);

            // Properties
            this.Property(t => t.Hakkinda)
                .IsRequired();

            this.Property(t => t.Nick)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Ad)
                .HasMaxLength(50);

            this.Property(t => t.Soyad)
                .HasMaxLength(50);

            this.Property(t => t.mail)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("YazarlikBasvurusu");
            this.Property(t => t.BasvuruID).HasColumnName("BasvuruID");
            this.Property(t => t.kID).HasColumnName("kID");
            this.Property(t => t.Hakkinda).HasColumnName("Hakkinda");
            this.Property(t => t.Nick).HasColumnName("Nick");
            this.Property(t => t.Ad).HasColumnName("Ad");
            this.Property(t => t.Soyad).HasColumnName("Soyad");
            this.Property(t => t.mail).HasColumnName("mail");

            // Relationships
            this.HasRequired(t => t.Kullanici)
                .WithMany(t => t.YazarlikBasvurusus)
                .HasForeignKey(d => d.kID);

        }
    }
}

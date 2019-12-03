using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class YorumMap : EntityTypeConfiguration<Yorum>
    {
        public YorumMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.Baslik)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.icerik)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Yorum");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.YorumYapanID).HasColumnName("YorumYapanID");
            this.Property(t => t.Baslik).HasColumnName("Baslik");
            this.Property(t => t.icerik).HasColumnName("icerik");
            this.Property(t => t.MakaleID).HasColumnName("MakaleID");
            this.Property(t => t.EklemeTarihi).HasColumnName("EklemeTarihi");
            this.Property(t => t.Aktif).HasColumnName("Aktif");

            // Relationships
            this.HasRequired(t => t.Kullanici)
                .WithMany(t => t.Yorums)
                .HasForeignKey(d => d.YorumYapanID);
            this.HasRequired(t => t.Makale)
                .WithMany(t => t.Yorums)
                .HasForeignKey(d => d.MakaleID);

        }
    }
}

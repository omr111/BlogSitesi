using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class MesajlarMap : EntityTypeConfiguration<Mesajlar>
    {
        public MesajlarMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Gonderen, t.Alan, t.Mesaj });

            // Properties
            this.Property(t => t.Mesaj)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Mesajlar");
            this.Property(t => t.Gonderen).HasColumnName("Gonderen");
            this.Property(t => t.Alan).HasColumnName("Alan");
            this.Property(t => t.Mesaj).HasColumnName("Mesaj");
            this.Property(t => t.Tarih).HasColumnName("Tarih");
            this.Property(t => t.Goruldu).HasColumnName("Goruldu");
        }
    }
}

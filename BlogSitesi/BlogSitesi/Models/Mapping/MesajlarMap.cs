using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class MesajlarMap : EntityTypeConfiguration<Mesajlar>
    {
        public MesajlarMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.Mesaj)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Mesajlar");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.GonderenId).HasColumnName("GonderenId");
            this.Property(t => t.AlanId).HasColumnName("AlanId");
            this.Property(t => t.Mesaj).HasColumnName("Mesaj");
            this.Property(t => t.Tarih).HasColumnName("Tarih");
            this.Property(t => t.Goruldu).HasColumnName("Goruldu");

            // Relationships
            this.HasRequired(t => t.Kullanici)
                .WithMany(t => t.Mesajlars)
                .HasForeignKey(d => d.GonderenId);
            this.HasRequired(t => t.Kullanici1)
                .WithMany(t => t.Mesajlars1)
                .HasForeignKey(d => d.AlanId);

        }
    }
}

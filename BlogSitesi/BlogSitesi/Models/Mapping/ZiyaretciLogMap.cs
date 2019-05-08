using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class ZiyaretciLogMap : EntityTypeConfiguration<ZiyaretciLog>
    {
        public ZiyaretciLogMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ipAdres, t.Tarih });

            // Properties
            this.Property(t => t.ipAdres)
                .IsRequired()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("ZiyaretciLog");
            this.Property(t => t.ipAdres).HasColumnName("ipAdres");
            this.Property(t => t.Tarih).HasColumnName("Tarih");
        }
    }
}

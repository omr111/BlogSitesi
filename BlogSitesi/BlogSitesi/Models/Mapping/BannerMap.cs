using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class BannerMap : EntityTypeConfiguration<Banner>
    {
        public BannerMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.bannerPicPath)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.buttonName)
                .HasMaxLength(50);

            this.Property(t => t.textArea)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Banners");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.bannerPicPath).HasColumnName("bannerPicPath");
            this.Property(t => t.buttonName).HasColumnName("buttonName");
            this.Property(t => t.textArea).HasColumnName("textArea");
        }
    }
}

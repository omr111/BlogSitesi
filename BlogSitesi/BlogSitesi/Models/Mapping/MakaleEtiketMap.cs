using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace BlogSitesi.Models.Mapping
{
    public class MakaleEtiketMap : EntityTypeConfiguration<MakaleEtiket>
    {
        public MakaleEtiketMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            // Table & Column Mappings
            this.ToTable("MakaleEtiket");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.MakaleID).HasColumnName("MakaleID");
            this.Property(t => t.EtiketID).HasColumnName("EtiketID");

            // Relationships
            this.HasRequired(t => t.Etiket)
                .WithMany(t => t.MakaleEtikets)
                .HasForeignKey(d => d.EtiketID);
            this.HasRequired(t => t.Makale)
                .WithMany(t => t.MakaleEtikets)
                .HasForeignKey(d => d.MakaleID);

        }
    }
}

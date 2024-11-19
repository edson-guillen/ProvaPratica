using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProvaPratica.Domain.Models;

namespace ProvaPratica.Repository.Mappings
{
    public class BaseMapping<TModel> : IEntityTypeConfiguration<TModel> where TModel : BaseModel
    {
        public virtual void Configure(EntityTypeBuilder<TModel> builder)
        {
            builder.ToTable(typeof(TModel).Name.ToLower().Replace("model", string.Empty));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .UseIdentityColumn();

            builder.Property(x => x.CriadoEm)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(x => x.AtualizadoEm)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}

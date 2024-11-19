using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProvaPratica.Domain.Models;

namespace ProvaPratica.Repository.Mappings
{
    public class UsuarioMapping : BaseMapping<UsuarioModel>
    {
        public override void Configure(EntityTypeBuilder<UsuarioModel> builder)
        {
            builder.Property(x => x.Nome)
                .HasColumnName("nome")
                .HasMaxLength(255);

            builder.Property(x => x.Idade)
                .HasColumnName("nome");

            builder.HasMany(x => x.Posts)
                .WithOne(x => x.Usuario)
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}

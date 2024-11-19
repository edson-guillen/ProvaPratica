using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProvaPratica.Domain.Models;

namespace ProvaPratica.Repository.Mappings
{
    public class PostMapping : BaseMapping<PostModel>
    {
        public override void Configure(EntityTypeBuilder<PostModel> builder)
        {
            builder.Property(x => x.Conteudo)
                .HasColumnName("conteudo")
                .HasMaxLength(255);

            builder.Property(x => x.IdUsuario)
                .HasColumnName("idusuario")
                .IsRequired();


            builder.HasOne(x => x.Usuario)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}

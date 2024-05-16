using MetalCoin.Domain;
using MetalCoin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetalCoin.Infra.Data.Mappings
{
    public class CupomConfiguration : IEntityTypeConfiguration<Cupom>
    {
        public void Configure(EntityTypeBuilder<Cupom> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Codigo)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(c => c.Descricao)
                .HasColumnType("varchar(100)");

            builder.Property(c => c.ValorDesconto)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(c => c.TipoDesconto)
                .IsRequired()
                .HasColumnType("int"); 

            builder.Property(c => c.DataValidade)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(c => c.QuantidadeLiberada)
                .IsRequired();

            builder.Property(c => c.QuantidadeUsada)
                .IsRequired();

            builder.Property(c => c.Status)
                .IsRequired();
                 
        }
    }
}

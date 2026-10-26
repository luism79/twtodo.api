using backend.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.data.configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder
                .ToTable("usuarios")
                .HasKey(p => p.Id)
                .HasName("pk_usuarios");

            builder
                .Property(f => f.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("uuidv7()")
                .ValueGeneratedOnAdd();

            builder
                .Property(f => f.Email)
                .HasColumnName("e_mail")
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder
                .Property(f => f.Password)
                .HasColumnName("senha")
                .HasColumnType("varchar(64)");

            builder
                .Property(f => f.Active)
                .HasColumnName("ativo")
                .HasColumnType("boolean")
                .IsRequired()
                .HasDefaultValue(true);

            builder
                .Property(f => f.CreatedAt)
                .HasColumnName("data_hora_incluido")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            builder
                .Property(f => f.UpdatedAt)
                .HasColumnName("data_hora_alterado")
                .HasColumnType("timestamp");

            builder
                .Property(f => f.DeactivatedAt)
                .HasColumnName("data_hora_desativado")
                .HasColumnType("timestamp");
        }
    }
}

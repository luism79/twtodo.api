using backend.enums;
using backend.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.data.configurations
{
    public class TaskConfigurarion : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> builder)
        {
            builder
                .ToTable("tarefas", t =>
                {
                    t.HasCheckConstraint("tarefas_status_check", "status between 0 and 3");
                    t.HasCheckConstraint("tarefas_prioridade_check", "prioridade between 0 and 2");
                })
                .HasKey(p => p.Id)
                .HasName("pk_tarefas");

            builder
                .Property(f => f.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("uuidv7()")
                .ValueGeneratedOnAdd();

            builder
                .Property(f => f.UserId)
                .HasColumnName("id_usuario")
                .IsRequired();

            builder
                .Property(f => f.Title)
                .HasColumnName("titulo")
                .HasColumnType("varchar(50)");

            builder
                .Property(f => f.Description)
                .HasColumnName("descricao")
                .HasColumnType("varchar(150)");

            builder
                .Property(f => f.Priority)
                .HasColumnName("prioridade")
                .HasDefaultValue(TaskPriority.Low);

            builder
                .Property(f => f.Status)
                .HasColumnName("status")
                .HasDefaultValue(enums.TaskStatus.Pending);

            builder
                .Property(f => f.CreatedAt)
                .HasColumnName("data_hora_incluido")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            builder
                .Property(f => f.UpdatedAt)
                .HasColumnName("data_hora_alterado")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnUpdate();

            builder
                .Property(f => f.CompletedAt)
                .HasColumnName("data_hora_completado")
                .HasColumnType("timestamp");

            builder
                .Property(f => f.CancelledAt)
                .HasColumnName("data_hora_cancelado")
                .HasColumnType("timestamp");

            builder
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .HasConstraintName("fk_tarefas_usuario");

            builder
                .HasIndex(t => t.UserId)
                .HasDatabaseName("idx_tarefa_usuario");
        }
    }
}

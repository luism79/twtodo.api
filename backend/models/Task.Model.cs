using backend.enums;
using System.Text.Json.Serialization;

namespace backend.models
{
    public class TaskModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("id_usuario")]
        public Guid UserId { get; set; }

        [JsonPropertyName("titulo")]
        public string? Title { get; set; }

        [JsonPropertyName("descricao")]
        public string? Description { get; set; }

        [JsonPropertyName("prioridade")]
        public TaskPriority Priority { get; set; }

        [JsonPropertyName("status")]
        public enums.TaskStatus Status { get; set; }

        [JsonPropertyName("data_hora_incluido")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("data_hora_alterado")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("data_hora_completado")]
        public DateTime? CompletedAt { get; set; }

        [JsonPropertyName("data_hora_cancelado")]
        public DateTime? CancelledAt { get; set; }

        [JsonPropertyName("e_mail_usuario")]
        public UserModel User { get; set; } = null!;
    }
}

using backend.enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.models.DTOs
{
    public record TaskDto(
        [property: JsonPropertyName("id")]
        Guid Id,

        [property: JsonPropertyName("id_usuario")]
        Guid UserId,

        [property: JsonPropertyName("titulo")]
        string? Title,

        [property: JsonPropertyName("descricao")]
        string? Description,

        [property: JsonPropertyName("prioridade")]
        TaskPriority Priority,

        [property: JsonPropertyName("status")]
        enums.TaskStatus Status,

        [property: JsonPropertyName("data_hora_incluido")]
        DateTime CreatedAt,

        [property: JsonPropertyName("data_hora_alterado")]
        DateTime? UpdatedAt,

        [property: JsonPropertyName("data_hora_completado")]
        DateTime? CompletedAt,

        [property: JsonPropertyName("data_hora_cancelado")]
        DateTime? CancelledAt,

        [property: JsonPropertyName("e_mail_usuario")]
        string? UserEmail
    );

    public record TaskInputDto(
        [Required]
        [property: JsonPropertyName("titulo")]
        string? Title,

        [Required]
        [property: JsonPropertyName("descricao")]
        string? Description,

        [property: JsonPropertyName("prioridade")]
        int Priority,

        [property: JsonPropertyName("status")]
        int Status
    );
}

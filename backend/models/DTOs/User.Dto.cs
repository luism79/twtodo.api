using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.models.DTOs
{
    public record UserDto(
        [property: JsonPropertyName("e_mail")]
        string Email,

        [property: JsonPropertyName("id")]
        Guid Id
    );

    public record UserAddDto(
        [Required]
        [property: JsonPropertyName("e_mail")]
        string Email,

        [Required]
        [property: JsonPropertyName("password")]
        string Password
    );
    
}

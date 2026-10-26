using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.models.DTOs
{
    public record LoginInputDto(
        [Required]
        [property: JsonPropertyName("e_mail")]
        string Email,

        [Required]
        [property: JsonPropertyName("password")]
        string Password
    );

    public record AuthDto(
        [property: JsonPropertyName("user_id")]
        Guid UserId,

        [property: JsonPropertyName("access_token")]
        string AccessToken,

        [property: JsonPropertyName("expires_in_minutes")]
        int ExpiresInMinutes
    );
}

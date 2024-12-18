using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Application.DTOs
{
    public record AppUserDTO
    (
        Guid Id,
        [Required] string Name,
        [Required, EmailAddress] string Email,
        [Required] string TelephoneNumber,
        [Required] string UserName,
        [Required] string Password,
        [Required] string Role
    );
}

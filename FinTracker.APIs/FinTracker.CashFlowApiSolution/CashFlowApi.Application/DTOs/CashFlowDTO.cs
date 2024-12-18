using System.ComponentModel.DataAnnotations;

namespace CashFlowApi.Application.DTOs
{
    public record CashFlowDTO
    (
        int Id,
        [Required] string Name,
        [Required, DataType(DataType.Currency)] decimal Amount
    );
}

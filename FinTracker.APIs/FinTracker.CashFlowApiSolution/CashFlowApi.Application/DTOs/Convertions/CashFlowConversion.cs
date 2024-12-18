using CashFlowApi.Domain.Entities;

namespace CashFlowApi.Application.DTOs.Convertions
{
    public static class CashFlowConversion
    {
        public static CashFlow ToEntity(CashFlowDTO cashFlow) => new CashFlow
        {
            Id = cashFlow.Id,
            Amount = cashFlow.Amount,
            Name = cashFlow.Name
        };

        public static (CashFlowDTO?, IEnumerable<CashFlowDTO>?) FromEntity(CashFlow? cashFlow, IEnumerable<CashFlow>? cashFlows)
        {
            if (cashFlow is not null && cashFlows is null)
            {
                var singleCashFlow = new CashFlowDTO
                    (
                    cashFlow.Id,
                    cashFlow.Name,
                    cashFlow.Amount
                    );
                return (singleCashFlow, null);
            }

            if (cashFlows is not null && cashFlow is null)
            {
                var _cashFlows = cashFlows.Select(x =>
                new CashFlowDTO(
                    x.Id,
                    x.Name,
                    x.Amount)
                ).ToList();
                return (null, _cashFlows);
            }
            return (null, null);
        }
    }
}

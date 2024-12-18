using CashFlowApi.Application.DTOs;
using CashFlowApi.Application.DTOs.Convertions;
using CashFlowApi.Application.Interfaces;
using FinTracker.SharedLibrary.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashFlowsController(ICashFlow cashFlowInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CashFlowDTO>>> GetCashFlows()
        {
            var cashFlows = await cashFlowInterface.GetAllAsync();
            if (!cashFlows.Any())
            {
                return NotFound("No cash flows detected in database");
            }
            var (_, list) = CashFlowConversion.FromEntity(null!, cashFlows);
            return list is not null ? Ok(list) : NotFound("No cash flow found");    
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CashFlowDTO>> GetCashFlow(int id)
        {
            var cashFlow = await cashFlowInterface.FindByIdAsync(id);
            if (cashFlow is null) {
                return NotFound($"Cash flow with id = {id} does not exist");
            }
            var (_cashFlow, _) = CashFlowConversion.FromEntity(cashFlow, null!);
            return _cashFlow is not null ? Ok(_cashFlow) : NotFound("Cash flow not found");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateCashFlow(CashFlowDTO cashFlowDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cashFlow = CashFlowConversion.ToEntity(cashFlowDTO);
            var resp = await cashFlowInterface.CreateAsync(cashFlow);
            return resp.Flag ? Ok(resp) : BadRequest(resp.Message);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateCashFlow(CashFlowDTO cashFlowDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cashFlow = CashFlowConversion.ToEntity(cashFlowDTO);
            var resp = await cashFlowInterface.UpdateAsync(cashFlow);
            return resp.Flag ? Ok(resp) : BadRequest(resp.Message);
        }
        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteCashFlow(CashFlowDTO cashFlowDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cashFlow = CashFlowConversion.ToEntity(cashFlowDTO);
            var resp = await cashFlowInterface.DeleteAsync(cashFlow);
            return resp.Flag ? Ok(resp) : BadRequest(resp.Message);
        }
    }
}

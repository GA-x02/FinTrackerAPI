using CashFlowApi.Application.Interfaces;
using CashFlowApi.Domain.Entities;
using CashFlowApi.Infrastructure.Data;
using FinTracker.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApi.Infrastructure.Repositories
{
    public class CashFlowRepository(CashFlowDbContext context) : ICashFlow
    {
        public async Task<Response> CreateAsync(CashFlow entity)
        {
            try
            {
                var currentEntity = context.СashFlows.Add(entity).Entity;
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} is added successfully");
            }
            catch (Exception ex)
            {
                return new Response(false, $"Error occurred adding new cash flow");
            }
        }

        public async Task<Response> DeleteAsync(CashFlow entity)
        {
            try
            {
                var currentEntity = await FindByIdAsync(entity.Id);
                if (currentEntity is null)
                {
                    return new Response(false, $"{entity.Name} not found");
                }

                context.СashFlows.Remove(currentEntity);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} is deleted successfully");
            }
            catch (Exception)
            {
                return new Response(false, $"Error occurred deleting cash flow");
            }
        }

        public async Task<CashFlow> FindByIdAsync(int id)
        {
            try
            {
                var entity = await context.СashFlows.FirstOrDefaultAsync(x => x.Id == id);
                return entity!;
            }
            catch (Exception)
            {
                throw new Exception("Error occurred retrieving cash flow");
            }
        }

        public async Task<IEnumerable<CashFlow>> GetAllAsync()
        {
            try
            {
                var entities = await context.СashFlows.AsNoTracking().ToListAsync();
                return entities;
            }
            catch (Exception)
            {
                throw new Exception("Error occurred retrieving cash flows");
            }
        }

        public async Task<CashFlow> GetByAsync(Expression<Func<CashFlow, bool>> predicate)
        {
            try
            {
                var entity = await context.СashFlows.Where(predicate).FirstOrDefaultAsync();
                return entity!;
            }
            catch (Exception)
            {    
                throw new Exception("Error occurred retrieving cash flow");
            }
        }

        public async Task<Response> UpdateAsync(CashFlow entity)
        {
            try
            {
                var currentEntity = await FindByIdAsync(entity.Id);
                if (currentEntity is null)
                {
                    return new Response(false, $"{entity.Name} not found");
                }
                context.Entry(currentEntity).State = EntityState.Detached;
                context.СashFlows.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} is updated successfully");
            }
            catch (Exception)
            {
                return new Response(false, $"Error occurred updating cash flow");
            }
        }
    }
}

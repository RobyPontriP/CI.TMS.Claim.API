using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class ClaimPerdiemService : BaseService
    {
        public ClaimPerdiemService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<ClaimPerdiemService> log)
           : base(context, httpContextAccessor, log)
        {
        }
        public async Task<List<ClaimPerdiemResponseDTO>> Get(Expression<Func<ClaimPerdiem, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.Id != Guid.Empty;

                return await context.ClaimPerdiem.Where(predicate).AsNoTracking().Project().To<ClaimPerdiemResponseDTO>().ToListAsync();

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
        public async Task<List<ClaimPerdiemResponseDTO>> GetPerdiemById(Guid id,Expression<Func<ClaimPerdiem, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.Id != Guid.Empty && x.Id == id;

                return await (from p in context.ClaimPerdiem
                              select new
                              {
                                  Id = p.Id == null ? Guid.Empty : p.Id,
                                  ClaimId = p.ClaimId == null ? Guid.Empty : p.ClaimId,
                                  DateFrom = p.DateFrom == null ? null : p.DateFrom,
                                  DateTo = p.DateTo == null ? null : p.DateTo,
                                  CountryId = p.CountryId == null ? "" : p.CountryId,
                                  CountryName = p.CountryName == null ? "" : p.CountryName,
                                  CityId = p.CityId == null ? "" : p.CityId,
                                  CityName = p.CityName == null ? "" : p.CityName,
                                  CityOther = p.CityOther == null ? "" : p.CityOther,
                                  B = p.B == null ? false : p.B,
                                  L = p.L == null ? false : p.L,
                                  D = p.D == null ? false : p.D,
                                  I = p.I == null ? false : p.I,
                                  F = p.F == null ? false : p.F,
                                  TotalAmount = p.TotalAmount == null ? 0 : p.TotalAmount,
                                  TotalAmount0 = p.TotalAmount0 == null ? 0 : p.TotalAmount0,
                                  TotalAmountFinance = p.TotalAmountFinance == null ? 0 : p.TotalAmountFinance,
                                  TotalAmount0Finance = p.TotalAmount0Finance == null ? 0 : p.TotalAmount0Finance,
                                  TotalPerdiemRate = p.TotalPerdiemRate,
                                  TotalPerdiemRate0 = p.TotalPerdiemRate0 == null ? 0 : p.TotalPerdiemRate0,
                                  Currency = p.Currency == null ? "" : p.Currency,
                                  IsActive = p.IsActive,
                                  IsFinance = p.IsFinance
                              }).Where(x => x.ClaimId == id && x.IsActive == true).Project().To<ClaimPerdiemResponseDTO>().ToListAsync();

                 
                
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<Guid> AddOrUpdate(ClaimPerdiemRequestDTO data)
        {
            var model = new ClaimPerdiem();
            try
            {
                if (data.Id == Guid.Empty)
                {
                    model.Id = await this.Add(data);
                }
                else if (data.Id == Guid.Empty && data.IsActive == false)
                {
                    await this.Delete(data.Id);
                }
                else
                {
                    await this.Update(data);
                    model.Id = data.Id;
                }

                return model.Id;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
           
        }
        public async Task<Guid> Add(ClaimPerdiemRequestDTO data)
        {
            try
            {
                var model = new ClaimPerdiem();
                model.MapFrom(data);
                model.DateFrom = ((Convert.ToDateTime(data.DateFrom) == Convert.ToDateTime("1/1/0001 12:00:00 AM") || (data.DateFrom == null)) ? null : (Convert.ToDateTime(data.DateFrom)));
                model.DateTo = ((Convert.ToDateTime(data.DateTo) == Convert.ToDateTime("1/1/0001 12:00:00 AM") || (data.DateTo == null)) ? null : (Convert.ToDateTime(data.DateTo)));
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = userId;
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = userId;
                model.IsActive = true;
                model.TotalAmount0 = Convert.ToDecimal(data.TotalAmount0) == null ? 0 : Convert.ToDecimal(data.TotalAmount0);
                model.TotalPerdiemRate = Convert.ToDecimal(data.TotalPerdiemRate) == null ? 0 : Convert.ToDecimal(data.TotalPerdiemRate);
                model.TotalPerdiemRate0 = Convert.ToDecimal(data.TotalPerdiemRate0) == null ? 0 : Convert.ToDecimal(data.TotalPerdiemRate0);
                model.TotalAmountFinance = Convert.ToDecimal(data.TotalAmountFinance) == null ? 0 : Convert.ToDecimal(data.TotalAmountFinance);
                model.TotalAmount0Finance = Convert.ToDecimal(data.TotalAmount0Finance) == null ? 0 : Convert.ToDecimal(data.TotalAmount0Finance);
                model.IsFinance = (data.IsFinance == null || !data.IsFinance) ? false : true;

                await context.ClaimPerdiem.AddAsync(model);
                await context.SaveChangesAsync();

                return model.Id;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<ClaimPerdiemResponseDTO> Update(ClaimPerdiemRequestDTO data)
        {
            try
            {
                var model = await context.ClaimPerdiem.AsNoTracking().FirstOrDefaultAsync(x => x.Id == data.Id);
                var newModel = new ClaimPerdiem();

                if (model == null)
                    throw new Exception("Claim perdiem detail not found.");
                newModel.MapFrom(model);
                newModel.DateFrom = ((Convert.ToDateTime(data.DateFrom) == Convert.ToDateTime("1/1/0001 12:00:00 AM") || (data.DateFrom == null)) ? null : (Convert.ToDateTime(data.DateFrom)));
                newModel.DateTo = ((Convert.ToDateTime(data.DateTo) == Convert.ToDateTime("1/1/0001 12:00:00 AM") || (data.DateTo == null)) ? null : (Convert.ToDateTime(data.DateTo)));
                newModel.CountryId = data.CountryId;
                newModel.CountryName = data.CountryName;
                newModel.CityId = data.CityId;
                newModel.CityName = data.CityName;
                newModel.CityOther = data.CityOther;
                newModel.B = data.B;
                newModel.L = data.L;
                newModel.D = data.D;
                newModel.I = data.I;
                newModel.F = data.F;
                newModel.Currency = data.Currency;
                newModel.TotalAmount = Convert.ToDecimal(data.TotalAmount);
                newModel.TotalPerdiemRate = Convert.ToDecimal(data.TotalPerdiemRate);
                newModel.TotalAmount0 = Convert.ToDecimal(data.TotalAmount0);
                newModel.TotalPerdiemRate0 = Convert.ToDecimal(data.TotalPerdiemRate0);
                newModel.TotalAmountFinance = Convert.ToDecimal(data.TotalAmountFinance) == null ? 0 : Convert.ToDecimal(data.TotalAmountFinance);
                newModel.IsFinance = (data.IsFinance == null || !data.IsFinance) ? false : true;
                newModel.IsActive = data.IsActive;
                newModel.UpdatedBy = userId;
                newModel.UpdatedAt = DateTime.Now;

                context.Update(newModel);
                await context.SaveChangesAsync();
                return (await this.Get(predicate: (x => x.Id == newModel.Id))).FirstOrDefault();

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<ClaimPerdiemResponseDTO> UpdateIsActive(Guid id)
        {
            try
            {
                var model = await context.ClaimPerdiem.FirstOrDefaultAsync(x => x.Id == id);
                if (model == null)
                    throw new Exception("Claim perdiem detail not found.");

                model.IsActive = false;
                model.UpdatedBy = userId;
                model.UpdatedAt = DateTime.Now;

                context.Update(model);
                await context.SaveChangesAsync();
                return (await this.Get(predicate: (x => x.Id == model.Id))).FirstOrDefault();

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                var model = await context.ClaimPerdiem.FirstOrDefaultAsync(x => x.ClaimId == id);

                model.UpdatedBy = userId;
                model.UpdatedAt = DateTime.Now;
                model.IsActive = false;

                context.ClaimPerdiem.Remove(model);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<ClaimPerdiemResponseDTO>> GetById(Guid id, Expression<Func<ClaimPerdiem, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "" && x.Id == id;
                return await context.ClaimPerdiem.Where(predicate).Project().To<ClaimPerdiemResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
        public async Task<List<ClaimPerdiemResponseDTO>> GetByClaimId(Guid id, Expression<Func<ClaimPerdiem, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "" && x.ClaimId == id;
                return await context.ClaimPerdiem.Where(predicate).Project().To<ClaimPerdiemResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }


        public async Task AddOrUpdateOrDelete(ClaimPerdiemRequestDTO data)
        {
            try
            {
                if (data.Id == Guid.Empty)
                {
                    await this.Add(data);
                }
                else if (data.Id != Guid.Empty && data.IsActive == false)
                {
                    await this.Delete(data.Id);
                }
                else
                {
                    await this.Update(data);
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public int CalculateDaysDifference(DateTime dateFrom, DateTime dateTo)
        {
            // Ensure dateTo is greater than or equal to dateFrom
            if (dateTo < dateFrom)
            {
                throw new ArgumentException("dateTo must be greater than or equal to dateFrom");
            }

            // Calculate the number of days
            TimeSpan timeSpan = dateTo - dateFrom;
            return timeSpan.Days;
        }

    }
}
using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using myTree.MicroService.Helper;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class ClaimPerdiemDetailService : BaseService
    {
        public ClaimPerdiemDetailService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<ClaimPerdiemDetailService> log)
           : base(context, httpContextAccessor, log)
        {
        }
        public async Task<IEnumerable<ClaimPerdiemDetailResponseDTO>> Get(Expression<Func<ClaimPerdiemDetail, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "";
                return await context.ClaimPerdiemDetail.Where(predicate).Project().To<ClaimPerdiemDetailResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<ClaimPerdiemDetailResponseDTO>> GetPerdiemDetailById(Guid id, Expression<Func<ClaimPerdiemDetail, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.Id != Guid.Empty && x.Id == id;

                return await (from p in context.ClaimPerdiem
                              join q in context.ClaimPerdiemDetail on p.Id equals q.ClaimPerdiemId
                              select new
                              {
                                  Id = q.Id == null ? Guid.Empty : q.Id,
                                  ClaimId = p.ClaimId == null ? Guid.Empty : p.ClaimId,
                                  ClaimPerdiemId = q.ClaimPerdiemId == null ? Guid.Empty : q.ClaimPerdiemId,
                                  Date = q.Date == null ? null : q.Date,
                                  CountryId = q.CountryId == null ? "" : q.CountryId,
                                  CountryName = p.CountryName,
                                  CityId = q.CityId == null ? "" : q.CityId,
                                  CityName = p.CityName,
                                  CityOther = q.CityOther == null ? "" : q.CityOther,
                                  PerdiemRate = q.PerdiemRate,
                                  B = q.B == null ? false : q.B,
                                  L = q.L == null ? false : q.L,
                                  D = q.D == null ? false : q.D,
                                  I = q.I == null ? false : q.I,
                                  F = q.F == null ? false : q.F,
                                  Amount = q.Amount,
                                  BAmount = q.BAmount,
                                  LAmount = q.LAmount,
                                  DAmount = q.DAmount,
                                  IAmount = q.IAmount,
                                  Amount0 = q.Amount0,
                                  BAmount0 = q.BAmount0,
                                  LAmount0 = q.LAmount0,
                                  DAmount0 = q.DAmount0,
                                  IAmount0 = q.IAmount0,
                                  BFinance = q.BFinance == null ? false : q.BFinance,
                                  LFinance = q.LFinance == null ? false : q.LFinance,
                                  DFinance = q.DFinance == null ? false : q.DFinance,
                                  IFinance = q.IFinance == null ? false : q.IFinance,
                                  FFinance = q.FFinance == null ? false : q.FFinance,
                                  AmountFinance = q.AmountFinance,
                                  BFinanceAmount = q.BFinanceAmount,
                                  LFinanceAmount = q.LFinanceAmount,
                                  DFinanceAmount = q.DFinanceAmount,
                                  IFinanceAmount = q.IFinanceAmount,
                                  AmountFinance0 = q.AmountFinance,
                                  BFinanceAmount0 = q.BFinanceAmount0,
                                  LFinanceAmount0 = q.LFinanceAmount0,
                                  DFinanceAmount0 = q.DFinanceAmount0,
                                  IFinanceAmount0 = q.IFinanceAmount0,
                                  Currency = q.Currency,
                                  IsActive = p.IsActive,
                                  IsFinance = q.IsFinance


                              }).Where(x => x.ClaimId == id && x.IsActive == true).Project().To<ClaimPerdiemDetailResponseDTO>().ToListAsync();



            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }


        public async Task AddOrUpdateOrDelete(ClaimPerdiemDetailRequestDTO data)
        {
            try
            {
                if (data.Id == Guid.Empty)
                {
                    await this.Add(data);
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
        public async Task<Guid> Add(ClaimPerdiemDetailRequestDTO data)
        {
            try
            {
                var model = new ClaimPerdiemDetail();
                model.MapFrom(data);
                model.Date = ((Convert.ToDateTime(data.Date) == Convert.ToDateTime("1/1/0001 12:00:00 AM") || (data.Date == null)) ? null : Convert.ToDateTime(data.Date));
                model.BAmount = Convert.ToDecimal(data.BAmount);
                model.LAmount = Convert.ToDecimal(data.LAmount);
                model.DAmount = Convert.ToDecimal(data.DAmount);
                model.IAmount = Convert.ToDecimal(data.IAmount);
                model.Amount = Convert.ToDecimal(data.Amount);
                model.BAmount0 = Convert.ToDecimal(data.BAmount0);
                model.LAmount0 = Convert.ToDecimal(data.LAmount0);
                model.DAmount0 = Convert.ToDecimal(data.DAmount0);
                model.IAmount0 = Convert.ToDecimal(data.IAmount0);
                model.Amount0 = Convert.ToDecimal(data.Amount0);
                model.PerdiemRate = Convert.ToDecimal(data.PerdiemRate);
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = userId;
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = userId;
                model.IsActive = true;
                if (data.IsActive == true)
                {
                    await context.ClaimPerdiemDetail.AddAsync(model);
                    await context.SaveChangesAsync();
                }
                return model.Id;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<ClaimPerdiemDetailResponseDTO> Update(ClaimPerdiemDetailRequestDTO data)
        {
            try
            {
                var model = await context.ClaimPerdiemDetail.AsNoTracking().FirstOrDefaultAsync(x => x.Id == data.Id);
                var newModel = new ClaimPerdiemDetail();
                if (model == null)
                    throw new Exception("Claim perdiem detail not found.");

                newModel.MapFrom(model);
                newModel.Date = ((Convert.ToDateTime(data.Date) == Convert.ToDateTime("1/1/0001 12:00:00 AM") || (data.Date == null)) ? null : Convert.ToDateTime(data.Date));
                newModel.CountryId = data.CountryId;
                newModel.CityId = data.CityId;
                newModel.CityOther = data.CityOther;
                newModel.BFinance = data.BFinance;
                newModel.LFinance = data.LFinance;
                newModel.DFinance = data.DFinance;
                newModel.IFinance = data.IFinance;
                newModel.FFinance = data.FFinance;
                newModel.BFinanceAmount = Convert.ToDecimal(data.BFinanceAmount);
                newModel.LFinanceAmount = Convert.ToDecimal(data.LFinanceAmount);
                newModel.DFinanceAmount = Convert.ToDecimal(data.DFinanceAmount);
                newModel.IFinanceAmount = Convert.ToDecimal(data.IFinanceAmount);
                newModel.AmountFinance = Convert.ToDecimal(data.AmountFinance);
                newModel.BFinanceAmount0 = Convert.ToDecimal(data.BFinanceAmount0);
                newModel.LFinanceAmount0 = Convert.ToDecimal(data.LFinanceAmount0);
                newModel.DFinanceAmount0 = Convert.ToDecimal(data.DFinanceAmount0);
                newModel.IFinanceAmount0 = Convert.ToDecimal(data.IFinanceAmount0);
                newModel.AmountFinance0 = Convert.ToDecimal(data.AmountFinance0);
                newModel.PerdiemRate = Convert.ToDecimal(data.PerdiemRate);
                newModel.Currency = data.Currency;
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

        public async Task Delete(ClaimPerdiemDetailRequestDTO data)
        {
            try
            {

                var model = await context.ClaimPerdiemDetail.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (model == null)
                    throw new Exception("Claim perdiem detail not found.");

                context.ClaimPerdiemDetail.Remove(model);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<ClaimPerdiemDetailResponseDTO>> GetById(Guid id, Expression<Func<ClaimPerdiemDetail, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "" && x.Id == id && x.ClaimId != Guid.Empty;
                return await context.ClaimPerdiemDetail.Where(predicate).Project().To<ClaimPerdiemDetailResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }


    }
}
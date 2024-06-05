using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Xml.Linq;

namespace CI.TMS.Claim.API.Services
{
    public class ClaimCommentService : BaseService
    {
        public ClaimCommentService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<ClaimCommentService> log)
            : base(context, httpContextAccessor, log)
        {
        }

        public async Task<List<ClaimCommentResponseDTO>> Get(Expression<Func<ClaimComment, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id != Guid.Empty;

                return await context.ClaimComment.Where(predicate)
                    .SelectMany(cc => context.Employee.Where(emp => emp.EmpUserId.Equals(cc.Name)).DefaultIfEmpty(), (cc, emp) => new { Comment = cc, Emp = emp})
                    .Select( select => new
                    {
                        Id = select.Comment.Id,
                        ClaimId = select.Comment.ClaimId,
                        Name = select.Emp.EmpName,
                        Date = select.Comment.Date,
                        ActionTaken = select.Comment.ActionTaken,
                        Role = select.Comment.Role,
                        Comment = select.Comment.Comment,
                        ActivityId = select.Comment.ActivityId,
                        Sn = select.Comment.Sn,
                        DocumentId = select.Comment.DocumentId
                    })
                    .Project().To<ClaimCommentResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<ClaimCommentResponseDTO>> GetById(Guid Id, Expression<Func<ClaimComment, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Role);

                return await (from p in context.ClaimComment
                              join q in context.Employee on p.Name equals q.EmpUserId
                              select new
                              {
                                  Id = p.Id,
                                  ClaimId = p.ClaimId,
                                  Name = q.EmpName,
                                  Date = p.Date,
                                  ActionTaken = p.ActionTaken,
                                  Role = p.Role,
                                  Comment = p.Comment,
                                  ActivityId = p.ActivityId,
                                  Sn = p.Sn,
                                  DocumentId = p.DocumentId
                              }).Where(p =>p.Name != "" && p.ClaimId != Guid.Empty && p.Id != Guid.Empty && p.ClaimId == Id).Distinct().OrderByDescending(x => x.Date).Project().To<ClaimCommentResponseDTO>().ToListAsync();


                //return await context.ClaimComment.Where(predicate).OrderBy(x => x.Date).AsNoTracking().Project().To<ClaimCommentResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task Insert(ClaimCommentRequestDTO data, string userId)
        {
            try
            {
                var model = new ClaimComment();
                double countSN = (from Sn in context.ClaimComment select Sn).Where(p => p.ClaimId == data.ClaimId).Count();

                model.MapFrom(data);
                model.ActivityId = data.ActivityId;
                model.Name = userId;
                model.Sn = Convert.ToDouble(countSN + 1).ToString();
                model.Date = DateTime.Now;
                context.ClaimComment.Add(model);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                log.LogError(e.Detail());
                throw;
            }
        }

        public async Task<Guid> Add(ClaimCommentRequestDTO data)
        {
            try
            {
                var model = new ClaimComment();
                model.Name = userId;
                model.MapFrom(data);


                await context.ClaimComment.AddAsync(model);
                await context.SaveChangesAsync();

                return model.Id;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<ClaimCommentResponseDTO> Update(ClaimCommentRequestDTO data)
        {
            try
            {
                var model = await context.ClaimComment.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (model == null)
                    throw new Exception("Claim comment code not found.");

                //model.MapFrom(data);
                model.Name = data.Name;
                model.Date = data.Date;
                model.ActionTaken = data.ActionTaken;
                model.Role = data.Role;
                model.Comment = data.Comment;
                model.Sn = data.Sn == null ? "" : data.Sn;

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
    }
}

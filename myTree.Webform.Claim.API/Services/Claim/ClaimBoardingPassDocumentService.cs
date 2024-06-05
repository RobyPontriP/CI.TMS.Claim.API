using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using myTree.MicroService.Helper;
using System.Data;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class ClaimBoardingPassDocumentService : BaseService
    {
        public ClaimBoardingPassDocumentService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<ClaimBoardingPassDocumentService> log)
           : base(context, httpContextAccessor, log)
        {
        }
        public async Task<List<ClaimBoardingPassDocumentResponseDTO>> GetById(Guid id, Expression<Func<ClaimBoardingPassDocument, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "" && x.Id == id;
                return await context.ClaimBoardingPassDocument.Where(predicate).Project().To<ClaimBoardingPassDocumentResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }


        public async Task<List<ClaimBoardingPassDocumentResponseDTO>> GetByClaimId(Guid claimId, Expression<Func<ClaimBoardingPassDocument, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "" && x.ClaimId == claimId && x.IsActive == true;
                return await context.ClaimBoardingPassDocument.Where(predicate)
                    .SelectMany(supDoc => context.ClaimDocument.Where(doc => supDoc.ClaimDocumentId == doc.Id).DefaultIfEmpty(), (supDoc, doc) => new { supDoc = supDoc, doc = doc })
                    .Select(x => new ClaimBoardingPassDocumentResponseDTO
                    {
                        Id = x.supDoc.Id,
                        ClaimDocumentId = x.supDoc.ClaimDocumentId,
                        ClaimId = x.supDoc.ClaimId,
                        Description = x.supDoc.Description,
                        SeqNo = x.supDoc.SeqNo,
                        IsActive = x.supDoc.IsActive,
                        FileName = x.doc.FileName,
                        FileUrl = x.doc.FileUrl,
                        IsFinance = x.supDoc.IsFinance
                    }).OrderBy(x => x.IsFinance).ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
        public async Task AddDocument(ClaimBoardingPassDocument document)
        {
            try
            {
                if (document == null)
                    throw new Exception("Document is required.");
                await context.ClaimBoardingPassDocument.AddAsync(document);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                log.LogError(ex.Detail());
                throw;
            }
        }
        public async Task<Guid> AddOrUpdateOrDelete(ClaimBoardingPassDocumentRequestDTO data)
        {
            var model = new ClaimBoardingPassDocument();
            try
            {
                if (data.Id == Guid.Empty)
                {
                    model.Id = await this.Add(data);
                }
                else if (data.Id == Guid.Empty && data.IsActive == false)
                {
                    await this.Delete(data.Id);
                    model.Id = data.Id;
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
        public async Task<Guid> Add(ClaimBoardingPassDocumentRequestDTO data)
        {
            try
            {
                var model = new ClaimBoardingPassDocument();
                model.MapFrom(data);

                model.IsActive = true;
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = userId;
                model.UpdatedBy = userId;
                model.UpdatedAt = DateTime.Now;

                await context.ClaimBoardingPassDocument.AddAsync(model);
                await context.SaveChangesAsync();

                return model.Id;
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
                var model = await context.ClaimBoardingPassDocument.FirstOrDefaultAsync(x => x.Id == id);

                model.IsActive = false;

                context.ClaimBoardingPassDocument.Remove(model);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
        public async Task UpdateDocument(ClaimBoardingPassDocument document)
        {
            try
            {
                if (document == null)
                    throw new Exception("Document is required.");

                var model = await context.ClaimBoardingPassDocument.Where(x => x.Id == document.Id).AsNoTracking().FirstOrDefaultAsync();
                if (model != null)
                {
                    model.MapFrom(document);
                    model.Id = document.Id;
                    model.UpdatedBy = userId;
                    model.UpdatedAt = DateTime.Now;

                    context.ClaimBoardingPassDocument.Update(document);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Detail());
                throw;
            }
        }

        public async Task<List<ClaimBoardingPassDocumentResponseDTO>> Get(Expression<Func<ClaimBoardingPassDocument, bool>> predicate = null,
            bool isArchived = false, bool isArchiveIncluded = false)
        {
            try
            {
                //if (predicate == null)
                //    predicate = (x => x.IsActive == true && x.IsArchived == false);
                var data =
                    await context.ClaimBoardingPassDocument.Where(predicate)
                        .Select(a => new ClaimBoardingPassDocumentResponseDTO
                        {
                            Id = a.Id,
                            ClaimDocumentId = a.ClaimDocumentId,
                            ClaimId = a.ClaimId,
                            Description = a.Description,
                            SeqNo = a.SeqNo,
                            IsActive = a.IsActive
                        }).OrderBy(x => x.SeqNo)
                        .ToListAsync();



                return data;
            }
            catch (Exception ex)
            {
                log.LogError(ex.Detail());
                throw;
            }
        }

        public async Task<ClaimBoardingPassDocumentResponseDTO> Add(ClaimBoardingPassDocumentRequestDTO data, string userId = "")
        {
            try
            {
                var model = new ClaimBoardingPassDocument();
                model.MapFrom(data);
                model.ClaimId = data.ClaimId;
                model.ClaimDocumentId = data.ClaimDocumentId;
                model.DocumentDate = DateTime.Now;
                model.Description = "BoardingPass Document";
                model.RelatedId = Guid.Empty;
                model.SeqNo = 0;
                model.IsActive = true;

                //model.SeqNo = (context.ClaimBoardingPassDocument
                //    .Where(x => x.ProjectMEId == model.ProjectMEId && x.CategoryId == model.CategoryId)?
                //    .Max(x => (int?)x.SeqNo) ?? 0) + 1;

                //Validate(model);

                await context.ClaimBoardingPassDocument.AddAsync(model);
                await context.SaveChangesAsync();
                return (await this.Get(predicate: (x => x.Id == model.Id))).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.LogError($"Id: {data.Id}, Error: {ex.Detail()}");
                throw;
            }
        }

        public async Task<ClaimBoardingPassDocumentResponseDTO> Update(ClaimBoardingPassDocumentRequestDTO data)
        {
            try
            {
                Guid idToUpdate = data.Id;
                var model = await context.ClaimBoardingPassDocument.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (model == null)
                    throw new Exception("MELIA document not found.");

                model.Id = idToUpdate;
                model.DocumentDate = DateTime.Now;
                model.Description = data.Description;
                model.ClaimDocumentId = data.ClaimDocumentId;
                model.IsActive = data.IsActive;
                model.UpdatedBy = userId;
                model.UpdatedAt = DateTime.Now;

                context.Update(model);
                await context.SaveChangesAsync();

                return (await this.Get(predicate: (x => x.Id == model.Id))).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.LogError($"Id: {data.Id}, Error: {ex.Detail()}");
                throw;
            }
        }

        public async Task Delete(Guid id, string userId)
        {
            try
            {

                var model = await context.ClaimBoardingPassDocument.FirstOrDefaultAsync(x => x.Id == id);


                model.IsActive = false;

                context.Update(model);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                log.LogError($"Id: {id}, Error: {ex.Detail()}");
                throw;
            }
        }

        private void Validate(ClaimBoardingPassDocument data)
        {
            if (data.Id == Guid.Empty)
                throw new Exception("MELIA project id is required.");
            //if (string.IsNullOrEmpty(data.CategoryId))
            //    throw new Exception("MELIA category is required.");
        }

        public async Task CompareDocument(ClaimBoardingPassDocumentResponseDTO source, ClaimBoardingPassDocumentResponseDTO target,
            string projectId, string userId, DateTime changeTime, string categoryId)
        {
            try
            {
                #region Document
                DataTable dtSource = new DataTable();
                //AuditClaimBoardingPassDocumentResponseDTO auditObj = new AuditClaimBoardingPassDocumentResponseDTO();
                //foreach (PropertyInfo info in auditObj.GetType().GetProperties())
                //{
                //    dtSource.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
                //}

                DataTable dtTarget = dtSource.Clone();

                //populate data
                //if (source != null)
                //{
                //    var dr = dtSource.NewRow();
                //    foreach (PropertyInfo info in auditObj.GetType().GetProperties())
                //    {
                //        dr[info.Name] = source.GetType().GetProperty(info.Name).GetValue(source);
                //    }

                //    dtSource.Rows.Add(dr);
                //}

                //if (target != null)
                //{
                //    var dr = dtTarget.NewRow();
                //    foreach (PropertyInfo info in auditObj.GetType().GetProperties())
                //    {
                //        dr[info.Name] = target.GetType().GetProperty(info.Name).GetValue(target);
                //    }

                //    dtTarget.Rows.Add(dr);
                //}

                List<DataTableParam> columParams;
                string[] meCoreElements = new string[] { "LOGFRAMEWORK", "MEPLAN", "TOC" };
                if (meCoreElements.Contains(categoryId.ToUpper()))
                {
                    columParams = new List<DataTableParam>()
                    {
                        new DataTableParam{ColumnName = "Date", ColumnTitle = "Date", DataType = typeof(DateTime), CustomFormat = "{0:dd MMM yyyy}", IsSkipped = false},
                        new DataTableParam{ColumnName = "PICName", ColumnTitle = "PIC", DataType = typeof(string), CustomFormat = "{0}", IsSkipped = false},
                        new DataTableParam{ColumnName = "Description", ColumnTitle = "Description", DataType = typeof(string), CustomFormat = "{0}", IsSkipped = false},
                        new DataTableParam{ColumnName = "DocumentFileName", ColumnTitle = "File", DataType = typeof(string), CustomFormat = "<a href='{0}' target='blank'>{1}</a>", IsSkipped = false},
                        new DataTableParam{ColumnName = "DocumentFileURL", ColumnTitle = "File", DataType = typeof(string), CustomFormat = "{0}", IsSkipped = true}
                    };
                }
                else
                {
                    columParams = new List<DataTableParam>()
                    {
                        new DataTableParam{ColumnName = "Date", ColumnTitle = "Date", DataType = typeof(DateTime), CustomFormat = "{0:dd MMM yyyy}", IsSkipped = false},
                        new DataTableParam{ColumnName = "PICName", ColumnTitle = "PIC", DataType = typeof(string), CustomFormat = "{0}", IsSkipped = false},
                        new DataTableParam{ColumnName = "DocumentTypeName", ColumnTitle = "Type", DataType = typeof(string), CustomFormat = "{0}", IsSkipped = false},
                        new DataTableParam{ColumnName = "Description", ColumnTitle = "Description", DataType = typeof(string), CustomFormat = "{0}", IsSkipped = false},
                        new DataTableParam{ColumnName = "DocumentFileName", ColumnTitle = "File", DataType = typeof(string), CustomFormat = "<a href='{0}' target='blank'>{1}</a>", IsSkipped = false},
                        new DataTableParam{ColumnName = "DocumentFileURL", ColumnTitle = "File", DataType = typeof(string), CustomFormat = "{0}", IsSkipped = true}
                    };
                }


                //List<Audit> auditRows = new List<Audit>();
                //string categoryName = "Document";
                //DataRow sourceRow = null;
                //DataRow targetRow = null;

                //var cat = context.MECategory
                //    .Where(x => x.Id.ToLower() == categoryId.ToLower()).FirstOrDefault();
                //if (cat != null)
                //    categoryName = cat.Name;

                //if (dtSource.Rows.Count > 0)
                //{
                //    sourceRow = dtSource.Rows[0];
                //    if (!meCoreElements.Contains(categoryId.ToUpper()))
                //    {
                //        sourceRow["DocumentTypeName"] = sourceRow["DocumentTypeOther"].ToString() != "" ?
                //            $"{sourceRow["DocumentTypeName"]}, {sourceRow["DocumentTypeOther"]}"
                //            : sourceRow["DocumentTypeName"].ToString();
                //    }
                //}

                //if (dtTarget.Rows.Count > 0)
                //{
                //    targetRow = dtTarget.Rows[0];
                //    if (!meCoreElements.Contains(categoryId.ToUpper()))
                //    {
                //        targetRow["DocumentTypeName"] = targetRow["DocumentTypeOther"].ToString() != "" ?
                //            $"{targetRow["DocumentTypeName"]}, {targetRow["DocumentTypeOther"]}"
                //            : targetRow["DocumentTypeName"].ToString();
                //    }
                //}

                //auditRows.AddRange(DataTableService.CompareData(dc: dtSource.Columns, source: sourceRow, target: targetRow,
                //    columns: columParams, moduleName: "MELIA", subModuleName: categoryName,
                //    moduleId: Guid.Parse(projectId), keyFieldName: "Id", userId: userId, isTabular: true, changeTime: changeTime));
                #endregion

                //context.AddRange(auditRows);
                //await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                log.LogError($"Error: {ex.Detail()}");
                throw;
            }
        }

    }
}
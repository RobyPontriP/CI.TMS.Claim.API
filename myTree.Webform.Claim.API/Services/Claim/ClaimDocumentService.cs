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
    public class ClaimDocumentService : BaseService
    {
        public ClaimDocumentService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<ClaimDocumentService> log)
           : base(context, httpContextAccessor, log)
        {
        }

        public async Task AddDocument(ClaimDocument document)
        {
            try
            {
                if (document == null)
                    throw new Exception("Document is required.");
                await context.ClaimDocument.AddAsync(document);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                log.LogError(ex.Detail());
                throw;
            }
        }

        public async Task UpdateDocument(ClaimDocument document)
        {
            try
            {
                if (document == null)
                    throw new Exception("Document is required.");

                var model = await context.ClaimDocument.Where(x => x.Id == document.Id).AsNoTracking().FirstOrDefaultAsync();
                if (model != null)
                {
                    model.MapFrom(document);
                    model.Id = document.Id;

                    context.ClaimDocument.Update(document);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Detail());
                throw;
            }
        }
        public async Task<List<ClaimDocumentResponseDTO>> GetById(Guid id, Expression<Func<ClaimDocument, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "" && x.Id == id;
                return await context.ClaimDocument.Where(predicate).Project().To<ClaimDocumentResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<ClaimDocumentResponseDTO>> Get(Expression<Func<ClaimDocument, bool>> predicate = null,
            bool isArchived = false, bool isArchiveIncluded = false)
        {
            try
            {
                //if (predicate == null)
                //    predicate = (x => x.IsActive == true && x.IsArchived == false);
                var data =
                    await context.ClaimDocument.Where(predicate)
                        .Select(a => new ClaimDocumentResponseDTO
                        {
                            Id = a.Id,
                            FileName = a.FileName,
                            FileUrl = a.FileUrl
                        })
                        .ToListAsync();

                

                return data;
            }
            catch (Exception ex)
            {
                log.LogError(ex.Detail());
                throw;
            }
        }
        public async Task<List<ClaimDocumentResponseDTO>> GetByIdDocument(Guid id, Expression<Func<ClaimDocument, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "" && x.Id == id;
                return await context.ClaimDocument.Where(predicate).Project().To<ClaimDocumentResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<ClaimDocumentResponseDTO> Add(ClaimDocumentRequestDTO data, string userId = "")
        {
            try
            {
                var model = new ClaimDocument();
                model.MapFrom(data);

                model.CreatedAt = DateTime.Now;
                model.CreatedBy = userId;
                model.UpdatedBy = userId;
                model.UpdatedAt = DateTime.Now;
                model.IsActive = true;

                //model.SeqNo = (context.ClaimDocument
                //    .Where(x => x.ProjectMEId == model.ProjectMEId && x.CategoryId == model.CategoryId)?
                //    .Max(x => (int?)x.SeqNo) ?? 0) + 1;

                //Validate(model);

                await context.ClaimDocument.AddAsync(model);
                await context.SaveChangesAsync();
                return (await this.Get(predicate: (x => x.Id == model.Id))).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.LogError($"Id: {data.Id}, Error: {ex.Detail()}");
                throw;
            }
        }

        public async Task<ClaimDocumentResponseDTO> Update(ClaimDocumentRequestDTO data, string userId = "")
        {
            try
            {
                var model = await context.ClaimDocument.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (model == null)
                    throw new Exception("MELIA document not found.");

                // var seqNo = model.SeqNo;
                model.MapFrom(data);

                model.UpdatedBy = userId;
                model.UpdatedAt = DateTime.Now;
                model.IsActive = true;


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

                var model = await context.ClaimDocument.FirstOrDefaultAsync(x => x.Id == id);

                model.UpdatedBy = userId;
                model.UpdatedAt = DateTime.Now;
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

        private void Validate(ClaimDocument data)
        {
            if (data.Id == Guid.Empty)
                throw new Exception("MELIA project id is required.");
            //if (string.IsNullOrEmpty(data.CategoryId))
            //    throw new Exception("MELIA category is required.");
        }

        public async Task CompareDocument(ClaimDocumentResponseDTO source, ClaimDocumentResponseDTO target,
            string projectId, string userId, DateTime changeTime, string categoryId)
        {
            try
            {
                #region Document
                DataTable dtSource = new DataTable();
                //AuditClaimDocumentResponseDTO auditObj = new AuditClaimDocumentResponseDTO();
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
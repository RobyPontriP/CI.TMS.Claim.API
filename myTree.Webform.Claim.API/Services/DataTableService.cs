using CI.TMS.Claim.API.Persistence;
using Microsoft.AspNetCore.Authentication;
using myTree.MicroService.Helper;
using System.Data;
using System.Reflection;

namespace CI.TMS.Claim.API.Services
{
    public class DataTableService
    {
        public static DataTable CreateDataTable<T>(List<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(T).FullName;
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        //public static List<Audit> CompareData(DataColumnCollection dc, DataRow source, DataRow target, List<DataTableParam> columns,
        //    string moduleName, string subModuleName, Guid moduleId, string keyFieldName, string userId,
        //    bool isTabular = false,
        //    DateTime? changeTime = null)
        //{
        //    int seqNo = 1;
        //    if (changeTime == null)
        //        changeTime = DateTime.Now;

        //    List<Audit> comparedResultAudit = new List<Audit>();

        //    string typeOfChange = "";
        //    if (source == null && target == null)
        //        throw new Exception("No object to be compared.");

        //    bool isChanged = false;

        //    if (source == null && target != null)
        //    {
        //        typeOfChange = "Added";
        //        isChanged = true;
        //    }

        //    if (source != null && target == null)
        //    {
        //        typeOfChange = "Deleted";
        //        isChanged = true;
        //    }

        //    if (source != null && target != null)
        //        typeOfChange = "Updated";

        //    string Id = Guid.NewGuid().ToString();
        //    if (dc.IndexOf(keyFieldName) != -1)
        //    {
        //        if (source != null)
        //            Id = source[keyFieldName].ToString();
        //        if (target != null)
        //            Id = target[keyFieldName].ToString();
        //    }

        //    int colLength = columns == null ? 0 : columns.Count;
        //    int emptyCol = 0;
        //    foreach (var auditCol in columns)
        //    {
        //        bool isChecked = true;
        //        System.Type colType = typeof(System.String);
        //        string customFormat = string.IsNullOrEmpty(auditCol.CustomFormat) ? "{0}" : auditCol.CustomFormat;

        //        DataColumn col = null;
        //        var idxCol = dc.IndexOf(auditCol.ColumnName);
        //        if (idxCol >= 0)
        //        {
        //            col = dc[idxCol];
        //            colType = auditCol.DataType ?? null;
        //        }
        //        else
        //        {
        //            isChecked = false;
        //        }

        //        if (isChecked)
        //        {
        //            colType = colType ?? col.DataType;

        //            string s = string.Empty;
        //            string d = string.Empty;
        //            switch (colType.Name.ToLower())
        //            {
        //                case "string":
        //                    if (auditCol.ColumnName.ToLower() == "documentfilename" &&
        //                        ((source != null && source.Table.Columns.Contains("documentfileurl"))
        //                         || (target != null && target.Table.Columns.Contains("documentfileurl"))
        //                        )
        //                        )
        //                    {
        //                        s = source == null ? "" : String.Format("{0}", source[col.ColumnName]);
        //                        d = target == null ? "" : String.Format("{0}", target[col.ColumnName]);
        //                    }
        //                    else
        //                    {
        //                        s = source == null ? "" : String.Format(customFormat, source[col.ColumnName]);
        //                        d = target == null ? "" : String.Format(customFormat, target[col.ColumnName]);
        //                    }

        //                    break;
        //                case "decimal":
        //                case "int32":
        //                case "float":
        //                case "double":
        //                case "long":
        //                    s = source == null ? "" : String.Format(customFormat, (source[col.ColumnName] ?? 0));
        //                    d = target == null ? "" : String.Format(customFormat, (target[col.ColumnName] ?? 0));
        //                    break;
        //                case "datetime":
        //                    DateTime sDate;
        //                    DateTime.TryParse(source == null ? "" : source[col.ColumnName].ToString(), out sDate);

        //                    s = (sDate == DateTime.MinValue || sDate == DateTime.Parse("1900-01-01"))
        //                        ? ""
        //                        : String.Format(customFormat, sDate);

        //                    DateTime tDate;
        //                    DateTime.TryParse(target == null ? "" : target[col.ColumnName].ToString(), out tDate);

        //                    d = (tDate == DateTime.MinValue || tDate == DateTime.Parse("1900-01-01"))
        //                        ? ""
        //                        : String.Format(customFormat, tDate);
        //                    break;
        //                case "boolean":
        //                    bool sB; Boolean.TryParse(source == null ? "" : source[col.ColumnName].ToString(), out sB);
        //                    bool dB; Boolean.TryParse(target == null ? "" : target[col.ColumnName].ToString(), out dB);
        //                    s = String.Format(customFormat, sB.ToString().ToLower() == "true" ? "Yes" : "No");
        //                    d = String.Format(customFormat, dB.ToString().ToLower() == "true" ? "Yes" : "No");
        //                    break;
        //                default:
        //                    break;
        //            }
        //            if (s != d)
        //                isChanged = true;
        //            else
        //                isChanged = false;

        //            if (auditCol.ColumnName.ToLower() == "documentfilename" &&
        //                ((source != null && source.Table.Columns.Contains("documentfileurl"))
        //                         || (target != null && target.Table.Columns.Contains("documentfileurl"))
        //                        ))
        //            {
        //                if (s != "")
        //                    s = String.Format(customFormat, source["documentfileurl"], s);

        //                if (d != "")
        //                    d = String.Format(customFormat, target["documentfileurl"], d);
        //            }

        //            bool isSkipped = auditCol.IsSkipped;

        //            if (string.IsNullOrEmpty(d) && string.IsNullOrEmpty(s))
        //            {
        //                emptyCol++;
        //                if (!isTabular)
        //                {
        //                    isSkipped = true;
        //                    isChanged = false;
        //                }
        //            }

        //            if ((isChanged && !isTabular || isTabular) && !isSkipped)
        //            {
        //                var auditRecord = new Audit()
        //                {
        //                    ApprovalNo = 0,
        //                    ChangeBy = string.Format("{0}", userId),
        //                    ChangeTime = changeTime,
        //                    Id = Guid.NewGuid(),
        //                    ModuleId = moduleId,
        //                    ModuleName = string.Format("{0}", moduleName),
        //                    ReasonOfChange = "",
        //                    SeqNo = seqNo,
        //                    SubModule = string.Format("{0}", subModuleName),
        //                    ChangeType = typeOfChange,
        //                    FieldName = auditCol.ColumnTitle ?? auditCol.ColumnName,
        //                    PreviousValue = s,
        //                    NewValue = d,
        //                    RecId = Id
        //                };
        //                seqNo++;
        //                comparedResultAudit.Add(auditRecord);
        //            }
        //        }
        //        else
        //        {
        //            emptyCol++;
        //        }
        //    }
        //    if (isTabular && emptyCol == colLength)
        //        isChanged = false;

        //    if (!isChanged && isTabular)
        //    {
        //        comparedResultAudit.RemoveAll(x => x.RecId.ToString() == Id);
        //    }

        //    return comparedResultAudit;
        //}

        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }

    public class DataTableParam
    {
        public string ColumnName { get; set; }
        public string ColumnTitle { get; set; }
        public System.Type DataType { get; set; }
        public string CustomFormat { get; set; }
        public bool IsSkipped { get; set; } = false;
    }


}

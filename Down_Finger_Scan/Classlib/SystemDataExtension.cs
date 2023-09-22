using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace System.Data {
    /// <summary>
    /// Author      : CSI.VJ
    /// Create date : 2018-07-17
    /// Description : System.Data Extension
    /// Latest
    /// Modifier    : 
    /// Modify date : 2018-08-28
    /// Remark      : 
    /// </summary>
    public static class SystemDataExtension {
        public static List<TSource> ToList<TSource>(this DataTable dataTable) where TSource : new() {
            var dataList = new List<TSource>();
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var objFieldNames = (from PropertyInfo p in typeof(TSource).GetProperties(flags)
                                 select new {
                                     Name = p.Name,
                                     Type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType
                                 }).ToList();
            var dtFieldNames = (from DataColumn c in dataTable.Columns
                                select new {
                                    Name = c.ColumnName,
                                    Type = c.DataType
                                }).ToList();
            var commonFields = (from o in objFieldNames
                                join d in dtFieldNames on new { o.Name } equals new { d.Name }
                                select new {
                                    Name = o.Name,
                                    Type = o.Type
                                }).ToList();
            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList()) {
                var aTSource = new TSource();
                foreach (var f in commonFields) {
                    PropertyInfo propertyInfos = aTSource.GetType().GetProperty(f.Name);
                    try {

                        if (dataRow[f.Name] == DBNull.Value || propertyInfos.PropertyType == dataRow[f.Name].GetType()) {
                            var value = (dataRow[f.Name] == DBNull.Value) ? null : dataRow[f.Name];
                            propertyInfos.SetValue(aTSource, value, null);
                        } else {
                            #region SetValue

                            if (propertyInfos.PropertyType == typeof(Boolean) && dataRow[f.Name].GetType() == typeof(String)) {
                                propertyInfos.SetValue(aTSource, (dataRow[f.Name].ToString() == "1"), null);
                            } else if (propertyInfos.PropertyType == typeof(Int16?)) {
                                propertyInfos.SetValue(aTSource, (Int16?)Convert.ChangeType(dataRow[f.Name], typeof(Int16)), null);
                            } else if (propertyInfos.PropertyType == typeof(Int32?)) {
                                propertyInfos.SetValue(aTSource, (Int32?)Convert.ChangeType(dataRow[f.Name], typeof(Int32)), null);
                            } else if (propertyInfos.PropertyType == typeof(Int64?)) {
                                propertyInfos.SetValue(aTSource, (Int64?)Convert.ChangeType(dataRow[f.Name], typeof(Int64)), null);
                            } else if (propertyInfos.PropertyType == typeof(Double?)) {
                                propertyInfos.SetValue(aTSource, (Double?)Convert.ChangeType(dataRow[f.Name], typeof(Double)), null);
                            } else if (propertyInfos.PropertyType == typeof(Decimal?)) {
                                propertyInfos.SetValue(aTSource, (Decimal?)Convert.ChangeType(dataRow[f.Name], typeof(Decimal)), null);
                            } else {
                                propertyInfos.SetValue(aTSource, dataRow[f.Name], null);
                            }

                            #endregion
                        }
                    } catch (Exception ex) {
                        Debug.WriteLine($"ToList ({dataRow[f.Name]}): " + ex.Message);
                    }

                }
                dataList.Add(aTSource);
            }
            return dataList;
        }

        public static TSource ToData<TSource>(this DataTable dataTable) where TSource : new() {
            var aTSource = new TSource();
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var objFieldNames = (from PropertyInfo p in typeof(TSource).GetProperties(flags)
                                 select new {
                                     Name = p.Name,
                                     Type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType
                                 }).ToList();
            var dtFieldNames = (from DataColumn c in dataTable.Columns
                                select new {
                                    Name = c.ColumnName,
                                    Type = c.DataType
                                }).ToList();
            var commonFields = (from o in objFieldNames
                                join d in dtFieldNames on new { o.Name } equals new { d.Name }
                                select new {
                                    Name = o.Name,
                                    Type = o.Type
                                }).ToList();
            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList()) {
                foreach (var f in commonFields) {
                    PropertyInfo propertyInfos = aTSource.GetType().GetProperty(f.Name);
                    if (dataRow[f.Name] == DBNull.Value || propertyInfos.PropertyType == dataRow[f.Name].GetType()) {
                        var value = (dataRow[f.Name] == DBNull.Value) ? null : dataRow[f.Name];
                        propertyInfos.SetValue(aTSource, value, null);
                    } else {
                        #region SetValue

                        if (propertyInfos.PropertyType == typeof(Boolean) && dataRow[f.Name].GetType() == typeof(String)) {
                            propertyInfos.SetValue(aTSource, (dataRow[f.Name].ToString() == "1"), null);
                        } else if (propertyInfos.PropertyType == typeof(Int16?)) {
                            propertyInfos.SetValue(aTSource, (Int16?)Convert.ChangeType(dataRow[f.Name], typeof(Int16)), null);
                        } else if (propertyInfos.PropertyType == typeof(Int32?)) {
                            propertyInfos.SetValue(aTSource, (Int32?)Convert.ChangeType(dataRow[f.Name], typeof(Int32)), null);
                        } else if (propertyInfos.PropertyType == typeof(Int64?)) {
                            propertyInfos.SetValue(aTSource, (Int64?)Convert.ChangeType(dataRow[f.Name], typeof(Int64)), null);
                        } else if (propertyInfos.PropertyType == typeof(Double?)) {
                            propertyInfos.SetValue(aTSource, (Double?)Convert.ChangeType(dataRow[f.Name], typeof(Double)), null);
                        } else if (propertyInfos.PropertyType == typeof(Decimal?)) {
                            propertyInfos.SetValue(aTSource, (Decimal?)Convert.ChangeType(dataRow[f.Name], typeof(Decimal)), null);
                        } else {
                            propertyInfos.SetValue(aTSource, dataRow[f.Name], null);
                        }

                        #endregion
                    }
                }
            }
            return aTSource;
        }
    }
}
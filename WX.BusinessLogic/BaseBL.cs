using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using WX.Helper;
using WX.Model;

namespace WX.BusinessLogic
{
    public class BaseBL
    {
        protected T GenneralT<T>(DataRow dr, DataColumnCollection colList) where T : BaseModel, new()
        {
            T result = new T();
            if (dr != null)
            {
                Type resultT = result.GetType();
                foreach (DataColumn col in colList)
                {
                    PropertyInfo pInfo = resultT.GetProperties().SingleOrDefault(item => item.Name == col.ColumnName);
                    if (pInfo != null)
                    {
                        try
                        {
                            if (pInfo.PropertyType == typeof(string))
                            {
                                pInfo.SetValue(result, dr[col.ColumnName].ToString(), null);
                            }
                            else if (pInfo.PropertyType == typeof(int))
                            {
                                pInfo.SetValue(result, Convert.ToInt32(dr[col.ColumnName].ToString()), null);
                            }
                            else if (pInfo.PropertyType == typeof(DateTime))
                            {
                                pInfo.SetValue(result, Convert.ToDateTime(dr[col.ColumnName].ToString()), null);
                            }
                            else if (pInfo.PropertyType == typeof(Boolean))
                            {
                                pInfo.SetValue(result, Convert.ToBoolean(dr[col.ColumnName].ToString()), null);
                            }
                        }
                        catch { }

                    }
                }
            }
            return result;
        }

        protected T GenneralT<T>(DataTable dt) where T : BaseModel, new()
        {
            T result = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                result = GenneralT<T>(dt.Rows[0], dt.Columns);
            }
            return result;
        }

        protected IList<T> GenneralListT<T>(DataTable dt) where T : BaseModel, new()
        {
            IList<T> resultList = new List<T>();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataColumnCollection colList = dt.Columns;
                foreach (DataRow row in dt.Rows)
                {
                    T result = GenneralT<T>(row, colList);
                    resultList.Add(result);
                }
            }
            return resultList;
        }

        /// <summary>
        /// 防SQL注入，所以使用''替换'
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public string NoSQLInjection_stringWhere(string sqlWhere)
        {
            return sqlWhere.Replace("'", "''");
        }

    }
}

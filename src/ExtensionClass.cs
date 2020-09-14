using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AGW.Base.Components;

namespace AGW.Base.Extensions
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtensionClass
    {
        /// <summary>
        /// 获取数据容器的DataTable
        /// </summary>
        /// <param name="Grid">数据容器</param>
        /// <returns></returns>
        public static DataTable GetDataTable(this ComponentDataGrid Grid)
        {
            return Grid.DataSource as DataTable;
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="TVal">值</param>
        /// <returns><see cref="string"/></returns>
        public static string TypeToStr<T>(this T TVal)
        {
            if (TVal == null || TVal is DBNull)
            {
                return string.Empty;
            }

            return TVal.ToString();
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="TVal">值</param>
        /// <returns><see cref="int"/></returns>
        public static int TypeToInt<T>(this T TVal)
        {
            if (TVal == null || TVal is DBNull)
            {
                return -1;
            }

            if (int.TryParse(TypeToStr(TVal), out int Result))
            {
                return Result;
            }
            return -1;
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="TVal">值</param>
        /// <returns><see cref="bool"/></returns>
        public static bool TypeToBoolean<T>(this T TVal)
        {
            if (TVal == null || TVal is DBNull)
            {
                return false;
            }

            if (bool.TryParse(TypeToStr(TVal), out bool Result))
            {
                return Result;
            }
            return false;
        }
    }
}

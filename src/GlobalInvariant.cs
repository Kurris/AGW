using AGW.Base.Helper;
using System;
using System.Collections.Generic;
using System.Data;

/* function: Global  parameter
 * Date:2020 06 12
 * Creator:  ligy  
 *
 * Data                                     Modifier                                    Details
 * 
 * 
 *
 *************************************************************************************************************************/

namespace AGW.Base
{
    public class GlobalInvariant
    {
        /// <summary>
        /// 语言缓存
        /// </summary>
        private static Dictionary<string, string> _mdicLanguage = null;

        /// <summary>
        /// 当前语言
        /// </summary>
        public static LanguageType Language { get; internal set; }

        /// <summary>
        /// 语言枚举
        /// </summary>
        public enum LanguageType
        {
            SimplifiedChinese = 0,
            TraditionalChinese = 1,
            English = 3
        }

        /// <summary>
        /// 获取当前Key的语言
        /// </summary>
        /// <param name="Key">Key</param>
        /// <param name="ReLoad">是否需要重新初始化语言缓存</param>
        /// <returns>翻译语言</returns>
        public static string GetLanguageByKey(string Key, bool ReLoad = false)
        {
            try
            {
                if (_mdicLanguage == null)
                {
                    InitializeLanguageCacha(ReLoad);
                }
                if (_mdicLanguage.ContainsKey(Key))
                {
                    return _mdicLanguage[Key];
                }
                return Key;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Initialize datatable
            void InitializeLanguageCacha(bool ReLoadlanguage)
            {
                try
                {
                    string sColName = string.Empty;

                    switch (Language)
                    {
                        case LanguageType.SimplifiedChinese:
                            sColName = "fCns";
                            break;
                        case LanguageType.TraditionalChinese:
                            sColName = "fCns";//待定
                            break;
                        case LanguageType.English:
                            sColName = "fEng";
                            break;
                        default:
                            break;
                    }
                    if (ReLoadlanguage)
                    {
                        if (_mdicLanguage != null)
                        {
                            _mdicLanguage.Clear();
                            _mdicLanguage = null;
                        }
                    }

                    if (_mdicLanguage == null)
                    {
                        _mdicLanguage = new Dictionary<string, string>(1000);
                    }

                    DataTable dtLanguage = DBHelper.GetDataTable($"select fkey,{sColName} from T_language with(nolock)");

                    if (dtLanguage == null) throw new NotImplementedException("Initialize Language Wrong!");

                    foreach (DataRow dr in dtLanguage.Rows)
                    {
                        string sKey = dr["fKey"] + "";
                        string sValue = dr[sColName] + "";

                        if (_mdicLanguage.ContainsKey(sKey)) continue;

                        _mdicLanguage.Add(sKey, sValue);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public UserInfo UserInfo { get; internal set; }
    }
}

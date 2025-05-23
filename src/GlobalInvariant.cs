﻿using AGW.Base.Helper;
using Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AGW.Base
{
    /// <summary>
    /// 全局帮助类
    /// </summary>
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
                            sColName = "Cns";
                            break;
                        case LanguageType.TraditionalChinese:
                            sColName = "Tns";//待定
                            break;
                        case LanguageType.English:
                            sColName = "Eng";
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

                    var languages = DBHelper.Db.Queryable<LanguageEntity>().ToList();

                    foreach (var dr in languages)
                    {
                        string sKey = dr.Key;
                        string sValue = (string)typeof(LanguageEntity).GetProperties().FirstOrDefault(x => x.Name == sColName).GetValue(dr);

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


        /// <summary>
        /// 用户信息
        /// </summary>
        public static UserInfo User { get; internal set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public class UserInfo
        {
            /// <summary>
            /// 用户代号
            /// </summary>
            public string UserCode { get; internal set; }

            /// <summary>
            /// 用户名称
            /// </summary>
            public string UserName { get; internal set; }

            /// <summary>
            /// 是否为管理员
            /// </summary>
            public bool IsAdmin { get; internal set; }

            /// <summary>
            /// 登录时间
            /// </summary>
            public DateTime LoginTime { get; internal set; }
        }
    }


}

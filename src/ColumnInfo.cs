namespace AGW.Base
{
    /// <summary>
    /// 列信息
    /// </summary>
    public class ColumnInfo
    {
        public string ColumnName { get; internal set; }

        public string DefaultValue { get; internal set; }

        public bool Visible { get; internal set; }

        public bool ReadOnly { get; internal set; }

        public string Translation { get; internal set; }

        public string DataPropertyName { get; internal set; }

        public int Num { get; internal set; }
    }
}

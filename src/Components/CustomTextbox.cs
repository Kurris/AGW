using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AGW.Base.Components
{
    /// <summary>
    /// 数据容器
    /// </summary>
    [ToolboxItem(false)]
    public class CustomTextBox : TextBox, IUserInput
    {
        public object GetValue()
        {
            return this.Text;
        }
    }
}

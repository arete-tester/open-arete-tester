using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace AreteTester.Actions
{
    public class DatabaseActionEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            DatabaseAction action = (DatabaseAction)value;
            using (DatabaseConfigForm form = new DatabaseConfigForm())
            {
                form.DatabaseAction = action;
                svc.ShowDialog(form);
            }
            return action;
        }
    }
}

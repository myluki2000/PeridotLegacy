using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using PeridotEngine.Engine.Resources;

namespace PeridotEngine.Engine.Editor.Forms.PropertiesForm
{
    class TextureEditor : UITypeEditor
    {
        public static string TextureDirectory;

        /// <inheritdoc />
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService editorService && value is TextureDataBase tex)
            {
                using TextureSelectionForm form = new TextureSelectionForm(TextureDirectory);
                if (editorService.ShowDialog(form) == DialogResult.OK)
                {
                    return form.SelectedTexture;
                }
            }

            return value;
        }

        /// <inheritdoc />
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}

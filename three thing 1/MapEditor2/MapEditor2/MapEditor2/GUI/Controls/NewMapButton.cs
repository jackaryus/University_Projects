using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MapEditor2.GUI.Controls
{
    //======================================================================================================================
    //
    //======================================================================================================================
    public class NewMapButton : Button
    {
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public NewMapButton(int x, int y, int width, int height, string label, Containers.Panel parent, Editor editor)
            : base(x, y, width, height, label, parent, editor)
        {
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public override void OnTrigger()
        {
            base.OnTrigger();
            GUI.Forms.NewMapForm form = new GUI.Forms.NewMapForm();
            form.ShowDialog();

            if (form.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this.editor.SetupMap(form.MapWidth, form.MapHeight);
            }
            form.Dispose();
        }
    }
}

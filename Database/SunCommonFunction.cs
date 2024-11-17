using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.SunTech
{
    public class SunCommonFunction
    {
        public static void EmptyTextBoxes(Control parent)
        {
            // Loop through all the controls on the page
            foreach (Control c in parent.Controls)
            {
                // Check and see if it's a textbox
                if ((c.GetType() == typeof(TextBox)))
                {
                    // Since its a textbox clear out the text       
                    ((TextBox)(c)).Text = "";
                }
                // Now we need to call itself (recursive) because
                // all items (Panel, GroupBox, etc) is a container
                // so we need to check all containers for any
                // textboxes so we can clear them
                if (c.HasControls())
                {
                    EmptyTextBoxes(c);
                }
            }



        }




      

        public static void ErrorMessage(Control Parent, string Message)
        {
            foreach (Control c in Parent.Controls)
            {
                if ((c.GetType() == typeof(Label)))
                {
                    if (((Label)(c)).ID.ToUpper() == "LABERROR")
                    {
                        ((Label)(c)).ForeColor = Color.Red;
                        ((Label)(c)).Text = Message;
                        break;
                    }
                }
                if (c.HasControls())
                {
                    ErrorMessage(c, Message);
                }

            }
        }



    }


}
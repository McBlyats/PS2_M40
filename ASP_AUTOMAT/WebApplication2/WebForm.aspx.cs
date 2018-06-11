using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string nrBinar = System.IO.File.ReadAllText(@"C:\Users\Vasile-Adrian TORJA\Desktop\ASP_AUTOMAT\WebApplication2\data.txt");
            Textbox1.Text = nrBinar;
            byte[] array = Encoding.ASCII.GetBytes(nrBinar);  //sa converteasca stringu in byte
        }
    }
}
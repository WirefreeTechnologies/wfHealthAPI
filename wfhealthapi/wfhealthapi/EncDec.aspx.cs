using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wfhealthapi.Classes;

namespace wfhealthapi
{
    public partial class EncDec : System.Web.UI.Page
    {
        EncryptDecrypt ut = new EncryptDecrypt();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Label1.Text = ut.Decrypt(TextBox1.Text);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Label1.Text = ut.Encrypt(TextBox1.Text);
        }
    }
}
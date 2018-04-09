using System;
using System.Web;
using System.Web.UI;

namespace TWAPI
{
	
	public partial class Default : System.Web.UI.Page
	{
		string text = "test";
		public void button1Clicked (object sender, EventArgs args)
		{
			button1.Text = "You clicked me";
			button1.ToolTip = text;
		}
	}
}


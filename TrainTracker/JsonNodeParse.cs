/*
 * Created by SharpDevelop.
 * User: RS
 * Date: 11/7/2015
 * Time: 6:20 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TrainWang
{
	/// <summary>
	/// Description of XmlNodeParse.
	/// </summary>
	public abstract class JsonNodeParse
	{
		protected bool isBlank = false;
		public bool hasRowentry = false;
		/*public virtual string ParseNode( XmlNode node, XmlNamespaceManager manager )
		{
			throw( new Exception("Error: not implemented"));
			//return "";
		}
		public virtual string ParseNode( XmlNode node, XmlNamespaceManager manager, NodeEntry rowentry )
		{
			throw( new Exception("Error: not implemented"));
			//return "";
		}*/
	}
}

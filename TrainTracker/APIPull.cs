/*
 * Created by SharpDevelop.
 * User: RS
 * Date: 10/25/2015
 * Time: 6:24 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace TrainTracker
{
	/// <summary>
	/// Description of APIPull.
	/// </summary>

	public abstract class APIPull
	{
		protected string httpurl;
		protected string key;
		protected string schemaurl;
		protected string apikey;
		protected Boolean defaultadd;
		protected string currenttable;
		protected string parameterstring;
		protected string reportpath;

		public virtual void DoPull()
		{

		}



		/*public void NodeToTable( XmlNode node, XmlNamespaceManager manager, NodeEntry keyexcept, String dbname, Dictionary<string, NodeEntry> nodedict )
		{
			foreach( XmlNode subnode in node.ChildNodes )
			{
				IEnumerable<NodeEntry> nodeentries = exceptionlist.Where( x => x.Name == subnode.Name );
				foreach (var nodeentry in nodeentries)  //do foreach so can have multiple entries per xml node.
				{
					if( !nodeentry.skip )
					{
						nodeentry.NodeParse( subnode, manager, keyexcept );
						if( !nodeentry.skipadd )
						{
							nodedict.Add( nodeentry.SqlName, nodeentry );
						}
					}
				}
				if( !nodeentries.Any() )
				{
					nodedict.Add(subnode.Name, new NodeEntry( subnode ) );
				}
			}
			DBQuery query = new DBQuery( dbname );
			query.AddToTable( nodedict, keyexcept, dbname, currenttable );
				
		}*/

	

		public void test()
		{
			//DBQuery z = new DBQuery (""); 
			//(new DBQuery("")).test ();
			//Hashtable ht = new Hashtable();
			//Train t = new Train( ht );

			Network net = new Network ( "SEPTA" );

			/*DBQuery test = new DBQuery ("traintracker");

			StoredAttribute keyatt = new SAString ("vehicleID");
			keyatt.Set ("5678");
			List<Hashtable> hl = test.SelectByKey( "train", keyatt );
			/*
			List<Hashtable> objlist = DoWebRequestJson("http://www3.septa.org/beta/TransitView/23");
			//string test = DoWebRequestString("http://www.surilegal.com/");
			foreach (Hashtable attlist in objlist ) {
				Train z = new Train ( attlist );

				//JObject jo2 = jobj2.ToDictionary( x => x == x );
			}
			Console.Beep();*/
		}

	/*	protected XmlDocument DoWebRequestXML( string requestUrl)
		{
			HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;	
			request.ContentType = "application/xml; charset=UTF-8";
			request.Accept = "application/xml";
			WebResponse response = request.GetResponse();
			Stream responsestream = response.GetResponseStream();
			//XmlTextReader reader = new XmlTextReader( responsestream );
			xmldoc = new XmlDocument();
			xmldoc.Load( responsestream );
			//xmldoc.Save(AppDomain.CurrentDomain.BaseDirectory + @"xmlfile.xml");
			//xmldoc.Save(@"C:\Users\RS\Dropbox\Database\xmlfile.xml");
			return xmldoc;
		}*/
		protected List<Hashtable> DoWebRequestJson( string requestUrl )
		{
			HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;	
			//request.ContentType = "application/json; charset=UTF-8";
			request.Accept = "application/json";
			request.Method = "POST";
			WebResponse response = request.GetResponse();
			Stream responsestream = response.GetResponseStream();
			StreamReader reader = new StreamReader(responsestream);
			JsonTextReader tread = new JsonTextReader (reader);

			List<Hashtable> jsonlist = JsonReaderToList (tread);
			//JObject obj = (JObject)serializer.Deserialize( tread );

			return jsonlist;
		}

		protected abstract List<Hashtable>  JsonReaderToList( JsonReader jreader );

		protected void LoadAPIkey( string path )
		{
			apikey = "";
			if (File.Exists(path))
			{
				apikey = File.ReadAllText( path );
			}
		}
		public virtual void SetupExceptions() 
		{
		}
		protected virtual void RunReport()
		{
			System.Diagnostics.Process.Start(reportpath);
		}


	};
}

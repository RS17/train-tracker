using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Globalization;

namespace TrainTracker
{
	public class Util
	{

		public Util ()
		{
		}

		/// <summary>
		/// Converts the data table to hash tables.
		/// </summary>
		/// <returns>The data table to hash tables.</returns>
		/// <param name="dt">Dt.</param>
		public static List<Hashtable> convertDataTableToHashTables(DataTable dt )   
		{    
			List<Hashtable> hashlist = new List<Hashtable>(); 

			foreach(DataRow drIn in dt.Rows)    
			{
				hashlist.Add (Util.convertDataRowToHashTable (drIn));
			}   
			return hashlist;    
		}

		public static Hashtable convertDataRowToHashTable(DataRow dr )   
		{    
			Hashtable ht = new Hashtable(); 
			DataTable dt = dr.Table;

			foreach (DataColumn c in dt.Columns) {
				ht.Add ( c.ToString(), dr [c] );
			}
			 
			return ht;    
		}

		public static string TitleCase( string str )
		{
			TextInfo ti = new CultureInfo("en-US", false).TextInfo;
			return ti.ToTitleCase (str);
		}
	}
}


/*
 * Created by SharpDevelop.
 * User: RS
 * Date: 11/13/2015
 * Time: 8:07 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Collections.Generic;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace TrainTracker
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class DBQuery
	{
		MySqlConnection connection;
		public void test ()
		{
			this.GetConnString ();
		}
		protected string GetConnString()
		{
			string path = AppDomain.CurrentDomain.BaseDirectory + "sqlconn.txt";
			string connstr = "";
			if (File.Exists(path))
			{
				connstr = File.ReadAllText( path );
			}
			connstr = "Server=localhost;" + "Database=traintracker;" + "User ID=ravi;" + "Password=;" + "Pooling=false";
			return connstr;
		}

		public DBQuery( string dbname )
		{
			// connection probles: 
			// no default driver etc - make sure maria odbc connector is installed, follow directions from mariadb site
			// cant connect to local mysql server through socket /tmp/mysql.sock - check that this file exists and links to ln -s /var/run/mysqld/mysqld.sock /tmp/mysql.sock
			String connectionstring = GetConnString();
			connection = new MySqlConnection( connectionstring );
			connection.Open();
			UseDB( dbname );
		}

		private void DoQuery( string query )
		{
		//	SQLI risk - do not use in exposed methods
			MySqlCommand sqlcmda = new MySqlCommand( query, connection );
			sqlcmda.ExecuteNonQuery();
		}

		private void DoQuery( MySqlCommand query )
		{
			query.ExecuteNonQuery();
		}

		public void TestQuery()
		{

		}

		/// <summary>
		/// Select by key overload
		/// </summary>
		/// <returns>Hashtable of results</returns>
		/// <param name="tablename">Tablename.</param>
		/// <param name="keyatt">Keyatt.</param>
		public List<Hashtable> Select( string tablename, StoredAttribute keyatt  )
		{
			return Select (tablename, keyatt, false);
		}

		/// <summary>
		/// SelectAll overload
		/// </summary>
		/// <returns>Hash table of results</returns>
		/// <param name="tablename">Tablename.</param>
		public List<Hashtable> Select( string tablename )
		{
			SAInt keyatt = new SAInt ("null");
			return Select (tablename, keyatt, true);
		}

		protected List<Hashtable> Select( string tablename, StoredAttribute keyatt, bool selectall  )
		{
			MySqlDataReader myReader;
			DataTable dt = new DataTable();
			List<Hashtable> hashlist = new List<Hashtable>();
			try
			{
				string commandstr = "SELECT * FROM " + tablename;
				if( !selectall )
				{
					commandstr += " WHERE " + keyatt.name + "= ? ";
				}
				
				MySqlCommand odbccommand = new MySqlCommand( commandstr, connection );
					
				if( !selectall )
				{
					odbccommand.Parameters.Add( new MySqlParameter( @"@"+  keyatt.name, keyatt.type ) ).Value = keyatt.GetString();
				}

				// load to dt
				myReader = odbccommand.ExecuteReader();
				dt.Load( myReader );

				// actually let's make it a list of hashtables
				hashlist = Util.convertDataTableToHashTables( dt );
				myReader.Close();
			
			}
			catch( Exception ex )
			{
				throw new Exception( "Error with key value " + keyatt.GetString() + " for column " + keyatt.name + ": " + ex.Message );
			}
				
			// I'll deal with the connection later if I remember.  I probably won't remember.  
			myReader.Close ();

			return hashlist;

		}
		public void CreateTableIfNotExists( StoredObject sobj )
		{
			DoQuery( "CREATE TABLE IF NOT EXISTS "
				+ sobj.tablename
				+ " ("
				+ sobj.key.name
				+ " " 
				+ sobj.key.type +"("+sobj.key.size +")"
				+ " NOT NULL PRIMARY KEY )" 
			);
		}

		public void DeleteFromTable( StoredObject sobj )
		{
			DoQuery( "DELETE FROM " + sobj.tablename + " WHERE " + sobj.key.name + " = " + sobj.key.GetString() );
		}

		public void AddColumns( string tablename, List<StoredAttribute> attlist )
		{
			foreach ( StoredAttribute entry in attlist ) 
			{
				string qstring = "ALTER TABLE " + tablename + " ADD COLUMN " + entry.name + " " + entry.type;
				if( entry.type.ToLower() == OdbcType.VarChar.ToString().ToLower() )
				{
					qstring += "("+ entry.size+ ")";
				}
				try
				{
					DoQuery( qstring );
				}
				catch( Exception ex )
				{
					if( !ex.Message.Contains("Duplicate") )
					{
						throw( new Exception( "Error altering table column: " + tablename +"."+entry.name+" " + ex.Message ) );
					}
					//no IGNORE statement that works for add column, so skip duplicate errors
				}
			}
		}
		public void InsertKeyRow( string tablename, StoredAttribute keyentry )
		{
			try
			{

				MySqlCommand odbccommand = new MySqlCommand( "INSERT IGNORE INTO " 
					+ tablename 
					+ " (" 
					+ keyentry.name 
					+ ") VALUES ( ? )" , connection );
				odbccommand.Parameters.Add( new MySqlParameter( @"@"+keyentry.name, keyentry.type ) ).Value = keyentry.GetString();
				DoQuery( odbccommand );

			}
			catch( Exception ex )
			{
				throw new Exception( "Error with key value " + keyentry.GetString() + " for column " + keyentry.name + ": " + ex.Message );
			}
		}
	
		public void UpdateRow( string tablename, StoredAttribute keyentry, List<StoredAttribute> attlist )
		{
			string eqstring = "";
			attlist.Remove( keyentry );
			foreach( StoredAttribute att in attlist )
			{
				if( eqstring.Length > 0 )
				{
					eqstring = eqstring + ",";
				}
				eqstring = eqstring + att.name + "= ? ";
			}
			try
			{
				MySqlCommand odbccommand = new MySqlCommand( "UPDATE " + tablename + " SET " + eqstring + " WHERE " + keyentry.name + "= ?", connection );
				foreach( StoredAttribute att in attlist )
				{
					odbccommand.Parameters.Add( new MySqlParameter( @"@"+att.name, att.type ) ).Value = att.GetString();
				}
				//odbccommand = AddParameters( odbccommand, rowdict );
				//odbccommand.Parameters.AddWithValue( "@sqlname", keyentry.SqlName );
				//odbccommand.Parameters.AddWithValue( "?keyval", keyentry.value );
				odbccommand.Parameters.Add( new MySqlParameter( @"@"+keyentry.name, keyentry.type ) ).Value = keyentry.GetString();
				DoQuery( odbccommand );
			}
			catch( Exception ex )
			{
				throw new Exception( "Error with updating row " + keyentry.ToString() + " in table " + tablename + ": " + ex.Message );
			}
		}
	
		private void UseDB( string dbname )
		{
			DoQuery( "USE " + dbname );
		}
		protected void SelectEqual( string tablename, string column, string equal )
		{
			DoQuery( "SELECT " + column + " WHERE " + column + " = " + equal );
		}
		public void AddToTable(  List<StoredAttribute> attlist, StoredAttribute key, string tablename )
		{
			//probably obsolete
			//if( isfirstnode )
			//{
			//need to add columns for each row, because could be new column after first group
			AddColumns( tablename, attlist );
			//}
			InsertKeyRow( tablename, key );
			UpdateRow( tablename, key, attlist );
		}



		public void Close()
		{
			this.connection.Close ();
		}

		/*public void AddToTable( Dictionary< string, NodeEntry > nodedict, NodeEntry keyexception, string dbname, string tablename )
		{
			//probably obsolete
			//if( isfirstnode )
			//{
			//need to add columns for each row, because could be new column after first group
			AddColumns( tablename, nodedict );
			//}
			InsertKeyRow( tablename, keyexception );
			UpdateRow( tablename, keyexception, nodedict );
		}
		public void AddToTable( Dictionary< string, NodeEntry > nodedict, List<NodeEntry> keyexceptions, string dbname, string tablename )
		{

			//if( isfirstnode )
			//{
			//need to add columns for each row, because could be new column after first group
			AddColumns( tablename, nodedict );
			//}
			InsertKeyRow( tablename, keyexceptions );
			UpdateRow( tablename, keyexceptions, nodedict );
		}
		public void AddToTable( Dictionary< string, NodeEntry > nodedict, string dbname, string tablename )
		{

			//if( isfirstnode )
			//{
			//need to add columns for each row, because could be new column after first group
			AddColumns( tablename, nodedict );
			InsertFullRow( tablename, nodedict );
		}
		public void SelectOrCreateRow()
		{
			//this will be used for M2M tables - get row with key or create it.
		}
*/
	}
}

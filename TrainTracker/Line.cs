using System;
using System.Collections;

namespace TrainWang
{

	/// <summary>
	/// THIS WILL PROBABLY NOT BE USED - Lines are for passenger understanding, not useful in system
	/// </summary>
	public class Line : StoredObject
	{
		protected string name = new SAString( "name" );
		protected Hashtable stations = new Hashtable();
		public Line () : base ("line")
		{
			// create stations if not exist

		}
	}
}


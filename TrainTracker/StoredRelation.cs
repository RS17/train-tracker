using System;
using System.Collections.Generic;

namespace TrainTracker
{
	public class StoredRelation
	{
		bool IsOwning = false;
		StoredObject leftside;
		List<StoredObject> rightside;

		public StoredRelation ()
		{
		}

		public void Delete()
		{
			// delete other object if owning
			if (IsOwning) {
				foreach (StoredObject obj in rightside) {
					obj.Delete ();
				}
			}
		}
	}
}


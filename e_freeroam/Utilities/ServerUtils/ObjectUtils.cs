using e_freeroam.Server_Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_freeroam.Utilities.ServerUtils
{
	public abstract class ObjectUtils
	{
		public static List<object> generateListOfNull(ushort listSize)
		{
			List<object> list = new List<object>(listSize);
			for(ushort i = 0; i < listSize; i++) list.Insert(i, null);
			return list;
		}

		public static List<object> removeObjectFromList(List<object> list, ushort id, ushort nullValue, ushort length) // 'Length' should not be inclusive of the highest index while 'nullValue' should be the MaxValue of the datatype used to index the list.
		{
			if(id >= length) return list;

			object newObject = null;
			ushort firstAvailID = id;
			for(ushort i = (ushort) (id + 1); i < length; i++) // Iterated over the list from index 'id' to non-inclusive 'length'.
			{
				newObject = list[i]; // Get list member at index.
				if(newObject == null) if(firstAvailID == nullValue) firstAvailID = i; // If we haven't already found the next free slot, we have now.
				else if(firstAvailID != nullValue)
				{
					list.Insert(firstAvailID, list[i]); // The earliest free slot is set to the value proceeding 'filled' one.
					list.Insert(i, null); // This slot becomes free.

					if(list[firstAvailID] is Business) // Object specific instructions.
					{
						Business bus = (Business) list[firstAvailID];
						bus.updateBusinessID((byte) firstAvailID); // Delete/Create necessary Business files.
					}

					i = firstAvailID; // Index set to the slot we just filled; the next iteration will be firstAvailID+1 and seach the rest of the list for values gapped by 'null'.
					firstAvailID = nullValue; // Set 
				}
			}

			if(firstAvailID == id) list.Insert(id, null);
			return list;
		}
	}
}

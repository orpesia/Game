using UnityEngine;
using System.Collections;

namespace UnnamedUtility
{

	public class Multiple
	{
		System.Object[] m_objects;
		int m_offset;
		public Multiple(int count)
		{
			m_objects = new System.Object[count];
		}

		public T At<T>( int index )
		{
			return (T)m_objects[index];
		}

		public void Set( int index, System.Object obj )
		{
			m_objects[index] = obj;
		}

		public T First<T>()
		{
			m_offset = 0;
			return this.At<T>(m_offset++);
		}
		public T Next<T>()
		{
			return this.At<T>(m_offset++);
		}

	}


}
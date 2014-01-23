using System;
using System.Collections.Generic;

using UnityEngine;

namespace BoardEditor
{
	using BatchContainer = List< List<Vector2> >;

	public class BoardShape
	{
		private static float WidthTerm = 0.75f;
		private static float HeightTerm = 0.435f;
		public static float Width = 1.0f;
		public static float Height = 0.87f;

		public class Data
		{
			public Vector2 innerSize = Vector2.zero;
			public BatchContainer container = new BatchContainer();
		};

		public BoardShape ()
		{
			
		}

		static public Data Batch(Vector2 size, float pixel )
		{
			Data data = new Data();

			float width = pixel * Width;
			float widthTerm = pixel * WidthTerm;
			float height = pixel * Height;
			float heightTerm = pixel * HeightTerm;

			float advanceX = width * 0.5f; //센터.
//			int advanceXCount = 0;

			while( advanceX + width < size.x )
			{
//				Debug.Log (advanceX);

				List<Vector2> vertical = new List<Vector2>();

				float advanceY = HeightTerm + height * 0.5f;
				float additionY = data.container.Count % 2 == 0 ? 0 : heightTerm;

				while( advanceY + height + additionY < size.y )
				{
					vertical.Add ( new Vector2(advanceX, advanceY + additionY ) );
					advanceY += height;
				}

				data.container.Add ( vertical );

				advanceX += widthTerm;
//				advanceX += widthTerm;
//				++advanceXCount;

				data.innerSize.x = Math.Max (advanceX, data.innerSize.x );
				data.innerSize.y = Math.Max (advanceY, data.innerSize.y );

			}

			return data;
		}
	}
}


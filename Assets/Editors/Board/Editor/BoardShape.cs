using System;
using System.Collections.Generic;

using UnityEngine;

using Game.Data;

namespace BoardEditor
{
	public class BoardShape
	{
		private static float WidthTerm = 0.75f;
		private static float HeightTerm = 0.435f;
		public static float Width = 1.0f;
		public static float Height = 0.87f;

		public class Data
		{
			public Vector2 innerSize = Vector2.zero;
			public BoardData.BlockList container = new BoardData.BlockList();
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

			while( advanceX + width < size.x )
			{

				BoardData.BlockElem vertical = new BoardData.BlockElem();

				float advanceY = HeightTerm + height * 0.5f;
				float additionY = data.container.X.Count % 2 == 0 ? 0 : heightTerm;

				while( advanceY + height + additionY < size.y )
				{
					BlockProperty property = new BlockProperty();
					property.Position = new Vector2(advanceX, advanceY + additionY);
					vertical.Y.Add ( property );
					advanceY += height;
				}

				data.container.X.Add ( vertical );

				advanceX += widthTerm;

				data.innerSize.x = Math.Max (advanceX, data.innerSize.x );
				data.innerSize.y = Math.Max (advanceY, data.innerSize.y );

			}

			return data;
		}
	}
}


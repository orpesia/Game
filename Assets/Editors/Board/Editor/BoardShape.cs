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

		static public Data Batch(BoardData boardData)
		{
			Vector2 size = boardData.BoardSize;
			float pixel = boardData.Pixel;

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

			Backup(data, boardData);

			return data;
		}

		public static void Backup(Data data, BoardData boardData)
		{
			if( null != boardData.BlockProperties )
			{
				for( int x = 0; x < data.container.X.Count; ++x )
				{
					for( int y = 0; y < data.container[x].Y.Count; ++y )
					{
						if( x >= boardData.BlockProperties.X.Count || y >= boardData.BlockProperties[x].Y.Count )
						{
							continue;
						} 
						
						data.container[x,y].Generate = boardData.BlockProperties[x,y].Generate;
						data.container[x,y].ItemType = boardData.BlockProperties[x,y].ItemType;
						data.container[x,y].Durable = boardData.BlockProperties[x,y].Durable;
					}
				}
			}
		}
	}
}


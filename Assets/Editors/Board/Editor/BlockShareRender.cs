
using Game;
using Game.Data;

using Unnamed;

using UnityEngine;
using UnityEditor;

namespace BoardEditor
{
	public class BlockShareRender
	{
		public const uint RENDER_BLANK = 0x00000001;
		public const uint RENDER_BLOCK = 0x00000002;


		public BlockDataSet DataSet;
		public Vector2 StartPosition;
		public float Scale;

		public const float FixedWidth = 190;
		public float FixedHeight = 70;
		public uint RenderFlag = RENDER_BLANK | RENDER_BLOCK;

		float m_tint = 5;
		float m_buttonInterval = 15;
		float m_heightInterval = 10;
		float m_buttonSize = 50;


		System.Action<GenerateType> m_callback;
		BlockRenderer m_renderer = new BlockRenderer();

		public BlockShareRender(BlockDataSet dataset, Vector2 startPosition, float scale, System.Action<GenerateType> callback)
		{
			DataSet = dataset;

			StartPosition = startPosition;
			Scale = scale;
			m_callback = callback;
			m_tint *= Scale;
			m_buttonInterval *= Scale;
			m_heightInterval *= Scale;
			m_buttonSize *= Scale;

			m_renderer.Load (dataset);
		}

		public Rect Draw(EditorWindow window)
		{
			window.BeginWindows();
			Rect rect = this.Draw ();
			window.EndWindows();

			return rect;
		}

		public Rect Draw()
		{
			Rect windowPosition = new Rect(StartPosition.x, StartPosition.y, FixedWidth * Scale, FixedHeight * Scale );
			 
			if( 0 != ( RENDER_BLANK & RenderFlag ) )
			{
				GUI.Window (0, windowPosition, Blank, "Blank" );
				windowPosition.y += windowPosition.height + m_heightInterval;
			}

			if( 0 != ( RENDER_BLOCK & RenderFlag ) )
			{
				GUI.Window ((int)GenerateType.TypeA_A, windowPosition, Blocks, "AType" );

				windowPosition.y += windowPosition.height + m_heightInterval;
				GUI.Window ((int)GenerateType.TypeB_A, windowPosition, Blocks, "BType" );

				windowPosition.y += windowPosition.height + m_heightInterval;
				GUI.Window ((int)GenerateType.TypeC_A, windowPosition, Blocks, "CType" );

				windowPosition.y += windowPosition.height + m_heightInterval;
				GUI.Window ((int)GenerateType.TypeD_A, windowPosition, Blocks, "DType" );

				windowPosition.y += windowPosition.height + m_heightInterval;
				GUI.Window ((int)GenerateType.TypeE_A, windowPosition, Blocks, "EType" );
			}

			windowPosition.y += windowPosition.height;

			return windowPosition;
		}
		                 
		void Blank(int id) 
		{
			if( ImageButton(new Rect( m_tint, 17 * Scale, m_buttonSize, m_buttonSize ), "Blank", GenerateType.Blank) )
			{
				m_callback(GenerateType.Blank);
			}
		}
		
		void Blocks( int blockType )
		{
			string[] subName = { "SubA", "SubB", "SubC" };
			
			for( int subIndex = 0; subIndex < BlockConst.SubCount; ++subIndex )
			{
				GenerateType generateType = (GenerateType)(blockType + subIndex);

				//15는 사이간격.
				float left = m_tint + ( subIndex * m_buttonInterval ) + ( subIndex * m_buttonSize);
				if( ImageButton(new Rect( left, 17 * Scale, m_buttonSize, m_buttonSize ), subName[subIndex], generateType) )
				{
					m_callback(generateType);
				}
			}
		}

		bool ImageButton(Rect pos, string name, GenerateType generate )
		{
			bool isPress = GUI.Button (pos, name );

			m_renderer.Draw (pos, generate);
			
			return isPress;
		}

	}
}


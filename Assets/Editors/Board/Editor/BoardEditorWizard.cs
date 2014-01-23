using System;

using UnityEngine;
using UnityEditor;

using UnnamedUtility;
using Game;

namespace BoardEditor
{

	public class BoardEditorWizard : ScriptableWizard 
	{
		[MenuItem("Assets/" + Named.Title + "/Create Board")]
		static void WizardCreate()
		{
			BoardEditorWizard boardWizard = ScriptableWizard.DisplayWizard<BoardEditorWizard>("BoardEditor");
			boardWizard.minSize = boardWizard.maxSize = new Vector2( FixedSize.Width + (MenuWidth * MENU_COUNT), FixedSize.Height );
		}

		private static int MENU_COUNT = 2;
		private const float MenuWidth = 150.0f;

		private UnnamedObject.HexagonVertex m_vertex = new UnnamedObject.HexagonVertex(BoardShape.Width, BoardShape.Height);
		private BoardShape.Data m_boardData = null;
		private float pixel = 150;

		void OnWizardUpdate()
		{

		}

		void OnFocus()
		{
			m_boardData = BoardShape.Batch(new Vector2( 1280, 720 ), pixel );
		}

		void OnGUI()
		{

			this.DrawBoard();
			GUI.WindowFunction[] functions 	= new GUI.WindowFunction[]{ BoardWindow, TextureWindow };
			string[] funcNames 				= { "Board", "Texture" };
			
			GUIStyle style = GUI.skin.GetStyle("Window");
			GUIStyleState backgupState = style.onNormal;
			style.onNormal = style.normal;

			BeginWindows();

			for( int i = 0; i < MENU_COUNT; ++i )
			{
				GUILayout.Window(i, new Rect( FixedSize.Width + MenuWidth * i, 0, MenuWidth, FixedSize.Height ), functions[i], funcNames[i], style );
			}

			EndWindows();

			style.onNormal = backgupState;
		}

		public void DrawBoard()
		{
			if( null == m_boardData )
			{
				return ;
			}

			Vector2[] vertices = m_vertex.ScaleUP( pixel ); 
			//Drawpoly 할라면 맨 뒤에 앞을 연결해 줘야 한다.
			vertices[0] = vertices[vertices.Length - 1];
			
			for( int x = 0; x < m_boardData.container.Count; ++x )
			{
				for( int y = 0; y < m_boardData.container[x].Count; ++y )
				{
					Vector2[] sumVector = vertices.SumVector ( m_boardData.container[x][y] );
					Handles.DrawPolyLine( sumVector.ConvertVector3() );

				}
			}
		}

		public void BoardWindow( int id )
		{
			GUILayout.Button ("AA");
		}

		public void TextureWindow( int id )
		{
			GUILayout.Button ("AA");
		}
	}

}
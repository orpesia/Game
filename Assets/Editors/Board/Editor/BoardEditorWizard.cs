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

			boardWizard.m_boardSize.x = BoardEditorSettingWizard.GetHexDefWidth();
			boardWizard.m_boardSize.y = BoardEditorSettingWizard.GetHexDefHeight();
			boardWizard.m_blockPixel = BoardEditorSettingWizard.GetHexDefPixel();
		}

		private static int MENU_COUNT = 2;
		private const float MenuWidth = 200.0f;
		private const float InnerWidth = 190.0f;
		private const float InnerHeight = 20.0f;

		//board
		private UnnamedObject.HexagonVertex m_vertex = new UnnamedObject.HexagonVertex(BoardShape.Width, BoardShape.Height);
		private BoardShape.Data m_boardData = null;

		private Vector2 m_boardSize = Vector2.zero;
		private float m_blockPixel;

		//block
		private Vector2 m_scrollbar; 

		void OnWizardUpdate()
		{

		}

		void OnFocus()
		{
//			m_boardData = BoardShape.Batch(new Vector2( 1280, 720 ), pixel );
		}

		void OnGUI()
		{

			this.DrawBoard();
			GUI.WindowFunction[] functions 	= new GUI.WindowFunction[]{ BoardWindow, BlockWindow };
			string[] funcNames 				= { "Board", "Block" };
			
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
			Color prevColor = Handles.color;
			Handles.color = Color.cyan;
			Handles.DrawLine( new Vector2( FixedSize.Width, 0 ), new Vector2( FixedSize.Width, FixedSize.Height ) );
			Handles.color = prevColor;

			if( null == m_boardData )
			{
				return ;
			}

			Vector2[] vertices = m_vertex.ScaleUP( m_blockPixel ); 
			//Drawpoly 할라면 맨 뒤에 앞을 연결해 줘야 한다.
			vertices[0] = vertices[vertices.Length - 1];
			
			for( int x = 0; x < m_boardData.container.Count; ++x )
			{
				for( int y = 0; y < m_boardData.container[x].Count; ++y )
				{
					Vector2[] sumVector = vertices.SumVector ( m_boardData.container[x][y] );
					Handles.DrawPolyLine( sumVector.ConvertVector3() );

					float halfPixel = m_blockPixel * 0.5f;
					float leftPixel = m_blockPixel * 0.25f;
					if( GUI.Button ( new Rect(m_boardData.container[x][y].x - leftPixel, m_boardData.container[x][y].y - leftPixel, halfPixel, halfPixel ), "B" ) )
					{

					}
				}
			}
		}

		public void BoardWindow( int id )
		{
			if( GUILayout.Button ("Setting", GUILayout.Height (20) ))
			{
				BoardEditorSettingWizard.ShowWizard();
			}

			m_boardSize.x = EditorGUILayout.FloatField("Width", m_boardSize.x, GUILayout.Width (InnerWidth), GUILayout.Height (InnerHeight));
			m_boardSize.y = EditorGUILayout.FloatField("Height", m_boardSize.y, GUILayout.Width (InnerWidth), GUILayout.Height (InnerHeight));
			m_blockPixel = EditorGUILayout.FloatField("Pixel", m_blockPixel, GUILayout.Width (InnerWidth), GUILayout.Height (InnerHeight));

			if( GUILayout.Button ("Generate Board", GUILayout.Height (20)) )
			{
				m_boardData = BoardShape.Batch( m_boardSize, m_blockPixel );
			}
		}

		public void BlockWindow( int id )
		{

			if( GUILayout.Button ("New") )
			{
				BoardEditorTextureWizard.ShowWizard(null, Path.GetSelectionPath());
			}

			GUILayout.Button ("Load");
			GUILayout.Button ("Add");

			EditorGUILayout.BeginVertical();
			m_scrollbar = EditorGUILayout.BeginScrollView(m_scrollbar, GUILayout.Height (250));

			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical();
			m_scrollbar = EditorGUILayout.BeginScrollView(m_scrollbar, GUILayout.Height (250));
			
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}
	}
}









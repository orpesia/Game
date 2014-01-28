using System;

using UnityEngine;
using UnityEditor;

using UnnamedUtility;
using UnnamedResource;
using Game;
using System.Collections.Generic;


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
		private BoardShape.Data m_boardShape = null;

		private Vector2 m_boardSize = Vector2.zero;
		private float m_blockPixel;

		//block
		private Vector2 m_scrollbar;
		private GameData.BoardProperty m_boardProperty = new GameData.BoardProperty();
		private GameData.BlockProperty[,] m_blockProperty;

		void OnWizardUpdate()
		{

		}

		void OnFocus()
		{
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

			if( null == m_boardShape )
			{
				return ;
			}

			Vector2[] vertices = m_vertex.ScaleUP( m_blockPixel ); 
			//Drawpoly 할라면 맨 뒤에 앞을 연결해 줘야 한다.
			vertices[0] = vertices[vertices.Length - 1];
			
			for( int x = 0; x < m_boardShape.container.Count; ++x )
			{
				for( int y = 0; y < m_boardShape.container[x].Count; ++y )
				{
					Vector2[] sumVector = vertices.SumVector ( m_boardShape.container[x][y] );
					Handles.DrawPolyLine( sumVector.ConvertVector3() );

					float halfPixel = m_blockPixel * 0.5f;
					float leftPixel = m_blockPixel * 0.25f;
					if( GUI.Button ( new Rect(m_boardShape.container[x][y].x - leftPixel, m_boardShape.container[x][y].y - leftPixel, halfPixel, halfPixel ), "B" ) )
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
				m_boardShape = BoardShape.Batch( m_boardSize, m_blockPixel );
			}
		}

		public void BlockWindow( int id )
		{
			EditorGUILayout.ObjectField(m_boardProperty.Atlas, typeof(TextureAtlas), true);
			if( GUILayout.Button ("Select Atlas", GUILayout.Height (20) ))
			{
				BoardEditorAtlasSelectorWizard.ShowWizard
				( 
                 	(GameObject select ) => 
          			{ 
						m_boardProperty.SetAtlas(select);
						Repaint ();
					}
				);
			}


			if( GUILayout.Button ("Save Setting", GUILayout.Height(20) ))
			{
				
			}

			if( GUILayout.Button ("Load Setting", GUILayout.Height(20)) )
			{
			}

			if( GUILayout.Button ("Add", GUILayout.Height(20)) )
			{
				BoardEditorSpriteSelectorWizard.ShowWizard
				(
					m_boardProperty.Atlas, 
					(TextureAtlas.Atlas atlas) =>
					{
						m_boardProperty.BlockAtlas.Add ( atlas );
						Repaint ();
					}
				);

			}

			m_scrollbar = EditorGUILayout.BeginScrollView(m_scrollbar, GUILayout.Height (250));

			for( int i = 0; i < m_boardProperty.BlockAtlas.Count; ++i )
			{
				int offset = 7 + ( i * 67 );
				if( GUILayout.Button ("", GUILayout.Width (64), GUILayout.Height ( 64 )))
				{
				}
				GUI.DrawTextureWithTexCoords(new Rect(8, offset, 56, 56 ), m_boardProperty.Atlas.Target, m_boardProperty.BlockAtlas[i].UV );
				GUI.Label(new Rect(74, offset + 20, 100, 56 ), m_boardProperty.BlockAtlas[i].name );
			}
			EditorGUILayout.EndScrollView();
		}
	}
}









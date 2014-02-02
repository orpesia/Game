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
		[MenuItem("Assets/" + Named.Title + "/Board Editor")]
		static void WizardCreate()
		{
			GameObject targetObject = UnityEditor.Selection.activeObject as GameObject;
			if( null == targetObject || null == targetObject.GetComponent<GameData.BoardData>() )
			{
				ShowWizard(null, Path.GetSelectionPath());
			}
			else
			{
				ShowWizard(targetObject, Path.GetSelectionPath());
			}
		}

		private static int MENU_COUNT = 2;
		private const float MenuWidth = 200.0f;
		private const float InnerWidth = 190.0f;
		private const float InnerHeight = 20.0f;

		//board
		private GameObject m_boardPrefab = null;
		private string m_boardName = "Unnamed";
		private string m_savePath = "";
		private UnnamedObject.HexagonVertex m_vertex = new UnnamedObject.HexagonVertex(BoardShape.Width, BoardShape.Height);

		//block
		private Texture2D m_blankTexture;
		private Texture2D m_randomTexture;

		private Vector2 m_scrollbar;
		private GameData.BoardData m_boardData;
//		private GameData.BoardProperty m_boardProperty;
//		private List<List<GameData.BlockProperty>> m_blockProperty;
		private int m_selected = Game.BlockGenerateType.Random.ToInt ();

		public static void ShowWizard( GameObject prefab, string path )
		{
			BoardEditorWizard boardWizard = ScriptableWizard.DisplayWizard<BoardEditorWizard>("BoardEditor");
			boardWizard.minSize = boardWizard.maxSize = new Vector2( FixedSize.Width + (MenuWidth * MENU_COUNT), FixedSize.Height );
			boardWizard.m_savePath = path;
			boardWizard.m_boardPrefab = prefab;

			if( null != prefab )
			{
				boardWizard.m_boardData = boardWizard.m_boardPrefab.GetComponent<GameData.BoardData>();
				boardWizard.m_boardName = boardWizard.m_boardData.name;
			}
		}

		void OnWizardUpdate()
		{

		}

		void OnFocus()
		{
			if( null == m_blankTexture )
			{
				m_blankTexture = AssetDatabase.LoadAssetAtPath( "Assets/Editors/Board/Resource/block_blank.png", typeof(Texture2D ))as Texture2D;
			}

			if( null ==  m_randomTexture )
			{
				m_randomTexture = AssetDatabase.LoadAssetAtPath( "Assets/Editors/Board/Resource/block_rand.png", typeof(Texture2D )) as Texture2D;
			}
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

			if( null == m_boardData || null == m_boardData.BlockProperties )
			{
				return ;
			}

			Vector2[] vertices = m_vertex.ScaleUP( m_boardData.Pixel ); 
			//Drawpoly 할라면 맨 뒤에 앞을 연결해 줘야 한다.
			vertices[0] = vertices[vertices.Length - 1];

			 
			for( int x = 0; x < m_boardData.BlockProperties.Count; ++x )
			{
				for( int y = 0; y < m_boardData.BlockProperties[x].Count; ++y )
				{
					Vector2[] sumVector = vertices.SumVector ( m_boardData.BlockProperties[x,y].Position );
					Handles.DrawPolyLine( sumVector.ConvertVector3() );

					float halfPixel = m_boardData.Pixel * 0.5f;
					float leftPixel = m_boardData.Pixel * 0.25f;
					Rect rect = new Rect(m_boardData.BlockProperties[x,y].Position.x - leftPixel, m_boardData.BlockProperties[x,y].Position.y - leftPixel, halfPixel, halfPixel );
					if( GUI.Button ( rect, "" ) )
					{
						if( Event.current.button == 0 ) //left
						{
							m_boardData.BlockProperties[x,y].GenerateType = m_selected;
						}

						if( Event.current.button == 1 ) //right
						{
						}
					}

					int genType = m_boardData.BlockProperties[x,y].GenerateType;
					Multiple m = this.GetDataByGenerate(genType);
					Texture2D texture = m.At<Texture2D>(1);
					Rect uv = m.At<Rect>(2);

					GUI.DrawTextureWithTexCoords( rect, texture, uv );

				}
			}
		}

		public void BoardWindow( int id )
		{
//			EditorGUILayout.LabelField("저장 경로 : " + m_savePath);

			EditorGUILayout.LabelField(m_savePath);
			if( GUILayout.Button("설정", GUILayout.Height (20) ))
			{
				BoardEditorSettingWizard.ShowWizard();
			}

			if( GUILayout.Button ("설정값 불러오기", GUILayout.Height (20) ))
			{
				if( null != m_boardData )
				{
					m_boardData.BoardSize.x = BoardEditorSettingWizard.GetHexDefWidth();
					m_boardData.BoardSize.y = BoardEditorSettingWizard.GetHexDefHeight();
					m_boardData.Pixel = BoardEditorSettingWizard.GetHexDefPixel();
				}
			}

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField( "이름", GUILayout.Width (40) );
			m_boardName = EditorGUILayout.TextField( m_boardName, GUILayout.Width (150) );
			EditorGUILayout.EndHorizontal();

			if( GUILayout.Button("보드 생성", GUILayout.Height (20)))
			{
				string prefabPath = m_savePath + "/" + m_boardName + ".prefab";
				
				bool isContinue = true;
				if( null != AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject ) ) )
				{
					isContinue = EditorUtility.DisplayDialog( "경고", "이대로 저장하면 기존 보드가 덮어씌어 집니다. 계속 하시겟습니까?", "예", "아니오" );
				}

				if( true == isContinue )
				{ 
					GameObject boardObject = new GameObject(m_boardName);
					GameData.BoardData boardData = boardObject.AddComponent<GameData.BoardData>();
					boardData.name = m_boardName;

					UnityEngine.Object prefabObject = PrefabUtility.CreateEmptyPrefab(prefabPath);
					PrefabUtility.ReplacePrefab(boardObject, prefabObject);

					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();

					GameObject.DestroyImmediate(boardObject);

					m_boardPrefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;
					m_boardData = m_boardPrefab.GetComponent<GameData.BoardData>();
				}

			}

			if( null == m_boardPrefab )
			{
				return ;
			}
			 
			if( GUILayout.Button("보드 저장", GUILayout.Height (20)))
			{
				EditorUtility.SetDirty (m_boardPrefab);

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh ();
			}


			EditorGUILayout.ObjectField( m_boardPrefab, typeof(GameObject), true );

			m_boardData.BoardSize.x = EditorGUILayout.FloatField("가로", m_boardData.BoardSize.x, GUILayout.Width (InnerWidth), GUILayout.Height (InnerHeight));
			m_boardData.BoardSize.y = EditorGUILayout.FloatField("세로", m_boardData.BoardSize.y, GUILayout.Width (InnerWidth), GUILayout.Height (InnerHeight));
			m_boardData.Pixel = EditorGUILayout.FloatField("블럭 픽셀", m_boardData.Pixel, GUILayout.Width (InnerWidth), GUILayout.Height (InnerHeight));

			if( GUILayout.Button ("격자 생성", GUILayout.Height (20)) )
			{
				if( null != m_boardData )
				{
					BoardShape.Data data = BoardShape.Batch( m_boardData.BoardSize, m_boardData.Pixel );

					if( null != m_boardData.BlockProperties )
					{
						for( int x = 0; x < data.container.X.Count; ++x )
						{
							for( int y = 0; y < data.container[x].Y.Count; ++y )
							{
								if( x >= m_boardData.BlockProperties.X.Count || y >= m_boardData.BlockProperties[x].Y.Count )
								{
									continue;
								}
								 
								data.container[x,y].GenerateType = m_boardData.BlockProperties[x,y].GenerateType;
								data.container[x,y].ItemType = m_boardData.BlockProperties[x,y].ItemType;
							}
						}
					}

					m_boardData.BlockProperties = null;
					m_boardData.BlockProperties = data.container;
					m_boardData.InnerSize = data.innerSize;

					EditorUtility.SetDirty (m_boardPrefab);

					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh ();
				}
				else
				{
					EditorUtility.DisplayDialog("", "생성된 보드가 없습니다.", "확인" );
				}
			}
		}

		public void BlockWindow( int id )
		{
			if( GUILayout.Button ("Save Setting", GUILayout.Height(20) ))
			{
				
			}
			
			if( GUILayout.Button ("Load Setting", GUILayout.Height(20)) )
			{
			}

			if( GUILayout.Button ("Select Atlas", GUILayout.Height (20) ))
			{
				BoardEditorAtlasSelectorWizard.ShowWizard
				( 
                 	(GameObject select ) => 
          			{ 
						m_boardData.SetAtlas(select);

						EditorUtility.SetDirty (m_boardData.Atlas);

						Repaint ();
					}
				);
			}

			if( null == m_boardData )
			{
				return ;
			}

			EditorGUILayout.ObjectField(m_boardData.Atlas, typeof(TextureAtlas), false);
			
			if( GUILayout.Button ("Add", GUILayout.Height(20)) )
			{
				BoardEditorSpriteSelectorWizard.ShowWizard
				(
					m_boardData.Atlas, 
					(TextureAtlas.Atlas atlas) =>
					{
						m_boardData.BlockAtlas.Add ( atlas.name );

						Repaint ();
					}
				);

			}

			m_scrollbar = EditorGUILayout.BeginScrollView(m_scrollbar, GUILayout.Height (350));


			Func<int, string, Texture2D, Rect, bool> DrawBlock = (int offset, string name, Texture2D texture, Rect uv)=>
			{
				bool pushed = false;
				int height = 7 + ( offset * 67 );
				if( GUILayout.Button ("", GUILayout.Width (64), GUILayout.Height ( 64 )))
				{
					pushed = true;
				}
				
				GUI.DrawTextureWithTexCoords(new Rect(8, height, 56, 56 ), texture, uv );
				GUI.Label(new Rect(74, height + 20, 100, 56 ), name );

				return pushed;
			};

			int additionCount = 0;

			if( DrawBlock(additionCount++, "Blank", m_blankTexture, new Rect( 0, 0, 1, 1 ) ) )
			{
				m_selected = Game.BlockGenerateType.Blank.ToInt ();
			}
			if( DrawBlock(additionCount++, "Rand", m_randomTexture, new Rect( 0, 0, 1, 1 ) ) )
			{
				m_selected = Game.BlockGenerateType.Random.ToInt ();
			}

			for( int i = 0; i < m_boardData.BlockAtlas.Count; ++i )
			{
				TextureAtlas.Atlas atlas = m_boardData.GetAtlas (i);
				
				if( DrawBlock(additionCount + i, atlas.name, m_boardData.Atlas.TargetTexture, atlas.UV ) )
				{
					m_selected = i;
				}
			}

			EditorGUILayout.EndScrollView();

			Multiple data = this.GetDataByGenerate(m_selected);
			string selectName = data.First<string>();
			Texture2D selectTex = data.Next<Texture2D>();
			Rect selectUV = data.Next<Rect>();

			GUI.Label(new Rect(85, 522, 100, 56 ), selectName );
			GUI.DrawTextureWithTexCoords(new Rect(10, 500, 64, 64 ), selectTex, selectUV );

//			if( m_selected == Game.BlockGenerateType.Blank.ToInt() )
//			{
//				GUI.Label(new Rect(85, 522, 100, 56 ), "Blank" );
//				GUI.DrawTexture(new Rect(10, 500, 64, 64 ), m_blankTexture );
//			}
//			else if( m_selected == Game.BlockGenerateType.Random.ToInt() )
//			{
//				GUI.Label(new Rect(85, 522, 100, 56 ), "Random" );
//				GUI.DrawTexture(new Rect(10, 500, 64, 64 ), m_randomTexture );
//			}
//			else
//			{
//				if( m_boardProperty.BlockAtlas.Count <= m_selected )
//				{
//					m_selected = Game.BlockGenerateType.Random.ToInt();
//					Repaint ();
//				}
//				else
//				{ 
//					GUI.Label(new Rect(78, 522, 100, 56 ), m_boardProperty.BlockAtlas[m_selected].name );
//					GUI.DrawTextureWithTexCoords(new Rect(10, 500, 64, 64), m_boardProperty.Atlas.TargetTexture, m_boardProperty.BlockAtlas[m_selected].UV );
//				}
//			}
		}

		private Multiple GetDataByGenerate(int genType )
		{
			Multiple m = new Multiple(3);

			if( genType == Game.BlockGenerateType.Random.ToInt() )
			{
				m.Set(0, "Random" );
				m.Set(1, m_randomTexture );
				m.Set(2, new Rect( 0, 0, 1, 1 ) );
			}

			else if( genType == Game.BlockGenerateType.Blank.ToInt () )
			{
				m.Set(0, "Blank" );
				m.Set(1, m_blankTexture );
				m.Set(2, new Rect( 0, 0, 1, 1 ) );

			}
			else
			{
				if( m_boardData.BlockAtlas.Count <= genType )
				{
					m.Set(0, "None" );
					m.Set(1, EditorGUIUtility.whiteTexture );
					m.Set(2, new Rect( 0, 0, 1, 1 ) );
				}
				else
				{
					TextureAtlas.Atlas atlas = m_boardData.GetAtlas(genType);
					m.Set(0, atlas.name );
					m.Set(1, m_boardData.Atlas.TargetTexture );
					m.Set(2, atlas.UV );
				}
			}

			return m;
		}
	}
}









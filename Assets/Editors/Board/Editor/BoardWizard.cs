using System;

using UnityEngine;
using UnityEditor;

using UnnamedUtility;
using UnnamedResource;
using Game;
using System.Collections.Generic;


namespace BoardEditor
{

	public class BoardWizard : ScriptableWizard 
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

		private Rect RECT_ONE = new Rect( 0, 0, 1, 1 );

		private static int MENU_COUNT = 3;
		private const float MenuWidth = 230.0f;
		private const float InnerWidth = 190.0f;
		private const float InnerHeight = 24.0f;

		//board
		private GameObject m_boardPrefab = null;
		private string m_boardName = "Unnamed";
		private string m_savePath = "";
		private UnnamedObject.HexagonVertex m_vertex = new UnnamedObject.HexagonVertex(BoardShape.Width, BoardShape.Height);

		//block
		private Texture2D m_blankTexture;
		private Texture2D m_randomTexture;

		private Texture2D m_itemATexture;
		private Texture2D m_itemSTexture;

		private Texture2D m_selectPropertyTexture;
		private Texture2D m_durableTexture;

		private Vector2 m_scrollbar;
		private GameData.BoardData m_boardData;
		private int m_selected = (int)Game.GenerateType.RandomSubA;

		//Property
		private GameData.BlockProperty m_selectProperty = null;
		private Vector2 m_selectPropertyPosition = Vector2.zero;

		public static void ShowWizard( GameObject prefab, string path )
		{
			BoardWizard boardWizard = ScriptableWizard.DisplayWizard<BoardWizard>("BoardEditor");
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

			if( null == m_itemATexture )
			{
				m_itemATexture = AssetDatabase.LoadAssetAtPath( "Assets/Editors/Board/Resource/itemtype_A.png", typeof(Texture2D )) as Texture2D;
			}

			if( null == m_itemSTexture )
			{
				m_itemSTexture = AssetDatabase.LoadAssetAtPath( "Assets/Editors/Board/Resource/itemtype_S.png", typeof(Texture2D )) as Texture2D;
			}

			if( null == m_selectPropertyTexture )
			{
				m_selectPropertyTexture = AssetDatabase.LoadAssetAtPath( "Assets/Editors/Board/Resource/select_property.png", typeof(Texture2D )) as Texture2D;
			}

			if( null == m_durableTexture )
			{
				m_durableTexture = AssetDatabase.LoadAssetAtPath( "Assets/Editors/Board/Resource/block_durable.png", typeof(Texture2D )) as Texture2D;
			}
		}

		void OnGUI()
		{
			this.DrawBoard();
			GUI.WindowFunction[] functions 	= new GUI.WindowFunction[]{ BoardWindow, BlockWindow, PropertyWindow };
			string[] funcNames 				= { "Board", "Block", "Property" };
			
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
							m_boardData.BlockProperties[x,y].Generate = (GenerateType)m_selected;
						}

						if( Event.current.button == 1 ) //right
						{
							m_selectProperty 			= m_boardData.BlockProperties[x,y];
							m_selectPropertyPosition.x 	= x;
							m_selectPropertyPosition.y 	= y;
						}
					}

					if( this.IsStartField(new Vector2(x, y) ))
					{
						GUI.color = new Color32(0,0,255,160);
					}

					int generateType	= (int)m_boardData.BlockProperties[x,y].Generate;
					Multiple m 			= this.GetDataByGenerate(generateType);
			
					GUI.DrawTextureWithTexCoords( rect, m.At<Texture2D>(1), m.At<Rect>(2) );

					Texture2D itemTexture = this.GetTextureByItemType( (Game.BlockItemType)m_boardData.BlockProperties[x,y].ItemType );
					if( null != itemTexture )
					{
						GUI.DrawTextureWithTexCoords( rect, itemTexture, RECT_ONE );
					}

					if( 0 != m_boardData.BlockProperties[x,y].Durable )
					{
						GUI.DrawTextureWithTexCoords( rect, m_durableTexture, RECT_ONE );
					}

					if( m_boardData.BlockProperties[x,y] == m_selectProperty )
					{
						GUI.DrawTextureWithTexCoords( rect, m_selectPropertyTexture, RECT_ONE );
					}

					GUI.color = Color.white;
				}
			}
		}

		public void BoardWindow( int id )
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField( "이름", GUILayout.Width (40) );
			m_boardName = EditorGUILayout.TextField( m_boardName );
			EditorGUILayout.EndHorizontal();

			if( GUILayout.Button("보드 생성", GUILayout.Height (InnerHeight)))
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

					m_selectProperty = null;

					this.LoadDefaultSettings(m_boardData);
				}

			}

			if( null == m_boardPrefab )
			{
				return ;
			}
			 
			if( GUILayout.Button("보드 저장", GUILayout.Height (InnerHeight)))
			{
				EditorUtility.SetDirty (m_boardPrefab);

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh ();
			}


			EditorGUILayout.ObjectField( m_boardPrefab, typeof(GameObject), true );

			m_boardData.BoardSize.x = EditorGUILayout.FloatField("가로", m_boardData.BoardSize.x);
			m_boardData.BoardSize.y = EditorGUILayout.FloatField("세로", m_boardData.BoardSize.y);
			m_boardData.Pixel = EditorGUILayout.FloatField("블럭 픽셀", m_boardData.Pixel);

			if( GUILayout.Button ("격자 생성", GUILayout.Height (InnerHeight)) )
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
								 
								data.container[x,y].Generate = m_boardData.BlockProperties[x,y].Generate;
								data.container[x,y].ItemType = m_boardData.BlockProperties[x,y].ItemType;
								data.container[x,y].Durable = m_boardData.BlockProperties[x,y].Durable;
							}
						}
					}

					m_boardData.BlockProperties = null;
					m_boardData.BlockProperties = data.container;
					m_boardData.InnerSize = data.innerSize;

					EditorUtility.SetDirty (m_boardPrefab);

					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh ();

					m_selectProperty = null;
				}
				else
				{
					EditorUtility.DisplayDialog("", "생성된 보드가 없습니다.", "확인" );
				}


				if( null == m_selectProperty )
				{
					EditorGUILayout.LabelField("블럭을 선택해 주세요." );
					return ;
				}
			}

			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("블럭 속성");
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("아이템 타입", GUILayout.Width(80));
			m_selectProperty.ItemType = (Game.BlockItemType)EditorGUILayout.EnumPopup(m_selectProperty.ItemType, GUILayout.Width(100));
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("내구도", GUILayout.Width(45));
			m_selectProperty.Durable = EditorGUILayout.IntField(m_selectProperty.Durable, GUILayout.Width (135));
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();
			
			bool toggleStart = IsStartField(m_selectPropertyPosition);
			bool currentToggle = EditorGUILayout.Toggle("시작지점", toggleStart, GUILayout.Width (InnerWidth) );
			if( toggleStart != currentToggle )
			{
				if( true == currentToggle )
				{
					m_boardData.StartField.Add (m_selectPropertyPosition);
				}
				else
				{
					m_boardData.StartField.Remove (m_selectPropertyPosition);
				}
			}
		}





		public void BlockWindow( int id )
		{
			if( GUILayout.Button ("이미지 지정", GUILayout.Height (InnerHeight) ))
			{
				bool isContinue = true;
				if( null != m_boardData.Atlas )
				{
					isContinue = EditorUtility.DisplayDialog("", "기존 찍어놨던 이미지가 없어집니다. 계속하시겠습니까?", "확인", "취소" );
				}

				if( false == isContinue )
				{
					return ;
				}

				AtlasSelectorWizard.ShowWizard
				( 
                 	(GameObject select ) => 
          			{ 
						m_boardData.SetAtlas(select);

						for( int x = 0; x < m_boardData.BlockProperties.Count; ++x )
						{
							for( int y = 0; y < m_boardData.BlockProperties[x].Count; ++y )
							{
								m_boardData.BlockProperties[x,y].Generate = Game.GenerateType.RandomCustomA;
							}
						}


						m_boardData.BlockAtlas.Clear ();

						EditorUtility.SetDirty (m_boardData);
						
						Repaint ();
					}
				);
			}

			if( null == m_boardData )
			{
				return ;
			}

			EditorGUILayout.ObjectField(m_boardData.Atlas, typeof(TextureAtlas), false);
			
//			if( GUILayout.Button ("추가", GUILayout.Height(InnerHeight)) )
//			{
//				SpriteSelectorWizard.ShowWizard
//				(
//					m_boardData.Atlas, 
//					(TextureAtlas.Atlas atlas) =>
//					{
//						m_boardData.BlockAtlas.Add ( atlas.name );
//
//						EditorUtility.SetDirty (m_boardData);
//						
//						Repaint ();
//					}
//				);
//
//			}

			m_scrollbar = EditorGUILayout.BeginScrollView(m_scrollbar, GUILayout.Height (480));


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

			if( DrawBlock(additionCount++, "공백", m_blankTexture, RECT_ONE ) )
			{
				m_selected = (int)Game.GenerateType.Blank;
			}

			if( DrawBlock(additionCount++, "무작위", m_randomTexture, RECT_ONE ) )
			{
				m_selected = (int)Game.GenerateType.RandomSubA;
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

			GUI.Label(new Rect(85, 650, 100, 56 ), selectName );
			GUI.DrawTextureWithTexCoords(new Rect(10, 622, 64, 64 ), selectTex, selectUV );

		}

		public void PropertyWindow( int id )
		{


		}

		private Multiple GetDataByGenerate(int genType )
		{
			Multiple m = new Multiple(3);

			if( genType == (int)Game.GenerateType.RandomSubA)
			{
				m.Set(0, "무작위" );
				m.Set(1, m_randomTexture );
				m.Set(2, RECT_ONE );
			}

			else if( genType == (int)Game.GenerateType.Blank )
			{
				m.Set(0, "공백" );
				m.Set(1, m_blankTexture );
				m.Set(2, RECT_ONE );

			}
			else
			{
				if( m_boardData.BlockAtlas.Count <= genType )
				{
					m.Set(0, "없음" );
					m.Set(1, EditorGUIUtility.whiteTexture );
					m.Set(2, RECT_ONE );
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

		private Texture2D GetTextureByItemType( Game.BlockItemType itemType )
		{
			switch( itemType )
			{
			case BlockItemType.Attack: return m_itemATexture;
			case BlockItemType.Skill: return m_itemSTexture;
			}
			return null;
		}

		private void LoadDefaultSettings( GameData.BoardData boardData )
		{
			Setting setting = new Setting();
			setting.Load();

			boardData.BoardSize.x = setting.DefaultWidth;
			boardData.BoardSize.y = setting.DefaultHeight;
			boardData.Pixel = setting.DefaultPixel;
			boardData.Atlas = ( null == setting.DefaultAtlas ) ? null : setting.DefaultAtlas.GetComponent<TextureAtlas>();
			boardData.BlockAtlas.AddRange(setting.DefaultAtlasElems);

		}

		private bool IsStartField( Vector2 position )
		{
			foreach( Vector2 field in m_boardData.StartField )
			{
				if( field.x == position.x && field.y == position.y )
				{
					return true;
				}
			}

			return false;
		}
	}
}









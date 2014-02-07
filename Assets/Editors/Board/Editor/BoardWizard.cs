using UnityEngine;
using UnityEditor;

using Unnamed;
using Game;
using Game.Data;
using System.Collections.Generic;

namespace BoardEditor
{

	public class BoardWizard : ScriptableWizard 
	{
		[MenuItem("Assets/" + Named.Title + "/Board Editor")]
		static void WizardCreate()
		{
			Setting setting = new Setting();
			setting.Load ();

			if( null == setting.DefaultBlockSet )
			{
				EditorUtility.DisplayDialog("", "기본 블럭을 먼저 지정해야 합니다.", "확인" );

				SettingWizard.ShowWizard();

				return ;
			}

			BlockDataSet dataset = setting.DefaultBlockSet.GetComponent<BlockDataSet>();

			GameObject targetObject = UnityEditor.Selection.activeObject as GameObject;
			if( null == targetObject || null == targetObject.GetComponent<Game.Data.BoardData>() )
			{
				ShowWizard(null, Path.GetSelectionPath(), dataset);
			}
			else
			{
				ShowWizard(targetObject, Path.GetSelectionPath(), dataset);
			}
		}

		const bool TOOL_DEV = true;

		private System.Action[] m_boardItems;

		private const float MenuWidth = 230.0f;
		private const float InnerWidth = 220.0f;
		private const float InnerHeight = 24.0f;

		//board
		private GameObject m_boardPrefab = null;
		private string m_boardName = "Unnamed";
		private string m_savePath = "";
		private HexagonVertex m_vertex = new HexagonVertex(BoardShape.Width, BoardShape.Height);

		//block
		private Texture2D m_itemATexture;
		private Texture2D m_itemSTexture;

		private Texture2D m_selectPropertyTexture;
		private Texture2D m_durableTexture;

		private Vector2 m_blockScrollBar;
		private BoardData m_boardData;
		private BlockDataSet m_blockDataSet;
		private BlockRenderer m_blockRenderer = new BlockRenderer();

		private GenerateType m_selectGenerate = GenerateType.RandomSubA;

		//Property
		private Game.Data.BlockProperty m_selectProperty = null;
		private Vector2 m_selectPropertyPosition = Vector2.zero;

		//Random
		private Vector2 m_randomScrollBar;

		public static void ShowWizard( GameObject prefab, string path, BlockDataSet dataSet )
		{
			BoardWizard boardWizard = ScriptableWizard.DisplayWizard<BoardWizard>("BoardEditor");

			boardWizard.m_boardPrefab = prefab;
			boardWizard.m_blockDataSet = dataSet;

			boardWizard.LoadToolResource();

			boardWizard.minSize = boardWizard.maxSize = new Vector2( FixedSize.Width + (MenuWidth * boardWizard.m_boardItems.Length), FixedSize.Height );
			boardWizard.m_savePath = path;


			if( null != prefab )
			{  
				boardWizard.m_boardData = boardWizard.m_boardPrefab.GetComponent<Game.Data.BoardData>();
				boardWizard.m_boardName = boardWizard.m_boardData.name;
			}
		}

		void OnFocus()
		{
			if(TOOL_DEV)
			{
				this.LoadToolResource();
			}

		}

		void LoadToolResource()
		{
//			m_boardItems = new System.Action[]{ BoardWindow, CustomRandomWindow, BlockWindow };
			m_boardItems = new System.Action[]{ BoardWindow, BlockWindow, CustomRandomWindow };
			m_blockRenderer.Load(m_blockDataSet);

			string front = "Assets/Editors/Board/Resource/";
			
			m_itemATexture = AssetDatabaseHelper.LoadAsset<Texture2D>( front + "itemtype_A.png" );
			m_itemSTexture = AssetDatabaseHelper.LoadAsset<Texture2D>( front + "itemtype_S.png" );
			m_selectPropertyTexture = AssetDatabaseHelper.LoadAsset<Texture2D>( front + "select_property.png" );
			m_durableTexture = AssetDatabaseHelper.LoadAsset<Texture2D>( front + "block_durable.png" );
		}

		void OnWizardUpdate()
		{
		}

		void OnGUI()
		{
			this.DrawBoard();

//			this.DrawLine(FixedSize.Width);

			Rect windowRect = new Rect( FixedSize.Width, 0, MenuWidth, FixedSize.Height );
			for( int i = 0; i < m_boardItems.Length; ++i )
			{

				GUILayout.BeginArea(windowRect, m_boardItems[i].Method.Name, GUI.skin.GetStyle("Window") );
				m_boardItems[i]();
				GUILayout.EndArea ();

				windowRect.x += MenuWidth;
				
//				this.DrawLine (windowRect.x + windowRect.width);
			}
		}

		public void DrawLine( float width )
		{
			Color prevColor = Handles.color;
			Handles.color = Color.green;
			Handles.DrawLine( new Vector2( width, 0 ), new Vector2( width, FixedSize.Height ) );
			Handles.color = prevColor;
		}

		public void DrawBoard()
		{
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
							m_boardData.BlockProperties[x,y].Generate = m_selectGenerate;
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

					this.DrawBoardBlock(rect, m_boardData.BlockProperties[x,y].Generate );
//
					Texture2D itemTexture = this.GetTextureByItemType( (Game.BlockItemType)m_boardData.BlockProperties[x,y].ItemType );
					if( null != itemTexture )
					{
						GUI.DrawTextureWithTexCoords(rect, itemTexture, Share.UV);
					}
//
					if( 0 != m_boardData.BlockProperties[x,y].Durable )
					{
						GUI.DrawTextureWithTexCoords( rect, m_durableTexture, Share.UV );
					}
//
					if( m_boardData.BlockProperties[x,y] == m_selectProperty )
					{
						GUI.DrawTextureWithTexCoords( rect, m_selectPropertyTexture, Share.UV );
					}

					GUI.color = Color.white;
				}
			}
		}

		public void BoardWindow()
		{
			GUILayout.BeginArea(new Rect(5, 20, InnerWidth, 200 ), "메뉴", GUI.skin.GetStyle ("Window") );

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
					Game.Data.BoardData boardData = boardObject.AddComponent<Game.Data.BoardData>();
					boardData.name = m_boardName;

					UnityEngine.Object prefabObject = PrefabUtility.CreateEmptyPrefab(prefabPath);
					PrefabUtility.ReplacePrefab(boardObject, prefabObject);

					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();

					GameObject.DestroyImmediate(boardObject);

					m_boardPrefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;
					m_boardData = m_boardPrefab.GetComponent<Game.Data.BoardData>();

					m_selectProperty = null;

					this.LoadDefaultSettings(m_boardData);
				}

			}


			if( null != m_boardPrefab )
			{
				if( GUILayout.Button("보드 저장", GUILayout.Height (InnerHeight)))
				{
					EditorUtility.SetDirty (m_boardPrefab);
					
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh ();
				}
			}
			else
			{
				GUILayout.EndArea ();
				
				return ;
			}

			 
			EditorGUILayout.ObjectField( m_boardPrefab, typeof(GameObject), true );

			m_boardData.BoardSize.x = EditorGUILayout.FloatField("가로", m_boardData.BoardSize.x);
			m_boardData.BoardSize.y = EditorGUILayout.FloatField("세로", m_boardData.BoardSize.y);
			m_boardData.Pixel = EditorGUILayout.FloatField("블럭 픽셀", m_boardData.Pixel);

			if( GUILayout.Button ("격자 생성", GUILayout.Height (InnerHeight)) )
			{
				if( null != m_boardData )
				{
					BoardShape.Data data = BoardShape.Batch( m_boardData );
					
					m_boardData.BlockProperties = null;
					m_boardData.BlockProperties = data.container;
					m_boardData.InnerSize = data.innerSize;

					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh ();
					 
					m_selectProperty = null;
				}
				else
				{
					EditorUtility.DisplayDialog("", "생성된 보드가 없습니다.", "확인" );
				}

			}

			GUILayout.EndArea ();


			GUILayout.BeginArea(new Rect(5, 230, InnerWidth, 320 ), "배경", GUI.skin.GetStyle ("Window") );

			GUILayout.EndArea ();

			GUILayout.BeginArea(new Rect(5, 560, InnerWidth, 150 ), "선택된 블럭", GUI.skin.GetStyle ("Window") );
			if( null == m_selectProperty )
			{
				EditorGUILayout.Separator();
				EditorGUILayout.LabelField("블럭을 선택해 주세요." );
			}
			else
			{
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("아이템 타입", GUILayout.Width(80));
				m_selectProperty.ItemType = (Game.BlockItemType)EditorGUILayout.EnumPopup(m_selectProperty.ItemType, GUILayout.Width(100));
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("내구도", GUILayout.Width(45));
				m_selectProperty.Durable = EditorGUILayout.IntField(m_selectProperty.Durable, GUILayout.Width (135));
				EditorGUILayout.EndHorizontal();
				
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

			GUILayout.EndArea();
		}

		public void BlockWindow()
		{
			System.Action<GenerateType> CodeCallback = (GenerateType generate) =>
			{  
				m_selectGenerate = generate;

				Repaint();
			};

			BeginWindows();

			//fixed block window
			BlockShareRender shareRenderer = new BlockShareRender(m_blockDataSet, new Vector2(10, 20), 1.08f, CodeCallback );
			Rect drawedRect = shareRenderer.Draw ();

			float tint = 12.0f;
			float interval = 15.0f;
			float size = 50.0f;

			System.Action<int> DrawShareBlocks = (int start) =>
			{
				float offset = tint;
				
				for( int subIndex = 0; subIndex < BlockConst.SubCount; ++subIndex )
				{
					Rect drawRect = new Rect( offset, 19, size, size);
					GenerateType generate = (GenerateType)(subIndex + start);
					if( GUI.Button(drawRect, "" ) )
					{
						m_selectGenerate = generate;
					}

					m_blockRenderer.Draw( drawRect, generate );
				
					offset += interval + size;
				}
			};

			//random block window
			drawedRect.y += interval;
			GUI.WindowFunction RandomBlocks = (int id) =>
			{
				DrawShareBlocks((int)GenerateType.RandomSubA);
			};

			GUI.Window (10000, drawedRect, RandomBlocks, "Random");

			//phase block window
			drawedRect.y += interval + drawedRect.height;
			GUI.WindowFunction SpecialBlocks = (int id) =>
			{
				DrawShareBlocks((int)GenerateType.RandomPhaseSubA);
			};
			
			GUI.Window (10001, drawedRect, SpecialBlocks, "Random Phase");

			EndWindows();

		}
		 
		public void CustomRandomWindow()
		{ 
			if( null == m_boardData )
			{
				EditorGUILayout.LabelField("보드가 없습니다.");
				return ;
			}

			float tint = 5;
			float height = 20;
			Rect position = new Rect( tint, height, InnerWidth, InnerHeight );

			float blockSize = 55;

			for( int i = 0; i < BlockConst.CustomCount; ++i )  
			{
				float blockHeight = height + ( i * blockSize ) + ( i * tint );
				Rect drawRect = new Rect( tint + tint, blockHeight, blockSize, blockSize );
				
				if( i < m_boardData.CustomRandom.Length )
				{
					if( GUI.Button ( drawRect, "" ) )
					{
						if( Event.current.button == 0 )
						{
							m_selectGenerate = (GenerateType)((int)(GenerateType.RandomCustomA) + i);
						}
						else
						{
							System.Action<BoardData.CustomBlocks> ModifyCallback = (BoardData.CustomBlocks blocks) =>
							{
								Repaint ();
							};

							BlockRandomWizard.ShowWizard(m_blockDataSet, m_boardData.CustomRandom[i], ModifyCallback);
						}
					}

					GenerateType[] generateArray = m_boardData.CustomRandom[i].Generates.ToArray();
					m_blockRenderer.DrawMultiple( drawRect, generateArray );

					float smallBlockSize = 20;
					float beginLeft = drawRect.x + blockSize + tint;
					float beginTop = drawRect.y + 5;
					float advanceLeft = beginLeft;

					for( int enumIndex = 0; enumIndex < generateArray.Length; ++enumIndex )
					{
						float x = enumIndex % 8;
						float y = enumIndex / 8;
						
						Rect smallImageRect = new Rect(beginLeft + x * smallBlockSize, beginTop + y * smallBlockSize, smallBlockSize, smallBlockSize);

						m_blockRenderer.Draw ( smallImageRect, generateArray[enumIndex]);

//						if( (enumIndex + 1 ) % 8 == 0 )
//						{
//							advanceLeft = beginLeft;
//							beginTop += smallBlockSize;
//						}
					}

				}
				else
				{
					GUI.enabled = false;
					GUI.Button ( drawRect, "" );
					GUI.enabled = true;
				}

			}

			GUI.BeginGroup ( new Rect( 5, 620, InnerWidth, 90 ), "Selection", GUI.skin.GetStyle("Window") );

			DrawBoardBlock( new Rect( 0, 15, 80, 80 ), m_selectGenerate );

			Rect labelRect = new Rect( 90, 45, 120, 50 );
			GUI.Label(labelRect, m_selectGenerate.ToString());
			GUI.EndGroup ();

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

		private void LoadDefaultSettings( Game.Data.BoardData boardData )
		{
			Setting setting = new Setting();
			setting.Load();

			boardData.BoardSize.x = setting.DefaultWidth;
			boardData.BoardSize.y = setting.DefaultHeight;
			boardData.Pixel = setting.DefaultPixel;
			boardData.BlockDataSet = setting.DefaultBlockSet.GetComponent<BlockDataSet>();

			m_blockRenderer.Load (boardData.BlockDataSet);
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

		private void DrawBoardBlock( Rect rect, GenerateType generate )
		{
			if( BlockEnumSupporter.IsCustomRandom( generate ) )
			{
				int randomOffset = (int)generate - (int)GenerateType.RandomCustomA;
				
				m_blockRenderer.DrawMultiple(rect, m_boardData.CustomRandom[randomOffset].Generates.ToArray());
			}
			else
			{
				m_blockRenderer.Draw (rect, generate);
			}
		}
		
	}
}









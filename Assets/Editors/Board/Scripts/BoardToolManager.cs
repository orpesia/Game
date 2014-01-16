using System;
using System.Collections.Generic;

using UnityEngine;

using UnnamedUtility;
using UnnamedResource;

using GameView;

namespace BoardEditor
{
    class BoardToolManager : MonoBehaviour
    {
#if UNITY_EDITOR
		public Color PanelSizeColor = Color.blue;
#endif
		private int MenuWidth = 180;
		private int MenuHeight = 25;
		private int MenuTint = 5;
		private int MenuItemWidth = 170;
		private BoardToolProcess m_process = null;

		private const string None = "None";
		
		private string m_hexWidth = None;
		private string m_hexHeight = None;
		private string m_hexAttachX = None;
		private string m_hexAttachY = None;
		private string m_pixel = None;

		void Awake()
		{
			m_process = gameObject.AddComponent<BoardToolProcess>();
		}

		void OnGUI()
		{
			this.DrawGUI ();
			this.DrawMenu ();
		}
	
		public void DrawGUI()
		{
#if UNITY_EDITOR
			Color prevColor = UnityEditor.Handles.color;
			UnityEditor.Handles.color = PanelSizeColor;
			
			UnityEditor.Handles.DrawLine( new Vector2( 0, BoardToolSize.FixHeight ), new Vector2(BoardToolSize.FixWidth, BoardToolSize.FixHeight));
			UnityEditor.Handles.DrawLine( new Vector2( BoardToolSize.FixWidth, BoardToolSize.FixHeight ), new Vector2(BoardToolSize.FixWidth, 0));
			
			UnityEditor.Handles.color = prevColor;
#endif

		}
		
		public void DrawMenu()
		{
			GUIStyle style = GUI.skin.GetStyle("Window"); //Or whatever
			GUIStyleState backgupState = style.onNormal;
			style.onNormal = style.normal;

			GUI.Window(0, new Rect( Screen.width - MenuWidth * 2, 0, MenuWidth, Screen.height ), BoardWindow, "BoardEditor", style );
			GUI.Window(1, new Rect( Screen.width - MenuWidth, 0, MenuWidth, Screen.height ), TextureWindow, "TextureEditor", style );

			style.onNormal = backgupState;
		}

		public void BoardWindow( int id )
		{
			Rect rect = new Rect( MenuWidth.Center (MenuItemWidth ), MenuHeight, MenuItemWidth, MenuHeight );
			if(GUI.Button ( rect, "New Board" ))
			{
				m_process.NewBoard();
				this.Binding(m_process.HexagonRenderer);
			}

			rect.y += MenuHeight + MenuTint;

			m_hexWidth = this.LabelNText(ref rect, "Hex Width", m_hexWidth);
			rect.y += MenuHeight + MenuTint;
			
			m_hexHeight = this.LabelNText(ref rect, "Hex Height", m_hexHeight);
			rect.y += MenuHeight + MenuTint;

			m_hexAttachX = this.LabelNText(ref rect, "Hex AttachX", m_hexAttachX);
			rect.y += MenuHeight + MenuTint;

			m_hexAttachY = this.LabelNText(ref rect, "Hex AttachY", m_hexAttachY);
			rect.y += MenuHeight + MenuTint;

			m_pixel = this.LabelNText(ref rect, "Pixel", m_pixel);
			rect.y += MenuHeight + MenuTint;

			if(GUI.Button ( rect, "Recreate Vertex" ))
			{
				this.UnBinding(m_process.HexagonRenderer);
			}

		}

		public void TextureWindow( int id )
		{
#if UNITY_EDITOR
			Rect rect = new Rect( MenuWidth.Center (MenuItemWidth ), MenuHeight, MenuItemWidth, MenuHeight );
			 
			if(GUI.Button ( rect, "New Atlas"))
			{
				string folderPath = UnityEditor.EditorUtility.OpenFolderPanel("Collect Texture", Application.dataPath, "*.png");
				if( string.IsNullOrEmpty(folderPath))
				{ 
					return ;
				}

				string savePath = UnityEditor.EditorUtility.SaveFilePanel("Save Texture", folderPath, "", "png");
				if( string.IsNullOrEmpty(savePath))
				{
					return ;
				} 

				List<Texture2D> pathList = new List<Texture2D>();

				string[] inFiles = System.IO.Directory.GetFiles (folderPath);
				for( int i = 0; i < inFiles.Length; ++i )
				{
					string extension = System.IO.Path.GetExtension(inFiles[i]);
					if( extension == ".PNG" || extension == ".png" )
					{
						WWW www = new WWW("file://"+inFiles[i]);
						Texture2D wwwTex = www.texture as Texture2D; 
						wwwTex.name = System.IO.Path.GetFileName(inFiles[i]);
						pathList.Add (wwwTex);
					} 
				}

				string saveName = System.IO.Path.GetFileNameWithoutExtension(savePath);
				GameObject atlasObject = new GameObject(saveName);
				atlasObject.transform.parent = transform;

				TextureAtlas atlas = atlasObject.AddComponent<TextureAtlas>();
				TextureAtlasGenerator.Create (savePath, pathList.ToArray(), atlas);

				string prefabPath = System.IO.Path.ChangeExtension(savePath,".prefab"); 
				prefabPath = this.SplitAssetPath(prefabPath);

				UnityEngine.Object prefabObject = UnityEditor.PrefabUtility.CreateEmptyPrefab( this.SplitAssetPath(prefabPath));
				UnityEditor.PrefabUtility.ReplacePrefab(atlasObject, prefabObject, UnityEditor.ReplacePrefabOptions.ConnectToPrefab);
//				GameObject prefabObject = UnityEditor.PrefabUtility.CreatePrefab(prefabPath, atlasObject, UnityEditor.ReplacePrefabOptions.ConnectToPrefab);


//				UnityEditor.AssetDatabase.AddObjectToAsset(atlasObject,prefabPath);
//				UnityEditor.AssetDatabase.ImportAsset(prefabPath, UnityEditor.ImportAssetOptions.ForceUpdate);
//				GameObject prefabObject = UnityEditor.AssetDatabase.GetAssetPath(prefabPath) as GameObject;
				UnityEditor.EditorUtility.SetDirty(prefabObject);


				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.ForceUpdate);

//				GameObject.Destroy(atlasObject);
				
				
			}
			
			rect.y += MenuHeight + MenuTint;
			if(GUI.Button ( rect, "Save Atlas"))
			{

			}
			
			rect.y += MenuHeight + MenuTint;
			if(GUI.Button ( rect, "Load Atlas"))
			{
			}

			rect.y += MenuHeight + MenuTint;
#endif		
		}

		private void Binding( HexagonBoardRenderer renderer)
		{
			m_hexWidth = m_process.HexagonRenderer.HexagonWidth.ToString();
			m_hexHeight = m_process.HexagonRenderer.HexagonHeight.ToString();
			m_hexAttachX = m_process.HexagonRenderer.HexagonAttachX.ToString();
			m_hexAttachY = m_process.HexagonRenderer.HexagonAttachY.ToString();

			m_pixel = m_process.HexagonRenderer.Pixel.ToString();
		}

		private void UnBinding(HexagonBoardRenderer renderer)
		{
			if( null != renderer )
			{
				m_process.HexagonRenderer.HexagonWidth = RegulateValue( 0.5f, 1.0f, m_hexWidth );
				m_process.HexagonRenderer.HexagonHeight = RegulateValue( 0.5f, 1.0f, m_hexHeight );
				m_process.HexagonRenderer.HexagonAttachX = RegulateValue( 0.5f, 1.0f, m_hexAttachX );
				m_process.HexagonRenderer.HexagonAttachY = RegulateValue( 0.5f, 1.0f, m_hexAttachY );
				m_process.HexagonRenderer.Pixel = RegulateValue( 40.0f, 300.0f, m_pixel );

				m_process.ReplaceBoard();
			}
		}

		private string LabelNText( ref Rect rect, string name, string value )
		{
			float halfWidth = MenuItemWidth * 0.5f;
			float prevX = rect.x;

			rect.width = halfWidth;
			GUI.Label (rect, name );

			rect.x = halfWidth;

			string result = GUI.TextField( rect, value );

			rect.x = prevX;
			rect.width = MenuItemWidth;

			return result;
		}

		private void NextLine( ref Rect rect)
		{
			rect.y += MenuHeight;
		}

		private float RegulateValue( float min, float max, string value )
		{
			float result = Convert.ToSingle(value);

			if( result < min ) 
			{
				result = min;
			}

			if( result > max )
			{
				result = max;
			}

			return result;
		}

		private string SplitAssetPath(string path)
		{
			int index = path.IndexOf("Assets");
			return path.Substring(index);
		}
		
    }
}

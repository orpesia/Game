﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.18408
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using Unnamed;

namespace BoardEditor
{
	public class TextureAtlasWizard : ScriptableWizard
	{
		class BoardTextureData
		{
			public string name;
			public string path;
			public bool isMissing;
		};

		private List<BoardTextureData> m_textures = new List<BoardTextureData>();
		private List<BoardTextureData> m_removes = new List<BoardTextureData>();

		private Vector2 m_scrollPosition;
		private Texture2D m_texture;
		private string m_savePath;
		private string m_name = "Unnamed";
		private GameObject m_prefab;

		public TextureAtlasWizard ()
		{
		}

		[MenuItem("Assets/" + Named.Title + "/Texture Atlas Editor")]
		public static void CreateAtlas()
		{
			GameObject targetObject = UnityEditor.Selection.activeObject as GameObject;
			if( null == targetObject || null == targetObject.GetComponent<TextureAtlas>() )
			{
				ShowWizard(null, Path.GetSelectionPath());
			}
			else
			{
				ShowWizard(targetObject, Path.GetSelectionPath());
			}
		}

		public static void ShowWizard(GameObject prefab, string savePath)
		{
			if( null != prefab && null == prefab.GetComponent<TextureAtlas>() )
			{
				Debug.Log ("Invalid texture atlas" );
				return ;
			}


			TextureAtlasWizard wizard = ScriptableWizard.DisplayWizard<TextureAtlasWizard>("Texture Editor");
			wizard.minSize = wizard.maxSize = new Vector2( 900, 600 );
			wizard.m_savePath = savePath;
			wizard.m_prefab = prefab;

			wizard.Replace ();
		}

		void OnWizardUpdate()
		{

		}

		void OnGUI()
		{
			System.Func<string, bool> Less = (string name) => 
			{
				foreach( BoardTextureData d in m_textures )
				{
					if( name == d.name )
					{
						return true;
					}
				}

				return false;
			};

			DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
			if( Event.current.type == EventType.DragExited )
			{
				for( int i = 0; i < DragAndDrop.objectReferences.Length; ++i )
				{
					Texture2D texture = DragAndDrop.objectReferences[i] as Texture2D;
					if( null == texture )
					{ 
						continue;
					}

					string texturePath = AssetDatabase.GetAssetPath(texture);
					string name = System.IO.Path.GetFileNameWithoutExtension(texturePath);

					if( true == Less(name) )
					{
						continue; 
					}

					BoardTextureData data  = new BoardTextureData();
					data.isMissing = false;
					data.path = texturePath;
					data.name = name;

					m_textures.Add( data );
				}

				Repaint ();

				return ;
			}


			if( null == m_texture )
			{ 
				GUI.DrawTexture(new Rect(0, 0, 600, 600 ), EditorGUIUtility.whiteTexture );
			}
			else
			{
				int max = Math.Max (m_texture.width, m_texture.height);
				float ratio = 600.0f / max;
				
				GUI.DrawTexture(new Rect(0, 0, m_texture.width * ratio, m_texture.height * ratio ), m_texture );
			}

			BeginWindows ();
			GUI.Window(0, new Rect( 610, 5, 280, 195 ), MenuWindow, "Atlas Editor" );
			GUI.Window(1, new Rect( 610, 205, 280, 390 ), TextureWindow, "Textures" );
			EndWindows ();
		}

		void MenuWindow( int id )
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Path:", GUILayout.Width (33));
			EditorGUILayout.LabelField(m_savePath, GUILayout.Width (245));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Separator();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Name:", GUILayout.Width (40));
			m_name = EditorGUILayout.TextField(m_name, GUILayout.Width (200));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.ObjectField(m_prefab, typeof(TextureAtlas), true );

			if( null == m_prefab )
			{
				if( GUILayout.Button ("New", GUILayout.Height (30)))
				{ 
					string prefabPath = m_savePath + "/" + m_name + ".prefab";
					
					m_prefab = new GameObject( m_name );
					m_prefab.AddComponent<TextureAtlas>(); 
					 
					UnityEngine.Object emptyPrefab = PrefabUtility.CreateEmptyPrefab(prefabPath);
					PrefabUtility.ReplacePrefab(m_prefab, emptyPrefab);

					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
					
					GameObject.DestroyImmediate( m_prefab );
					
					m_prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;

					this.Replace();


				}
			}
			else
			{
				if( GUILayout.Button ("Save", GUILayout.Height (30) ))
				{
					string texturePath = m_savePath + "/" + m_name + ".png";
					string[] paths = new string[m_textures.Count];
					for( int i = 0; i < paths.Length; ++i )
					{
						paths[i] = m_textures[i].path;
					}

					TextureAtlas atlas = m_prefab.GetComponent<TextureAtlas>();
					atlas.name = "TextureAtlas";
					atlas.TargetTexture = null;
					atlas.Textures.Clear ();
					 
					TextureAtlasGenerator.Create ( texturePath, paths, atlas );
					 
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
	
					this.Replace ();
				}
			}	

	
		}

		void TextureWindow( int id )
		{
			EditorGUILayout.BeginVertical();
			
			m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition, GUILayout.Width (265), GUILayout.Height (400));
			for( int i = 0; i < m_textures.Count; ++i )
			{
				EditorGUILayout.BeginHorizontal();
				string name = m_textures[i].isMissing ? m_textures[i].name + "-Missing" : m_textures[i].name;
				if( GUILayout.Button(name, GUILayout.Width (255), GUILayout.Height (20)) )
				{
					m_removes.Add (m_textures[i]);
				}
				
				EditorGUILayout.EndHorizontal();			
			}
			
			foreach( BoardTextureData r in m_removes )
			{
				m_textures.Remove( r );
			}
			
			m_removes.Clear();
			
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}

		private void Replace()
		{
			m_textures.Clear ();

			if( null == m_prefab )
			{
				return ;
			}

			TextureAtlas atlas = m_prefab.GetComponent<TextureAtlas>();
			m_name 		= atlas.name;
			m_texture 	= atlas.TargetTexture;
			 
			foreach( TextureAtlas.Atlas v in atlas.Textures )
			{
				Texture2D texture = AssetDatabase.LoadAssetAtPath(v.path, typeof(Texture2D)) as Texture2D;
				
				BoardTextureData data  = new BoardTextureData();
				data.isMissing = null == texture ? true : false;
				data.path = v.path;
				data.name = v.name;
				
				m_textures.Add (data);
			}
		}
	}
}


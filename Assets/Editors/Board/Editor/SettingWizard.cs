
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnnamedResource;

namespace BoardEditor
{
	public class SettingWizard : ScriptableWizard
	{
		private Vector2 ScrollView = Vector2.zero;
		private Setting m_setting = new Setting();

		public SettingWizard ()
		{
		}

		[MenuItem("Assets/" + Named.Title + "/Settings")]
		public static void Settings()
		{
			ShowWizard();
		}
		 
		public static void ShowWizard()
		{
			SettingWizard wizard = ScriptableWizard.DisplayWizard<SettingWizard>("Setting");
			wizard.minSize = wizard.maxSize = new UnityEngine.Vector2( 256, 512 );
			wizard.m_setting.Load();
		}

		void OnGUI()
		{
			EditorGUILayout.LabelField("설정");
			 

			m_setting.DefaultWidth = EditorGUILayout.IntField ( "세로", m_setting.DefaultWidth );
			m_setting.DefaultHeight = EditorGUILayout.IntField ( "세로", m_setting.DefaultHeight );
			m_setting.DefaultPixel = EditorGUILayout.IntField ( "픽셀", m_setting.DefaultPixel );

			string atlasButtonName = m_setting.DefaultAtlas == null ? "선택" : m_setting.DefaultAtlas.name;

			if( GUILayout.Button (atlasButtonName))
			{
				System.Action<GameObject> action = (GameObject atlas)=>
				{ 
					if( atlas == m_setting.DefaultAtlas )
					{
						return ;
					}

					if( null != atlas )
					{
						m_setting.DefaultAtlas = atlas; 
						m_setting.DefaultAtlasPath = AssetDatabase.GetAssetPath(m_setting.DefaultAtlas);

						m_setting.DefaultAtlasElems.Clear ();
						Repaint ();
					}
				};

				AtlasSelectorWizard.ShowWizard(action);
			}

			GUI.enabled = null == m_setting.DefaultAtlas ? false : true;

			if( GUILayout.Button ("추가"))
			{
				System.Action<TextureAtlas.Atlas> action = (TextureAtlas.Atlas atlas)=>
				{
					m_setting.DefaultAtlasElems.Add (atlas.name);
					Repaint ();
				};

				SpriteSelectorWizard.ShowWizard(m_setting.DefaultAtlas.GetComponent<TextureAtlas>(), action );
			}

			GUI.enabled = true;


			ScrollView = EditorGUILayout.BeginScrollView(ScrollView, GUILayout.Height (150));
			for( int i = 0; i < m_setting.DefaultAtlasElems.Count; ++i )
			{
				EditorGUILayout.BeginHorizontal();
				if( GUILayout.Button ("Remove", GUILayout.Width (80)))
				{
					m_setting.DefaultAtlasElems.Remove (m_setting.DefaultAtlasElems[i]);

					Repaint();

					return ;
				}

				EditorGUILayout.LabelField (m_setting.DefaultAtlasElems[i], GUILayout.Width (150));

				EditorGUILayout.EndHorizontal();
				
			}

			EditorGUILayout.EndScrollView();

			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();

			if( GUILayout.Button ("확인") )
			{
				m_setting.Save();

				Close ();
			}
		}

		void OnWizardUpdate()
		{
		}

		void OnWizardCreate()
		{
		}

	}
}


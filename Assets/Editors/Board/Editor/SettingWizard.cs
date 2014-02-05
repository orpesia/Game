
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Unnamed;
using Game;
using Game.Data;

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
			wizard.minSize = wizard.maxSize = new UnityEngine.Vector2( 250, 512 );
			wizard.m_setting.Load();
		}

		void OnGUI()
		{
			EditorGUILayout.LabelField("설정");

			m_setting.DefaultWidth = EditorGUILayout.IntField ( "세로", m_setting.DefaultWidth );
			m_setting.DefaultHeight = EditorGUILayout.IntField ( "세로", m_setting.DefaultHeight );
			m_setting.DefaultPixel = EditorGUILayout.IntField ( "픽셀", m_setting.DefaultPixel );

			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("기본 블럭");
			EditorGUILayout.Separator();

			bool isPress = false;
			if( null == m_setting.DefaultBlockSet )
			{
				isPress = GUILayout.Button ("선택", GUILayout.Width (250), GUILayout.Height(250));
			}
			else
			{
				BlockDataSet dataSet = m_setting.DefaultBlockSet.GetComponent<BlockDataSet>();
				isPress = GUILayout.Button ( new GUIContent(dataSet.Atlas.TargetTexture), GUILayout.Width (200), GUILayout.Height(200));
			}

			if( isPress )
			{
				System.Action<Object> SelectCallback = (Object obj) =>
				{
					BlockDataSet target = obj as BlockDataSet;
					m_setting.DefaultBlockSet 		= target.gameObject;
					m_setting.DefaultBlockSetPath 	= AssetDatabase.GetAssetPath(target);

					Repaint ();
				};
				
				System.Func<Object, Texture2D> ViewCallback = (Object obj) =>
				{
					BlockDataSet target = obj as BlockDataSet;
					return target.Atlas.TargetTexture;
				};
				
				ObjectSelectorWizard.ShowWizard<BlockDataSet>("Block selector", SelectCallback, ViewCallback, 1.5f );
			}

			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();

			if( GUILayout.Button ("저장") )
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


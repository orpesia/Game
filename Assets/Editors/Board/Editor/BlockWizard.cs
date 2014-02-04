//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.18408
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------
using System;

using UnityEngine;
using UnityEditor;

namespace BoardEditor
{
	public class BlockWizard : ScriptableWizard
	{
		GameObject m_prefab = null;
		GameObject m_atlas = null;
		string m_savePath = "";
		string m_blockName = "";

		public BlockWizard ()
		{
		}

		[MenuItem("Assets/" + Named.Title + "/Block Editor")]
		static void OpenEditor()
		{
			ShowWizard(null, "");
		}

		static void ShowWizard(GameObject prefab, string savePath)
		{
			BlockWizard blockWizard = DisplayWizard<BlockWizard>("Block Editor");
			blockWizard.minSize = blockWizard.maxSize = new Vector2(500, 720);
		}

		void OnGUI()
		{
			EditorGUILayout.LabelField("Block Editor");

			m_blockName = EditorGUILayout.TextField( m_blockName );

			if( GUILayout.Button ("Create"))
			{

			}

			if( null == m_prefab )
			{
				return ;
			}

			if(GUILayout.Button ("Select", GUILayout.Width (490), GUILayout.Height (490)))
			{
				Action<GameObject> selectCallback = (GameObject atlas)=>
				{
					m_atlas = atlas;
					Repaint();
				};

				AtlasSelectorWizard.ShowWizard(selectCallback);
			}

		}

	}
}


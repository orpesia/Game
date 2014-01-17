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
			boardWizard.minSize = boardWizard.maxSize = new Vector2( FixedSize.Width + (MenuWidth * boardWizard.m_functions.Length), FixedSize.Height );
		}

		private const float MenuWidth = 150.0f;
		private string[] m_functions = { "Board", "Texture" };

		void OnWizardUpdate()
		{

		}

		void OnGUI()
		{
			GUI.WindowFunction[] functions = new GUI.WindowFunction[]{ BoardWindow, TextureWindow };

			GUIStyle style = GUI.skin.GetStyle("Window");
			GUIStyleState backgupState = style.onNormal;
			style.onNormal = style.normal;

			BeginWindows();

			for( int i = 0; i < m_functions.Length; ++i )
			{

				GUILayout.Window(i, new Rect( FixedSize.Width + MenuWidth * i, 0, MenuWidth, FixedSize.Height ), functions[i], m_functions[i], style );
			}

			EndWindows();
			style.onNormal = backgupState;
		}


		public void BoardWindow( int id )
		{
			GUILayout.Button ("AA");
		}

		public void TextureWindow( int id )
		{
			GUILayout.Button ("AA");
		}
	}

}
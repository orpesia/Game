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

using System.Collections.Generic;

using UnnamedResource;

namespace BoardEditor
{
	public class BoardEditorSpriteSelectorWizard : ScriptableWizard
	{
		public delegate void SelectDelegate(TextureAtlas.Atlas atlas);

		private SelectDelegate m_delegate;

		private TextureAtlas m_atlas;
		private Vector2 m_scrollPos = Vector2.zero;
		
		public BoardEditorSpriteSelectorWizard ()
		{
		}
		
		public static void ShowWizard( TextureAtlas atlas, SelectDelegate selectDelegate)
		{
			BoardEditorSpriteSelectorWizard wizard = ScriptableWizard.DisplayWizard<BoardEditorSpriteSelectorWizard>("Sprite selector");
			wizard.m_delegate = selectDelegate;
			wizard.minSize = wizard.maxSize = new Vector2( 256, 763 );
			wizard.m_atlas = atlas;
		}
		
		void OnWizardUpdate()
		{
			
		}
		
		void OnGUI()
		{
			float top = 5;
			float nameSize = 20;
			float textureSize = 230;

			float scrollHeight = top + nameSize + textureSize + 5;

			m_scrollPos = GUI.BeginScrollView(new Rect(0, 0, 251, 758 ), m_scrollPos, new Rect(5, 5, 230, scrollHeight * m_atlas.Textures.Count ));

			for( int i = 0; i < m_atlas.Textures.Count; ++i )
			{ 
				GUI.Label ( new Rect(10,top, textureSize, nameSize ), m_atlas.Textures[i].name, EditorStyles.whiteLabel );
				top += nameSize;

				if( GUI.Button ( new Rect(10,top, textureSize, textureSize ), "" ) )
				{
					m_delegate(m_atlas.Textures[i]);
					this.Close ();
				}

				GUI.DrawTextureWithTexCoords( new Rect(15, top + 5, textureSize - 10, textureSize - 10 ), m_atlas.Target, m_atlas.Textures[i].UV );

				top += textureSize + 10;
			}

			GUI.EndScrollView();
			
		}

	}
}


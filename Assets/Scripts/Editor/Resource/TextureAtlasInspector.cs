using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnnamedResource;

namespace UnnamedEditor
{

	[CustomEditor(typeof(TextureAtlas))]
	public class TextureAtlasInspector : Editor
	{
		private int m_select = 0;

		public override void OnInspectorGUI()
		{
			TextureAtlas textureAtlas = target as TextureAtlas;

			for( int i = 0; i < textureAtlas.Textures.Count; ++i )
			{
				TextureAtlas.Atlas atlas = textureAtlas.Textures[i];

				EditorGUILayout.LabelField ("Name - " + atlas.name, GUILayout.Height(20));
				EditorGUILayout.LabelField (atlas.path, GUILayout.Height(20));
				EditorGUILayout.RectField(atlas.UV, GUILayout.Height(50));

//				if( GUILayout.Button (atlas.name, GUILayout.Height (20)) ) 
//				{
//					m_select = i;
//
//					OnPreviewGUI(GUILayoutUtility.GetRect(500, 500), EditorStyles.whiteLabel);
//				}
			}
		}

		public override void OnPreviewGUI (Rect rect, GUIStyle background)
		{
//			base.OnPreviewGUI(rect, background);
			TextureAtlas atlas = target as TextureAtlas;
//			if( atlas.Textures.Count >= m_select )
//			{
//				GUI.DrawTextureWithTexCoords(new Rect( 0, 0, 256, 256 ), atlas.Target, atlas.Textures[m_select].UV);
//			}
//			else 
			{
				GUI.DrawTexture(new Rect( 0, 0, 256, 256 ), atlas.Target, ScaleMode.StretchToFill);
			}
		}

	}


}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnnamedResource;

namespace UnnamedEditor
{

	[CustomEditor(typeof(TextureAtlas))]
	public class TextureAtlasInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			TextureAtlas textureAtlas = target as TextureAtlas;

			GUILayout.Button ( textureAtlas.TargetTexture, GUILayout.Width (256), GUILayout.Height (256) );

			for( int i = 0; i < textureAtlas.Textures.Count; ++i )
			{
				TextureAtlas.Atlas atlas = textureAtlas.Textures[i];

				EditorGUILayout.LabelField ("Name - " + atlas.name, GUILayout.Height(20));
				EditorGUILayout.LabelField (atlas.path, GUILayout.Height(20));
				EditorGUILayout.RectField(atlas.UV, GUILayout.Height(50));
			}
		}

//		public override void OnPreviewGUI (Rect rect, GUIStyle background)
//		{
////			base.OnPreviewGUI(rect, background);
//			TextureAtlas atlas = target as TextureAtlas;
////			if( atlas.Textures.Count >= m_select )
////			{
////				GUI.DrawTextureWithTexCoords(new Rect( 0, 0, 256, 256 ), atlas.Target, atlas.Textures[m_select].UV);
////			}
////			else 
//			{
//				GUI.DrawTexture(new Rect( 0, 0, 256, 256 ), atlas.TargetTexture, ScaleMode.StretchToFill);
//			}
//		}

	}


}
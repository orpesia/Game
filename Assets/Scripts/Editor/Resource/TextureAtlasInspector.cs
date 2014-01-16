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
			TextureAtlas atlas = target as TextureAtlas;
			
			foreach( KeyValuePair<string, Rect> pair in atlas.Textures )
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField (pair.Key);
				EditorGUILayout.RectField(pair.Value);
				EditorGUILayout.EndHorizontal();
			}
		}
	}


}
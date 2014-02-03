using UnityEngine;
using UnityEditor;
using System.Collections;

namespace GameEditor
{

[CustomEditor(typeof(GameData.BoardData))]
public class BoardDataInspector : Editor 
{
	public override void OnInspectorGUI()
	{
		GameData.BoardData boardData = target as GameData.BoardData;
		
		if( null == boardData.Atlas )
		{
			EditorGUILayout.LabelField("선택된 이미지가 없습니다");
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
		}
		else
		{
			GUILayout.Label ( "이름 :" + boardData.Atlas.name );
			GUILayout.Button ( boardData.Atlas.TargetTexture, GUILayout.Width (256), GUILayout.Height (256) );
		}

		EditorGUILayout.Vector2Field("Board Size ", boardData.BoardSize);
		EditorGUILayout.Vector2Field("Inner Size ", boardData.InnerSize);
		EditorGUILayout.FloatField("Pixel", boardData.Pixel );
		
	}
}

}
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace GameEditor
{

[CustomEditor(typeof(Game.Data.BoardData))]
public class BoardDataInspector : Editor 
{
	public override void OnInspectorGUI()
	{
		Game.Data.BoardData boardData = target as Game.Data.BoardData;
		
//		if( null == boardData.BlockDataSet )
//		{
//			EditorGUILayout.LabelField("선택된 블럭이 없습니다");
//			EditorGUILayout.Separator();
//			EditorGUILayout.Separator();
//		}
//		else
//		{
//			GUILayout.Label ( "이름 :" + boardData.Atlas.name );
//			GUILayout.Button ( boardData.Atlas.TargetTexture, GUILayout.Width (256), GUILayout.Height (256) );
//		}
//
//		EditorGUILayout.Vector2Field("Board Size ", boardData.BoardSize);
//		EditorGUILayout.Vector2Field("Inner Size ", boardData.InnerSize);
//		EditorGUILayout.FloatField("Pixel", boardData.Pixel );
		
	}
}

}
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Unnamed{
	
	public class Path 
	{
		static public string GetSelectionPath()
		{
			string path = "";
			if( null != UnityEditor.Selection.activeObject )
			{
				path = AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject);
			}
			
			if( string.IsNullOrEmpty(path) )
			{
				return "Assets";
			}
			else if( System.IO.Directory.Exists( path ))
			{
				return path;
			}
			else
			{
				return System.IO.Path.GetDirectoryName(path);
			}
		}
		
		static public string GetSelectionFile()
		{
			if (null != UnityEditor.Selection.activeObject)
			{
				return AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject);
			}
			else
			{
				return "";
			}
		}
	}
	
}
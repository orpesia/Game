using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnnamedEditor{

public class SceneBuilder
{
	const string MacroName = "SceneBuilder";
	const string ProjectName = "Game";
	
	[MenuItem(MacroName + "/Build/Window")]
	static void BuildWindow()
	{
		_GenericBuild(scenes, "../../Bin/Window/" + ProjectName + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);
	}
//
//	[MenuItem(MacroName + "Build IOS")]
//	static void BuildIOS()
//	{
//		_GenericBuild(scenes, "../../Bin/IOS", BuildTarget.iPhone, BuildOptions.None);
//	}

	[MenuItem(MacroName + "/Build/Android")]
	static void BuildAndroid()
	{
		string timeString = System.DateTime.Now.ToString("s");
		timeString = timeString.Replace(':', '-');
		string fileName = "../../Bin/Android/" + ProjectName + "_" + timeString + ".apk";
		_GenericBuild(GetContainBuildScene(), fileName, BuildTarget.Android, BuildOptions.None);
	}

//	[MenuItem(MacroName + "/Build Android Development")]
//	static void BuildAndroidDev()
//	{
//		_GenericBuild(GetContainBuildScene("Game"), "../../Bin/Android/" + ProjectName + "Dev.apk", BuildTarget.Android, BuildOptions.Development);
//	}
//
//	[MenuItem(MacroName + "/Build Android InGame Development")]
//	static void BuildAndroidInGameDev()
//	{
//		_GenericBuild(GetContainBuildScene("InGame"), "../../Bin/Android/"+ ProjectName + "InGameDev.apk", BuildTarget.Android, BuildOptions.Development);
//	}

	private static string[] _FindEnabledEditorScenes()
	{
		List<string> editor_scenes = new List<string>();
		foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
		{
			if (false == scene.enabled)
				continue;

			editor_scenes.Add(scene.path);
		}
		return editor_scenes.ToArray();
	}

	private static string[] GetContainBuildScene()
	{
		List<string> scenes = new List<string>();
		foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
		{
//			if (scene.path.Contains(str))
//			{
				scenes.Add(scene.path);
//			}
		}
		return scenes.ToArray();
	}

	private static void _GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
	{
		string fullpath = Path.GetDirectoryName(target_dir);
		string fileName = Path.GetFileName(target_dir);
		string[] dirs = fullpath.Split('/', '\\');

		string path = Application.dataPath;
		foreach (string dir in dirs)
		{
			if (dir == "..")
				path = Directory.GetParent(path).ToString();
			else
			{
				path += ("/" + dir);
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
					Debug.Log("CreateDirectory = " + path);
				}
			}
		}

		EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
		string res = BuildPipeline.BuildPlayer(scenes, path + "/" + fileName, build_target, build_options);
		if (res.Length > 0)
		{
			throw new Exception("BuildPlayer failure: " + res);
		}
	}

	private static string[] scenes = _FindEnabledEditorScenes();
}


}
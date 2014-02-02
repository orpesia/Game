using System;
using UnityEngine;

using System.Collections.Generic;

namespace UnnamedResource{

	public class TextureAtlas : MonoBehaviour
	{
		[Serializable]
		public class Atlas
		{
			public string name;
			public string path;
			public Rect UV; 
		};

		[HideInInspector][SerializeField] public Texture2D TargetTexture;
		[HideInInspector][SerializeField] public List<Atlas> Textures = new List<Atlas>();

		public TextureAtlas ()
		{
		}

		public bool BindTextures( string[] name, string[] paths, Rect[] uvs )
		{
			if( name.Length != uvs.Length )
			{
				return false;
			}

			for( int i = 0; i < name.Length; ++i )
			{
				this.AddTexture(name[i], paths[i], uvs[i]);
			}
			 
			return true;

		}

		public void AddTexture(string name, string path, Rect uv)
		{
			Atlas atlas = new Atlas();
			atlas.name 	= name;
			atlas.path 	= path;
			atlas.UV	= uv;

			Textures.Add (atlas);
		}

		public Atlas GetAtlasByName(string name)
		{
			foreach(Atlas atlas in Textures )
			{
				if( atlas.name == name )
				{
					return atlas;
				}
			}

			return null;
		}

}
	
}
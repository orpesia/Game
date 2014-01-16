using System;
using UnityEngine;

using System.Collections.Generic;

namespace UnnamedResource{

	public class TextureAtlas : MonoBehaviour
	{
		[SerializeField] public Texture2D Target { get; set; }
		[SerializeField] public Dictionary<string, Rect> Textures = new Dictionary<string, Rect>();

		public TextureAtlas ()
		{
		}

		public bool BindTextures( Texture2D[] texs, Rect[] uvs )
		{
			if( texs.Length != uvs.Length )
			{
				return false;
			}

			for( int i = 0; i < texs.Length; ++i )
			{
				this.AddTexture(texs[i], uvs[i]);
			}

			return true;

		}

		public void AddTexture(Texture2D texture, Rect uv)
		{
			Textures.Add (texture.name, uv);
		}

}
	
}
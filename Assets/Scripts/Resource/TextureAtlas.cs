using UnityEngine;

using System.Collections.Generic;

namespace UnnamedResource{

	public class TextureAtlas : MonoBehaviour
	{
		private Dictionary<string, Rect> m_textures = new Dictionary<string, Rect>();

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
			m_textures.Add (texture.name, uv);
		}

}
	
}
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;


namespace UnnamedResource
{
	public class TextureAtlasGenerator
	{
		public TextureAtlasGenerator ()
		{
		}

		static public void Create( string savePath, string[] paths, TextureAtlas atlas )
		{
			Texture2D[] textures = new Texture2D[paths.Length];
			for( int i = 0; i < paths.Length; ++i )
			{
				textures[i] = AssetDatabase.LoadAssetAtPath( paths[i], typeof(Texture2D)) as Texture2D;
			}

			Create( savePath, textures, atlas );
		}

		static public void Create( string savePath, Texture2D[] textures, TextureAtlas atlas )
		{
			Texture2D newTexture = new Texture2D( 2048, 2048, TextureFormat.ARGB32, false);
			Rect[] rects = newTexture.PackTextures( textures, 4, 2048 );
			newTexture.Apply ();

			System.IO.File.WriteAllBytes(savePath, newTexture.EncodeToPNG());

			newTexture = null;

			atlas.BindTextures( textures, rects );
		}

	}
}


#endif

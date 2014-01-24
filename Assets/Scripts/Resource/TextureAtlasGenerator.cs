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
			for( int i = 0; i < textures.Length; ++i )
			{
				string assetPath = AssetDatabase.GetAssetPath(textures[i]);
				TextureImporter importer = TextureImporter.GetAtPath(assetPath) as TextureImporter;
				if( null == importer )
				{
					continue;
				}

				importer.textureType = TextureImporterType.Advanced;
				importer.mipmapEnabled = false;
				importer.isReadable = true;

				AssetDatabase.ImportAsset( assetPath, ImportAssetOptions.ForceUpdate );
			}
//			AssetDatabase.SaveAssets();

			Texture2D newTexture = new Texture2D( 2048, 2048, TextureFormat.ARGB32, false);
			Rect[] rects = newTexture.PackTextures( textures, 4, 2048 );
			newTexture.Apply ();

			System.IO.File.WriteAllBytes(savePath, newTexture.EncodeToPNG());

			atlas.Target = newTexture;
			atlas.BindTextures( textures, rects );
			newTexture = null;
			
		}

	}
}


#endif

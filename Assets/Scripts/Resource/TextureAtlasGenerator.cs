using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;


namespace Unnamed
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
			string[] paths = new string[textures.Length];
			string[] names = new string[textures.Length];

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

				paths[i] = assetPath;
				names[i] = textures[i].name;
			}
			  
			Texture2D newTexture = new Texture2D( 2048, 2048, TextureFormat.ARGB32, false);
			Rect[] rects = newTexture.PackTextures( textures, 4, 2048 );
			newTexture.Apply ();

//			AssetDatabase.CreateAsset(newTexture, savePath ); 
			System.IO.File.WriteAllBytes(savePath, newTexture.EncodeToPNG());
			   
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh(); 

			atlas.TargetTexture = AssetDatabase.LoadAssetAtPath(savePath, typeof( Texture2D )) as Texture2D;

			atlas.BindTextures( names, paths, rects );
			GameObject.DestroyImmediate(newTexture); 
			newTexture = null;
			
		}

	}
}


#endif

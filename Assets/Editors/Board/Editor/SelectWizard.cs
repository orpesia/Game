
using Unnamed;

using UnityEngine;

namespace BoardEditor
{
	public class SelectWizard
	{
		public SelectWizard ()
		{
		}

		public static void AtlasSelector(System.Action<TextureAtlas> selectDelegate)
		{
			System.Action<System.Object> SelectCallback = (System.Object selected) =>
			{
				selectDelegate( selected as TextureAtlas );
			};
			
			System.Func<System.Object,ObjectViewInfo> ViewCallback = (System.Object selected) =>
			{
				TextureAtlas atlas = selected as TextureAtlas;

				return new ObjectViewInfo(atlas.TargetTexture, Share.UV);
			};
			
			ObjectSelectWizard.ShowWizard<TextureAtlas>("Atlas selector", SelectCallback, ViewCallback, 1.0f );
		}

		public static void SpriteSelector(TextureAtlas atlas, System.Action<TextureAtlas.Atlas> selectDelegate)
		{
			System.Action<System.Object> SelectCallback = (System.Object selected) =>
			{
				selectDelegate( selected as TextureAtlas.Atlas );
			};
			
			System.Func<System.Object,ObjectViewInfo> ViewCallback = (System.Object selected) =>
			{
				TextureAtlas.Atlas target = selected as TextureAtlas.Atlas;

				return new ObjectViewInfo(atlas.TargetTexture, target.UV);
			};

			System.Func<System.Object[]> FindCallback = ()=>
			{
				TextureAtlas.Atlas[] objs = new TextureAtlas.Atlas[atlas.Textures.Count];
				for( int i = 0; i < objs.Length; ++i )
				{
					objs[i] = atlas.Textures[i];
				}

				return objs;

			};

			ObjectSelectWizard.ShowWizard<TextureAtlas>("Atlas selector", SelectCallback, ViewCallback, FindCallback, 1.0f );
		}
	}
}


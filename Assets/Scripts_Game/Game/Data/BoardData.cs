using System.Collections.Generic;

using UnityEngine;

using UnnamedResource;

namespace GameData
{
	public class BoardData : MonoBehaviour
	{
		[System.Serializable]
		public class BlockElem
		{
			public List<BlockProperty> Y = new List<BlockProperty>();

			public int Count { get { return Y.Count; } }
		};

		[System.Serializable]
		public class BlockList
		{
			public List<BlockElem> X = new List<BlockElem>();
		
			public BlockElem this[int x]
			{
				get{ return X[x]; }
			}

			public BlockProperty this[int x, int y]
			{
				get{ return X[x].Y[y]; }
			}

			public int Count { get{ return X.Count; } }

		};

		[HideInInspector][SerializeField] public TextureAtlas Atlas = null;
		[HideInInspector][SerializeField] public List<string> BlockAtlas = new List<string>();

		[HideInInspector][SerializeField] public BlockList BlockProperties;
		[HideInInspector][SerializeField] public Vector2 BoardSize;
		[HideInInspector][SerializeField] public Vector2 InnerSize;
		[HideInInspector][SerializeField] public float Pixel;
		[HideInInspector][SerializeField] public List<Vector2> StartField = new List<Vector2>();


		public BoardData ()
		{
		}

		public void SetAtlas( GameObject atlas )
		{
			Atlas = atlas.GetComponent<TextureAtlas>();
		}

		public TextureAtlas.Atlas GetAtlas(int index)
		{
			if( null == Atlas || index >= BlockAtlas.Count )
			{
				return null;
			}

			return Atlas.GetAtlasByName(BlockAtlas[index]);
		}
	}
}


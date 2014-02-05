using System.Collections.Generic;

using UnityEngine;

using Unnamed;

namespace Game.Data
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

		[HideInInspector] public BlockDataSet BlockDataSet; //동적으로 붇임.

		[HideInInspector][SerializeField] public BlockList BlockProperties;
		[HideInInspector][SerializeField] public Vector2 BoardSize;
		[HideInInspector][SerializeField] public Vector2 InnerSize;
		[HideInInspector][SerializeField] public float Pixel;
		[HideInInspector][SerializeField] public List<Vector2> StartField = new List<Vector2>();


		public BoardData ()
		{
		}
	}
}


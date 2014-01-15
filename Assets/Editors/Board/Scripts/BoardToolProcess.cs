using UnityEditor;
using UnityEngine;

using GameView;

namespace BoardEditor
{
    public class BoardToolProcess : MonoBehaviour
    {
		public GameObject BoardObject { get; set; }
		public HexagonBoardRenderer HexagonRenderer { get; set; }

		public BoardToolProcess()
		{

		}

		public void NewBoard()
		{
			this.DeleteBoard();

			BoardObject = new GameObject("HexagonBoard");
			BoardObject.transform.parent = this.transform;
			BoardObject.transform.position = new Vector2( -Game.FixedSize.Width * 0.5f, Game.FixedSize.Height * 0.5f );
			BoardObject.transform.localScale = Vector2.one * BoardToolSize.BoardRatio;
			
			HexagonRenderer = BoardObject.AddComponent<HexagonBoardRenderer>();
			HexagonRenderer.Texture = null;
			HexagonRenderer.Pixel = 100;
			HexagonRenderer.Create(Vector2.zero, new Vector2( Game.FixedSize.Width, Game.FixedSize.Height));
		}

		public void DeleteBoard()
		{
			if( null != BoardObject )
			{
				Destroy (BoardObject);

				BoardObject = null;
				HexagonRenderer = null;
			}
		}

		public void ReplaceBoard()
		{
			if( null != HexagonRenderer )
			{
				//백업 필요.
				HexagonRenderer.Create(Vector2.zero, new Vector2( Game.FixedSize.Width, Game.FixedSize.Height));
			}
		}
    }
}

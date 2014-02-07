using UnityEngine;


namespace GameView
{
    public enum HexCoordinate
    {
        None = 0,
        Top,
        RightUp,
        RightDown,
        Bottom,
        LeftDown,
        LeftUp,
        Max,
    };


    public class HexagonRenderData : MonoBehaviour
    {
		public static int VertexCount = 7;
        public static int[] FixedTriangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6, 0, 6, 1 };

        public Vector2[] Vertices = new Vector2[VertexCount];
        public Vector2[] UVs = new Vector2[VertexCount];
		public int[] Triangles = new int[18];
        public int x;
        public int y;
    };
}

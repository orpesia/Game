using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameView
{
    public class HexagonBoardRenderer : MonoBehaviour
    {

        public float HexagonWidth = 0.87f;
        public float HexagonHeight = 1.0f;
        public float HexagonAttachX = 0.5f;
		public float HexagonAttachY = 0.75f;
//        public float HexagonDistanceX = 0.0f;
        public float Pixel = 50.0f;

        private Vector2 m_boardSize;
        private Vector2 m_boardPosition;

        public Texture2D Texture;

        private Vector2[] StandardVertex;
        private Mesh m_mesh = null;

        private List<List<HexagonRenderData>> m_datas = new List<List<HexagonRenderData>>();

        public HexagonBoardRenderer()
        {
        }

        void Awake()
        {
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find ("Mobile/VertexLit"));

			gameObject.AddComponent<MeshFilter>();
        }

		public void Create(Vector2 pos, Vector2 size)
		{
			this.Release();

			this.ResizeHexagon(HexagonWidth, HexagonHeight);
			
			MeshFilter mf = gameObject.GetComponent<MeshFilter>();
			m_mesh = mf.mesh = new Mesh();

			m_boardSize = size;
			m_boardPosition = pos;
			
			float width = Pixel * HexagonWidth;
			float height = Pixel * HexagonHeight;
			
			//좌상단 이므로 한 블럭씩 뺌.
			size.x = size.x - width;
			size.y = size.y - height;
			

			float attachY = height * HexagonAttachY;
			float heightCount = size.y / attachY;
			
			int startTriangle = 0;
			for (int y = 0; y < heightCount; ++y)
			{
				List<HexagonRenderData> XList = new List<HexagonRenderData>();
				
				float XOffset = 0.0f;
				float YOffset = 0.0f;
				float XLimit = size.x;
				
				if (y % 2 != 0)
				{
					XOffset = width * HexagonAttachX;
					XLimit = size.x - width;
				}
				
				int x = 0;
				
				while (x * width < XLimit)
				{
					float posX = XOffset + pos.x + width * x;
					float posY = YOffset + pos.y + -attachY * y;
					startTriangle = ApplyVertex(startTriangle, x, y, posX, posY, Pixel, XList);
					++x;
				}
				
				m_datas.Add(XList);
				
			}
			
			this.ApplyMesh();
			
			m_mesh.RecalculateNormals();
			m_mesh.RecalculateBounds();
		}

		public void Batch(Vector2 pos, Vector2 size)
		{
		}

		public void Release()
		{
			m_datas.Clear();
			

			gameObject.GetComponent<MeshFilter>().mesh = null;

			this.removeChildren();
			
		}
		
		public HexagonRenderData Raycast(Camera camera, Vector3 position)
		{
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			Collider2D collider = Physics2D.OverlapPoint(ray.origin);
			
			return (null == collider) ? null : collider.GetComponent<HexagonRenderData>();

        }

        public HexagonRenderData GetData(int x, int y)
        {
            return m_datas[y][x];
        }

        private int ApplyVertex(int startTriangle, int x, int y, float posX, float posY, float pixel, List<HexagonRenderData> XList)
        {
            //apply vertex.
            GameObject children = this.createChildren();

            HexagonRenderData hexData = children.GetComponent<HexagonRenderData>();

            for (int i = 0; i < hexData.Vertices.Length; ++i)
            {
                hexData.Vertices[i] = StandardVertex[i];
                hexData.Vertices[i].x = posX + (hexData.Vertices[i].x * pixel);
                hexData.Vertices[i].y = posY + (hexData.Vertices[i].y * pixel);
            }

            //apply triangles.
            for (int i = 0; i < hexData.Triangles.Length; ++i)
            {
                hexData.Triangles[i] = HexagonRenderData.FixedTriangles[i] + startTriangle;
            }

            //apply uvs
            for (int i = 0; i < hexData.UVs.Length; i++)
            {
                hexData.UVs[i] = new Vector2(StandardVertex[i].x / HexagonWidth, ( 1.0f + StandardVertex[i].y ) / HexagonHeight);
            }

            hexData.x = x;
            hexData.y = y;

            PolygonCollider2D collider = children.GetComponent<PolygonCollider2D>();
            Vector2[] points = new Vector2[hexData.Vertices.Length];
            for (int i = 0; i < hexData.Vertices.Length; ++i )
            {
                points[i] = hexData.Vertices[i];
            }

            points[(int)HexCoordinate.None] = hexData.Vertices[(int)HexCoordinate.LeftUp];

            collider.points = points;

            XList.Add(hexData);

            return startTriangle + hexData.Vertices.Length;
        }

        private void ApplyMesh()
        {
            List<Vector3> vertices  = new List<Vector3>();
            List<Vector2> uvs       = new List<Vector2>();
            List<int> triangles     = new List<int>();

            for (int y = 0; y < m_datas.Count; ++y)
            {
                for (int x = 0; x < m_datas[y].Count; ++x)
                {
                    vertices.AddRange(ConvertVector(m_datas[y][x].Vertices));
                    uvs.AddRange(m_datas[y][x].UVs);
                    triangles.AddRange(m_datas[y][x].Triangles);
                }
            }

            m_mesh.vertices = vertices.ToArray();
            m_mesh.triangles = triangles.ToArray();
            m_mesh.uv = uvs.ToArray();
        }

        private void ResizeHexagon(float WidthRatio, float HeightRatio)
        {
            float halfWidth = HexagonWidth * 0.5f;
            float halfHeight = HexagonHeight * 0.5f;

            StandardVertex = new Vector2[(int)HexCoordinate.Max];

            Action<HexCoordinate, Vector2> SetVertex = (HexCoordinate coord, Vector2 vert) => { StandardVertex[(int)coord] = vert; };

            SetVertex(HexCoordinate.None, new Vector2(halfWidth, -halfHeight));
            SetVertex(HexCoordinate.Top, new Vector2(halfWidth, 0.0f));
            SetVertex(HexCoordinate.RightUp, new Vector2(HexagonWidth, -0.25f));
            SetVertex(HexCoordinate.RightDown, new Vector2(HexagonWidth, -0.75f));
            SetVertex(HexCoordinate.Bottom, new Vector2(halfWidth, -1.0f));
            SetVertex(HexCoordinate.LeftDown, new Vector2(0.0f, -0.75f));
            SetVertex(HexCoordinate.LeftUp, new Vector2(0.0f, -0.25f));
        }
     
        private GameObject createChildren()
        {
            GameObject children = new GameObject("Hexagon");

            children.transform.parent = this.transform;
            children.transform.localPosition = Vector3.zero;
            children.transform.localScale = Vector3.one;

            children.AddComponent<PolygonCollider2D>();
            children.AddComponent<HexagonRenderData>();

            return children;
        }

        private void removeChildren()
        {
            HexagonRenderData[] datas = this.transform.GetComponentsInChildren<HexagonRenderData>();
            foreach(HexagonRenderData data in datas )
            {
                Destroy(data.gameObject);
            }
        }

        private Vector3[] ConvertVector(Vector2[] vector)
        {
            Vector3[] conv = new Vector3[vector.Length];
            for (int i = 0; i < vector.Length; ++i)
            {
                conv[i] = new Vector3(vector[i].x, vector[i].y);
            }
            return conv;
        }
        
    }

}


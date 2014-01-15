using System;

using UnityEngine;
using UnityEditor;

using GameView;

namespace BoardEditor
{
    class BoardToolManager : MonoBehaviour
    {
		public Color PanelSizeColor = Color.blue;
		public int MenuWidth = 200;
		public int MenuHeight = 25;
		public int MenuTint = 5;

		private int MenuXPos = 0;


		private BoardToolProcess m_process = null;

		private const string None = "None";
		
		private string m_hexWidth = None;
		private string m_hexHeight = None;
		private string m_hexAttachX = None;
		private string m_hexAttachY = None;
		private string m_hexDistanceX = None;
		private string m_pixel = None;

		void Awake()
		{
			m_process = gameObject.AddComponent<BoardToolProcess>();
		}

		void OnGUI()
		{
			this.DrawGUI ();
			this.DrawMenu ();
		}

		public void DrawGUI()
		{
			Color prevColor = Handles.color;
			Handles.color = PanelSizeColor;
			
			Handles.DrawLine( new Vector2( 0, BoardToolSize.FixHeight ), new Vector2(BoardToolSize.FixWidth, BoardToolSize.FixHeight));
			Handles.DrawLine( new Vector2( BoardToolSize.FixWidth, BoardToolSize.FixHeight ), new Vector2(BoardToolSize.FixWidth, 0));
			
			Handles.color = prevColor;
		}
		
		public void DrawMenu()
		{
			MenuXPos = Screen.width - MenuWidth;

			Rect rect = new Rect( MenuXPos, 0, MenuWidth, MenuHeight );
			if(GUI.Button ( rect, "New Board" ))
			{
				m_process.NewBoard();
				this.Binding(m_process.HexagonRenderer);
			}

			rect.y += MenuHeight + MenuTint;

			m_hexWidth = this.DoubleField(ref rect, "Hex Width", m_hexWidth);
			rect.y += MenuHeight + MenuTint;
			
			m_hexHeight = this.DoubleField(ref rect, "Hex Height", m_hexHeight);
			rect.y += MenuHeight + MenuTint;

			m_hexAttachX = this.DoubleField(ref rect, "Hex AttachX", m_hexAttachX);
			rect.y += MenuHeight + MenuTint;

			m_hexAttachY = this.DoubleField(ref rect, "Hex AttachY", m_hexAttachY);
			rect.y += MenuHeight + MenuTint;

//			m_hexDistanceX = this.DoubleField(ref rect, "Hex DistanceX", m_hexDistanceX);
//			rect.y += MenuHeight + MenuTint;

			m_pixel = this.DoubleField(ref rect, "Pixel", m_pixel);
			rect.y += MenuHeight + MenuTint;

			if(GUI.Button ( rect, "Recreate Vertex" ))
			{
				this.UnBinding(m_process.HexagonRenderer);
			}
			
		}

		private void Binding( HexagonBoardRenderer renderer)
		{
			m_hexWidth = m_process.HexagonRenderer.HexagonWidth.ToString();
			m_hexHeight = m_process.HexagonRenderer.HexagonHeight.ToString();
			m_hexAttachX = m_process.HexagonRenderer.HexagonAttachX.ToString();
			m_hexAttachY = m_process.HexagonRenderer.HexagonAttachY.ToString();
//			m_hexDistanceX = m_process.HexagonRenderer.HexagonDistanceX.ToString();

			m_pixel = m_process.HexagonRenderer.Pixel.ToString();
		}

		private void UnBinding(HexagonBoardRenderer renderer)
		{
			if( null != renderer )
			{
				m_process.HexagonRenderer.HexagonWidth = RegulateValue( 0.5f, 1.0f, m_hexWidth );
				m_process.HexagonRenderer.HexagonHeight = RegulateValue( 0.5f, 1.0f, m_hexHeight );
				m_process.HexagonRenderer.HexagonAttachX = RegulateValue( 0.5f, 1.0f, m_hexAttachX );
				m_process.HexagonRenderer.HexagonAttachY = RegulateValue( 0.5f, 1.0f, m_hexAttachY );
//				m_process.HexagonRenderer.HexagonDistanceX = RegulateValue( 0.0f, 10.0f, m_hexDistanceX );
				m_process.HexagonRenderer.Pixel = RegulateValue( 40.0f, 300.0f, m_pixel );

				m_process.ReplaceBoard();
			}
		}

		private string DoubleField( ref Rect rect, string name, string value )
		{
			rect.x = MenuXPos;
			rect.width = MenuWidth * 0.5f;
			GUI.Label (rect, name );

			rect.x = MenuXPos + rect.width;

			string result = GUI.TextField( rect, value );

			rect.x = MenuXPos;
			rect.width = MenuWidth;

			return result;
		}

		private void NextLine( ref Rect rect)
		{
			rect.y += MenuHeight;
		}

		private float RegulateValue( float min, float max, string value )
		{
			float result = Convert.ToSingle(value);

			if( result < min ) 
			{
				result = min;
			}

			if( result > max )
			{
				result = max;
			}

			return result;
		}

		
    }
}

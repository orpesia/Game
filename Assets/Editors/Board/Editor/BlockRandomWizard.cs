
using UnityEngine;
using UnityEditor;
using Game;
using Game.Data;

namespace BoardEditor
{
	public class BlockRandomWizard : ScriptableWizard
	{
		private BoardData.CustomBlocks m_selected;
		private System.Action<BoardData.CustomBlocks> m_callback;

		private BlockDataSet m_dataset;
		private BlockShareRender m_shareRender;
		private BlockRenderer m_blockRenderer;
		private int m_addedIndex;

		private Vector2 m_selectScrollBar;

		public static void ShowWizard(BlockDataSet dataset, BoardData.CustomBlocks blocks, System.Action<BoardData.CustomBlocks> callback)
		{
			BlockRandomWizard wizard = ScriptableWizard.DisplayWizard<BlockRandomWizard>("랜덤블럭 생성기");
			wizard.m_callback = callback;
			wizard.m_dataset = dataset;
			wizard.minSize = wizard.maxSize = new Vector2(200, 750);

			wizard.CreateRender();

			if( null == blocks )
			{
				wizard.m_selected = new BoardData.CustomBlocks();
			}
			else
			{
				wizard.m_selected = blocks;
			}

		}

		public BlockRandomWizard ()
		{
		}


		public void CreateRender()
		{
			m_shareRender = new BlockShareRender( m_dataset, new Vector2(5, 5), 1.0f, SelectCallback );
			m_shareRender.RenderFlag = BlockShareRender.RENDER_BLOCK; //draw only block

			m_blockRenderer = new BlockRenderer();
			m_blockRenderer.Load(m_dataset);

		}

		public void SelectCallback(GenerateType generate)
		{
			if(m_selected.Generates.Contains(generate))
			{
				return ;
			}

			m_selected.Generates.Add (generate);
		}

		void OnGUI()
		{
			Rect drawedRect = m_shareRender.Draw(this);

			drawedRect.y += 20;
			drawedRect.height = 320;


			float blockSize = 50.0f;
			float tint = 5.0f;

			Rect blockRect = new Rect( tint, tint, blockSize, blockSize );

			GUI.BeginGroup(drawedRect);

			float heightLength = ( blockSize + tint ) * m_selected.Generates.Count;

			Rect scrollViewRect = new Rect( 0, 0, 190, 280);
			Rect scrollViewScrollRect = new Rect( 0, 0, 170, heightLength);

			m_selectScrollBar = GUI.BeginScrollView(scrollViewRect, m_selectScrollBar, scrollViewScrollRect);

			for( int i = 0; i < m_selected.Generates.Count; ++i )
			{
				blockRect.y = tint + ( i * blockSize ) + ( i * 5 );

				if( true == GUI.Button (blockRect, "") )
				{
					m_selected.Generates.RemoveAt(i);
					break;
				}
				Rect labelRect = new Rect(blockRect.x + blockRect.width + 5, blockRect.y + blockRect.height * 0.3f, 100, 20);

				GUI.Label (labelRect, m_selected.Generates[i].ToString());
				m_blockRenderer.Draw (blockRect, m_selected.Generates[i]);
			}

			GUI.EndScrollView();

			if( m_selected.Generates.Count <= 0 )
			{
				GUI.enabled = false;
			}
		
			if(GUI.Button ( new Rect( 0, scrollViewRect.height + 10, 200, 20 ), "생성"))
			{
				m_callback( m_selected );
				Close ();
			}

			GUI.enabled = true;

			GUI.EndGroup();

		}
	}
}


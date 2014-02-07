using UnityEngine;
using UnityEditor;

using Unnamed;
using Game;
using Game.Data;

namespace BoardEditor
{
	public class BlockRenderer
	{
		public class RenderData
		{
			public GenerateType generate;
			public int value;
			public System.Action<Rect, RenderData> function;

			public RenderData(GenerateType generate, int value, System.Action<Rect, RenderData> function)
			{
				this.generate = generate;
				this.value = value;
				this.function = function;
			}
		};

		private RenderData[] m_renderData;
		private BlockDataSet m_blockDataSet;

		public BlockRenderer ()
		{
		}

		public void Load(BlockDataSet dataset)
		{
			m_blockDataSet = dataset;

			m_renderData = new RenderData[(int)GenerateType.Max];

			this.AddView(GenerateType.Blank, (int)BlockCode.Blank, Render );
			this.AddView(GenerateType.TypeA_A, (int)BlockCode.TypeA_A, Render );
			this.AddView(GenerateType.TypeA_B, (int)BlockCode.TypeA_B, Render );
			this.AddView(GenerateType.TypeA_C, (int)BlockCode.TypeA_C, Render );
			this.AddView(GenerateType.TypeB_A, (int)BlockCode.TypeB_A, Render );
			this.AddView(GenerateType.TypeB_B, (int)BlockCode.TypeB_B, Render );
			this.AddView(GenerateType.TypeB_C, (int)BlockCode.TypeB_C, Render );
			this.AddView(GenerateType.TypeC_A, (int)BlockCode.TypeC_A, Render );
			this.AddView(GenerateType.TypeC_B, (int)BlockCode.TypeC_B, Render );
			this.AddView(GenerateType.TypeC_C, (int)BlockCode.TypeC_C, Render );
			this.AddView(GenerateType.TypeD_A, (int)BlockCode.TypeD_A, Render );
			this.AddView(GenerateType.TypeD_B, (int)BlockCode.TypeD_B, Render );
			this.AddView(GenerateType.TypeD_C, (int)BlockCode.TypeD_C, Render );
			this.AddView(GenerateType.TypeE_A, (int)BlockCode.TypeE_A, Render );
			this.AddView(GenerateType.TypeE_B, (int)BlockCode.TypeE_B, Render );
			this.AddView(GenerateType.TypeE_C, (int)BlockCode.TypeE_C, Render );
			this.AddView(GenerateType.RandomSubA, 0, MultipleRender );
			this.AddView(GenerateType.RandomSubB, 1, MultipleRender );
			this.AddView(GenerateType.RandomSubC, 2, MultipleRender );
			this.AddView(GenerateType.RandomPhaseSubA, 0, MultipleRender );
			this.AddView(GenerateType.RandomPhaseSubB, 1, MultipleRender );
			this.AddView(GenerateType.RandomPhaseSubC, 2, MultipleRender );
		}


		public void Draw(Rect rect, GenerateType generate )
		{
			int generateNumber = (int)generate;
			m_renderData[generateNumber].function(rect, m_renderData[generateNumber]);
		}

		public void DrawMultiple( Rect rect, GenerateType[] generate )
		{
			string[] names = new string[generate.Length];
			for( int i = 0; i < generate.Length; ++i )
			{
				BlockCode code = BlockEnumSupporter.GetCode(generate[i]);

				names[i] = m_blockDataSet.FindDataByCode(code).name;
			}

			this.MultipleAtlasDraw(rect, m_blockDataSet.Atlas, names );
		}

		void AddView( GenerateType generate, int value, System.Action<Rect, RenderData> function)
		{
			m_renderData[(int)generate] = new RenderData(generate, value, function);
		}

		void Render(Rect rect, RenderData data)
		{
			Rect smallRect = GUIHelper.GetSmallRect(rect);

			TextureAtlas.Atlas sprite = m_blockDataSet.FindSpriteByCode((BlockCode)data.value);
			if( null == sprite )
			{
				GUI.DrawTexture(smallRect, EditorGUIUtility.whiteTexture);
			}
			else
			{
				GUI.DrawTextureWithTexCoords(smallRect, m_blockDataSet.Atlas.TargetTexture, sprite.UV);
			}
		}

		void MultipleRender(Rect rect, RenderData data) 
		{
			string[] names = new string[BlockConst.BlockCount];

			for( int blockIndex = 0; blockIndex < BlockConst.BlockCount; ++blockIndex )
			{
				int blockType = ((int)BlockType.TypeA) + blockIndex;
				
				BlockCode code = ((BlockType)blockType).ToBlockCode(data.value);
				BlockDataSet.KeyValue keyValue = m_blockDataSet.FindDataByCode(code);
				
				names[blockIndex] = keyValue.name;
			}
			
			MultipleAtlasDraw(rect, m_blockDataSet.Atlas, names );
		}

		private void MultipleAtlasDraw(Rect rect, TextureAtlas atlas, string[] names)
		{
			Rect smallRect = GUIHelper.GetSmallRect(rect);

			float length = names.Length + 1;
			float width = smallRect.width;
			float divide = width / length;
			
			for(int i = 0; i < names.Length; ++i)
			{ 
				TextureAtlas.Atlas sprite = atlas.GetSpriteByName(names[i]);
				if( null == sprite )
				{
					GUI.DrawTexture(smallRect, EditorGUIUtility.whiteTexture);
				}
				else
				{
					Rect spriteUV = sprite.UV;
					spriteUV.width -= ( sprite.UV.width / length ) * i;
					
					GUI.DrawTextureWithTexCoords( smallRect, atlas.TargetTexture, spriteUV );
				}
				
				smallRect.x += divide;
				smallRect.width -= divide;
			}
		}
		
	}
}


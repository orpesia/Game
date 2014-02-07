//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.18408
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------
using System;

namespace Game
{
	public enum GenerateType
	{
		Blank = 0,
		TypeA_A,
		TypeA_B,
		TypeA_C,
		TypeB_A,
		TypeB_B,
		TypeB_C,
		TypeC_A,
		TypeC_B,
		TypeC_C,
		TypeD_A,
		TypeD_B,
		TypeD_C,
		TypeE_A,
		TypeE_B,
		TypeE_C,
		RandomSubA,
		RandomSubB,
		RandomSubC,
		RandomPhaseSubA,
		RandomPhaseSubB,
		RandomPhaseSubC,
		Max,
		RandomCustomA = 1001,
		RandomCustomB,
		RandomCustomC,
		RandomCustomD,
		RandomCustomE,
		RandomCustomF,
		RandomCustomG,
		RandomCustomH,
		RandomCustomI,
		RandomCustomJ,
	}

	public enum BlockType
	{
		Blank = 0,
		TypeA,
		TypeB,
		TypeC,
		TypeD,
		TypeE,
	}

	public static class BlockConst
	{
		public const int Bit = 4;
		public const int BlockCount = 5;
		public const int SubCount = 3;
		public const int CustomCount = 10;
	};

	public enum BlockCode //--> SettingType.Fixed
	{
		Blank = ((int)BlockType.Blank) << BlockConst.Bit,

		TypeA_A = ((int)BlockType.TypeA) << BlockConst.Bit,
		TypeA_B,
		TypeA_C,
		
		TypeB_A = ((int)BlockType.TypeB) << BlockConst.Bit,
		TypeB_B,
		TypeB_C,
		
		TypeC_A = ((int)BlockType.TypeC) << BlockConst.Bit,
		TypeC_B,
		TypeC_C,

		TypeD_A = ((int)BlockType.TypeD) << BlockConst.Bit,
		TypeD_B,
		TypeD_C,

		TypeE_A = ((int)BlockType.TypeE) << BlockConst.Bit,
		TypeE_B,
		TypeE_C,
	}

	public enum BlockItemType
	{
		Blank,
		Attack,
		Skill,
		Max,
	}


	public static class BlockEnumSupporter
	{
		public static int ToBlockType( this BlockCode type)
		{
			return ((int)type) >> BlockConst.Bit;
		}

		public static BlockCode ToBlockCode( this BlockType type, int codeIndex )
		{
			return (BlockCode)((int)type << BlockConst.Bit) + codeIndex;
		}

		public static GenerateType GetGenerate(BlockCode code)
		{
			switch(code)
			{
			case BlockCode.Blank: return GenerateType.Blank;
			case BlockCode.TypeA_A: return GenerateType.TypeA_A;
			case BlockCode.TypeA_B: return GenerateType.TypeA_B;
			case BlockCode.TypeA_C: return GenerateType.TypeA_C;
			case BlockCode.TypeB_A: return GenerateType.TypeB_A;
			case BlockCode.TypeB_B: return GenerateType.TypeB_B;
			case BlockCode.TypeB_C: return GenerateType.TypeB_C;
			case BlockCode.TypeC_A: return GenerateType.TypeC_A;
			case BlockCode.TypeC_B: return GenerateType.TypeC_B;
			case BlockCode.TypeC_C: return GenerateType.TypeC_C;
			case BlockCode.TypeD_A: return GenerateType.TypeD_A;
			case BlockCode.TypeD_B: return GenerateType.TypeD_B;
			case BlockCode.TypeD_C: return GenerateType.TypeD_C;
			case BlockCode.TypeE_A: return GenerateType.TypeE_A;
			case BlockCode.TypeE_B: return GenerateType.TypeE_B;
			case BlockCode.TypeE_C: return GenerateType.TypeE_C;
			}

			return GenerateType.Blank;
		}

		public static BlockCode GetCode(GenerateType generate)
		{
			switch(generate)
			{
			case GenerateType.Blank: 	return BlockCode.Blank;
			case GenerateType.TypeA_A: 	return BlockCode.TypeA_A;
			case GenerateType.TypeA_B: 	return BlockCode.TypeA_B;
			case GenerateType.TypeA_C: 	return BlockCode.TypeA_C;
			case GenerateType.TypeB_A: 	return BlockCode.TypeB_A;
			case GenerateType.TypeB_B: 	return BlockCode.TypeB_B;
			case GenerateType.TypeB_C: 	return BlockCode.TypeB_C;
			case GenerateType.TypeC_A: 	return BlockCode.TypeC_A;
			case GenerateType.TypeC_B: 	return BlockCode.TypeC_B;
			case GenerateType.TypeC_C: 	return BlockCode.TypeC_C;
			case GenerateType.TypeD_A: 	return BlockCode.TypeD_A;
			case GenerateType.TypeD_B: 	return BlockCode.TypeD_B;
			case GenerateType.TypeD_C: 	return BlockCode.TypeD_C;
			case GenerateType.TypeE_A: 	return BlockCode.TypeE_A;
			case GenerateType.TypeE_B: 	return BlockCode.TypeE_B;
			case GenerateType.TypeE_C: 	return BlockCode.TypeE_C;
			}
			
			return BlockCode.Blank;
		}

		public static bool IsCustomRandom(GenerateType generate)
		{
			if((int)generate >= (int)GenerateType.RandomCustomA && (int)generate <= (int)GenerateType.RandomCustomJ)
			{
				return true;
			}
			return false;
		}

	};

//	public static class EnumExtension
//	{
//		public static int ToInt( this Enum type )
//		{
//			return (int)type;
//		}
//
//		public static int ToInt( this BlockItemType type )
//		{
//			return (int)type;
//		}
//	};

}


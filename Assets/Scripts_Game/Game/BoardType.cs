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
		TypeA,
		TypeB,
		TypeC,
		TypeD,
		TypeE,
		TypeA_A,
		TypeA_B,
		TypeA_C,
		TypeB_A,
		TypeB_B,
		TypeB_C,
		TypeC_A,
		TypeC_B,
		TypeC_C,
		RandomSubA,
		RandomSubB,
		RandomSubC,
		RandomPhase,

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
		public const int SubCount = 3;
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



namespace Utility
{

public class BitMacro
{
	#region Static Methods
	public static short MAKEWORD(byte a, byte b)
	{
		return ((short)(((byte)(a & 0xff)) | ((short)((byte)(b & 0xff))) << 8));
	}
	
	public static byte LOBYTE(short a)
	{
		return ((byte)(a & 0xff));
	}
	
	public static byte HIBYTE(short a)
	{
		return ((byte)(a >> 8));
	}
	
	public static int MAKELONG(short a, short b)
	{
		return (((int)(a & 0xffff)) | (((int)(b & 0xffff)) << 16));
	}
	
	public static short HIWORD(int a)
	{
		return ((short)(a >> 16));
	}
	
	public static short LOWORD(int a)
	{
		return ((short)(a & 0xffff));
	}

	public static int MAKEQUAD(char a, char b, char c, char d)
	{
		return (((int)(a & 0xff)) | (((int)(b & 0xff)) << 8) | (((int)(c & 0xff)) << 16) | (((int)(d & 0xff)) << 24) );
	}
	
	public static char QUADHIWORD(int a)
	{
		return ((char)(a >> 24));
	}
		
	public static char QUADMIDHIWORD(int a)
	{
			return ((char)(a >> 16));
		
	}
	
	public static char QUADMIDLOWORD(int a)
	{
		return ((char)(a >> 8));
		
	}

	public static char QUADLOWORD(int a)
	{
		return ((char)(a & 0xff));
	}
	#endregion
}

}
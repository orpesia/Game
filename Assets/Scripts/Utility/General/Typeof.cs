//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;

namespace UnnamedUtility
{
		public class Typeof
		{
		//최상위 부모 바로 아래의 자식을 찾아준다.
			static public Type FindRootTree<Target>( Type type )
			{
				Type origin = type;
				Type basis = type;
				while( null != basis && basis != typeof(Target) ) 
				{
					origin = basis;
					basis = basis.BaseType;
				}
				
				return origin;
			}
		}
}


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
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UnnamedUtility
{
	public static class ObjectExtension
	{
		static public Delegate FunctionByString<T> ( this T value, string func )
		{
			MethodInfo method = typeof(T).GetMethod(func);
			List<Type> args = new List<Type>(
				method.GetParameters().Select(p => p.ParameterType));
			Type delegateType;
			if (method.ReturnType == typeof(void)) {
				delegateType = Expression.GetActionType(args.ToArray());
			} else {
				args.Add(method.ReturnType);
				delegateType = Expression.GetFuncType(args.ToArray());
			}

			return Delegate.CreateDelegate(delegateType, null, method);

		}
		
	}
}


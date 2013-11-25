using System;

namespace Surreal.Core
{
	public abstract class FTransactionBase
	{
		public abstract void SaveObject( UObject Object );
		public abstract void SaveArray( UObject Object, FArray Array, int Index, int Count, int Oper, int ElementSize, STRUCT_AR Serializer, STRUCT_DTOR Destructor);
		public abstract void Apply();
	}
}


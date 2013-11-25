using System;

namespace Surreal.Core
{
	public abstract class FMalloc
	{
		public abstract object Malloc (UInt16 Count, string Tag);
		public abstract object Realloc (object Original, UInt16 Count, string Tag);
		public abstract void Free (object Original);
		public abstract void DumpAllocs();
		public abstract void HeapCheck();
		public abstract void Init();
		public abstract void Exit();
	}
}


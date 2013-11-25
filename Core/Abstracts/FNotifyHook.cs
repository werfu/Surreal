using System;

namespace Surreal.Core
{
	public abstract class FNotifyHook
	{
		public abstract void NotifyDestroy(object Src);
		public abstract void NotifyPreChange(object Src);
		public abstract void NotifyPostChange(object Src);
		public abstract void NotifyExec(object Src, string Cmd);
	}
}


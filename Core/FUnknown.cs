using System;

namespace Surreal.Core
{
	public abstract class FUnknown
	{
		public FUnknown ()
		{
		}

		public abstract UInt16 QueryInterface (FGuid RefIID, Func InterfacePtr);
		public abstract UInt16 AddRef ();
		public abstract UInt16 Release ();
	}
}


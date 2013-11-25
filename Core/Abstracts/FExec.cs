using System;

namespace Surreal.Core
{
	public abstract class FExec
	{
		public abstract bool Exec( string Cmd, ref FOutputDevice Ar);
	}
}


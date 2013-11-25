using System;

namespace Surreal.Core
{
	public abstract class FOutputDevice
	{
		public abstract void Serialize( string v, EName Event);
		public abstract void Log (string s);
		public abstract void Logf (string s);
	}
}
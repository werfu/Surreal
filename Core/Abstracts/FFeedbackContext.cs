using System;

namespace Surreal.Core
{
	public abstract class FFeedbackContext : FOutputDevice
	{
		public abstract bool YesNof ( string Fmt,  params string[] strings);
		public abstract void BeginSlowTask ( string Task, bool StatusWindow, bool Cancelable);
		public abstract void EndSlowTask();
		public abstract bool StatusUpdatef( int Numerator, int Denominator, string Fmt, params string[] strings);
		public abstract void SetContext ( FContextSupplier InSupplier);
	}
}


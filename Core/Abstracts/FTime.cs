using System;

namespace Surreal.Core
{
	public class FTime
	{
		public const float FIXTIME = 4294967296f;
		private long v;
		public FTime ()
		{
			v = 0;
		}
		public FTime (float f)
		{
			v = (long) (f * FIXTIME);
		}

		public FTime (double d)
		{
			v = (long) (d * FIXTIME);
		}

		private FTime (FTime t)
		{
			v = t.v;
		}

		public float GetFloat ()
		{
			return v / FIXTIME;
		}

		public static FTime operator +(FTime t, float f) { return new FTime(t.v+f*FIXTIME); }
		public static float operator -(FTime t1, FTime t2) { return (t1.v - t2.v) / FIXTIME; }
		public static FTime operator *(FTime t, float f) {return new FTime(t.v*f);}
		public static FTime operator /(FTime t, float f) {return new FTime(t.v/f);}
		public static bool   operator ==(FTime t1, FTime t2) {return t1.v==t2.v;}
		public static bool   operator !=(FTime t1, FTime t2) {return t1.v!=t2.v;}
		public static bool   operator >(FTime t1, FTime t2) {return t1.v>t2.v;}
		public static bool   operator <(FTime t1, FTime t2) {return t1.v<t2.v;}
		public override bool Equals (Object t)
		{
			if (t is FTime) {
				return ((FTime) t).v == v;
			} else return false; }
		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}
}


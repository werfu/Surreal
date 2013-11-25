using System;

namespace Surreal.Core
{
	public abstract class FArchive
	{
		#region Privates

		private int ArVer;
		private int ArNetVer;
		private int ArLicenseeVer;
		private bool ArIsLoading;
		private bool ArIsSaving;
		private bool ArIsTrans;
		private bool ArIsPersistent;
		private bool ArIsError;
		private bool ArForEdit;
		private bool ArForClient;
		private bool ArForServer;

		#endregion

		#region Ctor

		public FArchive () {
			this.ArVer = Global.PACKAGE_FILE_VERSION;
			this.ArNetVer = Global.ENGINE_NEGOTIATION_VERSION;
			this.ArLicenseeVer = Global.PACKAGE_FILE_VERSION_LICENSEE;
			this.ArIsLoading = false;
			this.ArIsSaving = false;
			this.ArIsTrans = false;
			this.ArIsPersistent = false;
			this.ArIsError = false;
			this.ArForEdit = false;
			this.ArForClient = false;
			this.ArForServer = false;
		}

		public static T Arctor<T>(ref FArchive Ar)
		{
			T tmp = new T();
			Ar.Put (tmp);
			return tmp;
		}

		#endregion

		#region File functions
		public virtual bool AtEnd ()
		{
			int Pos = Tell ();
			return Pos != (int) Global.INDEX_NONE && Pos >= TotalSize();
		}
		public virtual bool Close () { return !ArIsError; }
		public abstract void CountBytes( UInt32 InNum, UInt32 InMax);
		public abstract void Flush ();
		public virtual bool GetError() { return ArIsError; }
		public abstract void Precache ( int HintCount );
		public abstract void Seek (int pos);
		public abstract int TotalSize();
		public virtual int Tell () {return (int) Global.INDEX_NONE; }
		#endregion

		#region Unreal functions
		public abstract void AttachLazyLoader ( ref FLazyLoader LazyLoader );
		public abstract void DetachLazyLoader ( ref FLazyLoader LazyLaoder );
		public virtual int MapName( ref FName Name) { return 0; }
		public virtual int MapObject (ref UObject Object) {	return 0; }
		public abstract void Preload( ref UObject Object);
		#endregion


		#region Serialization functions
		public abstract void Serialize ( byte[] buffer );
		public abstract void Serialize ( byte[] buffer, int length );
		public abstract void Serialize ( TArray<byte> buffer, int length);

		public virtual void SerializeBits (byte[] buffer, int LengthBits)
		{
			Serialize (buffer, (LengthBits + 7) / 8);
			if (IsLoading ()) {
				buffer[LengthBits / 8] &= (byte) (( 1 << (LengthBits&7)) - 1);
			}
		}

		public virtual void SerializeInt( ref UInt16 value, UInt16 Max)
		{
			this.Put(value);
		}

		public FArchive ByteOrderSerialize (byte[] buffer, int length)
		{
			if (BitConverter.IsLittleEndian || !ArIsPersistent) {
				Serialize (buffer, length);
			} else {
				for ( int i = length -1 ; i>= 0; i--)
				{
					Serialize( new byte[] { buffer[i] }, 1);
				}
			}
			return this;
		}

		#endregion

		#region Puts

		public virtual FArchive Put(ref FName N)
		{
			return this;
		}

		public virtual FArchive Put(ref UObject Res)
		{
			return this;
		}

		public abstract FArchive Put(ref FTime F);
		public abstract FArchive Put(ref FCompactIndex I);

		public FArchive Put (object o)
		{
			/// TODO : Write a generic object to byte transformer
			throw new NotImplementedException();
		}

		public FArchive Put (byte b)
		{
			this.Serialize(new byte[] { b });
			return this;
		}

		public FArchive Put (bool b)
		{
			this.Serialize(BitConverter.GetBytes(b));
			return this;
		}

		public FArchive Put (char c)
		{
			this.Serialize(BitConverter.GetBytes(c));
			return this;
		}
		public FArchive Put (double d)
		{
			this.Serialize(BitConverter.GetBytes(d));
			return this;
		}
		public FArchive Put (float f)
		{
			this.Serialize(BitConverter.GetBytes(f));
			return this;
		}
		public FArchive Put (int i)
		{
			this.Serialize(BitConverter.GetBytes(i));
			return this;
		}
		public FArchive Put (long l)
		{
			this.Serialize(BitConverter.GetBytes(l));
			return this;
		}
		public FArchive Put (short s)
		{
			this.Serialize(BitConverter.GetBytes(s));
			return this;
		}
		public FArchive Put (uint u)
		{
			this.Serialize(BitConverter.GetBytes(u));
			return this;
		}
		public FArchive Put (ulong u)
		{
			this.Serialize(BitConverter.GetBytes(u));
			return this;
		}
		public FArchive Put (ushort u)
		{
			this.Serialize(BitConverter.GetBytes(u));
			return this;
		}

		public FArchive Put (string s)
		{
			System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
			this.Serialize(encoder.GetBytes(s));
			return this;
		}

		#endregion

		#region Properties

		public int Ver ()
		{
			return ArVer;
		}

		public int NetVer()
		{
			return ArNetVer & 0x7fffffff;
		}

		public int LicenseeVer ()
		{
			return ArLicenseeVer;
		}

		public bool IsLoading ()
		{
			return ArIsLoading;
		}

		public bool IsSaving()
		{
			return ArIsSaving;
		}

		public bool IsTrans()
		{
			return ArIsTrans;
		}

		public bool IsNet()
		{
			return (ArNetVer&0x80000000)!=0;
		}

		public bool IsPersistant()
		{
			return ArIsPersistent;
		}

		public bool IsError ()
		{
			return ArIsError;
		}

		public bool ForEdit ()
		{
			return ArForEdit;
		}

		public bool ForClient ()
		{
			return ArForClient;
		}

		public bool ForServer ()
		{
			return ArForServer;
		}

		#endregion

	}

}


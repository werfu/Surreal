using System;

namespace Surreal.Core
{
	public abstract class FFileManager
	{
		public abstract FArchive CreateFileReader ( string Filename, UInt16 ReadFlags = 0, FOutputDevice Error = Global.GNull);
		public abstract FArchive CreateFileWriter ( string Filename, UInt16 ReadFlags = 0, FOutputDevice Error = Global.GNull);
		public abstract int FileSize ( string Filename);
		public abstract bool Delete ( string Filename, bool RequireExists = false, bool EvenReadOnly = false );
		public abstract bool Copy ( string Dest, string Src, bool Replace = true, bool EvenIfReadOnly = false, bool Attributes = false, ProgressDelegate Progress = null);
		public abstract bool Move ( string Dest, string Src, bool Replace = true, bool EvenIfReadOnly = false, bool Attributes = false);
		public abstract Int32 GetGlobalTime ( string Filename );
		public abstract bool SetGlobalTime ( string Filename );
		public abstract bool MakeDirectory ( string Path, bool Tree = false );
		public abstract bool DeleteDirectory ( string Path, bool RequireExist = false, bool Tree = false );
		public abstract TArray<FString> FindFiles ( string Filename, bool Files, bool Directories );
		public abstract bool SetDefaultDirectory ( string Filename );
		public abstract FString GetDefaultDirectory ();
		public virtual void Init (bool Startup) {}
	}
}


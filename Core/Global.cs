using System;
using Core;

namespace Surreal.Core
{
	public enum Constants : uint {
		MAXBYTE = 0xff,
		MAXWORD	= 0xffffU,
		MAXUInt16 = 0xffffffffU,
		MAXSBYTE = 0x7f,
		MAXSWORD = 0x7fff,
		MAXINT = 0x7fffffff,
		UNICODE_BOM = 0xfeff
	}

	public enum EFileTimes
	{
		FILETIME_Create      = 0,
		FILETIME_LastAccess  = 1,
		FILETIME_LastWrite   = 2,
	}
	public enum EFileWrite
	{
		FILEWRITE_NoFail            = 0x01,
		FILEWRITE_NoReplaceExisting = 0x02,
		FILEWRITE_EvenIfReadOnly    = 0x04,
		FILEWRITE_Unbuffered        = 0x08,
		FILEWRITE_Append			= 0x10,
		FILEWRITE_AllowRead         = 0x20,
	}
	public enum EFileRead
	{
		FILEREAD_NoFail             = 0x01,
	}

	public enum EConstructorType {
		ENativeConstructor,
		EStaticConstructor,
		EInPlaceConstructor
	}

	public delegate void STRUCT_AR (ref FArchive Ar, IntPtr TPtr);
	public delegate void STRUCT_DTOR ( IntPtr TPtr );

	public delegate void ProgressDelegate(float Fraction);
	public delegate void FuncDelegate();
	public delegate FConfigCache FConfigCacheDelegate();

	public static partial class Global {
		public static int INDEX_NONE = -1;
		public static int PACKAGE_FILE_VERSION;
		public static int ENGINE_NEGOTIATION_VERSION;
		public static int PACKAGE_FILE_VERSION_LICENSEE;

		public static AApp App;

		#region Global Vars
		public static FMemStack 			GMem;
		public const FOutputDevice 		GNull = null;
		public static FOutputDevice			GLog;
		public static FOutputDevice		    GThrow;
		public static FOutputDeviceError	GError;
		public static FFeedbackContext		GWarn;
		public static FConfigCache			GConfig;
		public static FTransactionBase		GUndo;
		public static FOutputDevice			GLogHook;
		public static FExec					GExec;
		public static FMalloc				GMalloc;
		public static FFileManager			GFileManager;
		public static USystem 				GSys;
		public static UProperty				GProperty;
		public static byte[]				GPropAddr;
		public static USubsystem			GWindowManager;
		public static string				GErrorHist;
		public static string                GTrue, GFalse, GYes, GNo, GNone;
		public static string				GCdPath;
		public static float					GSecondsPerCycle;
		public static FTime					GTempTime;
		public static FuncDelegate			GTempFunc;
		public static UInt32				GTicks;
		public static int                   GScriptCycles;
		public static UInt16				GPageSize;
		public static UInt16				GProcessorCount;
		public static UInt16				GPhysicalMemory;
		public static UInt16                GUglyHackFlags;
		public static bool					GIsScriptable;
		public static bool					GIsEditor;
		public static bool					GIsClient;
		public static bool					GIsServer;
		public static bool					GIsCriticalError;
		public static bool					GIsStarted;
		public static bool					GIsRunning;
		public static bool					GIsSlowTask;
		public static bool					GIsGuarded;
		public static bool					GIsRequestingExit;
		public static bool					GIsStrict;
		public static bool                  GScriptEntryTag;
		public static bool                  GLazyLoad;
		public static bool					GUnicode;
		public static bool					GUnicodeOS;
		public static FGlobalMath			GMath;
		public static URenderDevice			GRenderDevice;
		public static FArchive         		GDummySave;
		public static UInt16				GCurrentViewport;
		public const string					GPackage = "GPackage";
		public static UInt16[]				GCRCTable;
		#endregion

		#region Macros Replacement

		public static void Check(bool condition)
		{
			if(!condition)
				throw new Exception();
		}

		public static uint NEXT_BITFIELD(uint b)
		{
			if(BitConverter.IsLittleEndian)
				return b << 1;
			else
				return b >> 1;
		}

		public static uint FIRST_BITFIELD()
		{
			if(BitConverter.IsLittleEndian)
				return 1;
			else
				return 0x80000000;
		}

		public static uint INTEL_ORDER(uint b)
		{
			if(BitConverter.IsLittleEndian)
				return b;
			else
				return (b >> 24) + ((b >> 8) & 0xff00) + ((b << 8) & 0xff0000) + (b << 24);
		}



		#endregion
	}
}


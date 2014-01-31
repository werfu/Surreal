using System;
using Surreal.Core;
using System.Reflection;
using System.Diagnostics;

namespace Core
{
	public abstract  class AApp
	{
		// Most of the stuff comes from UnFile.h

		#region Global Init and exit.
		abstract public void Init ( string InPackage, string InCmdLine, ref FMalloc InMalloc, ref FOutputDevice InLog, ref FOutputDeviceError InError, ref FFeedbackContext InWarn, ref FFileManager InFileManager, FConfigCacheDelegate ConfigFactory, bool RequireConfig);

		abstract public void PreExit();
		abstract public void Exit ();
		#endregion

		#region Logging and critical errors.
		abstract public void RequestExit (bool Force);
		abstract public void FailAssert( string Expr, string File, int Line, params object[] argsRest );
		abstract public void Unwindf (string fmt, params object[] argsRest);
		abstract public string GetSystemErrorMessage (int Error = 0);
		abstract public void DebugMessagef( string fmt, params object[] argsRest);
		abstract public void Msgf( string fmt, params object[] argsRest);
		abstract public void GetLastError();
		#endregion

		#region Misc
		abstract public Assembly GetDllHandle( string DllName );
		abstract public void FreeDllHandle( Assembly DllHandle );
		abstract public void GetDllExport( Assembly DllHandle, string ExportName );
		abstract public void LaunchURL( string URL, string Parms = null, FString Error = null );
		abstract public Process CreateProc( string URL, string Parms , bool bRealTime );
		abstract public bool GetProcReturnCode( Process ProcHandle, int ReturnCode );
		abstract public void EnableFastMath( bool Enable );
		abstract public FGuid CreateGuid();
		abstract public void CreateTempFilename( string Path, string Result256 );
		abstract public void CleanFileCache();
		abstract public bool FindPackageFile( string In, FGuid Guid, out string Out );
		#endregion

		#region Clipboard
		abstract public void ClipboardCopy( string Str );
		abstract public FString ClipboardPaste();
		#endregion

		abstract public void Throwf (string Fmt, params object[] list);
		abstract public void Format (FString Src, ref TMultimap<FString, FString> Map);

		#region Localization
		abstract public string Localize( string  Section, string  Key, string Package=Global.GPackage, string  LangExt = null, bool Optional = false );
		abstract public string LocalizeError( string  Key, string Package=Global.GPackage, string  LangExt = null );
		abstract public string LocalizeProgress( string  Key, string Package=Global.GPackage, string  LangExt = null );
		abstract public string LocalizeQuery( string  Key, string Package=Global.GPackage, string  LangExt = null );
		abstract public string LocalizeGeneral( string  Key, string Package=Global.GPackage, string  LangExt = null );
		#endregion

		#region File functions.
		abstract public string FExt( string Filename );
		abstract public bool UpdateFileModTime( string Filename );
		abstract public FString GetGMTRef();
		#endregion

		#region OS functions.
		abstract public string CmdLine();
		abstract public string BaseDir();
		abstract public string Package();
		abstract public string ComputerName();
		abstract public string UserName();
		#endregion

		#region Timing functions.
		abstract public UInt32 Cycles();
		abstract public FTime Seconds();

		abstract public void SystemTime( ref int Year, ref int Month, ref int DayOfWeek, ref int Day, ref int Hour, ref int Min, ref int Sec, ref int MSec );
		abstract public string Timestamp();
		abstract public FTime SecondsSlow();
		abstract public void Sleep( float Seconds );
		#endregion

		#region Character type functions.
		abstract public char ToUpper (char c);
		abstract public char ToLower (char c);
		abstract public bool Alpha (char c);
		abstract public bool Digit (char c);
		abstract public bool IsAlnum (char c);
		#endregion

		/*-----------------------------------------------------------------------------
  		String functions.
		-----------------------------------------------------------------------------*/

		abstract public string ToAnsi( string Str );
		abstract public string ToUnicode( string Str );
		abstract public string FromAnsi( string Str );
		abstract public string FromUnicode( string Str );
		abstract public bool IsPureAnsi( string Str );

		abstract public string Strcpy( string Dest, string Src );
		abstract public int Strcpy( string String );
		abstract public int Strlen( string String );
		abstract public string Strstr( string String, string Find );
		abstract public string Strchr( string String, int c );
		abstract public string Strcat( string Dest, string Src );
		abstract public int Strcmp( string String1, string String2 );
		abstract public int Stricmp( string String1, string String2 );
		abstract public int Strncmp( string String1, string String2, int Count );
		abstract public string StaticString1024();
		abstract public string AnsiStaticString1024();

		abstract public string Spc( int Num );
		abstract public string Strncpy( string Dest, string Src, int Max);
		abstract public string Strncat( string Dest, string Src, int Max);
		abstract public string Strupr( string String );
		abstract public string Strfind(string Str, string Find);
		abstract public UInt32 StrCrc( string Data );
		abstract public UInt32 StrCrcCaps( string Data );
		abstract public int Atoi( string Str );
		abstract public float Atof( string Str );
		abstract public int Strtoi( string Start, string End, int Base );
		abstract public int Strnicmp( string A, string B, int Count );
		abstract public int Sprintf( string Dest, string Fmt, params object[] list );
		abstract public void TrimSpaces( string String);

		public delegate int QSORT_COMPARE<T>(T A, T B);
		abstract public void Qsort<T>( object Base, int Num, int Width, QSORT_COMPARE<T> Compare );

		//
		// Case insensitive string hash function.
		//
		UInt16 Strihash( string Data )
		{
			UInt16 Hash=0;
			foreach(char c in Data)
			{
				char Ch = ToUpper(c);
				byte  B  = (byte)Ch;
				Hash     = (ushort) (((Hash >> 8) & 0x00FFFFFF) ^ Global.GCRCTable[(Hash ^ B) & 0x000000FF]);
			}
			return Hash;
		}

		/*-----------------------------------------------------------------------------
  		Parsing functions.
		-----------------------------------------------------------------------------*/

		abstract public bool ParseCommand( string Stream, string Match );
		abstract public bool Parse( string Stream, string Match, ref FName Name );
		abstract public bool Parse( string Stream, string Match, ref UInt32 Value );
		abstract public bool Parse( string Stream, string Match, ref FGuid Guid );
		abstract public bool Parse( string Stream, string Match, string Value, int MaxLen );
		abstract public bool Parse( string Stream, string Match, ref byte Value );
		abstract public bool Parse( string Stream, string Match, ref sbyte Value );
		abstract public bool Parse( string Stream, string Match, ref UInt16 Value );
		abstract public bool Parse( string Stream, string Match, ref Int16 Value );
		abstract public bool Parse( string Stream, string Match, ref float Value );
		abstract public bool Parse( string Stream, string Match, ref int Value );
		abstract public bool Parse( string Stream, string Match, ref FString Value );
		abstract public bool Parse( string Stream, string Match, ref UInt64 Value );
		abstract public bool Parse( string Stream, string Match, ref Int64 Value );
		abstract public bool ParseUBOOL( string Stream, string Match, ref bool OnOff );
		abstract public bool ParseObject( string Stream, string Match, UClass Type, ref UObject DestRes, UObject InParent );
		abstract public bool ParseLine( string Stream, ref string Result, int MaxLen, bool Exact = false );
		abstract public bool ParseLine( string Stream, ref FString Resultd, bool Exact = false );
		abstract public bool ParseToken( ref string Str, string Result, int MaxLen, bool UseEscape );
		abstract public bool ParseToken( ref string Str, ref FString Arg, bool UseEscape );
		abstract public FString ParseToken( ref string Str, bool UseEscape );
		abstract public void ParseNext( string Stream );
		abstract public bool ParseParam( string Stream, string Param );

		/*-----------------------------------------------------------------------------
  		Math functions.
		-----------------------------------------------------------------------------*/

		abstract public double Exp( double Value );
		abstract public double Loge( double Value );
		abstract public double Fmod( double A, double B );
		abstract public double Sin( double Value );
		abstract public double Cos( double Value );
		abstract public double Acos( double Value );
		abstract public double Tan( double Value );
		abstract public double Atan( double Value );
		abstract public double Atan2( double Y, double X );
		abstract public double Sqrt( double Value );
		abstract public double Pow( double A, double B );
		abstract public bool IsNan( double Value );
		abstract public int Rand();
		abstract public float Frand();
		abstract public float RandRange( float Min, float Max );
		abstract public int RandRange( int Min, int Max );
	
		abstract public int Round( float Value );
		abstract public int Floor( float Value );
		abstract public int Ceil( float Value );


		/*-----------------------------------------------------------------------------
  		Array functions.
		-----------------------------------------------------------------------------*/

// Core functions depending on TArray and FString.
		abstract public bool LoadFileToArray( ref TArray<byte> Result, string Filename, FFileManager FileManager);
		abstract public bool LoadFileToString( ref FString Result, string Filename, FFileManager FileManager );
		abstract public bool SaveArrayToFile( ref TArray<byte> Array, string Filename, FFileManager FileManager );
		abstract public bool SaveStringToFile( ref FString String, string Filename, FFileManager FileManager );

///*-----------------------------------------------------------------------------
//  Memory functions.
//-----------------------------------------------------------------------------*/
//
//void* appMemmove( void* Dest, const void* Src, int Count );
//int Memcmp( const void* Buf1, const void* Buf2, int Count );
//bool appMemIsZero( const void* V, int Count );
//uint32 MemCrc( const void* Data, int Length, DWORD CRC=0 );
//void appMemswap( void* Ptr1, void* Ptr2, DWORD Size );
//void appMemset( void* Dest, int C, int Count );
//
//#ifndef DEFINED_appMemcpy
//void appMemcpy( void* Dest, const void* Src, int Count );
//#endif
//
//#ifndef DEFINED_appMemzero
//void appMemzero( void* Dest, int Count );
//#endif
//
////
//// C style memory allocation stubs.
////
//#define appMalloc     GMalloc->Malloc
//#define appFree       GMalloc->Free
//#define appRealloc    GMalloc->Realloc
//
////
//// C++ style memory allocation.
////
//		#include <new>
//
//inline void* operator new( size_t Size, string Tag ) throw (std::bad_alloc)
//{
//  guardSlow(new);
//#ifdef UTGLR_NO_APP_MALLOC
//  return malloc(Size);
//#else
//  return appMalloc( Size, Tag );
//#endif
//  unguardSlow;
//}
//inline void* operator new( size_t Size ) throw (std::bad_alloc)
//{
//  guardSlow(new);
//#ifdef UTGLR_NO_APP_MALLOC
//  return malloc(Size);
//#else
//  return appMalloc( Size, TEXT("new") );
//#endif
//  unguardSlow;
//}
//inline void operator delete( void* Ptr ) throw ()
//{
//  guardSlow(delete);
//#ifdef UTGLR_NO_APP_MALLOC
//  free(Ptr);
//#else
//  appFree( Ptr );
//#endif
//  unguardSlow;
//}
//		#if PLATFORM_NEEDS_ARRAY_NEW
//inline void* operator new[]( size_t Size, string Tag ) throw (std::bad_alloc)
//{
//  guardSlow(new);
//#ifdef UTGLR_NO_APP_MALLOC
//  return malloc(Size);
//#else
//  return appMalloc( Size, Tag );
//#endif
//  unguardSlow;
//}
//inline void* operator new[]( size_t Size ) throw (std::bad_alloc)
//{
//  guardSlow(new);
//#ifdef UTGLR_NO_APP_MALLOC
//  return malloc(Size);
//#else
//  return appMalloc( Size, TEXT("new") );
//#endif
//  unguardSlow;
//}
//inline void operator delete[]( void* Ptr ) throw ()
//{
//  guardSlow(delete);
//#ifdef UTGLR_NO_APP_MALLOC
//  free(Ptr);
//#else
//  appFree( Ptr );
//#endif
//  unguardSlow;
//}
//#endif

		/*-----------------------------------------------------------------------------
	  	Math.
		-----------------------------------------------------------------------------*/

		abstract public byte CeilLogTwo( UInt16 Arg );

		/*-----------------------------------------------------------------------------
  		MD5 functions.
		-----------------------------------------------------------------------------*/

		//
		// MD5 Context.
		//
		unsafe public struct FMD5Context
		{
			public fixed UInt16 state[4]; // 4
			public fixed UInt16 count[2]; // 2
			public fixed byte buffer[64]; // 64
		};


		//
		// MD5 functions.
		//!!it would be cool if these were implemented as subclasses of
		// FArchive.
		//
		abstract public void MD5Init( FMD5Context context );
		abstract public void MD5Update( FMD5Context context, byte[] input, int inputLen );
		abstract public void MD5Final( byte[] digest, FMD5Context context );
		abstract public void MD5Transform( UInt16[] state, byte[] block );
		abstract public void MD5Encode( byte[] output, UInt16[] input, int len );
		abstract public void MD5Decode( UInt16[] output, byte[] input, int len );
	}
}


using System;

namespace Surreal.Core
{
	public class UObject : FUnknown
	{
		#region Constructor
		static UObject() {
			GObjHash = new UObject[4096];
			GLanguage = new char[64];
		}

		public UObject() {}
		public UObject (ref UObject Src) {}
		public UObject (EConstructorType type, UClass InClass, string InName, string InPackageName, UInt16 InFlags) 
		{} 
		public UObject (EConstructorType type, string InName, string InPackageName, UInt16 InFlags) 
		{}
		public UObject (EConstructorType type, UClass InClass, UObject InOuter, FName InName, UInt16 Flags)
		{}

		// public static operator #( ref UObject src) {}

		public void StaticConstructor() {
		}

		public static void InternalConstructor( Func x ) {
		}

		protected override void Finalize() {
			try
			{
				// Cleanup statements...
			}
			finally
			{
				base.Finalize();
			}
		}

		#endregion

		public static string StaticConfigName() { return "System"; }

		#region Privates
		private int Index;
		private UObject HashNext;
		private FStateFrame StateFrame;
		private ULinkerLoad _Linker;
		private int _LinkerIndex;
		private UObject Outer;
		private UInt16 ObjectFlag;
		private FName Name;
		private UClass Class;
		#endregion

		#region Static
		static bool GObjInitialized;
		static bool GObjNoRegister;
		static int GObjBeginLoadCount;
		static int GObjRegisterCount;
		static UObject[] GObjHash;
		static UObject[] GAutoRegister;
		static TArray<UObject> GObjLoaded;
		static TArray<UObject> GObjRoot;
		static TArray<UObject> GObjObjects;
		static TArray<int> GObjAvailble;
		static TArray<UObject> GObjLoader;
		static UPackage GObjTransientPkg;
		static string GObjCachedLanguage;
		static TArray<UObject> GObjRegistrants;
		static TArray<FPreferencesInfo> GObjPreferences;
		static TArray<FRegistryObjectInfo> GObjDrivers;
		static TMultimap<FName, FName> GObjPackageRemap;
		static char[] GLanguage;
		#endregion

		#region Private Function
		private void AddObject( int index ) { throw new NotImplementedException (); }
		private void HashObject() {  throw new NotImplementedException ();  }
		private void UnHashObject(int OuterIndex) {  throw new NotImplementedException ();  }
		private void SetLinker( ULinkerLoad L, int I) {  throw new NotImplementedException (); }
		#endregion

		#region Private systemwide functions
		private static ULinkerLoad GetLoader ( int i ) {
			throw new NotImplementedException ();
		}

		private static FName MakeUniqueObjectName ( UObject Outer, UClass Class) {
			throw new NotImplementedException ();
		}

		private static bool ResolveName( ref UObject Outer, string Name, bool Create, bool Throw) {
			throw new NotImplementedException ();
		}

		private static void SafeLoadError( UInt16 LoadFlags, string Error, string Fmt, params object[] list) {
			throw new NotImplementedException ();
		}

		private static void PurgeGarbage() {
			throw new NotImplementedException ();
		}

		private static void CacheDrivers( bool ForceRefresh) {
			throw new NotImplementedException ();
		}
		#endregion

		#region Uobject virtual functions
		public virtual void ProcessEvent (UFonction Function, object[] Params, out object Result);
		public virtual void ProcessState (float DeltaSeconds);
		public virtual bool ProcessRemoteFunction (UFonction Function, object[] Params, ref FFrame Stack );
		public virtual void Modify();
		public virtual void PostLoad ();
		public virtual void Destroy ();
		public virtual void Serialize ( ref FArchive Ar );
		public virtual bool IsPendingKill ( ) { return false; }
		public virtual EGotoState GotoState ( FName State );
		public virtual int GotoLabel (FName Label);
		public virtual void InitExecution ();
		public virtual void ShutdownAfterError();
		public virtual void PostEditChange();
		public virtual void CallFunction (ref FFrame TheStack, out object Result, UFonction Function);
		public virtual bool ScriptConsoleExec( string Cmd, ref FOutputDevice Ar, UObject Executor);
		public virtual void Register ();
		public virtual void LanguageChange();
		#endregion

		#region Systemwide Functions UnObjBas.h:551
		#endregion
	}
}


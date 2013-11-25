using System;

namespace Surreal.Core
{
	public abstract class FConfigCache
	{
		public abstract bool GetBool(string Section, string Key, out bool Value, string Filename);
		public abstract bool GetInt(string Section, string Key, out int Value, string Filename);
		public abstract bool GetFloat(string Section, string Key, out float Value, string Filename);
		public abstract bool GetString(string Section, string Key, out string Value, int Size, string Filename);
		public abstract bool GetString(string Section, string Key, out FString Value, string Filename);
		public abstract bool GetStr(string Section, string Key, out string Value, string Filename);
		public abstract bool GetSection(string Section, out String Value, int Size, string Filename);
		public abstract TMultimap<FString, FString> GetSectionPrivate(string Section, bool Force, bool Const, string Filename);
		public abstract void EmptySection(string Section, string Filename);
		public abstract void SetBool(string Section, string Key, bool Value, string Filename);
		public abstract void SetInt(string Section, string Key, int Value, string Filename);
		public abstract void SetFloat(string Section, string Key, float Value, string Filename);
		public abstract void SetString(string Section, string Key, String Value, string Filename);
		public abstract void Flush(bool Read, string Filename);
		public abstract void Detach(string Filename);
		public abstract void Init(string InSystem, string InUser, bool RequireConfig);
		public abstract void Exit();
		public abstract void Dump( ref FOutputDevice Ar);
	}
}


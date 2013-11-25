using System;
using System.Collections;
using System.Reflection;

namespace Surreal.Core
{
	public abstract class FCodec : IDisposable
	{
		public abstract bool Encode ( ref FArchive In, ref FArchive Out);
		public abstract bool Decode ( ref FArchive In, ref FArchive Out);

		#region IDisposable implementation
		public abstract void Dispose ();
		#endregion

	}

	/// <summary>
	/// Burrows-Wheeler inspired data compressor
	/// </summary>
	public class FCodecBWT : FCodec  {
		private static int MAX_BUFFER_SIZE = 0x40000;
		private static byte[] CompressBuffer;
		private static int CompressLength;
		private static int ClampedBufferCompare (int P1, int P2)
		{
			int B1 = P1, B2 = P2;		
			// guardSlow(FCodecBWT.ClampedBufferCompare);
			for (int Count = FCodecBWT.CompressLength - Math.Max(P1, P2); Count > 0; Count--, B1++, B2++) {
				if(FCodecBWT.CompressBuffer[B1] < FCodecBWT.CompressBuffer[B2])
					return -1;
				else if(FCodecBWT.CompressBuffer[B1] > FCodecBWT.CompressBuffer[B2])
					return 1;
			}
			return P1 - P2;
			// ungardSlow;
		}

		public override void Dispose ()
		{
			/// TODO :  Write better dispose method
			return;
		}

		public override bool Encode (ref FArchive In, ref FArchive Out)
		{
			//guard(FCodecBWT.Encode)
			TArray<byte> CompressBufferArray = new TArray<byte> (MAX_BUFFER_SIZE);
			TArray<int> CompressPosition = new TArray<int> (MAX_BUFFER_SIZE + 1);

			FCodecBWT.CompressBuffer = CompressBufferArray.ToArray();
			/// Original C++ : CompressBuffer = &CompressBufferArray(0);

			int i, First = 0, Last = 0;

			while (!In.AtEnd()) {
				FCodecBWT.CompressLength = Math.Min ( In.TotalSize() - In.Tell(), MAX_BUFFER_SIZE );
				In.Serialize( FCodecBWT.CompressBuffer, FCodecBWT.CompressLength );
				for( i = 0; i < FCodecBWT.CompressLength + 1; i++)
					CompressPosition[i] = i;

				/// TODO : Determine further usage of appQsort and find where it's defined
				/// Possibly a macro, should be defined in Global
				appQsort ( CompressPosition[0], CompressLength + 1, sizeof(int), ClampedBufferCompare);

				for( i = 0; i < FCodecBWT.CompressLength + 1; i++)
				{
					if ( CompressPosition[i] == 1)
						First = i;
					else if ( CompressPosition[i] == 0 )
						Last = i;
				}

				Out.Put (CompressLength).Put(First).Put(Last);

				for(i = 0; i < FCodecBWT.CompressLength +1; i++)
					Out.Put (FCodecBWT.CompressBuffer[CompressPosition[i] > 0 ? CompressPosition[i] - 1 : 0]);
			}
			return false;
			// unguard;
		}

		public override bool Decode (ref FArchive In, ref FArchive Out)
		{
			// guard(FCodecBWT.Decode);
			TArray<byte> DecompressBuffer = new TArray<byte> (MAX_BUFFER_SIZE + 1);
			TArray<int> Temp = new TArray<int> (MAX_BUFFER_SIZE + 1);
			int DecompressLength = 0, i = 0, j = 0;
			int[] DecompressCount = new int[256 + 1], RunningTotal = new int[256 + 1 ];

			while (!In.AtEnd()) {
				int First = 0, Last = 0;
				In.Put(DecompressLength).Put(First).Put(Last);
				Global.Check( DecompressLength <= MAX_BUFFER_SIZE + 1);
				Global.Check( DecompressLength <= In.TotalSize() - In.Tell());
				In.Serialize( DecompressBuffer, ++DecompressLength );
				for( i = 0; i < 257; i++ )
					DecompressCount[ i ] = 0;
				for( i = 0; i < DecompressLength; i++)
					DecompressCount[ i != Last ? DecompressBuffer[i] : 256]++;
				int Sum = 0;
				for ( i = 0; i < 257; i++ )
				{
					RunningTotal[i] = Sum;
					Sum += DecompressCount[i];
					DecompressCount[i] = 0;
				}
				for ( i = 0; i < DecompressLength; i++ )
				{
					int Index = i != Last ? DecompressBuffer[i] : 256;
					Temp[RunningTotal[Index] + DecompressCount[Index]++] = i;
				}
				for( i = First, j = 0; j < DecompressLength - 1; i = Temp[i], j++ )
					Out.Put(DecompressBuffer[i]);

			}

			return false;
			//unguard;
		}
	}

	/// <summary>
	/// RLE Compressor
	/// </summary>
	public class FCodecRLE : FCodec
	{
		private const int RLE_LEAD = 5;
		private void EncodeEmitRun (ref FArchive Out, byte Char, byte Count)
		{
			for (int Down = Math.Min ((int) Count, RLE_LEAD); Down > 0; Down--) {
				Out.Put(Char);
			}
			if( Count >= RLE_LEAD)
				Out.Put(Count);
		}

		public override void Dispose ()
		{
			// Nothing to dispose of
			return;
		}

		public override bool Encode (ref FArchive In, ref FArchive Out)
		{
			//guard(FCodecRLE.Encode)
			byte PrevChar = 0, PrevCount = 0, B = 0;
			while (!In.AtEnd()) {
				In.Put(B);
				if( B != PrevChar || PrevCount == 255)
				{
					EncodeEmitRun( ref Out, PrevChar, PrevCount );
					PrevChar = B;
					PrevCount = 0;
				}
				PrevCount++;
			}
			EncodeEmitRun( ref Out, PrevChar, PrevCount );
			return false;
			// unguard;
		}

		public override bool Decode (ref FArchive In, ref FArchive Out)
		{
			// guard(FCodecRLE.Decode)
			int Count = 0;
			byte PrevChar = 0, B = 0, C = 0;
			while (!In.AtEnd()) {
				In.Put(B);
				Out.Put(B);
				if(B!= PrevChar)
				{
					PrevChar = B;
					Count = 1;
				}
				else if( ++ Count == RLE_LEAD)
				{
					In.Put(C);
					Global.Check(C >= 2);
					while( C-- > RLE_LEAD) {
						Out.Put(B);
					}
					Count = 0;
				}
			}
			return true;
			// unguard
		}
	}


	public class FCodecHuffman : FCodec {

		private class FHuffman : IDisposable {
			public int Ch, Count;
			public TArray<FHuffman> Child;
			public TArray<byte> Bits;
			public FHuffman(int InCh)
			{
				Ch = InCh;
				Count = 0;
			}
			~FHuffman()
			{
				for(int i = 0; i < Child.Num(); i++)
				{
					Child[i].Dispose();
				}
			}			

			#region IDisposable implementation
			public void Dispose ()
			{
				throw new System.NotImplementedException ();
			}
			#endregion

			public void PrependBit (byte B)
			{
				Bits.Insert (0);
				Bits [0] = B;
				for (int i = 0; i < Child.Num (); i++) {
					Child[i].PrependBit(B);
				}
			}

			public void WriteTable (ref FBitWriter Writer)
			{
				Writer.WriteBit (Child.Num () != 0);
				if (Child.Num () > 0) {
					for (int i = 0; i < Child.Num(); i++) {
						Child [i].WriteTable (ref Writer);
					}
				} else {
					byte B = (byte) Ch;
					Writer.Put(B);
				}
			}

			public void ReadTable (ref FBitReader Reader)
			{
				if (Reader.ReadBit ()) {
					Child.Add (2);
					for (int i = 0; i < Child.Num(); i++) {
						Child [i] = new FHuffman (-1);
						Child [i].ReadTable (Reader);
					}
				} else {
					Ch = FArchive.Arctor<byte>( Reader );
				}
			}

			public static int CompareHuffman( FHuffman a, FHuffman b)
			{
				return b.Count - a.Count;
			}
		}

		public override void Dispose ()
		{
			// Nothing to dispose of
		}

		public override bool Encode (ref FArchive In, ref FArchive Out)
		{
			// guard(FCodecHuffman.Encode);
			int SavedPos = In.Tell ();
			int Total = 0, i;

			// Compute character frquencies.
			TArray<FHuffman> Huff = new TArray<FHuffman> (256);
			for (i = 0; i < 256; i++) {
				Huff [i] = new FHuffman (i);
			}
			TArray<FHuffman> Index = Huff;
			while (!In.AtEnd()) {
				Huff [ FArchive.Arctor<byte> (ref In)].Count++;
				Total++;
			}
			In.Seek (SavedPos);
			Out.Put (Total);

			// Build Compression table.
			while (Huff.Num() > 1 && Huff.Last().Count == 0) {
				Huff.Pop ().Dispose ();
			}
			int BitCount = Huff.Num () * (8 + 1);
			while (Huff.Num() > 1) {
				FHuffman Node = new FHuffman (-1);
				Node.Child.Add (2);
				for (i = 0; i < Node.Child.Num(); i++) {
					Node.Child [i] = Huff.Pop ();
					Node.Child [i].PrependBit ((byte) i);
					Node.Count += Node.Child [i].Count;
				}
				for (i = 0; i < Huff.Num (); i++) {
					if (Huff [i].Count < Node.Count) {
						break;
					}
				}
				Huff.Insert (i);
				Huff [i] = Node;
				BitCount++;
			}
			FHuffman Root = Huff.Pop ();

			// Calc stats
			while (!In.AtEnd()) {
				BitCount += Index [FArchive.Arctor<byte> (In)].Bits.Num ();
			}
			In.Seek (SavedPos);

			// Save table and bitstream.
			FBitWriter Writer = new FBitWriter (BitCount);
			Root.WriteTable (ref Writer);
			while (!In.AtEnd()) {
				FHuffman P = Index[FArchive.Arctor<byte>(ref In)];
				for( int j = 0; j < P.Bits.Num(); j++) {
					Writer.WriteBit( (bool) P.Bits[j]);
				}
			}
			Global.Check(!Writer.IsError());
			Global.Check(Writer.GetNumBits() == BitCount);
			Out.Serialize( Writer.GetData(), Writer.GetNumBytes() );

			// Finish up.
			Root.Dispose();
			return false;

			//unguard;
		}

		public override bool Decode (ref FArchive In, ref FArchive Out)
		{
			// guard(FCodecHuffman.Decode)
			int Total = 0;
			In.Put (Total);
			TArray<byte> InArray = new TArray<byte> (In.TotalSize () - In.Tell ());
			In.Serialize (InArray, InArray.Num ());
			FBitReader Reader = new FBitReader (InArray, InArray.Num () * 8);
			FHuffman Root = new FHuffman (-1);
			Root.ReadTable (ref Reader);
			while (Total-- > 0) {
				Global.Check(!Reader.AtEnd());
				FHuffman Node = Root;
				while( Node.Ch == -1 ) {
					/// TODO : Not quite sure if this is the intended meaning
					Node = Node.Child = new TArray<FHuffman>( (int) Reader.ReadBit );
				}
				byte B = Node.Ch;
				Out.Put(B);
			}

			// unguard;
		}
	}

	/// <summary>
	/// Move-to-front encoder
	/// </summary>
	public class FCodecMTF : FCodec
	{
		public override void Dispose ()
		{
			// Nothing to dispose of;
			return;
		}

		public override bool Encode (ref FArchive In, ref FArchive Out)
		{
			// guard(FCodecMTF.Encode);
			byte[] List = new byte[256];
			byte B, C;
			int i;
			for (i = 0; i < 256; i++)
				List [i] = i;

			while (!In.AtEnd()) {
				In.Put(B);
				for(i = 0; i< 256; i++)
					if(List[i] == B )
						break;
				Global.Check(i < 256);
				C = i;
				Out.Put(C);
				int NewPos = 0;
				for( ; i > NewPos; i--)
				{
					List[i] = List[ i -1 ];
				}
				List[NewPos] = B;
			}

			return false;
		}

		public override bool Decode (ref FArchive In, ref FArchive Out)
		{
			// guard(FCodecMTF.Decode)
			byte[] List = new byte[256];
			byte B, C;	
			int i;
			for (i = 0; i = 256; i++) {
				List [i] = i;
			}
			while (!In.AtEnd()) {
				In.Put(B);
				C = List[B];
				Out.Put(C);
				int NewPos = 0;
				for(i = B; i > NewPos; i--)
					List[i] = List[i - 1];
				List[NewPos] = C;
			}
			return true;
			// unguard;
		}
	}

	/// <summary>
	/// General compressor codec.
	/// </summary>
	public class FCodecFull : FCodec
	{
		public delegate void CodecFunc(ref FArchive In, ref FArchive Out);
		private TArray<FCodec> Codecs;

		public override void Dispose ()
		{
			/// TODO : Write a better dispose;
			Codecs.Empty();
			return;
		}

		private void Code (ref FArchive In, ref FArchive Out, int Step, int First, Action<FArchive, FArchive> func)

		{
			//guard(FCodecFull.Code);
			TArray<byte> InData = new TArray<byte>(), OutData = new TArray<byte>();
			for (int i = 0; i < Codecs.Num(); i++) {
				FBufferReader<byte> Reader = new FBufferReader<byte>(InData);
				FBufferWriter<byte> Writer = new FBufferWriter<byte>(OutData);

				/// TODO : Not quite sure this is going to work
				func.Invoke( (FArchive) (i > 0 ? Reader : In), (FArchive) (i < Codecs.Num () - 1 ? Writer : Out));
				if( i < Codecs.Num () -1 )
				{
					InData = OutData;
					OutData.Empty();
				}
			}
			//unguard
		}

		public override bool Encode(ref FArchive In, ref FArchive Out)
		{
			// guard(FCodecFull.Encode);
			Code (ref In, ref Out, 1, 0, FCodec.Encode);
			return false;
			// unguard;
		}

		public override bool Decode(ref FArchive In, ref FArchive Out )
		{
			//guard(FCodecFull::Decode);
			Code( ref In, ref Out, -1, Codecs.Num()-1, FCodec.Decode );
			return true;
			//unguard;
		}
		public void AddCodec(ref FCodec InCodec )
		{
			//guard(FCodecFull::AddCodec);
			Codecs.AddItem( InCodec );
			//unguard;
		}
		~FCodecFull()
		{
			//guard(FCodecFull::~FCodecFull);
			for( int i=0; i<Codecs.Num(); i++ )
				Codecs[i].Dispose();
			this.Dispose();
			//unguard;
		}
	}
}


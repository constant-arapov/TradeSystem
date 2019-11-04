namespace DEALS{

	// Scheme "DEALS" description
	
	
		public partial class deal
		{
			public const int table_index = 0;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public deal() {
			}
			public deal(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public deal(System.IO.UnmanagedMemoryStream stream_, int offset_) {
				Data = stream_;
				offset = offset_;
			}
			public System.IO.UnmanagedMemoryStream Data {
				set {
					stream = value;
					reader = null;
					writer = null;
				}
				get {
					return stream;
				}
			}
			public int Offset {
				get	{
					return offset;
				}
			}
			private void checkReader() {
				if (reader == null)
					reader = new System.IO.BinaryReader(stream);
			}
			private void checkWriter() {
				if (writer == null)
					writer = new System.IO.BinaryWriter(stream);
			}
		
			
				
					public  long replID
					{
						get {
							checkReader();
							stream.Position = offset + 0;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 0;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long replRev
					{
						get {
							checkReader();
							stream.Position = offset + 8;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 8;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long replAct
					{
						get {
							checkReader();
							stream.Position = offset + 16;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 16;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int sess_id
					{
						get {
							checkReader();
							stream.Position = offset + 24;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 24;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int isin_id
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long id_deal
					{
						get {
							checkReader();
							stream.Position = offset + 32;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 32;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int pos
					{
						get {
							checkReader();
							stream.Position = offset + 40;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 40;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xpos
					{
						get {
							checkReader();
							stream.Position = offset + 44;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 44;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int amount
					{
						get {
							checkReader();
							stream.Position = offset + 52;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 52;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xamount
					{
						get {
							checkReader();
							stream.Position = offset + 56;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 56;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long id_ord_buy
					{
						get {
							checkReader();
							stream.Position = offset + 64;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 64;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long id_ord_sell
					{
						get {
							checkReader();
							stream.Position = offset + 72;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 72;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal price
					{
						get {
							checkReader();
							stream.Position = offset + 80;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 80;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte price_scale
					{
						get {
							stream.Position = offset + 80;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime moment
					{
						get {
							checkReader();
							stream.Position = offset + 92;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 92;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  ulong moment_ns
					{
						get {
							checkReader();
							stream.Position = offset + 104;
							return reader.ReadUInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 104;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte nosystem
					{
						get {
							checkReader();
							stream.Position = offset + 112;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 112;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
	


}
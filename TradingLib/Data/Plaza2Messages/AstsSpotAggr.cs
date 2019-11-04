namespace AstsSpotAggr{

	// Scheme "AstsSpotAggr" description
	
	
		public partial class ORDERBOOK
		{
			public const int table_index = 0;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public ORDERBOOK() {
			}
			public ORDERBOOK(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public ORDERBOOK(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string SECBOARD
					{
						get {
							checkReader();
							stream.Position = offset + 24;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
						}
						set {
							checkWriter();
							stream.Position = offset + 24;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
						}
					}
					
				
			
		
			
				
					public  string SECCODE
					{
						get {
							checkReader();
							stream.Position = offset + 29;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 12);
						}
						set {
							checkWriter();
							stream.Position = offset + 29;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 12);
						}
					}
					
				
			
		
			
				
					public  string BUYSELL
					{
						get {
							checkReader();
							stream.Position = offset + 42;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 1);
						}
						set {
							checkWriter();
							stream.Position = offset + 42;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 1);
						}
					}
					
				
			
		
			
				
					public  decimal PRICE
					{
						get {
							checkReader();
							stream.Position = offset + 44;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 44;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.6", (decimal)value);
						}
					}
					
					public byte PRICE_scale
					{
						get {
							stream.Position = offset + 44;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime ACTIVATIONTIME
					{
						get {
							checkReader();
							stream.Position = offset + 54;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 54;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  long QUANTITY
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
					
				
			
		
			
				
					public  decimal YIELD
					{
						get {
							checkReader();
							stream.Position = offset + 72;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 72;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d9.2", (decimal)value);
						}
					}
					
					public byte YIELD_scale
					{
						get {
							stream.Position = offset + 72;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal REPOVALUE
					{
						get {
							checkReader();
							stream.Position = offset + 79;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 79;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.2", (decimal)value);
						}
					}
					
					public byte REPOVALUE_scale
					{
						get {
							stream.Position = offset + 79;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
		}
	
	


}
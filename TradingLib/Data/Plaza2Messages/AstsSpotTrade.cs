namespace AstsSpotTrade{

	// Scheme "AstsSpotTrade" description
	
	
		public partial class ALL_TRADES
		{
			public const int table_index = 0;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public ALL_TRADES() {
			}
			public ALL_TRADES(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public ALL_TRADES(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  long TRADENO
					{
						get {
							checkReader();
							stream.Position = offset + 24;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 24;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime TRADETIME
					{
						get {
							checkReader();
							stream.Position = offset + 32;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 32;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  string SECBOARD
					{
						get {
							checkReader();
							stream.Position = offset + 42;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
						}
						set {
							checkWriter();
							stream.Position = offset + 42;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
						}
					}
					
				
			
		
			
				
					public  string SECCODE
					{
						get {
							checkReader();
							stream.Position = offset + 47;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 12);
						}
						set {
							checkWriter();
							stream.Position = offset + 47;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 12);
						}
					}
					
				
			
		
			
				
					public  decimal PRICE
					{
						get {
							checkReader();
							stream.Position = offset + 60;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 60;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.6", (decimal)value);
						}
					}
					
					public byte PRICE_scale
					{
						get {
							stream.Position = offset + 60;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  long QUANTITY
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
					
				
			
		
			
				
					public  decimal VALUE
					{
						get {
							checkReader();
							stream.Position = offset + 80;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 80;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.2", (decimal)value);
						}
					}
					
					public byte VALUE_scale
					{
						get {
							stream.Position = offset + 80;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal ACCRUEDINT
					{
						get {
							checkReader();
							stream.Position = offset + 90;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 90;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.2", (decimal)value);
						}
					}
					
					public byte ACCRUEDINT_scale
					{
						get {
							stream.Position = offset + 90;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal YIELD
					{
						get {
							checkReader();
							stream.Position = offset + 100;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 100;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d9.2", (decimal)value);
						}
					}
					
					public byte YIELD_scale
					{
						get {
							stream.Position = offset + 100;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  string PERIOD
					{
						get {
							checkReader();
							stream.Position = offset + 107;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 1);
						}
						set {
							checkWriter();
							stream.Position = offset + 107;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 1);
						}
					}
					
				
			
		
			
				
					public  string SETTLECODE
					{
						get {
							checkReader();
							stream.Position = offset + 109;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 12);
						}
						set {
							checkWriter();
							stream.Position = offset + 109;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 12);
						}
					}
					
				
			
		
			
				
					public  string BUYSELL
					{
						get {
							checkReader();
							stream.Position = offset + 122;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 1);
						}
						set {
							checkWriter();
							stream.Position = offset + 122;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 1);
						}
					}
					
				
			
		
			
				
					public  decimal REPORATE
					{
						get {
							checkReader();
							stream.Position = offset + 124;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 124;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.6", (decimal)value);
						}
					}
					
					public byte REPORATE_scale
					{
						get {
							stream.Position = offset + 124;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal REPOVALUE
					{
						get {
							checkReader();
							stream.Position = offset + 134;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 134;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.2", (decimal)value);
						}
					}
					
					public byte REPOVALUE_scale
					{
						get {
							stream.Position = offset + 134;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal REPO2VALUE
					{
						get {
							checkReader();
							stream.Position = offset + 144;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 144;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.2", (decimal)value);
						}
					}
					
					public byte REPO2VALUE_scale
					{
						get {
							stream.Position = offset + 144;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  int REPOTERM
					{
						get {
							checkReader();
							stream.Position = offset + 156;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 156;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int MICROSECONDS
					{
						get {
							checkReader();
							stream.Position = offset + 160;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 160;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime SETTLEDATE
					{
						get {
							checkReader();
							stream.Position = offset + 164;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 164;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
		}
	
	


}
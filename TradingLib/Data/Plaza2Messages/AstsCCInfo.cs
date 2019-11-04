namespace AstsCCInfo{

	// Scheme "AstsCCInfo" description
	
	
		public partial class SECURITIES
		{
			public const int table_index = 0;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public SECURITIES() {
			}
			public SECURITIES(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public SECURITIES(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string SECNAME
					{
						get {
							checkReader();
							stream.Position = offset + 42;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 30);
						}
						set {
							checkWriter();
							stream.Position = offset + 42;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 30);
						}
					}
					
				
			
		
			
				
					public  string REMARKS
					{
						get {
							checkReader();
							stream.Position = offset + 73;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 8);
						}
						set {
							checkWriter();
							stream.Position = offset + 73;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 8);
						}
					}
					
				
			
		
			
				
					public  string SHORTNAME
					{
						get {
							checkReader();
							stream.Position = offset + 82;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 10);
						}
						set {
							checkWriter();
							stream.Position = offset + 82;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 10);
						}
					}
					
				
			
		
			
				
					public  string STATUS
					{
						get {
							checkReader();
							stream.Position = offset + 93;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 1);
						}
						set {
							checkWriter();
							stream.Position = offset + 93;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 1);
						}
					}
					
				
			
		
			
				
					public  string TRADINGSTATUS
					{
						get {
							checkReader();
							stream.Position = offset + 95;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 1);
						}
						set {
							checkWriter();
							stream.Position = offset + 95;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 1);
						}
					}
					
				
			
		
			
				
					public  string MARKETCODE
					{
						get {
							checkReader();
							stream.Position = offset + 97;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
						}
						set {
							checkWriter();
							stream.Position = offset + 97;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
						}
					}
					
				
			
		
			
				
					public  string INSTRID
					{
						get {
							checkReader();
							stream.Position = offset + 102;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
						}
						set {
							checkWriter();
							stream.Position = offset + 102;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
						}
					}
					
				
			
		
			
				
					public  string SECTORID
					{
						get {
							checkReader();
							stream.Position = offset + 107;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
						}
						set {
							checkWriter();
							stream.Position = offset + 107;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
						}
					}
					
				
			
		
			
				
					public  int LOTSIZE
					{
						get {
							checkReader();
							stream.Position = offset + 112;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 112;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal MINSTEP
					{
						get {
							checkReader();
							stream.Position = offset + 116;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 116;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.6", (decimal)value);
						}
					}
					
					public byte MINSTEP_scale
					{
						get {
							stream.Position = offset + 116;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal FACEVALUE
					{
						get {
							checkReader();
							stream.Position = offset + 126;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 126;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.6", (decimal)value);
						}
					}
					
					public byte FACEVALUE_scale
					{
						get {
							stream.Position = offset + 126;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  string FACEUNIT
					{
						get {
							checkReader();
							stream.Position = offset + 136;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
						}
						set {
							checkWriter();
							stream.Position = offset + 136;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
						}
					}
					
				
			
		
			
				
					public  System.DateTime PREVDATE
					{
						get {
							checkReader();
							stream.Position = offset + 142;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 142;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  decimal PREVPRICE
					{
						get {
							checkReader();
							stream.Position = offset + 152;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 152;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.6", (decimal)value);
						}
					}
					
					public byte PREVPRICE_scale
					{
						get {
							stream.Position = offset + 152;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte DECIMALS
					{
						get {
							checkReader();
							stream.Position = offset + 162;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 162;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal PREVWAPRICE
					{
						get {
							checkReader();
							stream.Position = offset + 163;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 163;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.6", (decimal)value);
						}
					}
					
					public byte PREVWAPRICE_scale
					{
						get {
							stream.Position = offset + 163;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  string CURRENCYID
					{
						get {
							checkReader();
							stream.Position = offset + 173;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
						}
						set {
							checkWriter();
							stream.Position = offset + 173;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
						}
					}
					
				
			
		
			
				
					public  string LATNAME
					{
						get {
							checkReader();
							stream.Position = offset + 178;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 30);
						}
						set {
							checkWriter();
							stream.Position = offset + 178;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 30);
						}
					}
					
				
			
		
		}
	
	


}
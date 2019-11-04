namespace POS{

	// Scheme "POS" description
	
	
		public partial class position
		{
			public const int table_index = 0;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public position() {
			}
			public position(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public position(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string client_code
					{
						get {
							checkReader();
							stream.Position = offset + 24;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 24;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  int isin_id
					{
						get {
							checkReader();
							stream.Position = offset + 32;
							return reader.ReadInt32();
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
							stream.Position = offset + 36;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 36;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xpos
					{
						get {
							checkReader();
							stream.Position = offset + 40;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 40;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int buys_qty
					{
						get {
							checkReader();
							stream.Position = offset + 48;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 48;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xbuys_qty
					{
						get {
							checkReader();
							stream.Position = offset + 52;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 52;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int sells_qty
					{
						get {
							checkReader();
							stream.Position = offset + 60;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 60;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xsells_qty
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
					
				
			
		
			
				
					public  int open_qty
					{
						get {
							checkReader();
							stream.Position = offset + 72;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 72;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xopen_qty
					{
						get {
							checkReader();
							stream.Position = offset + 76;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 76;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal waprice
					{
						get {
							checkReader();
							stream.Position = offset + 84;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 84;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte waprice_scale
					{
						get {
							stream.Position = offset + 84;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal net_volume
					{
						get {
							checkReader();
							stream.Position = offset + 95;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 95;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d26.5", (decimal)value);
						}
					}
					
					public byte net_volume_scale
					{
						get {
							stream.Position = offset + 95;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal net_volume_rur
					{
						get {
							checkReader();
							stream.Position = offset + 111;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 111;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d26.2", (decimal)value);
						}
					}
					
					public byte net_volume_rur_scale
					{
						get {
							stream.Position = offset + 111;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte opt_type
					{
						get {
							checkReader();
							stream.Position = offset + 126;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 126;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long last_deal_id
					{
						get {
							checkReader();
							stream.Position = offset + 128;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 128;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte account_type
					{
						get {
							checkReader();
							stream.Position = offset + 136;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 136;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
	


}
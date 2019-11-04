namespace AGGR{

	// Scheme "AGGR" description
	
	
		public partial class orders_aggr
		{
			public const int table_index = 0;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public orders_aggr() {
			}
			public orders_aggr(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public orders_aggr(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int isin_id
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
					
				
			
		
			
				
					public  decimal price
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte price_scale
					{
						get {
							stream.Position = offset + 28;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  long volume
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
					
				
			
		
			
				
					public  System.DateTime moment
					{
						get {
							checkReader();
							stream.Position = offset + 48;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 48;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  ulong moment_ns
					{
						get {
							checkReader();
							stream.Position = offset + 60;
							return reader.ReadUInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 60;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte dir
					{
						get {
							checkReader();
							stream.Position = offset + 68;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 68;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime timestamp
					{
						get {
							checkReader();
							stream.Position = offset + 70;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 70;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  int sess_id
					{
						get {
							checkReader();
							stream.Position = offset + 80;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 80;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
	


}
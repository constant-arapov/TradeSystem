namespace USER_DEAL{

	// Scheme "USER_DEAL" description
	
	
		public partial class user_deal
		{
			public const int table_index = 0;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public user_deal() {
			}
			public user_deal(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public user_deal(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  long id_deal_multileg
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
					
				
			
		
			
				
					public  long id_repo
					{
						get {
							checkReader();
							stream.Position = offset + 48;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 48;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int pos
					{
						get {
							checkReader();
							stream.Position = offset + 56;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 56;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xpos
					{
						get {
							checkReader();
							stream.Position = offset + 60;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 60;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int amount
					{
						get {
							checkReader();
							stream.Position = offset + 68;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 68;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xamount
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
					
				
			
		
			
				
					public  long id_ord_buy
					{
						get {
							checkReader();
							stream.Position = offset + 80;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 80;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long id_ord_sell
					{
						get {
							checkReader();
							stream.Position = offset + 88;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 88;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal price
					{
						get {
							checkReader();
							stream.Position = offset + 96;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 96;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte price_scale
					{
						get {
							stream.Position = offset + 96;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime moment
					{
						get {
							checkReader();
							stream.Position = offset + 108;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 108;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  ulong moment_ns
					{
						get {
							checkReader();
							stream.Position = offset + 120;
							return reader.ReadUInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 120;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte nosystem
					{
						get {
							checkReader();
							stream.Position = offset + 128;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 128;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xstatus_buy
					{
						get {
							checkReader();
							stream.Position = offset + 132;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 132;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xstatus_sell
					{
						get {
							checkReader();
							stream.Position = offset + 140;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 140;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int status_buy
					{
						get {
							checkReader();
							stream.Position = offset + 148;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 148;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int status_sell
					{
						get {
							checkReader();
							stream.Position = offset + 152;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 152;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ext_id_buy
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
					
				
			
		
			
				
					public  int ext_id_sell
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
					
				
			
		
			
				
					public  string code_buy
					{
						get {
							checkReader();
							stream.Position = offset + 164;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 164;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string code_sell
					{
						get {
							checkReader();
							stream.Position = offset + 172;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 172;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string comment_buy
					{
						get {
							checkReader();
							stream.Position = offset + 180;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 180;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string comment_sell
					{
						get {
							checkReader();
							stream.Position = offset + 201;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 201;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  sbyte trust_buy
					{
						get {
							checkReader();
							stream.Position = offset + 222;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 222;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte trust_sell
					{
						get {
							checkReader();
							stream.Position = offset + 223;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 223;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge_buy
					{
						get {
							checkReader();
							stream.Position = offset + 224;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 224;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge_sell
					{
						get {
							checkReader();
							stream.Position = offset + 225;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 225;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal fee_buy
					{
						get {
							checkReader();
							stream.Position = offset + 226;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 226;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d26.2", (decimal)value);
						}
					}
					
					public byte fee_buy_scale
					{
						get {
							stream.Position = offset + 226;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal fee_sell
					{
						get {
							checkReader();
							stream.Position = offset + 241;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 241;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d26.2", (decimal)value);
						}
					}
					
					public byte fee_sell_scale
					{
						get {
							stream.Position = offset + 241;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  string login_buy
					{
						get {
							checkReader();
							stream.Position = offset + 256;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 256;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string login_sell
					{
						get {
							checkReader();
							stream.Position = offset + 277;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 277;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string code_rts_buy
					{
						get {
							checkReader();
							stream.Position = offset + 298;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 298;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string code_rts_sell
					{
						get {
							checkReader();
							stream.Position = offset + 306;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 306;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
		}
	
	


}
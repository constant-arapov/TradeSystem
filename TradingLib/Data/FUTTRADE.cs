

	// Scheme "FUTTRADE" description
	
	
		partial class orders_log
		{
			public const int table_index = 0;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public orders_log() {
			}
			public orders_log(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public orders_log(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  long id_ord
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
					
				
			
		
			
				
					public  int sess_id
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
					
				
			
		
			
				
					public  int isin_id
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
					
				
			
		
			
				
					public  int amount
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
					
				
			
		
			
				
					public  int amount_rest
					{
						get {
							checkReader();
							stream.Position = offset + 44;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 44;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long id_deal
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
					
				
			
		
			
				
					public  long xstatus
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
					
				
			
		
			
				
					public  int status
					{
						get {
							checkReader();
							stream.Position = offset + 64;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 64;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal price
					{
						get {
							checkReader();
							stream.Position = offset + 68;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 68;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte price_scale
					{
						get {
							stream.Position = offset + 68;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime moment
					{
						get {
							checkReader();
							stream.Position = offset + 80;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 80;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  ulong moment_ns
					{
						get {
							checkReader();
							stream.Position = offset + 92;
							return reader.ReadUInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 92;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte dir
					{
						get {
							checkReader();
							stream.Position = offset + 100;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 100;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte action
					{
						get {
							checkReader();
							stream.Position = offset + 101;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 101;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal deal_price
					{
						get {
							checkReader();
							stream.Position = offset + 102;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 102;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte deal_price_scale
					{
						get {
							stream.Position = offset + 102;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  string client_code
					{
						get {
							checkReader();
							stream.Position = offset + 113;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 113;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string login_from
					{
						get {
							checkReader();
							stream.Position = offset + 121;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 121;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string comment
					{
						get {
							checkReader();
							stream.Position = offset + 142;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 142;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge
					{
						get {
							checkReader();
							stream.Position = offset + 163;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 163;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte trust
					{
						get {
							checkReader();
							stream.Position = offset + 164;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 164;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ext_id
					{
						get {
							checkReader();
							stream.Position = offset + 168;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 168;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string broker_to
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
					
				
			
		
			
				
					public  string broker_to_rts
					{
						get {
							checkReader();
							stream.Position = offset + 180;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 180;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string broker_from_rts
					{
						get {
							checkReader();
							stream.Position = offset + 188;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 188;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  System.DateTime date_exp
					{
						get {
							checkReader();
							stream.Position = offset + 196;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 196;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  long id_ord1
					{
						get {
							checkReader();
							stream.Position = offset + 208;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 208;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte is_gate
					{
						get {
							checkReader();
							stream.Position = offset + 216;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 216;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime local_stamp
					{
						get {
							checkReader();
							stream.Position = offset + 218;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 218;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
		}
	
		partial class multileg_orders_log
		{
			public const int table_index = 1;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public multileg_orders_log() {
			}
			public multileg_orders_log(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public multileg_orders_log(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  long id_ord
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
					
				
			
		
			
				
					public  int sess_id
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
					
				
			
		
			
				
					public  int isin_id
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
					
				
			
		
			
				
					public  int amount
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
					
				
			
		
			
				
					public  int amount_rest
					{
						get {
							checkReader();
							stream.Position = offset + 44;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 44;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long id_deal
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
					
				
			
		
			
				
					public  long xstatus
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
					
				
			
		
			
				
					public  int status
					{
						get {
							checkReader();
							stream.Position = offset + 64;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 64;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal price
					{
						get {
							checkReader();
							stream.Position = offset + 68;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 68;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte price_scale
					{
						get {
							stream.Position = offset + 68;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime moment
					{
						get {
							checkReader();
							stream.Position = offset + 80;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 80;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  ulong moment_ns
					{
						get {
							checkReader();
							stream.Position = offset + 92;
							return reader.ReadUInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 92;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte dir
					{
						get {
							checkReader();
							stream.Position = offset + 100;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 100;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte action
					{
						get {
							checkReader();
							stream.Position = offset + 101;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 101;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal deal_price
					{
						get {
							checkReader();
							stream.Position = offset + 102;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 102;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte deal_price_scale
					{
						get {
							stream.Position = offset + 102;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal rate_price
					{
						get {
							checkReader();
							stream.Position = offset + 113;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 113;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte rate_price_scale
					{
						get {
							stream.Position = offset + 113;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal swap_price
					{
						get {
							checkReader();
							stream.Position = offset + 124;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 124;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte swap_price_scale
					{
						get {
							stream.Position = offset + 124;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  string client_code
					{
						get {
							checkReader();
							stream.Position = offset + 135;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 135;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string login_from
					{
						get {
							checkReader();
							stream.Position = offset + 143;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 143;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string comment
					{
						get {
							checkReader();
							stream.Position = offset + 164;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 164;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge
					{
						get {
							checkReader();
							stream.Position = offset + 185;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 185;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte trust
					{
						get {
							checkReader();
							stream.Position = offset + 186;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 186;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ext_id
					{
						get {
							checkReader();
							stream.Position = offset + 188;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 188;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string broker_to
					{
						get {
							checkReader();
							stream.Position = offset + 192;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 192;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string broker_to_rts
					{
						get {
							checkReader();
							stream.Position = offset + 200;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 200;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string broker_from_rts
					{
						get {
							checkReader();
							stream.Position = offset + 208;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 208;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  System.DateTime date_exp
					{
						get {
							checkReader();
							stream.Position = offset + 216;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 216;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  long id_ord1
					{
						get {
							checkReader();
							stream.Position = offset + 228;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 228;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte is_gate
					{
						get {
							checkReader();
							stream.Position = offset + 236;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 236;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int isin_id_repo
					{
						get {
							checkReader();
							stream.Position = offset + 240;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 240;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int isin_id_rd
					{
						get {
							checkReader();
							stream.Position = offset + 244;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 244;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int isin_id_rb
					{
						get {
							checkReader();
							stream.Position = offset + 248;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 248;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int duration
					{
						get {
							checkReader();
							stream.Position = offset + 252;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 252;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime local_stamp
					{
						get {
							checkReader();
							stream.Position = offset + 256;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 256;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
		}
	
		partial class deal
		{
			public const int table_index = 2;
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
					
				
			
		
			
				
					public  int amount
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
					
				
			
		
			
				
					public  long xstatus_buy
					{
						get {
							checkReader();
							stream.Position = offset + 116;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 116;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xstatus_sell
					{
						get {
							checkReader();
							stream.Position = offset + 124;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 124;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int status_buy
					{
						get {
							checkReader();
							stream.Position = offset + 132;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 132;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int status_sell
					{
						get {
							checkReader();
							stream.Position = offset + 136;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 136;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ext_id_buy
					{
						get {
							checkReader();
							stream.Position = offset + 140;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 140;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ext_id_sell
					{
						get {
							checkReader();
							stream.Position = offset + 144;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 144;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string code_buy
					{
						get {
							checkReader();
							stream.Position = offset + 148;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 148;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string code_sell
					{
						get {
							checkReader();
							stream.Position = offset + 156;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 156;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string comment_buy
					{
						get {
							checkReader();
							stream.Position = offset + 164;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 164;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string comment_sell
					{
						get {
							checkReader();
							stream.Position = offset + 185;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 185;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  sbyte trust_buy
					{
						get {
							checkReader();
							stream.Position = offset + 206;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 206;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte trust_sell
					{
						get {
							checkReader();
							stream.Position = offset + 207;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 207;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge_buy
					{
						get {
							checkReader();
							stream.Position = offset + 208;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 208;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge_sell
					{
						get {
							checkReader();
							stream.Position = offset + 209;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 209;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal fee_buy
					{
						get {
							checkReader();
							stream.Position = offset + 210;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 210;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d26.2", (decimal)value);
						}
					}
					
					public byte fee_buy_scale
					{
						get {
							stream.Position = offset + 210;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal fee_sell
					{
						get {
							checkReader();
							stream.Position = offset + 225;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 225;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d26.2", (decimal)value);
						}
					}
					
					public byte fee_sell_scale
					{
						get {
							stream.Position = offset + 225;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  string login_buy
					{
						get {
							checkReader();
							stream.Position = offset + 240;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 240;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string login_sell
					{
						get {
							checkReader();
							stream.Position = offset + 261;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 261;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string code_rts_buy
					{
						get {
							checkReader();
							stream.Position = offset + 282;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 282;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string code_rts_sell
					{
						get {
							checkReader();
							stream.Position = offset + 290;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 290;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
		}
	
		partial class multileg_deal
		{
			public const int table_index = 3;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public multileg_deal() {
			}
			public multileg_deal(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public multileg_deal(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int isin_id_rd
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
					
				
			
		
			
				
					public  int isin_id_rb
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
					
				
			
		
			
				
					public  int isin_id_repo
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
					
				
			
		
			
				
					public  int duration
					{
						get {
							checkReader();
							stream.Position = offset + 44;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 44;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long id_deal
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
					
				
			
		
			
				
					public  long id_deal_rd
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
					
				
			
		
			
				
					public  long id_deal_rb
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
					
				
			
		
			
				
					public  long id_ord_buy
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
					
				
			
		
			
				
					public  long id_ord_sell
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
					
				
			
		
			
				
					public  int amount
					{
						get {
							checkReader();
							stream.Position = offset + 88;
							return reader.ReadInt32();
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
							stream.Position = offset + 92;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 92;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte price_scale
					{
						get {
							stream.Position = offset + 92;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal rate_price
					{
						get {
							checkReader();
							stream.Position = offset + 103;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 103;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte rate_price_scale
					{
						get {
							stream.Position = offset + 103;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal swap_price
					{
						get {
							checkReader();
							stream.Position = offset + 114;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 114;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte swap_price_scale
					{
						get {
							stream.Position = offset + 114;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal buyback_amount
					{
						get {
							checkReader();
							stream.Position = offset + 125;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 125;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.2", (decimal)value);
						}
					}
					
					public byte buyback_amount_scale
					{
						get {
							stream.Position = offset + 125;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime moment
					{
						get {
							checkReader();
							stream.Position = offset + 136;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 136;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  ulong moment_ns
					{
						get {
							checkReader();
							stream.Position = offset + 148;
							return reader.ReadUInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 148;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte nosystem
					{
						get {
							checkReader();
							stream.Position = offset + 156;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 156;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xstatus_buy
					{
						get {
							checkReader();
							stream.Position = offset + 160;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 160;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xstatus_sell
					{
						get {
							checkReader();
							stream.Position = offset + 168;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 168;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int status_buy
					{
						get {
							checkReader();
							stream.Position = offset + 176;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 176;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int status_sell
					{
						get {
							checkReader();
							stream.Position = offset + 180;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 180;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ext_id_buy
					{
						get {
							checkReader();
							stream.Position = offset + 184;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 184;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ext_id_sell
					{
						get {
							checkReader();
							stream.Position = offset + 188;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 188;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string code_buy
					{
						get {
							checkReader();
							stream.Position = offset + 192;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 192;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string code_sell
					{
						get {
							checkReader();
							stream.Position = offset + 200;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 200;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string comment_buy
					{
						get {
							checkReader();
							stream.Position = offset + 208;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 208;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string comment_sell
					{
						get {
							checkReader();
							stream.Position = offset + 229;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 229;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  sbyte trust_buy
					{
						get {
							checkReader();
							stream.Position = offset + 250;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 250;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte trust_sell
					{
						get {
							checkReader();
							stream.Position = offset + 251;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 251;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge_buy
					{
						get {
							checkReader();
							stream.Position = offset + 252;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 252;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge_sell
					{
						get {
							checkReader();
							stream.Position = offset + 253;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 253;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string login_buy
					{
						get {
							checkReader();
							stream.Position = offset + 254;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 254;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string login_sell
					{
						get {
							checkReader();
							stream.Position = offset + 275;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 275;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string code_rts_buy
					{
						get {
							checkReader();
							stream.Position = offset + 296;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 296;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string code_rts_sell
					{
						get {
							checkReader();
							stream.Position = offset + 304;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 304;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
		}
	
		partial class heartbeat
		{
			public const int table_index = 4;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public heartbeat() {
			}
			public heartbeat(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public heartbeat(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  System.DateTime server_time
					{
						get {
							checkReader();
							stream.Position = offset + 24;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 24;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
		}
	
		partial class sys_events
		{
			public const int table_index = 5;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public sys_events() {
			}
			public sys_events(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public sys_events(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  long event_id
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
					
				
			
		
			
				
					public  int sess_id
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
					
				
			
		
			
				
					public  int event_type
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
					
				
			
		
			
				
					public  string message
					{
						get {
							checkReader();
							stream.Position = offset + 40;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 64);
						}
						set {
							checkWriter();
							stream.Position = offset + 40;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 64);
						}
					}
					
				
			
		
		}
	
		partial class user_deal
		{
			public const int table_index = 6;
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
					
				
			
		
			
				
					public  int amount
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
					
				
			
		
			
				
					public  long xstatus_buy
					{
						get {
							checkReader();
							stream.Position = offset + 116;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 116;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xstatus_sell
					{
						get {
							checkReader();
							stream.Position = offset + 124;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 124;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int status_buy
					{
						get {
							checkReader();
							stream.Position = offset + 132;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 132;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int status_sell
					{
						get {
							checkReader();
							stream.Position = offset + 136;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 136;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ext_id_buy
					{
						get {
							checkReader();
							stream.Position = offset + 140;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 140;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ext_id_sell
					{
						get {
							checkReader();
							stream.Position = offset + 144;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 144;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string code_buy
					{
						get {
							checkReader();
							stream.Position = offset + 148;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 148;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string code_sell
					{
						get {
							checkReader();
							stream.Position = offset + 156;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 156;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string comment_buy
					{
						get {
							checkReader();
							stream.Position = offset + 164;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 164;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string comment_sell
					{
						get {
							checkReader();
							stream.Position = offset + 185;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 185;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  sbyte trust_buy
					{
						get {
							checkReader();
							stream.Position = offset + 206;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 206;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte trust_sell
					{
						get {
							checkReader();
							stream.Position = offset + 207;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 207;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge_buy
					{
						get {
							checkReader();
							stream.Position = offset + 208;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 208;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge_sell
					{
						get {
							checkReader();
							stream.Position = offset + 209;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 209;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal fee_buy
					{
						get {
							checkReader();
							stream.Position = offset + 210;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 210;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d26.2", (decimal)value);
						}
					}
					
					public byte fee_buy_scale
					{
						get {
							stream.Position = offset + 210;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal fee_sell
					{
						get {
							checkReader();
							stream.Position = offset + 225;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 225;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d26.2", (decimal)value);
						}
					}
					
					public byte fee_sell_scale
					{
						get {
							stream.Position = offset + 225;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  string login_buy
					{
						get {
							checkReader();
							stream.Position = offset + 240;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 240;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string login_sell
					{
						get {
							checkReader();
							stream.Position = offset + 261;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 261;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string code_rts_buy
					{
						get {
							checkReader();
							stream.Position = offset + 282;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 282;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string code_rts_sell
					{
						get {
							checkReader();
							stream.Position = offset + 290;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 290;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
		}
	
		partial class user_multileg_deal
		{
			public const int table_index = 7;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public user_multileg_deal() {
			}
			public user_multileg_deal(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public user_multileg_deal(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int isin_id_rd
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
					
				
			
		
			
				
					public  int isin_id_rb
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
					
				
			
		
			
				
					public  int isin_id_repo
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
					
				
			
		
			
				
					public  int duration
					{
						get {
							checkReader();
							stream.Position = offset + 44;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 44;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long id_deal
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
					
				
			
		
			
				
					public  long id_deal_rd
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
					
				
			
		
			
				
					public  long id_deal_rb
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
					
				
			
		
			
				
					public  long id_ord_buy
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
					
				
			
		
			
				
					public  long id_ord_sell
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
					
				
			
		
			
				
					public  int amount
					{
						get {
							checkReader();
							stream.Position = offset + 88;
							return reader.ReadInt32();
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
							stream.Position = offset + 92;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 92;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte price_scale
					{
						get {
							stream.Position = offset + 92;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal rate_price
					{
						get {
							checkReader();
							stream.Position = offset + 103;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 103;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte rate_price_scale
					{
						get {
							stream.Position = offset + 103;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal swap_price
					{
						get {
							checkReader();
							stream.Position = offset + 114;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 114;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte swap_price_scale
					{
						get {
							stream.Position = offset + 114;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal buyback_amount
					{
						get {
							checkReader();
							stream.Position = offset + 125;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 125;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.2", (decimal)value);
						}
					}
					
					public byte buyback_amount_scale
					{
						get {
							stream.Position = offset + 125;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime moment
					{
						get {
							checkReader();
							stream.Position = offset + 136;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 136;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  ulong moment_ns
					{
						get {
							checkReader();
							stream.Position = offset + 148;
							return reader.ReadUInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 148;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte nosystem
					{
						get {
							checkReader();
							stream.Position = offset + 156;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 156;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xstatus_buy
					{
						get {
							checkReader();
							stream.Position = offset + 160;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 160;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xstatus_sell
					{
						get {
							checkReader();
							stream.Position = offset + 168;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 168;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int status_buy
					{
						get {
							checkReader();
							stream.Position = offset + 176;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 176;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int status_sell
					{
						get {
							checkReader();
							stream.Position = offset + 180;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 180;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ext_id_buy
					{
						get {
							checkReader();
							stream.Position = offset + 184;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 184;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ext_id_sell
					{
						get {
							checkReader();
							stream.Position = offset + 188;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 188;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string code_buy
					{
						get {
							checkReader();
							stream.Position = offset + 192;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 192;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string code_sell
					{
						get {
							checkReader();
							stream.Position = offset + 200;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 200;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string comment_buy
					{
						get {
							checkReader();
							stream.Position = offset + 208;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 208;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string comment_sell
					{
						get {
							checkReader();
							stream.Position = offset + 229;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 229;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  sbyte trust_buy
					{
						get {
							checkReader();
							stream.Position = offset + 250;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 250;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte trust_sell
					{
						get {
							checkReader();
							stream.Position = offset + 251;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 251;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge_buy
					{
						get {
							checkReader();
							stream.Position = offset + 252;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 252;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge_sell
					{
						get {
							checkReader();
							stream.Position = offset + 253;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 253;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string login_buy
					{
						get {
							checkReader();
							stream.Position = offset + 254;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 254;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string login_sell
					{
						get {
							checkReader();
							stream.Position = offset + 275;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 275;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string code_rts_buy
					{
						get {
							checkReader();
							stream.Position = offset + 296;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 296;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string code_rts_sell
					{
						get {
							checkReader();
							stream.Position = offset + 304;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 304;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
		}
	
	



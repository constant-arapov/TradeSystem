using Common; 
namespace FUTTRADE{

	// Scheme "FUTTRADE" description
	
	
		public partial class orders_log: CClone 

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
					
				
			
		
			
				
					public  long xamount
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
					
				
			
		
			
				
					public  int amount_rest
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
					
				
			
		
			
				
					public  long xamount_rest
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
					
				
			
		
			
				
					public  long id_deal
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
					
				
			
		
			
				
					public  long xstatus
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
					
				
			
		
			
				
					public  int status
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
					
				
			
		
			
				
					public  decimal price
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
					
					public byte price_scale
					{
						get {
							stream.Position = offset + 84;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime moment
					{
						get {
							checkReader();
							stream.Position = offset + 96;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 96;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  ulong moment_ns
					{
						get {
							checkReader();
							stream.Position = offset + 108;
							return reader.ReadUInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 108;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte dir
					{
						get {
							checkReader();
							stream.Position = offset + 116;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 116;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte action
					{
						get {
							checkReader();
							stream.Position = offset + 117;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 117;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal deal_price
					{
						get {
							checkReader();
							stream.Position = offset + 118;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 118;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte deal_price_scale
					{
						get {
							stream.Position = offset + 118;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  string client_code
					{
						get {
							checkReader();
							stream.Position = offset + 129;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 129;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string login_from
					{
						get {
							checkReader();
							stream.Position = offset + 137;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 137;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string comment
					{
						get {
							checkReader();
							stream.Position = offset + 158;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 158;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge
					{
						get {
							checkReader();
							stream.Position = offset + 179;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 179;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte trust
					{
						get {
							checkReader();
							stream.Position = offset + 180;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 180;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ext_id
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
					
				
			
		
			
				
					public  string broker_to
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
					
				
			
		
			
				
					public  string broker_to_rts
					{
						get {
							checkReader();
							stream.Position = offset + 196;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 196;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string broker_from_rts
					{
						get {
							checkReader();
							stream.Position = offset + 204;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 204;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  System.DateTime date_exp
					{
						get {
							checkReader();
							stream.Position = offset + 212;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 212;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  long id_ord1
					{
						get {
							checkReader();
							stream.Position = offset + 224;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 224;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte is_gate
					{
						get {
							checkReader();
							stream.Position = offset + 232;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 232;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime local_stamp
					{
						get {
							checkReader();
							stream.Position = offset + 234;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 234;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
		}
	
		public partial class multileg_orders_log
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
					
				
			
		
			
				
					public  long xamount
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
					
				
			
		
			
				
					public  int amount_rest
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
					
				
			
		
			
				
					public  long xamount_rest
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
					
				
			
		
			
				
					public  long id_deal
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
					
				
			
		
			
				
					public  long xstatus
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
					
				
			
		
			
				
					public  int status
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
					
				
			
		
			
				
					public  decimal price
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
					
					public byte price_scale
					{
						get {
							stream.Position = offset + 84;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime moment
					{
						get {
							checkReader();
							stream.Position = offset + 96;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 96;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  ulong moment_ns
					{
						get {
							checkReader();
							stream.Position = offset + 108;
							return reader.ReadUInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 108;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte dir
					{
						get {
							checkReader();
							stream.Position = offset + 116;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 116;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte action
					{
						get {
							checkReader();
							stream.Position = offset + 117;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 117;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal deal_price
					{
						get {
							checkReader();
							stream.Position = offset + 118;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 118;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte deal_price_scale
					{
						get {
							stream.Position = offset + 118;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal rate_price
					{
						get {
							checkReader();
							stream.Position = offset + 129;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 129;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte rate_price_scale
					{
						get {
							stream.Position = offset + 129;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal swap_price
					{
						get {
							checkReader();
							stream.Position = offset + 140;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 140;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte swap_price_scale
					{
						get {
							stream.Position = offset + 140;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  string client_code
					{
						get {
							checkReader();
							stream.Position = offset + 151;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 151;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string login_from
					{
						get {
							checkReader();
							stream.Position = offset + 159;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 159;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string comment
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
					
				
			
		
			
				
					public  sbyte hedge
					{
						get {
							checkReader();
							stream.Position = offset + 201;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 201;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte trust
					{
						get {
							checkReader();
							stream.Position = offset + 202;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 202;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ext_id
					{
						get {
							checkReader();
							stream.Position = offset + 204;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 204;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string broker_to
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
					
				
			
		
			
				
					public  string broker_to_rts
					{
						get {
							checkReader();
							stream.Position = offset + 216;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 216;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  string broker_from_rts
					{
						get {
							checkReader();
							stream.Position = offset + 224;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 224;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  System.DateTime date_exp
					{
						get {
							checkReader();
							stream.Position = offset + 232;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 232;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  long id_ord1
					{
						get {
							checkReader();
							stream.Position = offset + 244;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 244;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte is_gate
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
					
				
			
		
			
				
					public  int isin_id_repo
					{
						get {
							checkReader();
							stream.Position = offset + 256;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 256;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int isin_id_rd
					{
						get {
							checkReader();
							stream.Position = offset + 260;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 260;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int isin_id_rb
					{
						get {
							checkReader();
							stream.Position = offset + 264;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 264;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int duration
					{
						get {
							checkReader();
							stream.Position = offset + 268;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 268;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime local_stamp
					{
						get {
							checkReader();
							stream.Position = offset + 272;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 272;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
		}
	
		public partial class heartbeat
		{
			public const int table_index = 2;
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
	
		public partial class sys_events
		{
			public const int table_index = 3;
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
	
		public partial class user_deal
		{
			public const int table_index = 4;
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
	
		public partial class user_multileg_deal
		{
			public const int table_index = 5;
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
					
				
			
		
			
				
					public  long xamount
					{
						get {
							checkReader();
							stream.Position = offset + 92;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 92;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal price
					{
						get {
							checkReader();
							stream.Position = offset + 100;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 100;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte price_scale
					{
						get {
							stream.Position = offset + 100;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal rate_price
					{
						get {
							checkReader();
							stream.Position = offset + 111;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 111;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte rate_price_scale
					{
						get {
							stream.Position = offset + 111;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal swap_price
					{
						get {
							checkReader();
							stream.Position = offset + 122;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 122;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte swap_price_scale
					{
						get {
							stream.Position = offset + 122;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal buyback_amount
					{
						get {
							checkReader();
							stream.Position = offset + 133;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 133;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.2", (decimal)value);
						}
					}
					
					public byte buyback_amount_scale
					{
						get {
							stream.Position = offset + 133;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime moment
					{
						get {
							checkReader();
							stream.Position = offset + 144;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 144;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  ulong moment_ns
					{
						get {
							checkReader();
							stream.Position = offset + 156;
							return reader.ReadUInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 156;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte nosystem
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
					
				
			
		
			
				
					public  long xstatus_buy
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
					
				
			
		
			
				
					public  long xstatus_sell
					{
						get {
							checkReader();
							stream.Position = offset + 176;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 176;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int status_buy
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
					
				
			
		
			
				
					public  int status_sell
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
					
				
			
		
			
				
					public  int ext_id_buy
					{
						get {
							checkReader();
							stream.Position = offset + 192;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 192;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ext_id_sell
					{
						get {
							checkReader();
							stream.Position = offset + 196;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 196;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string code_buy
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
					
				
			
		
			
				
					public  string code_sell
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
					
				
			
		
			
				
					public  string comment_buy
					{
						get {
							checkReader();
							stream.Position = offset + 216;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 216;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string comment_sell
					{
						get {
							checkReader();
							stream.Position = offset + 237;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 237;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  sbyte trust_buy
					{
						get {
							checkReader();
							stream.Position = offset + 258;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 258;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte trust_sell
					{
						get {
							checkReader();
							stream.Position = offset + 259;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 259;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge_buy
					{
						get {
							checkReader();
							stream.Position = offset + 260;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 260;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte hedge_sell
					{
						get {
							checkReader();
							stream.Position = offset + 261;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 261;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string login_buy
					{
						get {
							checkReader();
							stream.Position = offset + 262;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 262;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string login_sell
					{
						get {
							checkReader();
							stream.Position = offset + 283;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 283;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string code_rts_buy
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
					
				
			
		
			
				
					public  string code_rts_sell
					{
						get {
							checkReader();
							stream.Position = offset + 312;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 312;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
		}
	
	


}
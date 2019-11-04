namespace FUTINFO{

	// Scheme "FUTINFO" description
	
	
		public partial class rates
		{
			public const int table_index = 0;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public rates() {
			}
			public rates(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public rates(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int rate_id
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
					
				
			
		
			
				
					public  string curr_base
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 15);
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 15);
						}
					}
					
				
			
		
			
				
					public  string curr_coupled
					{
						get {
							checkReader();
							stream.Position = offset + 44;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 15);
						}
						set {
							checkWriter();
							stream.Position = offset + 44;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 15);
						}
					}
					
				
			
		
			
				
					public  decimal radius
					{
						get {
							checkReader();
							stream.Position = offset + 60;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 60;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte radius_scale
					{
						get {
							stream.Position = offset + 60;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
		}
	
		public partial class fut_sess_contents
		{
			public const int table_index = 1;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public fut_sess_contents() {
			}
			public fut_sess_contents(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public fut_sess_contents(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string short_isin
					{
						get {
							checkReader();
							stream.Position = offset + 32;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 32;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  string isin
					{
						get {
							checkReader();
							stream.Position = offset + 58;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 58;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  string name
					{
						get {
							checkReader();
							stream.Position = offset + 84;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 75);
						}
						set {
							checkWriter();
							stream.Position = offset + 84;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 75);
						}
					}
					
				
			
		
			
				
					public  int inst_term
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
					
				
			
		
			
				
					public  string code_vcb
					{
						get {
							checkReader();
							stream.Position = offset + 164;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 164;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  sbyte is_limited
					{
						get {
							checkReader();
							stream.Position = offset + 190;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 190;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal limit_up
					{
						get {
							checkReader();
							stream.Position = offset + 191;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 191;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte limit_up_scale
					{
						get {
							stream.Position = offset + 191;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal limit_down
					{
						get {
							checkReader();
							stream.Position = offset + 202;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 202;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte limit_down_scale
					{
						get {
							stream.Position = offset + 202;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal old_kotir
					{
						get {
							checkReader();
							stream.Position = offset + 213;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 213;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte old_kotir_scale
					{
						get {
							stream.Position = offset + 213;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal buy_deposit
					{
						get {
							checkReader();
							stream.Position = offset + 224;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 224;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.2", (decimal)value);
						}
					}
					
					public byte buy_deposit_scale
					{
						get {
							stream.Position = offset + 224;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal sell_deposit
					{
						get {
							checkReader();
							stream.Position = offset + 234;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 234;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.2", (decimal)value);
						}
					}
					
					public byte sell_deposit_scale
					{
						get {
							stream.Position = offset + 234;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  int roundto
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
					
				
			
		
			
				
					public  decimal min_step
					{
						get {
							checkReader();
							stream.Position = offset + 248;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 248;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte min_step_scale
					{
						get {
							stream.Position = offset + 248;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  int lot_volume
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
					
				
			
		
			
				
					public  decimal step_price
					{
						get {
							checkReader();
							stream.Position = offset + 264;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 264;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte step_price_scale
					{
						get {
							stream.Position = offset + 264;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime d_pg
					{
						get {
							checkReader();
							stream.Position = offset + 276;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 276;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  sbyte is_spread
					{
						get {
							checkReader();
							stream.Position = offset + 286;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 286;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime d_exp
					{
						get {
							checkReader();
							stream.Position = offset + 288;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 288;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  sbyte is_percent
					{
						get {
							checkReader();
							stream.Position = offset + 298;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 298;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal percent_rate
					{
						get {
							checkReader();
							stream.Position = offset + 299;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 299;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d6.2", (decimal)value);
						}
					}
					
					public byte percent_rate_scale
					{
						get {
							stream.Position = offset + 299;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal last_cl_quote
					{
						get {
							checkReader();
							stream.Position = offset + 304;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 304;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte last_cl_quote_scale
					{
						get {
							stream.Position = offset + 304;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  int signs
					{
						get {
							checkReader();
							stream.Position = offset + 316;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 316;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte is_trade_evening
					{
						get {
							checkReader();
							stream.Position = offset + 320;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 320;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int ticker
					{
						get {
							checkReader();
							stream.Position = offset + 324;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 324;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int state
					{
						get {
							checkReader();
							stream.Position = offset + 328;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 328;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte price_dir
					{
						get {
							checkReader();
							stream.Position = offset + 332;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 332;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int multileg_type
					{
						get {
							checkReader();
							stream.Position = offset + 336;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 336;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int legs_qty
					{
						get {
							checkReader();
							stream.Position = offset + 340;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 340;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal step_price_clr
					{
						get {
							checkReader();
							stream.Position = offset + 344;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 344;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte step_price_clr_scale
					{
						get {
							stream.Position = offset + 344;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal step_price_interclr
					{
						get {
							checkReader();
							stream.Position = offset + 355;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 355;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte step_price_interclr_scale
					{
						get {
							stream.Position = offset + 355;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal step_price_curr
					{
						get {
							checkReader();
							stream.Position = offset + 366;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 366;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte step_price_curr_scale
					{
						get {
							stream.Position = offset + 366;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime d_start
					{
						get {
							checkReader();
							stream.Position = offset + 378;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 378;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  int sort_order
					{
						get {
							checkReader();
							stream.Position = offset + 388;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 388;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte vm_calc_type
					{
						get {
							checkReader();
							stream.Position = offset + 392;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 392;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal old_step_price
					{
						get {
							checkReader();
							stream.Position = offset + 393;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 393;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte old_step_price_scale
					{
						get {
							stream.Position = offset + 393;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal min_step_curr
					{
						get {
							checkReader();
							stream.Position = offset + 404;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 404;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte min_step_curr_scale
					{
						get {
							stream.Position = offset + 404;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal volat_min
					{
						get {
							checkReader();
							stream.Position = offset + 415;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 415;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d20.15", (decimal)value);
						}
					}
					
					public byte volat_min_scale
					{
						get {
							stream.Position = offset + 415;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal volat_max
					{
						get {
							checkReader();
							stream.Position = offset + 428;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 428;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d20.15", (decimal)value);
						}
					}
					
					public byte volat_max_scale
					{
						get {
							stream.Position = offset + 428;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  int isin_id_rd
					{
						get {
							checkReader();
							stream.Position = offset + 444;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 444;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int isin_id_rb
					{
						get {
							checkReader();
							stream.Position = offset + 448;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 448;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal exch_pay
					{
						get {
							checkReader();
							stream.Position = offset + 452;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 452;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte exch_pay_scale
					{
						get {
							stream.Position = offset + 452;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal price_intercl
					{
						get {
							checkReader();
							stream.Position = offset + 463;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 463;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte price_intercl_scale
					{
						get {
							stream.Position = offset + 463;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal pctyield_coeff
					{
						get {
							checkReader();
							stream.Position = offset + 474;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 474;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte pctyield_coeff_scale
					{
						get {
							stream.Position = offset + 474;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal pctyield_total
					{
						get {
							checkReader();
							stream.Position = offset + 485;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 485;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte pctyield_total_scale
					{
						get {
							stream.Position = offset + 485;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
		}
	
		public partial class fut_vcb
		{
			public const int table_index = 2;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public fut_vcb() {
			}
			public fut_vcb(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public fut_vcb(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string code_vcb
					{
						get {
							checkReader();
							stream.Position = offset + 24;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 24;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  string name
					{
						get {
							checkReader();
							stream.Position = offset + 50;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 75);
						}
						set {
							checkWriter();
							stream.Position = offset + 50;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 75);
						}
					}
					
				
			
		
			
				
					public  string exec_type
					{
						get {
							checkReader();
							stream.Position = offset + 126;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 1);
						}
						set {
							checkWriter();
							stream.Position = offset + 126;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 1);
						}
					}
					
				
			
		
			
				
					public  string curr
					{
						get {
							checkReader();
							stream.Position = offset + 128;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 3);
						}
						set {
							checkWriter();
							stream.Position = offset + 128;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 3);
						}
					}
					
				
			
		
			
				
					public  string trade_scheme
					{
						get {
							checkReader();
							stream.Position = offset + 132;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 1);
						}
						set {
							checkWriter();
							stream.Position = offset + 132;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 1);
						}
					}
					
				
			
		
			
				
					public  string section
					{
						get {
							checkReader();
							stream.Position = offset + 134;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 50);
						}
						set {
							checkWriter();
							stream.Position = offset + 134;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 50);
						}
					}
					
				
			
		
			
				
					public  int rate_id
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
					
				
			
		
			
				
					public  string SECCODE
					{
						get {
							checkReader();
							stream.Position = offset + 192;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 12);
						}
						set {
							checkWriter();
							stream.Position = offset + 192;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 12);
						}
					}
					
				
			
		
			
				
					public  sbyte is_foreign
					{
						get {
							checkReader();
							stream.Position = offset + 205;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 205;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class fut_instruments
		{
			public const int table_index = 3;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public fut_instruments() {
			}
			public fut_instruments(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public fut_instruments(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string short_isin
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  string isin
					{
						get {
							checkReader();
							stream.Position = offset + 54;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 54;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  string name
					{
						get {
							checkReader();
							stream.Position = offset + 80;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 75);
						}
						set {
							checkWriter();
							stream.Position = offset + 80;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 75);
						}
					}
					
				
			
		
			
				
					public  int inst_term
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
					
				
			
		
			
				
					public  string code_vcb
					{
						get {
							checkReader();
							stream.Position = offset + 160;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 160;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  sbyte is_limited
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
					
				
			
		
			
				
					public  decimal old_kotir
					{
						get {
							checkReader();
							stream.Position = offset + 187;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 187;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte old_kotir_scale
					{
						get {
							stream.Position = offset + 187;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  int roundto
					{
						get {
							checkReader();
							stream.Position = offset + 200;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 200;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal min_step
					{
						get {
							checkReader();
							stream.Position = offset + 204;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 204;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte min_step_scale
					{
						get {
							stream.Position = offset + 204;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  int lot_volume
					{
						get {
							checkReader();
							stream.Position = offset + 216;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 216;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal step_price
					{
						get {
							checkReader();
							stream.Position = offset + 220;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 220;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte step_price_scale
					{
						get {
							stream.Position = offset + 220;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime d_pg
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
					
				
			
		
			
				
					public  sbyte is_spread
					{
						get {
							checkReader();
							stream.Position = offset + 242;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 242;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime d_exp
					{
						get {
							checkReader();
							stream.Position = offset + 244;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 244;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  sbyte is_percent
					{
						get {
							checkReader();
							stream.Position = offset + 254;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 254;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal percent_rate
					{
						get {
							checkReader();
							stream.Position = offset + 255;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 255;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d6.2", (decimal)value);
						}
					}
					
					public byte percent_rate_scale
					{
						get {
							stream.Position = offset + 255;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal last_cl_quote
					{
						get {
							checkReader();
							stream.Position = offset + 260;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 260;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte last_cl_quote_scale
					{
						get {
							stream.Position = offset + 260;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  int signs
					{
						get {
							checkReader();
							stream.Position = offset + 272;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 272;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal volat_min
					{
						get {
							checkReader();
							stream.Position = offset + 276;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 276;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d20.15", (decimal)value);
						}
					}
					
					public byte volat_min_scale
					{
						get {
							stream.Position = offset + 276;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal volat_max
					{
						get {
							checkReader();
							stream.Position = offset + 289;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 289;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d20.15", (decimal)value);
						}
					}
					
					public byte volat_max_scale
					{
						get {
							stream.Position = offset + 289;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte price_dir
					{
						get {
							checkReader();
							stream.Position = offset + 302;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 302;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int multileg_type
					{
						get {
							checkReader();
							stream.Position = offset + 304;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 304;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int legs_qty
					{
						get {
							checkReader();
							stream.Position = offset + 308;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 308;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal step_price_clr
					{
						get {
							checkReader();
							stream.Position = offset + 312;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 312;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte step_price_clr_scale
					{
						get {
							stream.Position = offset + 312;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal step_price_interclr
					{
						get {
							checkReader();
							stream.Position = offset + 323;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 323;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte step_price_interclr_scale
					{
						get {
							stream.Position = offset + 323;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal step_price_curr
					{
						get {
							checkReader();
							stream.Position = offset + 334;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 334;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte step_price_curr_scale
					{
						get {
							stream.Position = offset + 334;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime d_start
					{
						get {
							checkReader();
							stream.Position = offset + 346;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 346;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  sbyte vm_calc_type
					{
						get {
							checkReader();
							stream.Position = offset + 356;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 356;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal old_step_price
					{
						get {
							checkReader();
							stream.Position = offset + 357;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 357;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte old_step_price_scale
					{
						get {
							stream.Position = offset + 357;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal min_step_curr
					{
						get {
							checkReader();
							stream.Position = offset + 368;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 368;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte min_step_curr_scale
					{
						get {
							stream.Position = offset + 368;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte is_limit_opt
					{
						get {
							checkReader();
							stream.Position = offset + 379;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 379;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal limit_up_opt
					{
						get {
							checkReader();
							stream.Position = offset + 380;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 380;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d5.2", (decimal)value);
						}
					}
					
					public byte limit_up_opt_scale
					{
						get {
							stream.Position = offset + 380;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal limit_down_opt
					{
						get {
							checkReader();
							stream.Position = offset + 385;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 385;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d5.2", (decimal)value);
						}
					}
					
					public byte limit_down_opt_scale
					{
						get {
							stream.Position = offset + 385;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal adm_lim
					{
						get {
							checkReader();
							stream.Position = offset + 390;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 390;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte adm_lim_scale
					{
						get {
							stream.Position = offset + 390;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal adm_lim_offmoney
					{
						get {
							checkReader();
							stream.Position = offset + 401;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 401;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte adm_lim_offmoney_scale
					{
						get {
							stream.Position = offset + 401;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte apply_adm_limit
					{
						get {
							checkReader();
							stream.Position = offset + 412;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 412;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal pctyield_coeff
					{
						get {
							checkReader();
							stream.Position = offset + 413;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 413;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte pctyield_coeff_scale
					{
						get {
							stream.Position = offset + 413;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal pctyield_total
					{
						get {
							checkReader();
							stream.Position = offset + 424;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 424;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte pctyield_total_scale
					{
						get {
							stream.Position = offset + 424;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  string exec_name
					{
						get {
							checkReader();
							stream.Position = offset + 435;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 1);
						}
						set {
							checkWriter();
							stream.Position = offset + 435;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 1);
						}
					}
					
				
			
		
		}
	
		public partial class fut_bond_registry
		{
			public const int table_index = 4;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public fut_bond_registry() {
			}
			public fut_bond_registry(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public fut_bond_registry(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int bond_id
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
					
				
			
		
			
				
					public  string small_name
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  string short_isin
					{
						get {
							checkReader();
							stream.Position = offset + 54;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 54;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  string name
					{
						get {
							checkReader();
							stream.Position = offset + 80;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 75);
						}
						set {
							checkWriter();
							stream.Position = offset + 80;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 75);
						}
					}
					
				
			
		
			
				
					public  System.DateTime date_redempt
					{
						get {
							checkReader();
							stream.Position = offset + 156;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 156;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  decimal nominal
					{
						get {
							checkReader();
							stream.Position = offset + 166;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 166;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte nominal_scale
					{
						get {
							stream.Position = offset + 166;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  int bond_type
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
					
				
			
		
			
				
					public  short year_base
					{
						get {
							checkReader();
							stream.Position = offset + 184;
							return reader.ReadInt16();
						}
						set {
							checkWriter();
							stream.Position = offset + 184;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class diler
		{
			public const int table_index = 5;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public diler() {
			}
			public diler(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public diler(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string name
					{
						get {
							checkReader();
							stream.Position = offset + 32;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 200);
						}
						set {
							checkWriter();
							stream.Position = offset + 32;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 200);
						}
					}
					
				
			
		
			
				
					public  string rts_code
					{
						get {
							checkReader();
							stream.Position = offset + 233;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 50);
						}
						set {
							checkWriter();
							stream.Position = offset + 233;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 50);
						}
					}
					
				
			
		
			
				
					public  int signs
					{
						get {
							checkReader();
							stream.Position = offset + 284;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 284;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int status
					{
						get {
							checkReader();
							stream.Position = offset + 288;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 288;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string transfer_code
					{
						get {
							checkReader();
							stream.Position = offset + 292;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 292;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  decimal exp_weight
					{
						get {
							checkReader();
							stream.Position = offset + 300;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 300;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d3.2", (decimal)value);
						}
					}
					
					public byte exp_weight_scale
					{
						get {
							stream.Position = offset + 300;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  int num_clr_2delivery
					{
						get {
							checkReader();
							stream.Position = offset + 304;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 304;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte margin_type
					{
						get {
							checkReader();
							stream.Position = offset + 308;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 308;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte calendar_spread_margin_type
					{
						get {
							checkReader();
							stream.Position = offset + 309;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 309;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int num_clr_2delivery_client_default
					{
						get {
							checkReader();
							stream.Position = offset + 312;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 312;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal exp_weight_client_default
					{
						get {
							checkReader();
							stream.Position = offset + 316;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 316;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d3.2", (decimal)value);
						}
					}
					
					public byte exp_weight_client_default_scale
					{
						get {
							stream.Position = offset + 316;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal go_ratio
					{
						get {
							checkReader();
							stream.Position = offset + 320;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 320;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte go_ratio_scale
					{
						get {
							stream.Position = offset + 320;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte check_limit_on_withdrawal
					{
						get {
							checkReader();
							stream.Position = offset + 331;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 331;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte limit_tied_money
					{
						get {
							checkReader();
							stream.Position = offset + 332;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 332;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte limits_set
					{
						get {
							checkReader();
							stream.Position = offset + 333;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 333;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte no_fut_discount
					{
						get {
							checkReader();
							stream.Position = offset + 334;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 334;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte no_fut_discount_client_default
					{
						get {
							checkReader();
							stream.Position = offset + 335;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 335;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class sys_messages
		{
			public const int table_index = 6;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public sys_messages() {
			}
			public sys_messages(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public sys_messages(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int msg_id
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
					
				
			
		
			
				
					public  System.DateTime moment
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  string lang_code
					{
						get {
							checkReader();
							stream.Position = offset + 38;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 8);
						}
						set {
							checkWriter();
							stream.Position = offset + 38;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 8);
						}
					}
					
				
			
		
			
				
					public  sbyte urgency
					{
						get {
							checkReader();
							stream.Position = offset + 47;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 47;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte status
					{
						get {
							checkReader();
							stream.Position = offset + 48;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 48;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string text
					{
						get {
							checkReader();
							stream.Position = offset + 49;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
						}
						set {
							checkWriter();
							stream.Position = offset + 49;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
						}
					}
					
				
			
		
			
				
					public  System.DateTime cancel_moment
					{
						get {
							checkReader();
							stream.Position = offset + 306;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 306;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  string message_body
					{
						get {
							checkReader();
							stream.Position = offset + 316;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4000);
						}
						set {
							checkWriter();
							stream.Position = offset + 316;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4000);
						}
					}
					
				
			
		
		}
	
		public partial class prohibition
		{
			public const int table_index = 7;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public prohibition() {
			}
			public prohibition(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public prohibition(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int prohib_id
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
					
				
			
		
			
				
					public  string client_code
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  int initiator
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
					
				
			
		
			
				
					public  string section
					{
						get {
							checkReader();
							stream.Position = offset + 40;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 50);
						}
						set {
							checkWriter();
							stream.Position = offset + 40;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 50);
						}
					}
					
				
			
		
			
				
					public  string code_vcb
					{
						get {
							checkReader();
							stream.Position = offset + 91;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 91;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  int isin_id
					{
						get {
							checkReader();
							stream.Position = offset + 120;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 120;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int priority
					{
						get {
							checkReader();
							stream.Position = offset + 124;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 124;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long group_mask
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
					
				
			
		
			
				
					public  int type
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
					
				
			
		
			
				
					public  int is_legacy
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
					
				
			
		
		}
	
		public partial class RF
		{
			public const int table_index = 8;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public RF() {
			}
			public RF(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public RF(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string code
					{
						get {
							checkReader();
							stream.Position = offset + 24;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 2);
						}
						set {
							checkWriter();
							stream.Position = offset + 24;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 2);
						}
					}
					
				
			
		
			
				
					public  int op_input
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
					
				
			
		
			
				
					public  int rf_input
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
					
				
			
		
		}
	
		public partial class trade_modes
		{
			public const int table_index = 9;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public trade_modes() {
			}
			public trade_modes(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public trade_modes(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int trade_mode_id
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
					
				
			
		
			
				
					public  int status
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
					
				
			
		
		}
	
		public partial class money_transfers
		{
			public const int table_index = 10;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public money_transfers() {
			}
			public money_transfers(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public money_transfers(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int id
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
					
				
			
		
			
				
					public  string client_code
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  decimal amount
					{
						get {
							checkReader();
							stream.Position = offset + 36;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 36;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.2", (decimal)value);
						}
					}
					
					public byte amount_scale
					{
						get {
							stream.Position = offset + 36;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime moment
					{
						get {
							checkReader();
							stream.Position = offset + 46;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 46;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  int state
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
					
				
			
		
			
				
					public  System.DateTime moment_accepted
					{
						get {
							checkReader();
							stream.Position = offset + 60;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 60;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  string rcv_name
					{
						get {
							checkReader();
							stream.Position = offset + 70;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 200);
						}
						set {
							checkWriter();
							stream.Position = offset + 70;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 200);
						}
					}
					
				
			
		
			
				
					public  string tn_account
					{
						get {
							checkReader();
							stream.Position = offset + 271;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 271;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string bank
					{
						get {
							checkReader();
							stream.Position = offset + 292;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 200);
						}
						set {
							checkWriter();
							stream.Position = offset + 292;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 200);
						}
					}
					
				
			
		
			
				
					public  string corr_account
					{
						get {
							checkReader();
							stream.Position = offset + 493;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 493;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string bic
					{
						get {
							checkReader();
							stream.Position = offset + 514;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 514;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string vatin
					{
						get {
							checkReader();
							stream.Position = offset + 535;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 12);
						}
						set {
							checkWriter();
							stream.Position = offset + 535;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 12);
						}
					}
					
				
			
		
			
				
					public  int sess_id
					{
						get {
							checkReader();
							stream.Position = offset + 548;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 548;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte type
					{
						get {
							checkReader();
							stream.Position = offset + 552;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 552;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class trf_accounts
		{
			public const int table_index = 11;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public trf_accounts() {
			}
			public trf_accounts(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public trf_accounts(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string name
					{
						get {
							checkReader();
							stream.Position = offset + 32;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 200);
						}
						set {
							checkWriter();
							stream.Position = offset + 32;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 200);
						}
					}
					
				
			
		
			
				
					public  string tn_account
					{
						get {
							checkReader();
							stream.Position = offset + 233;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 34);
						}
						set {
							checkWriter();
							stream.Position = offset + 233;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 34);
						}
					}
					
				
			
		
			
				
					public  string bank
					{
						get {
							checkReader();
							stream.Position = offset + 268;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 200);
						}
						set {
							checkWriter();
							stream.Position = offset + 268;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 200);
						}
					}
					
				
			
		
			
				
					public  string corr_account
					{
						get {
							checkReader();
							stream.Position = offset + 469;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 34);
						}
						set {
							checkWriter();
							stream.Position = offset + 469;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 34);
						}
					}
					
				
			
		
			
				
					public  string bic
					{
						get {
							checkReader();
							stream.Position = offset + 504;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 504;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string vatin
					{
						get {
							checkReader();
							stream.Position = offset + 525;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 50);
						}
						set {
							checkWriter();
							stream.Position = offset + 525;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 50);
						}
					}
					
				
			
		
			
				
					public  sbyte is_multicode
					{
						get {
							checkReader();
							stream.Position = offset + 576;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 576;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class trf_accounts_map
		{
			public const int table_index = 12;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public trf_accounts_map() {
			}
			public trf_accounts_map(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public trf_accounts_map(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string tn_account
					{
						get {
							checkReader();
							stream.Position = offset + 32;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 34);
						}
						set {
							checkWriter();
							stream.Position = offset + 32;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 34);
						}
					}
					
				
			
		
		}
	
		public partial class multileg_dict
		{
			public const int table_index = 13;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public multileg_dict() {
			}
			public multileg_dict(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public multileg_dict(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int isin_id_leg
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
					
				
			
		
			
				
					public  int qty_ratio
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
					
				
			
		
			
				
					public  byte leg_order_no
					{
						get {
							checkReader();
							stream.Position = offset + 40;
							return reader.ReadByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 40;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class fut_ts_cons
		{
			public const int table_index = 14;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public fut_ts_cons() {
			}
			public fut_ts_cons(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public fut_ts_cons(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string code_ts
					{
						get {
							checkReader();
							stream.Position = offset + 24;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 2);
						}
						set {
							checkWriter();
							stream.Position = offset + 24;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 2);
						}
					}
					
				
			
		
			
				
					public  System.DateTime date_last_clear
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime date_last_open_day
					{
						get {
							checkReader();
							stream.Position = offset + 38;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 38;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
		}
	
		public partial class fut_rejected_orders
		{
			public const int table_index = 15;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public fut_rejected_orders() {
			}
			public fut_rejected_orders(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public fut_rejected_orders(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  long order_id
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
					
				
			
		
			
				
					public  System.DateTime moment
					{
						get {
							checkReader();
							stream.Position = offset + 36;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 36;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  int isin_id
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
					
				
			
		
			
				
					public  string client_code
					{
						get {
							checkReader();
							stream.Position = offset + 52;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 52;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  sbyte dir
					{
						get {
							checkReader();
							stream.Position = offset + 60;
							return reader.ReadSByte();
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
							stream.Position = offset + 64;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 64;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long xamount
					{
						get {
							checkReader();
							stream.Position = offset + 68;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 68;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal price
					{
						get {
							checkReader();
							stream.Position = offset + 76;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 76;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte price_scale
					{
						get {
							stream.Position = offset + 76;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime date_exp
					{
						get {
							checkReader();
							stream.Position = offset + 88;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 88;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  long id_ord1
					{
						get {
							checkReader();
							stream.Position = offset + 100;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 100;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime moment_reject
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
					
				
			
		
			
				
					public  int ret_code
					{
						get {
							checkReader();
							stream.Position = offset + 120;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 120;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string ret_message
					{
						get {
							checkReader();
							stream.Position = offset + 124;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
						}
						set {
							checkWriter();
							stream.Position = offset + 124;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
						}
					}
					
				
			
		
			
				
					public  string comment
					{
						get {
							checkReader();
							stream.Position = offset + 380;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 380;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  string login_from
					{
						get {
							checkReader();
							stream.Position = offset + 401;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
						}
						set {
							checkWriter();
							stream.Position = offset + 401;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
						}
					}
					
				
			
		
			
				
					public  int ext_id
					{
						get {
							checkReader();
							stream.Position = offset + 424;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 424;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class fut_intercl_info
		{
			public const int table_index = 16;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public fut_intercl_info() {
			}
			public fut_intercl_info(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public fut_intercl_info(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string client_code
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  decimal vm_intercl
					{
						get {
							checkReader();
							stream.Position = offset + 36;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 36;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.2", (decimal)value);
						}
					}
					
					public byte vm_intercl_scale
					{
						get {
							stream.Position = offset + 36;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
		}
	
		public partial class fut_bond_nkd
		{
			public const int table_index = 17;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public fut_bond_nkd() {
			}
			public fut_bond_nkd(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public fut_bond_nkd(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int bond_id
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
					
				
			
		
			
				
					public  System.DateTime date
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  decimal nkd
					{
						get {
							checkReader();
							stream.Position = offset + 38;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 38;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.7", (decimal)value);
						}
					}
					
					public byte nkd_scale
					{
						get {
							stream.Position = offset + 38;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte is_cupon
					{
						get {
							checkReader();
							stream.Position = offset + 49;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 49;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class fut_bond_nominal
		{
			public const int table_index = 18;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public fut_bond_nominal() {
			}
			public fut_bond_nominal(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public fut_bond_nominal(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int bond_id
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
					
				
			
		
			
				
					public  System.DateTime date
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  decimal nominal
					{
						get {
							checkReader();
							stream.Position = offset + 38;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 38;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte nominal_scale
					{
						get {
							stream.Position = offset + 38;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal face_value
					{
						get {
							checkReader();
							stream.Position = offset + 49;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 49;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte face_value_scale
					{
						get {
							stream.Position = offset + 49;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal coupon_nominal
					{
						get {
							checkReader();
							stream.Position = offset + 60;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 60;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d8.5", (decimal)value);
						}
					}
					
					public byte coupon_nominal_scale
					{
						get {
							stream.Position = offset + 60;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte is_nominal
					{
						get {
							checkReader();
							stream.Position = offset + 67;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 67;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class fut_bond_isin
		{
			public const int table_index = 19;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public fut_bond_isin() {
			}
			public fut_bond_isin(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public fut_bond_isin(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int bond_id
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
					
				
			
		
			
				
					public  decimal coeff_conversion
					{
						get {
							checkReader();
							stream.Position = offset + 32;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 32;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d5.4", (decimal)value);
						}
					}
					
					public byte coeff_conversion_scale
					{
						get {
							stream.Position = offset + 32;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
		}
	
		public partial class usd_online
		{
			public const int table_index = 20;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public usd_online() {
			}
			public usd_online(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public usd_online(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  long id
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
					
				
			
		
			
				
					public  decimal rate
					{
						get {
							checkReader();
							stream.Position = offset + 32;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 32;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.4", (decimal)value);
						}
					}
					
					public byte rate_scale
					{
						get {
							stream.Position = offset + 32;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  System.DateTime moment
					{
						get {
							checkReader();
							stream.Position = offset + 42;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 42;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
		}
	
		public partial class investr
		{
			public const int table_index = 21;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public investr() {
			}
			public investr(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public investr(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string name
					{
						get {
							checkReader();
							stream.Position = offset + 32;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 200);
						}
						set {
							checkWriter();
							stream.Position = offset + 32;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 200);
						}
					}
					
				
			
		
			
				
					public  int status
					{
						get {
							checkReader();
							stream.Position = offset + 236;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 236;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte calendar_spread_margin_type
					{
						get {
							checkReader();
							stream.Position = offset + 240;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 240;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class fut_sess_settl
		{
			public const int table_index = 22;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public fut_sess_settl() {
			}
			public fut_sess_settl(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public fut_sess_settl(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  System.DateTime date_clr
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  string isin
					{
						get {
							checkReader();
							stream.Position = offset + 38;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 38;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  int isin_id
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
					
				
			
		
			
				
					public  decimal settl_price
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
					
					public byte settl_price_scale
					{
						get {
							stream.Position = offset + 68;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
		}
	
		public partial class fut_margin_type
		{
			public const int table_index = 23;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public fut_margin_type() {
			}
			public fut_margin_type(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public fut_margin_type(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string code
					{
						get {
							checkReader();
							stream.Position = offset + 24;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 12);
						}
						set {
							checkWriter();
							stream.Position = offset + 24;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 12);
						}
					}
					
				
			
		
			
				
					public  sbyte type
					{
						get {
							checkReader();
							stream.Position = offset + 37;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 37;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte margin_type
					{
						get {
							checkReader();
							stream.Position = offset + 38;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 38;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte UCP_type
					{
						get {
							checkReader();
							stream.Position = offset + 39;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 39;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal prohibit_coeff
					{
						get {
							checkReader();
							stream.Position = offset + 40;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 40;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.2", (decimal)value);
						}
					}
					
					public byte prohibit_coeff_scale
					{
						get {
							stream.Position = offset + 40;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  int prohibit_type
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
					
				
			
		
			
				
					public  sbyte settlement_account_type
					{
						get {
							checkReader();
							stream.Position = offset + 56;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 56;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class fut_settlement_account
		{
			public const int table_index = 24;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public fut_settlement_account() {
			}
			public fut_settlement_account(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public fut_settlement_account(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string code
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
					
				
			
		
			
				
					public  sbyte type
					{
						get {
							checkReader();
							stream.Position = offset + 32;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 32;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string settlement_account
					{
						get {
							checkReader();
							stream.Position = offset + 33;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 12);
						}
						set {
							checkWriter();
							stream.Position = offset + 33;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 12);
						}
					}
					
				
			
		
		}
	
		public partial class fut_view_market_data_spot_instrument
		{
			public const int table_index = 25;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public fut_view_market_data_spot_instrument() {
			}
			public fut_view_market_data_spot_instrument(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public fut_view_market_data_spot_instrument(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string code_vcb
					{
						get {
							checkReader();
							stream.Position = offset + 24;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 24;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  string code_CLRL
					{
						get {
							checkReader();
							stream.Position = offset + 50;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 50;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  string code_md
					{
						get {
							checkReader();
							stream.Position = offset + 76;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 12);
						}
						set {
							checkWriter();
							stream.Position = offset + 76;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 12);
						}
					}
					
				
			
		
		}
	
		public partial class session
		{
			public const int table_index = 26;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public session() {
			}
			public session(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public session(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  System.DateTime begin
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime end
					{
						get {
							checkReader();
							stream.Position = offset + 38;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 38;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  int state
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
					
				
			
		
			
				
					public  int opt_sess_id
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
					
				
			
		
			
				
					public  System.DateTime inter_cl_begin
					{
						get {
							checkReader();
							stream.Position = offset + 56;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 56;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime inter_cl_end
					{
						get {
							checkReader();
							stream.Position = offset + 66;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 66;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  int inter_cl_state
					{
						get {
							checkReader();
							stream.Position = offset + 76;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 76;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte eve_on
					{
						get {
							checkReader();
							stream.Position = offset + 80;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 80;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime eve_begin
					{
						get {
							checkReader();
							stream.Position = offset + 82;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 82;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime eve_end
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
					
				
			
		
			
				
					public  sbyte mon_on
					{
						get {
							checkReader();
							stream.Position = offset + 102;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 102;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime mon_begin
					{
						get {
							checkReader();
							stream.Position = offset + 104;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 104;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime mon_end
					{
						get {
							checkReader();
							stream.Position = offset + 114;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 114;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime pos_transfer_begin
					{
						get {
							checkReader();
							stream.Position = offset + 124;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 124;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime pos_transfer_end
					{
						get {
							checkReader();
							stream.Position = offset + 134;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 134;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  sbyte cleared
					{
						get {
							checkReader();
							stream.Position = offset + 144;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 144;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class dealer
		{
			public const int table_index = 27;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public dealer() {
			}
			public dealer(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public dealer(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string name
					{
						get {
							checkReader();
							stream.Position = offset + 32;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 200);
						}
						set {
							checkWriter();
							stream.Position = offset + 32;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 200);
						}
					}
					
				
			
		
			
				
					public  string rts_code
					{
						get {
							checkReader();
							stream.Position = offset + 233;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 50);
						}
						set {
							checkWriter();
							stream.Position = offset + 233;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 50);
						}
					}
					
				
			
		
			
				
					public  int signs
					{
						get {
							checkReader();
							stream.Position = offset + 284;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 284;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int status
					{
						get {
							checkReader();
							stream.Position = offset + 288;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 288;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  string transfer_code
					{
						get {
							checkReader();
							stream.Position = offset + 292;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 7);
						}
						set {
							checkWriter();
							stream.Position = offset + 292;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 7);
						}
					}
					
				
			
		
			
				
					public  decimal exp_weight
					{
						get {
							checkReader();
							stream.Position = offset + 300;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 300;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d3.2", (decimal)value);
						}
					}
					
					public byte exp_weight_scale
					{
						get {
							stream.Position = offset + 300;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  int num_clr_2delivery
					{
						get {
							checkReader();
							stream.Position = offset + 304;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 304;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte margin_type
					{
						get {
							checkReader();
							stream.Position = offset + 308;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 308;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte calendar_spread_margin_type
					{
						get {
							checkReader();
							stream.Position = offset + 309;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 309;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int num_clr_2delivery_client_default
					{
						get {
							checkReader();
							stream.Position = offset + 312;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 312;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal exp_weight_client_default
					{
						get {
							checkReader();
							stream.Position = offset + 316;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 316;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d3.2", (decimal)value);
						}
					}
					
					public byte exp_weight_client_default_scale
					{
						get {
							stream.Position = offset + 316;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal go_ratio
					{
						get {
							checkReader();
							stream.Position = offset + 320;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 320;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte go_ratio_scale
					{
						get {
							stream.Position = offset + 320;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte check_limit_on_withdrawal
					{
						get {
							checkReader();
							stream.Position = offset + 331;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 331;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte limit_tied_money
					{
						get {
							checkReader();
							stream.Position = offset + 332;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 332;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte limits_set
					{
						get {
							checkReader();
							stream.Position = offset + 333;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 333;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte no_fut_discount
					{
						get {
							checkReader();
							stream.Position = offset + 334;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 334;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte no_fut_discount_client_default
					{
						get {
							checkReader();
							stream.Position = offset + 335;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 335;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class investor
		{
			public const int table_index = 28;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public investor() {
			}
			public investor(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public investor(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string name
					{
						get {
							checkReader();
							stream.Position = offset + 32;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 200);
						}
						set {
							checkWriter();
							stream.Position = offset + 32;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 200);
						}
					}
					
				
			
		
			
				
					public  int status
					{
						get {
							checkReader();
							stream.Position = offset + 236;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 236;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte calendar_spread_margin_type
					{
						get {
							checkReader();
							stream.Position = offset + 240;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 240;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class sys_events
		{
			public const int table_index = 29;
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
	
	


}
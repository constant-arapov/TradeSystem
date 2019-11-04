namespace INFO{

	// Scheme "INFO" description
	
	
		public partial class currency_params
		{
			public const int table_index = 0;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public currency_params() {
			}
			public currency_params(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public currency_params(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int currency_id
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
					
				
			
		
			
				
					public  double radius
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte signs
					{
						get {
							checkReader();
							stream.Position = offset + 36;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 36;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class base_contracts_params
		{
			public const int table_index = 1;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public base_contracts_params() {
			}
			public base_contracts_params(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public base_contracts_params(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string code_mcs
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
					
				
			
		
			
				
					public  sbyte volat_num
					{
						get {
							checkReader();
							stream.Position = offset + 76;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 76;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double subrisk_step
					{
						get {
							checkReader();
							stream.Position = offset + 80;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 80;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte is_percent
					{
						get {
							checkReader();
							stream.Position = offset + 88;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 88;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte has_options
					{
						get {
							checkReader();
							stream.Position = offset + 89;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 89;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal percent_rate
					{
						get {
							checkReader();
							stream.Position = offset + 90;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 90;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte percent_rate_scale
					{
						get {
							stream.Position = offset + 90;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal currency_volat
					{
						get {
							checkReader();
							stream.Position = offset + 101;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 101;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte currency_volat_scale
					{
						get {
							stream.Position = offset + 101;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte is_usd
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
					
				
			
		
			
				
					public  double somc
					{
						get {
							checkReader();
							stream.Position = offset + 116;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 116;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte msp_type
					{
						get {
							checkReader();
							stream.Position = offset + 124;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 124;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int currency_id
					{
						get {
							checkReader();
							stream.Position = offset + 128;
							return reader.ReadInt32();
						}
						set {
							checkWriter();
							stream.Position = offset + 128;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double spot_price
					{
						get {
							checkReader();
							stream.Position = offset + 132;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 132;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double mr1
					{
						get {
							checkReader();
							stream.Position = offset + 140;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 140;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double mr2
					{
						get {
							checkReader();
							stream.Position = offset + 148;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 148;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double mr3
					{
						get {
							checkReader();
							stream.Position = offset + 156;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 156;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long lk1
					{
						get {
							checkReader();
							stream.Position = offset + 164;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 164;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  long lk2
					{
						get {
							checkReader();
							stream.Position = offset + 172;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 172;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int risk_points_n
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
					
				
			
		
		}
	
		public partial class futures_params
		{
			public const int table_index = 2;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public futures_params() {
			}
			public futures_params(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public futures_params(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string isin
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
					
				
			
		
			
				
					public  int isin_id
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
					
				
			
		
			
				
					public  string code_vcb
					{
						get {
							checkReader();
							stream.Position = offset + 56;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 56;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  decimal settl_price
					{
						get {
							checkReader();
							stream.Position = offset + 82;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 82;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte settl_price_scale
					{
						get {
							stream.Position = offset + 82;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte spread_aspect
					{
						get {
							checkReader();
							stream.Position = offset + 93;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 93;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte subrisk
					{
						get {
							checkReader();
							stream.Position = offset + 94;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 94;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double step_price
					{
						get {
							checkReader();
							stream.Position = offset + 96;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 96;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime exp_date
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
					
				
			
		
			
				
					public  decimal settl_price_real
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
					
					public byte settl_price_real_scale
					{
						get {
							stream.Position = offset + 114;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  double min_step
					{
						get {
							checkReader();
							stream.Position = offset + 128;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 128;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int lot
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
					
				
			
		
			
				
					public  double interest_rate_risk_up
					{
						get {
							checkReader();
							stream.Position = offset + 140;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 140;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double interest_rate_risk_down
					{
						get {
							checkReader();
							stream.Position = offset + 148;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 148;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double time_to_expiration
					{
						get {
							checkReader();
							stream.Position = offset + 156;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 156;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double normalized_spot
					{
						get {
							checkReader();
							stream.Position = offset + 164;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 164;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class virtual_futures_params
		{
			public const int table_index = 3;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public virtual_futures_params() {
			}
			public virtual_futures_params(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public virtual_futures_params(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string isin
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
					
				
			
		
			
				
					public  string isin_base
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
					
				
			
		
			
				
					public  double a
					{
						get {
							checkReader();
							stream.Position = offset + 76;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 76;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double b
					{
						get {
							checkReader();
							stream.Position = offset + 84;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 84;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double c
					{
						get {
							checkReader();
							stream.Position = offset + 92;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 92;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double d
					{
						get {
							checkReader();
							stream.Position = offset + 100;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 100;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double e
					{
						get {
							checkReader();
							stream.Position = offset + 108;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 108;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  System.DateTime exp_date
					{
						get {
							checkReader();
							stream.Position = offset + 116;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 116;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
			
				
					public  sbyte use_null_volat
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
					
				
			
		
			
				
					public  double s
					{
						get {
							checkReader();
							stream.Position = offset + 128;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 128;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double strike_step
					{
						get {
							checkReader();
							stream.Position = offset + 136;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 136;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int exp_clearings_sa
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
					
				
			
		
			
				
					public  int exp_clearings_bf
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
					
				
			
		
			
				
					public  int exp_clearings_cc
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
					
				
			
		
			
				
					public  double volatility_risk
					{
						get {
							checkReader();
							stream.Position = offset + 156;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 156;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double volatility_risk_mismatch
					{
						get {
							checkReader();
							stream.Position = offset + 164;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 164;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  double time_to_expiration
					{
						get {
							checkReader();
							stream.Position = offset + 172;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 172;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class options_params
		{
			public const int table_index = 4;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public options_params() {
			}
			public options_params(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public options_params(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  string isin
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
					
				
			
		
			
				
					public  int isin_id
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
					
				
			
		
			
				
					public  string isin_base
					{
						get {
							checkReader();
							stream.Position = offset + 56;
							return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
						}
						set {
							checkWriter();
							stream.Position = offset + 56;
							ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
						}
					}
					
				
			
		
			
				
					public  decimal strike
					{
						get {
							checkReader();
							stream.Position = offset + 82;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 82;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte strike_scale
					{
						get {
							stream.Position = offset + 82;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte opt_type
					{
						get {
							checkReader();
							stream.Position = offset + 93;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 93;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  decimal settl_price
					{
						get {
							checkReader();
							stream.Position = offset + 94;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 94;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte settl_price_scale
					{
						get {
							stream.Position = offset + 94;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
		}
	
		public partial class investor
		{
			public const int table_index = 5;
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
					
				
			
		
			
				
					public  sbyte calendar_spread_margin_type
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
					
				
			
		
			
				
					public  int n_clr_2delivery
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
					
				
			
		
			
				
					public  decimal exp_weight
					{
						get {
							checkReader();
							stream.Position = offset + 40;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 40;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d3.2", (decimal)value);
						}
					}
					
					public byte exp_weight_scale
					{
						get {
							stream.Position = offset + 40;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal go_ratio
					{
						get {
							checkReader();
							stream.Position = offset + 44;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 44;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte go_ratio_scale
					{
						get {
							stream.Position = offset + 44;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte no_fut_discount
					{
						get {
							checkReader();
							stream.Position = offset + 55;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 55;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class dealer
		{
			public const int table_index = 6;
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
					
				
			
		
			
				
					public  sbyte margin_type
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
					
				
			
		
			
				
					public  sbyte calendar_spread_margin_type
					{
						get {
							checkReader();
							stream.Position = offset + 33;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 33;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte check_limit_on_withdrawal
					{
						get {
							checkReader();
							stream.Position = offset + 34;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 34;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  sbyte limit_tied_money
					{
						get {
							checkReader();
							stream.Position = offset + 35;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 35;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int num_clr_2delivery
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
					
				
			
		
			
				
					public  decimal exp_weight
					{
						get {
							checkReader();
							stream.Position = offset + 40;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 40;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d3.2", (decimal)value);
						}
					}
					
					public byte exp_weight_scale
					{
						get {
							stream.Position = offset + 40;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  decimal go_ratio
					{
						get {
							checkReader();
							stream.Position = offset + 44;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 44;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
						}
					}
					
					public byte go_ratio_scale
					{
						get {
							stream.Position = offset + 44;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte no_fut_discount
					{
						get {
							checkReader();
							stream.Position = offset + 55;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 55;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int num_clr_2delivery_client_default
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
					
				
			
		
			
				
					public  decimal exp_weight_client_default
					{
						get {
							checkReader();
							stream.Position = offset + 60;
							return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
						}
						set {
							checkWriter();
							stream.Position = offset + 60;
							ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d3.2", (decimal)value);
						}
					}
					
					public byte exp_weight_client_default_scale
					{
						get {
							stream.Position = offset + 60;
							return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
						}
					}
					
				
			
		
			
				
					public  sbyte no_fut_discount_client_default
					{
						get {
							checkReader();
							stream.Position = offset + 64;
							return reader.ReadSByte();
						}
						set {
							checkWriter();
							stream.Position = offset + 64;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class common_params
		{
			public const int table_index = 7;
			private System.IO.UnmanagedMemoryStream stream;
			private int offset;
			private System.IO.BinaryReader reader;
			private System.IO.BinaryWriter writer;
			public common_params() {
			}
			public common_params(System.IO.UnmanagedMemoryStream stream_) {
				Data = stream_;
				offset = 0;
			}
			public common_params(System.IO.UnmanagedMemoryStream stream_, int offset_) {
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
					
				
			
		
			
				
					public  int common_rev
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
					
				
			
		
			
				
					public  double edge_coeff
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return reader.ReadDouble();
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							writer.Write(value);
						}
					}
					
				
			
		
		}
	
		public partial class sys_events
		{
			public const int table_index = 8;
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
					
				
			
		
			
				
					public  int event_type
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
					
				
			
		
			
				
					public  long event_id
					{
						get {
							checkReader();
							stream.Position = offset + 28;
							return reader.ReadInt64();
						}
						set {
							checkWriter();
							stream.Position = offset + 28;
							writer.Write(value);
						}
					}
					
				
			
		
			
				
					public  int sess_id
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
					
				
			
		
			
				
					public  System.DateTime server_time
					{
						get {
							checkReader();
							stream.Position = offset + 106;
							return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
						}
						set {
							checkWriter();
							stream.Position = offset + 106;
							ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
						}
					}
					
				
			
		
		}
	
	


}
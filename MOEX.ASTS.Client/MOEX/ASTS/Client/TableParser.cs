namespace MOEX.ASTS.Client
{
    using System;
    using System.IO;

    internal class TableParser : Parser
    {
        private string filter;
        private Meta.Message source;

        internal TableParser(MOEX.ASTS.Client.Client client, Meta.Message source, string filter, byte[] buffer) : base(client, buffer)
        {
            this.source = source;
            this.filter = filter;
        }

        internal static TableParser Acquire(MOEX.ASTS.Client.Client client, Meta.Message source, string filter, byte[] buffer)
        {
            return new TableParser(client, source, filter, buffer);
        }

        protected override int Parse(IBinder binder)
        {
            if ((this.source == null) || (base.buffer == null))
            {
                return 0;
            }
            using (BinaryReader reader = new BinaryReader(new MemoryStream(base.buffer, false), Parser.Win1251))
            {
                reader.ReadInt32();
                ITarget target = (binder == null) ? null : binder.Detect(this.source);
                return base.Parse(reader, this.source, this.filter, target);
            }
        }

        public override bool IsEmpty
        {
            get
            {
                return (base.Length == 0);
            }
        }
    }
}


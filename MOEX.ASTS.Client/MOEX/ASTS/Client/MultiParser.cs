namespace MOEX.ASTS.Client
{
    using System;
    using System.IO;

    internal class MultiParser : Parser
    {
        private string[] filters;
        private Meta.Message[] sources;

        internal MultiParser(MOEX.ASTS.Client.Client client, Meta.Message[] sources, string[] filters, byte[] buffer) : base(client, buffer)
        {
            this.sources = sources;
            this.filters = filters;
        }

        internal static MultiParser Acquire(MOEX.ASTS.Client.Client client, Meta.Message[] sources, string[] filters, byte[] buffer)
        {
            return new MultiParser(client, sources, filters, buffer);
        }

        protected override int Parse(IBinder binder)
        {
            if ((this.sources == null) || (base.buffer == null))
            {
                return 0;
            }
            using (BinaryReader reader = new BinaryReader(new MemoryStream(base.buffer, false), Parser.Win1251))
            {
                int num = 0;
                for (int i = reader.ReadInt32(); i > 0; i--)
                {
                    int index = reader.ReadInt32();
                    Meta.Message source = this.sources[index];
                    ITarget target = (binder == null) ? null : binder.Detect(source);
                    num += base.Parse(reader, source, this.filters[index], target);
                }
                return num;
            }
        }

        public override bool IsEmpty
        {
            get
            {
                return (base.Length <= 4);
            }
        }
    }
}


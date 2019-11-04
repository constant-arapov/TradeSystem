namespace MOEX.ASTS.Client
{
    using System;

    public class ClientException : Exception
    {
        private readonly int code;

        public ClientException(int code, string text) : base(string.Concat(new object[] { "[", code, "]:", text }))
        {
            this.code = code;
        }

        public int Code
        {
            get
            {
                return this.code;
            }
        }
    }
}


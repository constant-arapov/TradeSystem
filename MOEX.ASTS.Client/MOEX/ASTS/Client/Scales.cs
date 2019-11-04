namespace MOEX.ASTS.Client
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Scales
    {
        private readonly Dictionary<string, int> codes = new Dictionary<string, int>();
        private readonly Dictionary<string, int> exact = new Dictionary<string, int>();
        private readonly ReaderWriterLockSlim guard = new ReaderWriterLockSlim();

        public void Add(string board, string paper, int scale)
        {
            this.guard.EnterWriteLock();
            try
            {
                this.exact[KeyOf(board, paper)] = scale;
                this.codes[paper] = scale;
            }
            finally
            {
                this.guard.ExitWriteLock();
            }
        }

        public void Clear()
        {
            this.guard.EnterWriteLock();
            try
            {
                this.exact.Clear();
                this.codes.Clear();
            }
            finally
            {
                this.guard.ExitWriteLock();
            }
        }

        public bool Find(string paper, out int scale)
        {
            bool flag;
            this.guard.EnterReadLock();
            try
            {
                flag = this.codes.TryGetValue(paper, out scale);
            }
            finally
            {
                this.guard.ExitReadLock();
            }
            return flag;
        }

        public bool Find(string board, string paper, out int scale)
        {
            bool flag;
            this.guard.EnterReadLock();
            try
            {
                flag = this.exact.TryGetValue(KeyOf(board, paper), out scale);
            }
            finally
            {
                this.guard.ExitReadLock();
            }
            return flag;
        }

        public static string KeyOf(string board, string paper)
        {
            if (string.IsNullOrEmpty(board))
            {
                board = string.Empty;
            }
            if (string.IsNullOrEmpty(paper))
            {
                paper = string.Empty;
            }
            return (board + "." + paper);
        }

        public bool Empty
        {
            get
            {
                bool flag;
                this.guard.EnterReadLock();
                try
                {
                    flag = this.exact.Count == 0;
                }
                finally
                {
                    this.guard.ExitReadLock();
                }
                return flag;
            }
        }
    }
}


namespace MOEX.ASTS.Client
{
    using System;

    public interface ITarget
    {
        void DoneRecordUpdate(Meta.Message source, string filter);
        void DoneTableUpdate(Meta.Message source, string filter);
        int GetRecordDecimals();
        bool InitRecordUpdate(Meta.Message source, string filter);
        void InitTableUpdate(Meta.Message source, string filter);
        void SetFieldValue(Meta.Field field, object value);
        void SetKeyValue(Meta.Field field, object value);
        void SetRecordDecimals(int decimals);
        void SwitchOrderbook(Meta.Message source, string filter, string board, string paper);
		bool IsNullDecimal { get; set; }
    }
}


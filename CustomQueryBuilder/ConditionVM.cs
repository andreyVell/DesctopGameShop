using System;

namespace DataBaseIndTask.CustomQueryBuilder
{
    public class ConditionVM
    {
        private enum EValueType
        {
            vt_string=0,
            vt_date=1,
        }
        private string _table;
        private string _field;
        private string _criterion;
        private string _link;
        private EValueType _valueType;
        private string _value;
        private DateTime _valueDate;
        public string Table
        {
            get { return _table; }
        }
        public string Field
        {
            get { return _field; }
        }
        public string Criterion
        {
            get
            {
                return _criterion;
            }
        }
        public string Link
        {
            get
            {
                return _link;
            }
        }
        public string Value
        {
            get
            {
                switch (_valueType)
                {
                    case EValueType.vt_string:
                        return _value;
                    case EValueType.vt_date:
                        return _valueDate.Date.ToString("dd.MM.yyyy");
                    default:
                        return string.Empty;
                }
            }
        }
        public ConditionVM(string table,string field, string criterion, string link, string value)
        {
            _table = table;
            _field = field;
            _criterion = criterion;
            _link = link;
            _valueType = EValueType.vt_string;
            _value = value;
        }
        public ConditionVM(string table, string field, string criterion, string link, DateTime value)
        {
            _table = table;
            _field = field;
            _criterion = criterion;
            _link = link;
            _valueType = EValueType.vt_date;
            _valueDate = value;
        }
        public override string ToString()
        {            
            switch (_valueType)
            {
                case EValueType.vt_string:                    
                    return _link + " " + _table + "." + _field + " " + _criterion + " " + _value;
                case EValueType.vt_date:
                    return _link + " " + _table + "." + _field + " " + _criterion + " " + _valueDate.Date.ToString("dd.MM.yyyy HH:mm:ss");
                default:
                    return string.Empty;
            }
        }
    }
}
using System.Collections.Generic;
using System.Dynamic;

namespace XudonV4NetFramework.Common
{
    public class CreateDynamicClass : DynamicObject
    {
        private IDictionary<string, object> _values;

        public CreateDynamicClass(IDictionary<string, object> values)
        {
            _values = values;
        }
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _values.Keys;
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_values.ContainsKey(binder.Name))
            {
                result = _values[binder.Name];
                return true;
            }
            result = null;
            return false;
        }
    }
}

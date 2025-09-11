using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Exceptions
{
    public class DuplicateFieldException : AppException
    {
        public string FieldName { get; }
        public string FieldValue { get; }

        public DuplicateFieldException(string fieldName, string fieldValue)
            : base(409, $"{fieldName} '{fieldValue}' already exists.") // 409 Conflict
        {
            FieldName = fieldName;
            FieldValue = fieldValue;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinewood.Customer.Business
{
    public class OperationResult
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }


        public List<string> Errors { get; } = new List<string>();

        public bool HasErrors
        {
            get
            {
                return Errors.Count > 0;
            }
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }

    }

    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; }
    }
}

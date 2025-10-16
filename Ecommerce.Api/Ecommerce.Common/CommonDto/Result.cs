using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Common.CommonDto
{
    public abstract class Result
    {
        public List<Errors> Errors { get; set; } = new();
        public bool isError => Errors != null && Errors.Any();
    }

    public class Result<T> : Result
    {
        public T Response { get; set; }
        public string warningMessage { get; set; }


    }
}

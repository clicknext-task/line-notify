using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LineNotifyService.API.ViewModels.BaseViewModel
{
    public class APIResult
    {
        public string id { get; set; }
        public int status { get; set; }
    }

    public class APIResult<T> : APIResult
    {
        public T data { get; set; }
    }
}

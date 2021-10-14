using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LineNotifyService.API.Helper.Model
{

    public class ResultAccessToken
    {
        public string access_token { get; set; }
    }

    public class ResultNotify
    {
        public int status { get; set; }
        public string message { get; set; }
    }

    public class ResultStatus
    {
        public int status { get; set; }
        public string message { get; set; }
        public string targetType { get; set; }
        public string target { get; set; }
    }

    public class ResultRevoke
    {
        public int status { get; set; }
        public string message { get; set; }
    }

}

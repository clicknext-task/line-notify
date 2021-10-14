using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LineNotifyService.API.ViewModels
{
    public class GetOAuthParam
    {
        [Required]
        public string redirect_uri { get; set; }
        [Required]
        public string user_id { get; set; }
    }
    public class GetOAuthViewModel
    {
        public string state { get; set; }
        public string code { get; set; }
    }
}

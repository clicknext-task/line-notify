﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LineNotifyService.API.ViewModels
{

    public class RevokeTokenParam
    {
        [Required]
        public string user_id { get; set; }
    }
}

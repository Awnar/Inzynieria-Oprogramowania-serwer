﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO_2_lib.Models
{
    public class AuthKey
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public DateTime? Expiration { get; set; }
        public string LastAccessIP { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}

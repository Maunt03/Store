﻿using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class Wallet
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Count { get; set; }
    }
}

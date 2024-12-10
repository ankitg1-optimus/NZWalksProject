﻿using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string  Username { get; set; }

        [Required]
        [DataType (DataType.Password)]
        public string Password { get; set; }
    }
}

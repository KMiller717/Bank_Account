using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace Bank_Account.Models
    {
        public class LoginUser
        {
        [Required]
        [EmailAddress]
        public string LoginEmail { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string LoginPassword {get;set;}
        }
    }
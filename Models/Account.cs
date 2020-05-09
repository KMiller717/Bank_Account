using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
    


namespace Bank_Account{

    public class Account{

        [Key]
        public int AccountId {get;set;}

        [Required]
        public int Amount {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public User AccountUser {get; set;}

        public int UserId {get;set;}
    
    }




}


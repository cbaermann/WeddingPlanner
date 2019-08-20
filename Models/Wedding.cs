using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId{get;set;}

        [Required]
        [MinLength(2)]
        public string PartnerOne{get;set;}

        [Required]
        [MinLength(2)]
        public string PartnerTwo{get;set;}
        
        [Required]
        public DateTime Date{get;set;}

        [Required]
        public string Address {get;set;}

        public User Host{get;set;}

        public List<UserWedding> UserWeddings{get;set;}
        public DateTime CreatedAt {get;set;}=DateTime.Now;
        public DateTime UpdatedAt{get;set;}=DateTime.Now;
    }
}
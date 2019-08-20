using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace WeddingPlanner.Models
{
    public class UserWedding
    {
        [Key]
        public int UserWeddingId{get;set;}

        public int UserId{get;set;}
        public int WeddingId{get;set;}

        public User User{get;set;}
        public Wedding Wedding {get;set;}
    }
}
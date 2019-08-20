using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class ViewWeddingViewModel
    {
        public Wedding thisWedding{get;set;}
        public UserWedding UserWedding{get;set;}

        public List<UserWedding> UserWeddings{get;set;}
        public List<User> Users{get;set;}
        public User thisUser {get;set;}

    }
}

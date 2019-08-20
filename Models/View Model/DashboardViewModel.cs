using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class DashboardViewModel
    {
        public User thisUser{get;set;}
        public List<Wedding> currentWeddings{get;set;}
        public UserWedding UserWedding{get;set;}
        public List<User> Hosts {get;set;}
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class CreateViewModel
    {
        public Wedding newWedding{get;set;}
        public UserWedding UserWedding{get;set;}
    }
}
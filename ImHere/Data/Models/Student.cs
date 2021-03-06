﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Data.Models
{
    public enum LocalChurch
    {
        None,
        Methodist,
        NonMethodist
    }
    public class Student
    {
        public int Id { get; set; }
        public int StudentTypeId { get; set; }
        public StudentType StudentType { get; set; }
        public bool IsMethodist { get; set; }
        public LocalChurch LocalChurch { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Differentiator { get; set; }
        [Phone]
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public IEnumerable<CheckIn> CheckIns { get; set; } = new List<CheckIn>();
    }
}

﻿using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyApp.Models
{
    public class Farmer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}

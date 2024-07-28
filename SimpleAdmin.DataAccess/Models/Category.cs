﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SimpleAdmin.DataAccess.Models
{
    public partial class Category
    {
        public Category()
        {
            ProductCategory = new HashSet<ProductCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public string Photo { get; set; }
        public string Url { get; set; }
        public string Seodescription { get; set; }
        public string Seotitle { get; set; }
        public string Seokeywords { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int LastModifiedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User LastModifiedByNavigation { get; set; }
        public virtual ICollection<ProductCategory> ProductCategory { get; set; }
    }
}
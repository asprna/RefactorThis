using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Domain
{
    //[Keyless]
    public record Product
    {
        //[Column(TypeName = "varchar(36)")]
        public Guid Id { get; set; }
        //[Column(TypeName = "varchar(17)")]
        public string Name { get; set; }
        //[Column(TypeName = "varchar(35)")]
        public string Description { get; set; }
        //[Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }
        //[Column(TypeName = "decimal(4,2)")]
        public decimal DeliveryPrice { get; set; }
    }
}

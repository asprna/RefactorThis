using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Domain
{
    //[Keyless]
    public record ProductOption
    {
        //[Column(TypeName = "varchar(36)")]
        public Guid Id { get; set; }
        //[Column(TypeName = "varchar(36)")]
        public Guid ProductId { get; set; }
        //[Column(TypeName = "varchar(9)")]
        public string Name { get; set; }
        //[Column(TypeName = "varchar(23)")]
        public string Description { get; set; }
    }
}

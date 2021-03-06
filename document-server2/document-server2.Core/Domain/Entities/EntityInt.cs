﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace document_server2.Core.Domain.Entities
{
    public abstract class EntityInt
    {
        [Key]
        [Column(TypeName = "int")]
        public int Id { get; protected set; }
    }
}

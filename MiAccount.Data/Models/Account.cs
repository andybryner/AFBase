using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiAccount.Data
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public int? TokenTimeout { get; set; }
        public bool Active { get; set; }
        public long CreatedById { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        public long UpdatedById { get; set; }
        public DateTimeOffset UpdateTime { get; set; }
    }
}

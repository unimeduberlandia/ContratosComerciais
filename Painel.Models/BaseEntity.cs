using System;
using System.ComponentModel.DataAnnotations;

namespace Painel.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public long? Id { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataAtualizado { get; set; }
        public virtual int? Status { get; set; }
        public string status { get; set; }
    }
}

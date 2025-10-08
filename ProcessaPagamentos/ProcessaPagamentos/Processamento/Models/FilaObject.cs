using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processamento.Models
{
    public class FilaObject
    {
        public Guid Id { get; set; } 
        public Guid UsuarioId { get; set; }    

        public decimal Valor { get; set; }
    }
}

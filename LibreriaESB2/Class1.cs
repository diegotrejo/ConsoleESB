using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaESB2
{
    public class PeticionESB
    {
        public string Usuario { get; set; }
        public DateTime FechaHora { get; set; }
        public string Aplicacion { get; set; }
        public string Maquina { get; set; }
        public string Modulo { get; set; }
        public string IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Iva { get; set; }
    }
}

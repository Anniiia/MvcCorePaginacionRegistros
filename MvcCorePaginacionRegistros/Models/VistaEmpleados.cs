using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcCorePaginacionRegistros.Models
{
    [Table("V_GRUPO_EMPLEADOS")]
    public class VistaEmpleados
    {
        [Key]
        [Column("EMP_NO")]
        public int IdEmpleado { get; set; }
        [Column("APELLIDO")]
        public string Apellido { get; set; }
        [Column("OFICIO")]
        public string Oficio { get; set; }
        [Column("SALARIO")]
        public int Salario { get; set; }
        [Column("DEPT_NO")]
        public int IdDepartamento { get; set; }
        [Column("POSICION")]
        public int Posicion { get; set; }
    }
}

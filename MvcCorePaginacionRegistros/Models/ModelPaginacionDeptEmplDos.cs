namespace MvcCorePaginacionRegistros.Models
{
    public class ModelPaginacionDeptEmplDos
    {
        public int NumeroRegistrosEmpleados { get; set; }
        public List<Empleado> Empleados { get; set; }
        //public Departamento DepartamentoElegido { get; set; }
        public Departamento Departamento { get; set; }
    }
}

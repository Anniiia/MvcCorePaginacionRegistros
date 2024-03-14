using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;

namespace MvcCorePaginacionRegistros.Controllers
{
    public class PaginacionController : Controller
    {

        private RepositoryHospital repo;

        public PaginacionController(RepositoryHospital repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> _EmpleadosDepartamentoDosPartial(int? posicion, int iddepartamento)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            ModelPaginacionDeptEmplDos model =
                await this.repo.GetEmpleadoDepartamentoDosAsync(posicion.Value, iddepartamento);
            int numeroRegistros = model.NumeroRegistrosEmpleados;
            int siguiente = posicion.Value + 2;
            if (siguiente > numeroRegistros)
            {
                siguiente = numeroRegistros;
            }
            int anterior = posicion.Value - 2;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["REGISTROS"] = numeroRegistros;
            ViewData["ÚLTIMO"] = numeroRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["DEPART"] = model.Departamento;
            ViewData["POSICION"] = posicion;
            List<Empleado> empleados = model.Empleados;
            return PartialView("_EmpleadosDepartamentoDosPartial", empleados);
        }

        public async Task<IActionResult> EmpleadosDepartamentoOutDos
            (int? posicion, int iddepartamento)
        {
            if (posicion == null)
            {
                //POSICION PARA EL EMPLEADO
                posicion = 1;
            }
            ModelPaginacionDeptEmplDos model = await
                this.repo.GetEmpleadoDepartamentoDosAsync
                (posicion.Value, iddepartamento);
            Departamento departamento =
                await this.repo.FindDepartamentosAsync(iddepartamento);
            ViewData["DEPART"] = departamento;
            ViewData["REGISTROS"] = model.NumeroRegistrosEmpleados;
            ViewData["DEPARTAMENTO"] = iddepartamento;
            int siguiente = posicion.Value + 2;
            //DEBEMOS COMPROBAR QUE NO PASAMOS DEL NUMERO DE REGISTROS
            if (siguiente > model.NumeroRegistrosEmpleados)
            {
                //EFECTO OPTICO
                siguiente = model.NumeroRegistrosEmpleados;
            }
            int anterior = posicion.Value - 2;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["ULTIMO"] = model.NumeroRegistrosEmpleados;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;
            List<Empleado> empleados = model.Empleados;
            return View(empleados);
        }
        public async  Task<IActionResult> _EmpleadosDepartamentoPartial(int? posicion, int iddepartamento)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            ModelPaginacionDepartamentosEmpleados model =
                await this.repo.GetEmpleadoDepartamentoAsync
                (posicion.Value, iddepartamento);
            int numeroRegistros = model.NumeroRegistrosEmpleados;
            int siguiente = posicion.Value + 1;
            if (siguiente > numeroRegistros)
            {
                siguiente = numeroRegistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["ÚLTIMO"] = numeroRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["DEPART"] = model.Departamento;
            ViewData["POSICION"] = posicion;
            return PartialView("_EmpleadosDepartamentoPartial", model.Empleado);
        }

            public async Task<IActionResult> EmpleadosDepartamentoOut
            (int? posicion, int iddepartamento)
        {
            if (posicion == null)
            {
                //POSICION PARA EL EMPLEADO
                posicion = 1;
            }
            ModelPaginacionDepartamentosEmpleados model = await
                this.repo.GetEmpleadoDepartamentoAsync
                (posicion.Value, iddepartamento);
            Departamento departamento =
                await this.repo.FindDepartamentosAsync(iddepartamento);
            ViewData["DEPARTAMENTOSELECCIONADO"] = departamento;
            ViewData["REGISTROS"] = model.NumeroRegistrosEmpleados;
            ViewData["DEPARTAMENTO"] = iddepartamento;
            int siguiente = posicion.Value + 1;
            //DEBEMOS COMPROBAR QUE NO PASAMOS DEL NUMERO DE REGISTROS
            if (siguiente > model.NumeroRegistrosEmpleados)
            {
                //EFECTO OPTICO
                siguiente = model.NumeroRegistrosEmpleados;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["ULTIMO"] = model.NumeroRegistrosEmpleados;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;
            return View(model.Empleado);
        }
        //public async Task<IActionResult> Departamentos()
        //{
        //    List<Departamento> departamentos = await this.repo.GetDepartamentosAsync();
        //    return View(departamentos);
        //}

        public async Task<IActionResult> EmpleadosOficioOut(int? posicion, string oficio)
        {
            if (posicion == null)
            {
                posicion = 1;
                return View();
            }
            else
            {
                ModelPaginacionEmpleados model = await
                   this.repo.GetGrupoEmpleadoOficioOutAsync(posicion.Value, oficio);
                ViewData["REGISTROS"] = model.NumeroRegistros;
                ViewData["OFICIO"] = oficio;
                return View(model.Empleados);

            }

        }

        [HttpPost]

        public async Task<IActionResult> EmpleadosOficioOut(string oficio)
        {

            ModelPaginacionEmpleados model = await
                this.repo.GetGrupoEmpleadoOficioOutAsync(1, oficio);

            ViewData["REGISTROS"] = model.NumeroRegistros;
            ViewData["OFICIO"] = oficio;
            return View(model.Empleados);

        }



        public async Task<IActionResult> PaginarGrupoEmpleadosOficio(int? posicion, string oficio)
        {
            if (posicion == null)
            {
                posicion = 1;
                return View();
            }
            else 
            {
                List<Empleado> empleados = await
                   this.repo.GetEmpleadosOficioAsync(posicion.Value, oficio);
                int registros = await this.repo.GetNumeroEmpleadosOficioAsync(oficio);
                ViewData["REGISTROS"] = registros;
                ViewData["OFICIO"] = oficio;
                return View(empleados);

            }

        }

        [HttpPost]

        public async Task<IActionResult> PaginarGrupoEmpleadosOficio(string oficio)
        {

            List<Empleado> empleados = await
                this.repo.GetEmpleadosOficioAsync(1, oficio);
            int registros = await this.repo.GetNumeroEmpleadosOficioAsync(oficio);
            ViewData["REGISTROS"] = registros;
            ViewData["OFICIO"] = oficio;
            return View(empleados);

        }

        public async Task<IActionResult> PaginarGrupoEmpleados(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }

            int numeroRegistros = await this.repo.GetNumeroRegistrosVistaEmpleadosAsync();
            ViewData["REGISTROS"] = numeroRegistros;
            List<Empleado> empleados = await this.repo.GetGrupoEmpleadosAsync(posicion.Value);


            return View(empleados);
        }
        public async Task<IActionResult> PaginarGrupoDepartamentos(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }

            int numeroRegistros = await this.repo.GetNumeroRegistrosVistaDepartamentos();
            ViewData["REGISTROS"] = numeroRegistros;
            List<Departamento> departamentos = await this.repo.GetGrupoDepartamentosAsync(posicion.Value);


            return View(departamentos);
        }

        public async Task<IActionResult> PaginarGrupoVistaDepartamento(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }

            int numeroRegistros = await this.repo.GetNumeroRegistrosVistaDepartamentos();
            ViewData["REGISTROS"] = numeroRegistros;
            List<VistaDepartamento> departamentos = await this.repo.GetGrupoVistaDepartamentosAsync(posicion.Value);

            return View(departamentos);
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> PaginarRegistroVistaDepartamento(int? posicion)
        {
            if (posicion == null)
            {
                //ponemos la posicion en el primer registro
                posicion = 1;
            }
            int numeroRegistros = await this.repo.GetNumeroRegistrosVistaDepartamentos();

            int siguiente = posicion.Value + 1;
            //debemos comprobar que no nos pasamos del numero de registros
            if (siguiente > numeroRegistros)
            {
                //efecto optico
                siguiente = numeroRegistros;
            }

            int anterior = posicion.Value - 1;

            if (anterior < 1)
            {
                anterior = 1;
            }

            VistaDepartamento vista = await this.repo.GetVistaDepartamentoAsync(posicion.Value);
            ViewData["ULTIMO"] = numeroRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            
            return View(vista);
        }
    }
}

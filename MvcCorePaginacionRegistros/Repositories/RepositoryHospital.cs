using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Data;
using MvcCorePaginacionRegistros.Models;
using System.Data;
using System.Diagnostics.Metrics;

#region PROCEDIMIENTOS ALMACENADOS
//create view V_DEPARTAMENTOS_INDIVIDUAL
//as
//	select cast(
//	ROW_NUMBER() over(order by dept_no) as int) as posicion,
//    isnull(dept_no, 0) as dept_no, dnombre, loc from dept
//go

//select* from V_DEPARTAMENTOS_INDIVIDUAL where posicion = 1




//alter procedure SP_GRUPO_DEPARTAMENTOS(@posicion int)
//as
//	select dept_no, dnombre, loc from V_DEPARTAMENTOS_INDIVIDUAL where posicion >=@posicion and posicion <(@posicion +2)
//go

//exec SP_GRUPO_DEPARTAMENTOS 1

//alter PROCEDURE SP_GRUPO_DEPARTAMENTOS
//(@POSICION INT)
//AS
//	SELECT DEPT_NO, DNOMBRE, LOC
//	FROM V_DEPARTAMENTOS_INDIVIDUAL
//	WHERE POSICION >= @POSICION AND POSICION < (@POSICION + 2)
//GO

//EXEC SP_GRUPO_DEPARTAMENTOS 1

//alter VIEW V_DEPARTAMENTOS_INDIVIDUAL
//AS
//	SELECT CAST(
//	ROW_NUMBER() OVER (ORDER BY DEPT_NO) AS INT) AS POSICION,
//    ISNULL(DEPT_NO, 0) AS DEPT_NO, DNOMBRE, LOC FROM DEPT
//GO

//SELECT * FROM V_DEPARTAMENTOS_INDIVIDUAL WHERE POSICION=1







//alter view V_GRUPO_EMPLEADOS
//as
//	select cast(
//	ROW_NUMBER() over(order by apellido) as int) as posicion,
//    isnull(emp_no, 0) as emp_no, apellido, oficio, salario, dept_no from emp
//go

//create PROCEDURE SP_GRUPO_EMPLEADOS
//(@POSICION INT)
//AS
//	SELECT emp_no, apellido, oficio, salario, dept_no
//	FROM V_GRUPO_EMPLEADOS
//	WHERE POSICION >= @POSICION AND POSICION < (@POSICION + 3)
//GO




//create procedure SP_GRUPO_EMPLEADOS_OFICIO (@posicion int, @oficio nvarchar(50))
//as
//	select * from(
//	select cast(
//	ROW_NUMBER() over(order by apellido) as int) as posicion,
//   emp_no, apellido, oficio, salario, dept_no from emp where oficio = @oficio) as query
//   where query.posicion >= @posicion and query.posicion <(@posicion+2)
//go

//exec SP_GRUPO_EMPLEADOS_OFICIO 1,'EMPLEADO'


//create procedure SP_GRUPO_EMPLEADOS_OFICIO_OUT
//(@posicion int, @oficio nvarchar(50)
//, @registros int out)
//as
//select @registros = count(EMP_NO) from EMP
//where OFICIO=@oficio
//select EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO from 
//    (select cast(
//    ROW_NUMBER() OVER (ORDER BY APELLIDO) as int) AS POSICION
//    , EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
//    from EMP
//    where OFICIO = @oficio) as QUERY
//    where QUERY.POSICION >= @posicion and QUERY.POSICION < (@posicion + 2)
//go


//alter procedure SP_GRUPO_EMPLEADOS_DEPARTAMENTO_OUT
//(@posicion int, @iddepartamento int
//, @registros int out)
//as
//select @registros = count(EMP_NO) from EMP
//where DEPT_NO=@iddepartamento
//select EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO from 
//    (select cast(
//    ROW_NUMBER() OVER (ORDER BY APELLIDO) as int) AS POSICION
//    , EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
//    from EMP
//    where DEPT_NO = @iddepartamento) as QUERY
//    where QUERY.POSICION >= @posicion and QUERY.POSICION < (@posicion + 1)
//go

//alter procedure SP_GRUPO_EMPLEADOS_DEPARTAMENTO_OUT
//(@posicion int, @iddepartamento int
//, @registros int out)
//as
//select @registros = count(EMP_NO) from EMP
//where DEPT_NO=@iddepartamento
//select EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO from 
//    (select cast(
//    ROW_NUMBER() OVER (ORDER BY APELLIDO) as int) AS POSICION
//    , EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
//    from EMP
//    where DEPT_NO = @iddepartamento) as QUERY
//    where QUERY.POSICION >= @posicion
//go

//declare @registros int 
//set @registros = 0
//exec SP_GRUPO_EMPLEADOS_DEPARTAMENTO_DOSOUT 1,10, @registros out

#endregion

namespace MvcCorePaginacionRegistros.Repositories
{

    public class RepositoryHospital
    {
        private HospitalContext context;

        public RepositoryHospital(HospitalContext context) {

            this.context = context;
        }
        public async Task<ModelPaginacionDeptEmplDos>
           GetEmpleadoDepartamentoDosAsync
           (int posicion, int iddepartamento)
        {
            string sql = "SP_GRUPO_EMPLEADOS_DEPARTAMENTO_DOSOUT @posicion, @departamento, "
                + " @registros out";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamDepartamento =
                new SqlParameter("@departamento", iddepartamento);
            SqlParameter pamRegistros = new SqlParameter("@registros", -1);
            pamRegistros.Direction = ParameterDirection.Output;
            var consulta =
                this.context.Empleados.FromSqlRaw
                (sql, pamPosicion, pamDepartamento, pamRegistros);
            //PRIMERO DEBEMOS EJECUTAR LA CONSULTA PARA PODER RECUPERAR 
            //LOS PARAMETROS DE SALIDA
            var datos = await consulta.ToListAsync();
            List<Empleado> empleados = datos;
            int registros = (int)pamRegistros.Value;
            return new ModelPaginacionDeptEmplDos
            {
                NumeroRegistrosEmpleados = registros,
                Empleados = empleados
            };
        }

        public async Task<ModelPaginacionDepartamentosEmpleados>
           GetEmpleadoDepartamentoAsync
           (int posicion, int iddepartamento)
        {
            string sql = "SP_GRUPO_EMPLEADOS_DEPARTAMENTO_OUT @posicion, @departamento, "
                + " @registros out";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamDepartamento =
                new SqlParameter("@departamento", iddepartamento);
            SqlParameter pamRegistros = new SqlParameter("@registros", -1);
            pamRegistros.Direction = ParameterDirection.Output;
            var consulta =
                this.context.Empleados.FromSqlRaw
                (sql, pamPosicion, pamDepartamento, pamRegistros);
            //PRIMERO DEBEMOS EJECUTAR LA CONSULTA PARA PODER RECUPERAR 
            //LOS PARAMETROS DE SALIDA
            var datos = await consulta.ToListAsync();
            Empleado empleado = datos.FirstOrDefault();
            int registros = (int)pamRegistros.Value;
            return new ModelPaginacionDepartamentosEmpleados
            {
                NumeroRegistrosEmpleados = registros,
                Empleado = empleado
            };
        }

        //public async Task<List<Departamento>> GetDepartamentosAsync()
        //{
        //    return await this.context.Departamentos.ToListAsync();
        //}

        public async Task<Departamento> FindDepartamentosAsync(int id)
        {
            return await this.context.Departamentos
                .FirstOrDefaultAsync(x => x.IdDepartamento == id);
        }



        //public async Task<int> GetNumeroEmpleadosDepartamentoAsync(int iddepartamento)
        //{
        //    var registros = this.context.Empleados.Where(x => x.IdDepartamento == iddepartamento).Count();

        //    return registros;
        //}
        public async Task<Departamento> GetDetalleDepartamentoAsync(int iddepartamento)
        {
            return await this.context.Departamentos.Where(x => x.IdDepartamento == iddepartamento).FirstOrDefaultAsync();
        }

        public async Task<ModelPaginacionEmpleados> GetGrupoEmpleadoOficioOutAsync(int posicion, string oficio)
        {
            string sql = "SP_GRUPO_EMPLEADOS_OFICIO_OUT @posicion, @oficio, @registros out";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamOficio = new SqlParameter("@oficio", oficio);
            SqlParameter pamRegistros = new SqlParameter("@registros", -1);
            pamRegistros.Direction = System.Data.ParameterDirection.Output;
            var consulta =
                this.context.Empleados.FromSqlRaw(sql, pamPosicion, pamOficio, pamRegistros);
            List<Empleado> empleados = await consulta.ToListAsync();
            int registros = (int)pamRegistros.Value;

            return new ModelPaginacionEmpleados { 
                NumeroRegistros = registros,
                Empleados = empleados
            };
        }

        public async Task<List<Empleado>> GetEmpleadosOficioAsync(int posicion, string oficio)
        {
            string sql = "SP_GRUPO_EMPLEADOS_OFICIO @posicion, @oficio";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamOficio = new SqlParameter("@oficio", oficio);
            var consulta =
                this.context.Empleados.FromSqlRaw(sql, pamPosicion, pamOficio);
            return await consulta.ToListAsync();
        }

        public async Task<int> GetNumeroEmpleadosOficioAsync(string oficio) {

            return await this.context.Empleados.Where(z => z.Oficio == oficio).CountAsync();
        }

        public async Task<List<Empleado>> GetGrupoEmpleadosAsync(int posicion)
        {
            string sql = "SP_GRUPO_EMPLEADOS @posicion";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            var consulta =
                this.context.Empleados.FromSqlRaw(sql, pamPosicion);
            return await consulta.ToListAsync();
        }

        public async Task<int> GetNumeroRegistrosVistaEmpleadosAsync()
        {
            return await this.context.Empleados.CountAsync();
        }
        public async Task<List<Departamento>> GetGrupoDepartamentosAsync(int posicion)
        {
            string sql = "SP_GRUPO_DEPARTAMENTOS @posicion";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            var consulta =
                this.context.Departamentos.FromSqlRaw(sql, pamPosicion);
            return await consulta.ToListAsync();
        }

        public async Task<int> GetNumeroRegistrosVistaDepartamentos()
        {
            return await this.context.VistaDepartamentos.CountAsync();
        }

        public async Task<VistaDepartamento> GetVistaDepartamentoAsync(int posicion)
        {
            VistaDepartamento vista = await this.context.VistaDepartamentos.Where(z => z.Posicion == posicion).FirstOrDefaultAsync();

            return vista;
        }

        public async Task<List<VistaDepartamento>> GetGrupoVistaDepartamentosAsync(int posicion)
        { 
            var consulta = from datos in this.context.VistaDepartamentos where datos.Posicion >= posicion && datos.Posicion < (posicion + 2) select datos;

            return await consulta.ToListAsync();
        }


        public async Task<List<Departamento>> GetDepartamentosAsync()
        {
            return await this.context.Departamentos.ToListAsync();
        }

        public async Task<List<Empleado>> GetEmpleadosDepartamentoAsync(int idDepartamento)
        {
            var empleados = this.context.Empleados.Where(x => x.IdDepartamento == idDepartamento);

            if (empleados.Count() == 0)
            {
                return null;
            }
            else 
            {
                return await empleados.ToListAsync();
            }
        }
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using NetCoreLinqToSqlInjection.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

#region PROCEDIMIENTOS ALMACENADOS

//--LENGUAJE PL / SQL
//create or replace procedure sp_delete_doctor
//(p_iddoctor DOCTOR.DOCTOR_NO%TYPE)
//as
//begin
//  delete from DOCTOR where DOCTOR_NO=p_iddoctor;
//commit;
//end;

#endregion

namespace NetCoreLinqToSqlInjection.Repositories
{
    public class RepositoryDoctoresOracle : IRepositoryDoctores
    {
        private DataTable tablaDoctores;
        private OracleConnection cn;
        private OracleCommand com;

        public RepositoryDoctoresOracle()
        {
            string connectionString = 
                @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True; User Id=SYSTEM; Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from DOCTOR";
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
            this.tablaDoctores = new DataTable();
            ad.Fill(this.tablaDoctores);
        }

        public void DeleteDoctor(int idDoctor)
        {
            OracleParameter pamIdDoctor = 
                new OracleParameter(":p_iddoctor", idDoctor);
            this.com.Parameters.Add(pamIdDoctor);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "sp_delete_doctor";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public List<Doctor> GetDoctores()
        {
            //LA VENTAJA DE LINQ ES QUE SE ABSTRAE DEL ORIGEN DE DATOS
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doc = new Doctor
                {
                    IdDoctor = row.Field<int>("DOCTOR_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Especialidad = row.Field<string>("ESPECIALIDAD"),
                    Salario = row.Field<int>("SALARIO"),
                    IdHospital = row.Field<int>("HOSPITAL_COD")
                };
                doctores.Add(doc);
            }
            return doctores;
        }

        public List<Doctor> GetDoctoresEspecialidad(string especialidad)
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           where datos.Field<string>("ESPECIALIDAD").ToUpper()
                           == especialidad.ToUpper()
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Doctor> doctores = new List<Doctor>();
                foreach (var row in consulta)
                {
                    Doctor doc = new Doctor
                    {
                        IdDoctor = row.Field<int>("DOCTOR_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Especialidad = row.Field<string>("ESPECIALIDAD"),
                        Salario = row.Field<int>("SALARIO"),
                        IdHospital = row.Field<int>("HOSPITAL_COD")
                    };
                    doctores.Add(doc);
                }
                return doctores;
            }
        }

        public void InsertDoctor(int id, string apellido
            , string especialidad, int salario, int idHospital)
        {
            string sql = "insert into DOCTOR values (:idhospital, :iddoctor "
                + ", :apellido, :especialidad, :salario)";
            //ORACLE TIENE EN CUENTA EL ORDEN DE LOS PARAMETROS, NO EL NOMBRE
            OracleParameter pamIdHospital = new OracleParameter(":idhospital", idHospital);
            this.com.Parameters.Add(pamIdHospital);
            OracleParameter pamIdDoctor = new OracleParameter(":iddoctor", id);
            this.com.Parameters.Add(pamIdDoctor);
            OracleParameter pamApellido = new OracleParameter(":apellido", apellido);
            this.com.Parameters.Add(pamApellido);
            OracleParameter pamEspe = new OracleParameter(":especialidad", especialidad);
            this.com.Parameters.Add(pamEspe);
            OracleParameter pamSalario = new OracleParameter(":salario", salario);
            this.com.Parameters.Add(pamSalario);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}

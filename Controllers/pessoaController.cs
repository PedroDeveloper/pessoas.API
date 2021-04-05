using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using pessoas.API.Models;

namespace pessoas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pessoaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        
        public pessoaController (IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]

        public  JsonResult Get()
        {
            DataTable table = new DataTable();
            try
            {
                string query = @"
                   select id, name, tarefa, feito, dia from dbo.task";

                string sqlDataSoucer = _configuration.GetConnectionString("dataConnection");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSoucer))
                {
                    myCon.Open();

                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("no data, try POST");
            }

        }

         [HttpGet("{id}")]
      

        public JsonResult Get(int id)
        {
            DataTable table = new DataTable();

            try
            {
                string query = @"
                   select *from dbo.task where id =" + id + @"";
                
                string sqlDataSoucer = _configuration.GetConnectionString("dataConnection");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSoucer))
                {
                    myCon.Open();

                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            if (table.Rows.Count> 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Id not exist");
            }
            

        }


        [HttpGet("buscar/{name}")]
        
        public JsonResult Get(string name = null)
        {
            DataTable table = new DataTable();
          
            try
            {
                string query = @"select *from dbo.task where name LIKE '%"+name+@"%'";


                string sqlDataSoucer = _configuration.GetConnectionString("dataConnection");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSoucer))
                {
                    myCon.Open();

                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                if (name == null)
                {
                    return new JsonResult("dado invalido,verifique nome");
                }
                else
                {
                    throw ex;
                }
                
            }

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("name not exist");
            }


        }



        [HttpDelete("{id}")]

        public JsonResult Delete(int id)
        {
            DataTable table = new DataTable();

            try
            {   
                //verificar se id existe no banco
                string query1 = @"select *from dbo.task where id =" + id + @"";

                string query = @"delete from dbo.task where id ="+id+@"";


                
                string sqlDataSoucer = _configuration.GetConnectionString("dataConnection");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSoucer))
                {
                    myCon.Open();

                    using (SqlCommand myCommand = new SqlCommand(query1, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        // caso a consulta retorne alguma linha pra o id , executa delet no id enviado
                        if (table.Rows.Count>0)
                        {
                            using (SqlCommand myCommand2 = new SqlCommand(query, myCon))
                            {
                                myReader = myCommand2.ExecuteReader();
                                table.Load(myReader);
                                myReader.Close();
                                myCon.Close();
                            }

                            return new JsonResult("Delete sucessful");

                        }
                        else
                        {
                            return new JsonResult("ID not exist");
                        }

                        
                       
                    }
                }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            



        }

        [HttpPut]

        public JsonResult Update(PesssoasModels p)
        {

            try
            {
                string query = @"UPDATE dbo.task SET

                                             name = '" + p.name+@"'
                                            , tarefa ='"+p.tarefa+@"'
                                            , feito ='"+p.feito+ @"'
                                            , dia ='" +p.dia+ @"
                                                                            

                                            WHERE ID =" + p.id+@"";

                DataTable table = new DataTable();
                string sqlDataSoucer = _configuration.GetConnectionString("dataConnection");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSoucer))
                {
                    myCon.Open();

                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("PUT sucessful");
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        [HttpPost]

        public JsonResult Insert(PesssoasModels p)
        {

            try
            {
                string query = @"INSERT INTO dbo.task
                                          ( 
                                              name
                                             ,tarefa
                                             ,feito
                                             ,dia
                                             
                                          )
                                         VALUES
                                          (
                                            '"+ p.name + @"'
                                            , '" + p.tarefa + @"'
                                            , " + p.feito + @"
                                            , '" + p.dia + @"'
                                                                        
                                           
                                            )";


                DataTable table = new DataTable();
                string sqlDataSoucer = _configuration.GetConnectionString("dataConnection");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSoucer))
                {
                    myCon.Open();

                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("POST sucessful");
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }










    }
}

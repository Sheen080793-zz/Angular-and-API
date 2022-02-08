using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json;
using API.Models;
using Newtonsoft.Json;
using System.Web.Http.Cors;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
     

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

       
        [HttpGet]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public JsonResult Get()
        {
            string query = @"
                    select EmployeeId, EmployeeName, Department, Position
                    from dbo.Employee
                    ";
            DataTable table = EmployeeDataTable(query);
            return new JsonResult(DataTableToJSON(table));
        }
    
        [HttpPost]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public JsonResult Post(Employee emp)
        {
            string query = @"
                    insert into dbo.Employee 
                    (EmployeeName,Department,Position)
                    values 
                    (
                    '" + emp.EmployeeName + @"'
                    ,'" + emp.Department + @"'
                    ,'" + emp.Position + @"'
                    )
                    ";
            EmployeeDataTable(query);

            return new JsonResult("Added Successfully");
        }


        [HttpPut]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public JsonResult Put(Employee emp)
        {
            string query = @"
                    update dbo.Employee set 
                    EmployeeName = '" + emp.EmployeeName + @"'
                    ,Department = '" + emp.Department + @"'
                    ,Position = '" + emp.Position + @"'
                    where EmployeeId = " + emp.EmployeeID + @" 
                    ";
            EmployeeDataTable(query);

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public JsonResult Delete(int id)
        {
            string query = @"
                    delete from dbo.Employee
                    where EmployeeId = " + id + @" 
                    ";
            EmployeeDataTable(query);
            return new JsonResult("Deleted Successfully");
        }

        private DataTable EmployeeDataTable(string query)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
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
            return table;
        }
        private static object DataTableToJSON(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = (Convert.ToString(row[col]));
                }
                list.Add(dict);
            }

            return list;
        }


    }
}

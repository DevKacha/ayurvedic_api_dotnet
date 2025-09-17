using Ayurvedic_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace Ayurvedic_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class remedies_Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public remedies_Controller(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAllRemedies()
        {
            List<Remedy> remedies = new List<Remedy>();
            SqlConnection connection = new SqlConnection
                (this._configuration.GetConnectionString("Ayurvedic_remedy"));
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "PR_Remedies_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            if (reader != null)
            {
                
                while (reader.Read()) 
                {
                    var remedy = new Remedy();
                    remedy.RemedyId = reader["RemedyId"] as string;
                    remedy.Title = reader["Title"] as string;
                    remedy.Description = reader["Description"] as string;
                    remedy.Ingredients = reader["Ingredients"] as string;
                    remedy.PreparationSteps = reader["PreparationSteps"] != DBNull.Value
                    ? (reader["PreparationSteps"] as string)?.Split(',').ToList()
                    : new List<string>();
                    remedy.Usage = reader["Usage"] as string;
                    remedy.Ailment = reader["Ailment"] as string; 
                    remedy.Precautions = reader["Precautions"] as string;
                    remedy.Category = reader["Category"] as string;
                    remedy.Dosage = reader["Dosage"] as string;
                    remedy.AgeGroup = reader["AgeGroup"] as string;
                    remedy.ImageUrl = reader["ImageUrl"] as string;
                    remedy.SourceReference = reader["SourceReference"] as string;
                    remedy.IsVerified = reader["IsVerified"] != DBNull.Value && (bool)reader["IsVerified"];
                    remedy.Language = reader["Language"] as string;
                    remedy.Tags = reader["Tags"] != DBNull.Value
                    ? (reader["Tags"] as string)?.Split(',').ToList()
                    : new List<string>();
                    remedy.Tip = reader["Tip"] as string;
                    remedy.Type = reader["Type"] as string;
                    remedies.Add(remedy);
                }
            }

            connection.Close();

            return Ok(remedies);
        }

        [HttpPost]
        public IActionResult AddRemedy([FromBody] Remedy remedy)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Ayurvedic_remedy")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_Remedies_Insert", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@remedyId", remedy.RemedyId);
                command.Parameters.AddWithValue("@title", remedy.Title);
                command.Parameters.AddWithValue("@description", remedy.Description);
                command.Parameters.AddWithValue("@ingredients", remedy.Ingredients);
                command.Parameters.AddWithValue("@preparationSteps", remedy.PreparationSteps != null ? string.Join(",", remedy.PreparationSteps) : (object)DBNull.Value);
                command.Parameters.AddWithValue("@usage", remedy.Usage);
                command.Parameters.AddWithValue("@ailment", remedy.Ailment);
                command.Parameters.AddWithValue("@precautions", (object?)remedy.Precautions ?? DBNull.Value);
                command.Parameters.AddWithValue("@category", (object?)remedy.Category ?? DBNull.Value);
                command.Parameters.AddWithValue("@dosage", (object?)remedy.Dosage ?? DBNull.Value);
                command.Parameters.AddWithValue("@ageGroup", (object?)remedy.AgeGroup ?? DBNull.Value);
                command.Parameters.AddWithValue("@imageUrl", (object?)remedy.ImageUrl ?? DBNull.Value);
                command.Parameters.AddWithValue("@sourceReference", (object?)remedy.SourceReference ?? DBNull.Value);
                command.Parameters.AddWithValue("@isVerified", remedy.IsVerified);
                command.Parameters.AddWithValue("@language", remedy.Language);
                command.Parameters.AddWithValue("@tags", remedy.Tags != null ? string.Join(",", remedy.Tags) : (object)DBNull.Value);
                command.Parameters.AddWithValue("@tip", (object?)remedy.Tip ?? DBNull.Value);
                command.Parameters.AddWithValue("@type", (object?)remedy.Type ?? DBNull.Value);

                command.ExecuteNonQuery();
            }

            return Ok("Remedy inserted successfully.");
        }

        [HttpPost("InsertAll")]
        public IActionResult InsertAllRemedies([FromBody] List<Remedy> remediesList)
        {
            try
            {
                // Convert remedies list to JSON
                string remediesJson = JsonConvert.SerializeObject(remediesList);

                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Ayurvedic_remedy")))
                {
                    using (SqlCommand cmd = new SqlCommand("PR_Remedies_InsertAll", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Remedies", SqlDbType.NVarChar)
                        {
                            Value = remediesJson
                        });

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok("All remedies inserted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error inserting remedies: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRemedy(string id, [FromBody] Remedy remedy)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Ayurvedic_remedy")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_Remedies_update", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@remedyId", id);
                command.Parameters.AddWithValue("@title", remedy.Title);
                command.Parameters.AddWithValue("@description", remedy.Description);
                command.Parameters.AddWithValue("@ingredients", remedy.Ingredients);
                command.Parameters.AddWithValue("@preparationSteps", remedy.PreparationSteps != null ? string.Join(",", remedy.PreparationSteps) : (object)DBNull.Value);
                command.Parameters.AddWithValue("@usage", remedy.Usage);
                command.Parameters.AddWithValue("@ailment", remedy.Ailment);
                command.Parameters.AddWithValue("@precautions", (object?)remedy.Precautions ?? DBNull.Value);
                command.Parameters.AddWithValue("@category", (object?)remedy.Category ?? DBNull.Value);
                command.Parameters.AddWithValue("@dosage", (object?)remedy.Dosage ?? DBNull.Value);
                command.Parameters.AddWithValue("@ageGroup", (object?)remedy.AgeGroup ?? DBNull.Value);
                command.Parameters.AddWithValue("@imageUrl", (object?)remedy.ImageUrl ?? DBNull.Value);
                command.Parameters.AddWithValue("@sourceReference", (object?)remedy.SourceReference ?? DBNull.Value);
                command.Parameters.AddWithValue("@isVerified", remedy.IsVerified);
                command.Parameters.AddWithValue("@language", remedy.Language);
                command.Parameters.AddWithValue("@tags", remedy.Tags != null ? string.Join(",", remedy.Tags) : (object)DBNull.Value);
                command.Parameters.AddWithValue("@tip", (object?)remedy.Tip ?? DBNull.Value);
                command.Parameters.AddWithValue("@type", (object?)remedy.Type ?? DBNull.Value);
                command.ExecuteNonQuery();
            }

            return Ok("Remedy updated successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRemedy(string id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Ayurvedic_remedy")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_Remedies_delete", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@remedyId", id);

                command.ExecuteNonQuery();
            }

            return Ok("Remedy deleted successfully.");
        }
    }
}

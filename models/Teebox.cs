using Newtonsoft.Json;
using Postgrest.Attributes;
using Postgrest.Models;

namespace api_publicGolf.models
{
    [JsonObject]
    [Table("teebox")]
    public class Teebox : BaseModel 
    {
        [PrimaryKey("id")]
        public int id { get; set; }

        [Column("course_id")]
        public int courseId { get; set; }

        [Column("name")]
        public string? name { get; set; }

        [Column("par")]
        public int par { get; set; }

        [Column("slope")]
        public int slope { get; set; }

        [Column("rating")]
        public float rating { get; set; }

        [Column("yardage")]
        public int yardage { get; set; }

        [Column("yardage_in")]
        public int yardage_in { get; set; }

        [Column("yardage_out")]
        public int yardage_out { get; set; }

        [Column("par_in")]
        public int parIn { get; set; }

        [Column("par_out")]
        public int parOut { get; set; }

        public Teebox() { }

        public Teebox(int id, int courseId, string? name, int par, int slope, float rating, int yardage, int yardage_in, int yardage_out, int parIn, int parOut)
        {
            this.id = id;
            this.courseId = courseId;
            this.name = name;
            this.par = par;
            this.slope = slope;
            this.rating = rating;
            this.yardage = yardage;
            this.yardage_in = yardage_in;
            this.yardage_out = yardage_out;
            this.parIn = parIn;
            this.parOut = parOut;
        }
    }
}
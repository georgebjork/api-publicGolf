using Postgrest.Attributes;
using Npgsql;

namespace api_publicGolf.models
{
    [Table("teebox")]
    public class Teebox
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
        public int yardageIn { get; set; }

        [Column("yardage_out")]
        public int yardageOut { get; set; }

        [Column("par_in")]
        public int parIn { get; set; }

        [Column("par_out")]
        public int parOut { get; set; }

        public List<Hole>? holes { get; set; }

        public Teebox() { }

        public Teebox(int id, int courseId, string? name, int par, int slope, float rating, int yardage, int yardageIn, int yardageOut, int parIn, int parOut)
        {
            this.id = id;
            this.courseId = courseId;
            this.name = name;
            this.par = par;
            this.slope = slope;
            this.rating = rating;
            this.yardage = yardage;
            this.yardageIn = yardageIn;
            this.yardageOut = yardageOut;
            this.parIn = parIn;
            this.parOut = parOut;
        }

        public Teebox(NpgsqlDataReader reader) {
            this.id = reader.GetInt16(0);
            this.courseId = reader.GetInt16(1);
            this.name = reader.GetString(2);
            this.par = reader.GetInt16(3);
            this.slope = reader.GetInt16(4);
            this.rating = reader.GetFloat(5);
            this.yardage = reader.GetInt16(6);
            this.yardageIn = reader.GetInt16(7);
            this.yardageOut = reader.GetInt16(8);
            this.parIn = reader.GetInt16(9);
            this.parOut = reader.GetInt16(10);
        }
    }
}
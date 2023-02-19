using Npgsql;
using Postgrest.Attributes;

namespace api_publicGolf.models
{     
    [Table("hole")]
    public class Hole
    {   
        [PrimaryKey("id")]
        public int id { get; set; }

        [Column("teebox_id")]
        public int teeboxId { get; set; }
        
        [Column("hole_number")]
        public int holeNumber { get; set; }

        [Column("par")]
        public int par { get; set; }

        [Column("yardage")]
        public int yardage { get; set; }

        [Column("handicap")]
        public int handicap { get; set; }
    
        public Hole() { }

        public Hole(int id, int teeboxId, int holeNumber, int par, int yardage, int handicap)
        {
            this.id = id;
            this.teeboxId = teeboxId;
            this.holeNumber = holeNumber;
            this.par = par;
            this.yardage = yardage;
            this.handicap = handicap;
        }

        public Hole(NpgsqlDataReader reader)
        {
            this.id = reader.GetInt16(0);
            this.teeboxId = reader.GetInt16(1);
            this.holeNumber = reader.GetInt16(2);
            this.par = reader.GetInt16(3);
            this.yardage = reader.GetInt16(4);
            this.handicap = reader.GetInt16(5);
        }

        public override string ToString()
        {
            return $"id: {id}, teeboxId: {teeboxId}, holeNumber: {holeNumber}, par: {par}, yardage: {yardage}, handicap: {handicap}";
        }
    }
}
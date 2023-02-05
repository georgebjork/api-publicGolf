using Newtonsoft.Json;
using Postgrest.Attributes;
using Postgrest.Models;

namespace api_publicGolf.models
{      
    [JsonObject]
    [Table("hole")]
    public class Hole : BaseModel
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
    }
}
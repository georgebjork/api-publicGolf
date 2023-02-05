﻿using Newtonsoft.Json;
using Postgrest.Attributes;
using Postgrest.Models;

namespace api_publicGolf.models
{      
    [JsonObject]
    [Table("course")]
    public class Course : BaseModel
    {   
        [PrimaryKey("id")]
        public int id { get; set; }

        [Column("name")]
        public string? name { get; set; }

        public List<Teebox>? teeboxes { get; set; }
    
        public Course() { }

        public Course(int id, string name) {
            this.id = id;
            this.name = name;
        }
    }
}
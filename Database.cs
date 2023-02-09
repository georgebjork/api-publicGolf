using Npgsql;
using dotenv.net;
using api_publicGolf.models;
using System.Data;

namespace api_publicGolf;

public class Database {

    // This holds our connection string 
    private readonly string connString;
    
    // This is our connection object
    private NpgsqlConnection connection;

    public Database() 
    {
        // Create a connection String
        connString = createConnectionString();

        // Create a connection to the database
        connection = initConnection();
    }
    


    /// <summary>
    /// This function will read our .env and get the username and password for the database and convert it to a connection string
    /// </summary>
    ///     
    /// <returns>String representing the connection string</returns>
    private String createConnectionString() 
    {
        var envVars = DotEnv.Read();
        var password = envVars["DB_PASSWORD"];
        var userId = envVars["DB_USERNAME"];

        return $"User Id={userId};Password={password};Server=db.xqnkfufucqnnmsljuzrz.supabase.co;Port=5432;Database=postgres";
    }



    /// <summary>
    /// This function will use our created connection string and init a connection to the database.
    /// </summary>
    ///     
    /// <returns>NpgsqlConnection object representing our database</returns>
    private NpgsqlConnection initConnection() => new NpgsqlConnection(connString);
    


    /// <summary>
    /// This function will close our connection to the database
    /// </summary>
    ///     
    /// <returns>void</returns>
    public void closeConnection() => connection.Close();



    /// <summary>
    /// This function will run a query on our database
    /// </summary>
    /// <param name="query">Query to run</param>   
    /// <returns>NpgsqlDataReader representing our database</returns>
    public NpgsqlDataReader query(String query) 
    {
        // If our connection is not open, lets open it.
        if(connection.State != ConnectionState.Open) { connection.Open(); }

        var command = new NpgsqlCommand(query, connection);
        return command.ExecuteReader();       
    }



    /// <summary>
    /// This function will run a query and return a list of all the golf courses
    /// </summary>
    ///     
    /// <returns>Returns a list of golf courses</returns>
    public List<Course> GetAllCourses() 
    {
        List<Course> courses = new List<Course>();

        using (var reader = query("SELECT * FROM course")) 
        {
            while (reader.Read())
            {
                Course c = new Course(reader.GetInt16(0), reader.GetString(1));
                courses.Add(c);
            }
        }
        courses = AssociateTeeboxesWithCourses(courses, GetAllTeeboxes());
        return courses;
    }



    /// <summary>
    /// This function will run a query and return a golf course object
    /// </summary>
    /// <param name="course_id">The id of the golf course</param>  
    /// <returns>A course object</returns>
    public List<Course> GetCourse(int course_id) 
    {
        List<Course> courses = new List<Course>();

        using (var reader = query($"SELECT * FROM course WHERE id = {course_id}")) 
        {
            while (reader.Read())
            {
                Course c = new Course(reader.GetInt16(0), reader.GetString(1));
                courses.Add(c);
            }
        }
        courses = AssociateTeeboxesWithCourses(courses, GetTeeboxes(course_id));
        return courses;
    }



    /// <summary>
    /// This function will run a query and return a list of Teeboxes
    /// </summary>
    ///     
    /// <returns>A teebox object</returns>
    public List<Teebox> GetAllTeeboxes()
    {
        List<Teebox> teeboxes = new List<Teebox>();

        using (var reader = query($"SELECT * FROM teebox")) 
        {
            while (reader.Read())
            {
                teeboxes.Add(new Teebox(reader));
            }
        }
        teeboxes = AssociateHolesWithTeeboxes(teeboxes, GetAllHoles());
        return teeboxes;
    }



    /// <summary>
    /// This function will run a query and return a teebox object
    /// </summary>
    /// <param name="course_id">The id of the golf course</param>    
    /// <returns>A teebox object</returns>
    public List<Teebox> GetTeeboxes(int course_id) 
    {
        List<Teebox> teeboxes = new List<Teebox>();

        using (var reader = query($"SELECT * FROM teebox WHERE course_id = {course_id}")) 
        {
            while (reader.Read())
            {
                teeboxes.Add(new Teebox(reader));
            }
        }
        teeboxes = AssociateHolesWithTeeboxes(teeboxes, GetHoles(course_id));
        return teeboxes;
    }



    /// <summary>
    /// This function will run a query and return a List of all Golf Holes in the database 
    /// </summary>
    ///
    /// <returns>List of Golf Holes</returns>
    public List<Hole> GetAllHoles() 
    {
        List<Hole> holes = new List<Hole>();

        using (var reader = query($"SELECT * FROM hole")) 
        {
            while (reader.Read())
            {
                holes.Add(new Hole(reader));
            }
        }
        return holes;
    }


    /// <summary>
    /// This function will run a query and return a List of Golf Holes 
    /// </summary>
    /// <param name="course_id">The id of the golf course</param>    
    /// <returns>List of Golf Holes</returns>
    public List<Hole> GetHoles(int course_id) 
    {
        List<Hole> holes = new List<Hole>();

        using (var reader = query($"SELECT hole.* FROM course JOIN teebox ON course.id = teebox.course_id JOIN hole ON teebox.id = hole.teebox_id WHERE course.id = {course_id}")) 
        {
            while (reader.Read())
            {
                holes.Add(new Hole(reader));
            }
        }
        return holes;
    }



    /// <summary>
    /// This function will take in a list of courses and teeboxes, and add a list of teeboxes to the correct course
    /// </summary>
    /// <param name="courses">A list of golf courses</param>
    /// <param name="teeboxes">A list of teeboxes courses</param>
    /// <returns>Returns a list of courses with teeboxes</returns>
    private static List<Course> AssociateTeeboxesWithCourses(List<Course> courses, List<Teebox> teeboxes) {
        
        // This dictionary will hold an id represening the course id and a list of teeboxes
        Dictionary<int, List<Teebox>> courseMap = new Dictionary<int, List<Teebox>>();

        // Let us go through all of the teeboxes. If there is an id of a course in the dictionary, add it to the list. Otherwise add a new one
        foreach (Teebox teeBox in teeboxes)
        {
            if (!courseMap.ContainsKey(teeBox.courseId))
            {
                courseMap[teeBox.courseId] = new List<Teebox>();
            }   
            courseMap[teeBox.courseId].Add(teeBox);
        }

        foreach (Course course in courses)
        {
            course.teeboxes = courseMap[course.id];
        }

        return courses;
    }


    /// <summary>
    /// This function will take in a list of teeboxes and holes, and adds a list of holes to the correct teebox
    /// </summary>
    /// <param name="teeboxes">A list of golf courses</param>
    /// <param name="holes">A list of teeboxes courses</param>
    /// <returns>Returns a list of teeboxes with holes</returns>
    private static List<Teebox> AssociateHolesWithTeeboxes(List<Teebox> teeboxes, List<Hole> holes) {
        
       // This dictionary will hold an id represening the teebox id and a list of holes
        Dictionary<int, List<Hole>> teeboxMap = new Dictionary<int, List<Hole>>();

        // For every hole in the holes list, let us go through all of them and 
        foreach (Hole hole in holes)
        {
            if (!teeboxMap.ContainsKey(hole.teeboxId))
            {
                teeboxMap[hole.teeboxId] = new List<Hole>();
            }   
            teeboxMap[hole.teeboxId].Add(hole);
        }

        foreach (Teebox teebox in teeboxes)
        {
            teebox.holes = teeboxMap[teebox.id];
        }

        return teeboxes;
    }
}

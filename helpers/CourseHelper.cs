using api_publicGolf.models;

namespace api_publicGolf.helpers;

public static class CourseHelper {
    
    /// <summary>
    /// This function will run a query and return a list of all the golf courses
    /// </summary>
    ///     
    /// <returns>Returns a list of golf courses</returns>
    public static List<Course> GetAllCourses() 
    {   
        Database db = new Database();

        List<Course> courses = new List<Course>();

        using (var reader = db.query("SELECT * FROM course ORDER BY id")) 
        {
            while (reader.Read())
            {
                Course c = new Course(reader.GetInt16(0), reader.GetString(1));
                courses.Add(c);
            }
        }
        db.closeConnection();
        
        courses = AssociateTeeboxesWithCourses(courses, GetAllTeeboxes());
        return courses;
    }


    /// <summary>
    /// This function will run a query and return a golf course object
    /// </summary>
    /// <param name="course_id">The id of the golf course</param>  
    /// <returns>A course object</returns>
    public static List<Course> GetCourse(int course_id) 
    {
        Database db = new Database();
        List<Course> courses = new List<Course>();

        using (var reader = db.query($"SELECT * FROM course WHERE id = {course_id} ORDER BY id")) 
        {
            while (reader.Read())
            {
                Course c = new Course(reader.GetInt16(0), reader.GetString(1));
                courses.Add(c);
            }
        }
        db.closeConnection();
        
        courses = AssociateTeeboxesWithCourses(courses, GetTeeboxes(course_id));
        return courses;
    }



    /// <summary>
    /// This function will run a query and return a list of Teeboxes
    /// </summary>
    ///     
    /// <returns>A teebox object</returns>
    public static List<Teebox> GetAllTeeboxes()
    {
        Database db = new Database();
        List<Teebox> teeboxes = new List<Teebox>();

        using (var reader = db.query($"SELECT * FROM teebox ORDER BY id")) 
        {
            while (reader.Read())
            {
                teeboxes.Add(new Teebox(reader));
            }
        }
        db.closeConnection();

        teeboxes = AssociateHolesWithTeeboxes(teeboxes, GetAllHoles());
        return teeboxes;
    }



    /// <summary>
    /// This function will run a query and return a teebox object
    /// </summary>
    /// <param name="course_id">The id of the golf course</param>    
    /// <returns>A teebox object</returns>
    public static List<Teebox> GetTeeboxes(int course_id) 
    {   
        Database db = new Database();
        List<Teebox> teeboxes = new List<Teebox>();

        using (var reader = db.query($"SELECT * FROM teebox WHERE course_id = {course_id} ORDER BY id")) 
        {
            while (reader.Read())
            {
                teeboxes.Add(new Teebox(reader));
            }
        }
        db.closeConnection();

        teeboxes = AssociateHolesWithTeeboxes(teeboxes, GetHoles(course_id));
        return teeboxes;
    }



    /// <summary>
    /// This function will run a query and return a List of all Golf Holes in the database 
    /// </summary>
    ///
    /// <returns>List of Golf Holes</returns>
    public static List<Hole> GetAllHoles() 
    {
        Database db = new Database();
        List<Hole> holes = new List<Hole>();

        using (var reader = db.query($"SELECT * FROM hole ORDER BY id")) 
        {
            while (reader.Read())
            {
                holes.Add(new Hole(reader));
            }
        }
        db.closeConnection();
        return holes;
    }


    /// <summary>
    /// This function will run a query and return a List of Golf Holes 
    /// </summary>
    /// <param name="course_id">The id of the golf course</param>    
    /// <returns>List of Golf Holes</returns>
    public static List<Hole> GetHoles(int course_id) 
    {
        Database db = new Database();
        List<Hole> holes = new List<Hole>();

        using (var reader = db.query($"SELECT hole.* FROM course JOIN teebox ON course.id = teebox.course_id JOIN hole ON teebox.id = hole.teebox_id WHERE course.id = {course_id} ORDER BY id")) 
        {
            while (reader.Read())
            {
                holes.Add(new Hole(reader));
            }
        }
        db.closeConnection();
        return holes;
    }


    /// <summary>
    /// This function will take in a a hole and update it in the database
    /// </summary>
    /// <param name="teebox">A teebox object</param>
    /// <returns>Returns a list of teeboxes with holes</returns>
    public static int UpdateTeebox(Teebox teebox) 
    {   
        Database db = new Database();
        
        var sql = @$"UPDATE teebox 
        SET yardage = {teebox.yardage}, yardage_out = {teebox.yardageOut}, yardage_in = {teebox.yardageIn}
        WHERE id = {teebox.id}";
        
        int rowsAffected = db.UpdateQuery(sql);
        
        db.closeConnection();

        return rowsAffected;
    }



    /// <summary>
    /// This function will take in a a hole and update it in the database
    /// </summary>
    /// <param name="hole">A golf Hole object</param>
    /// <returns>Returns a list of teeboxes with holes</returns>
    public static int UpdateHole(Hole hole) 
    {   
        Database db = new Database();
        
        var sql = @$"UPDATE hole 
        SET hole_number = {hole.holeNumber}, par = {hole.par}, yardage = {hole.yardage}, handicap = {hole.handicap}
        WHERE id = {hole.id} AND teebox_id = {hole.teeboxId}";
        
        int rowsAffected = db.UpdateQuery(sql);
        
        db.closeConnection();

        return rowsAffected;
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
};



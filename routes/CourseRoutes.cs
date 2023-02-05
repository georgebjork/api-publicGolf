
using api_publicGolf.models;
using Newtonsoft.Json;
using static Postgrest.Constants;

namespace api_publicGolf.routes;

public static class CourseRoutesConfig {

    private static Supabase.Client? supabase;

    const string CorsPolicyName = "_myCorsPolicy";

    // All Course Routes
    public static void CourseRoutes(this IEndpointRouteBuilder app, Supabase.Client s)
    {
        supabase = s;

        const string routePrefix = "/api";

        app.MapGet($"{routePrefix}/course", async () =>
        {
            var result = await GetAllCourses();
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }).RequireCors(CorsPolicyName);

        app.MapGet($"{routePrefix}/course/{{course_id}}", async (int course_id) => 
        {   
            var result = await GetCourse(course_id);
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }).RequireCors(CorsPolicyName);

        app.MapGet($"{routePrefix}/course/{{course_id}}/teebox", async (int course_id) =>
        {
            var result = await GetTeebox(course_id);
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }).RequireCors(CorsPolicyName);

        
    }

    /// <summary>
    /// This function will take in a list of courses and teeboxes, and add a list of teeboxes to the correct course
    /// </summary>
    /// <param name="courses">A list of golf courses</param>
    /// <param name="teeboxes">A list of teeboxes courses</param>
    /// <returns>Returns a list of courses with teeboxes</returns>
    private static List<Course> AssociateTeeboxesWithCourses(List<Course> courses, List<Teebox> teeboxes) {
        
        // This dictionary will hold an id (int) for a course, a list of teeboxes.
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


    private static List<Teebox> AssociateHolesWithTeeboxes(List<Teebox> teeboxes, List<Hole> holes) {
        
        // This dictionary will hold an id (int) for a teebox, a list of holes associacted with that teebox.
        Dictionary<int, List<Hole>> teeboxMap = new Dictionary<int, List<Hole>>();

        // 
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


    /// <summary>
    /// This function will run a supabase query to return all of the courses in the database
    /// </summary>
    ///     
    /// <returns>Returns a list of golf courses</returns>
    private static async Task<List<Course>> GetAllCourses() 
    {   
        // Get all the golf courses
        var result = await supabase!.From<Course>().Get();

        // Get all of the teeboxes
        List<Teebox> teeboxes = await GetAllTeeboxes();

        // Return the golf courses with teeboxes
        return AssociateTeeboxesWithCourses(result.Models, teeboxes);
    }


    /// <summary>
    /// This function will run a supabase query to return all of the courses in the database given an id
    /// </summary>
    /// <param name="course_id">The id of the golf course</param>
    /// <returns>Returns a list of courses from an id</returns>
    private static async Task<List<Course>> GetCourse(int course_id) 
    {
        // The single golf course
        var result = await supabase!.From<Course>().Filter("id", Operator.Equals, course_id).Get();

        // Get all of the teeboxes
        List<Teebox> teeboxes = await GetTeebox(course_id);

        // Return the golf courses with teeboxes
        return AssociateTeeboxesWithCourses(result.Models, teeboxes);
    }


    /// <summary>
    /// This function will run a supabase query to return all of the courses in the database
    /// </summary>
    ///     
    /// <returns>Returns a list of golf courses</returns>
    private static async Task<List<Teebox>> GetAllTeeboxes() 
    {
        var result = await supabase!.From<Teebox>().Get();

        List<Hole> holes = await GetAllHoles();

        return AssociateHolesWithTeeboxes(result.Models, holes);
    }


    /// <summary>
    /// This function will run a supabase query to return all of the teeboxes associated with a course id
    /// </summary>
    /// <param name="course_id">The id of the golf course</param>
    /// <returns>Returns a list of courses from an id</returns>
    private static async Task<List<Teebox>> GetTeebox(int course_id) 
    {
        var result = await supabase!.From<Teebox>().Filter("course_id", Operator.Equals, course_id).Get();

        List<Hole> holes = new List<Hole>();
        for(int i = 0; i < result.Models.Count; i++) {
            var holeResult = await GetHoles(result.Models[i].id);
            holes = holes.Concat(holeResult).ToList();
        }
        return AssociateHolesWithTeeboxes(result.Models, holes);
    }


    /// <summary>
    /// This function will run a supabase query to return all of the holes in the database
    /// </summary>
    ///     
    /// <returns>Returns a list of golf courses</returns>
    private static async Task<List<Hole>> GetAllHoles() 
    {
        var result = await supabase!.From<Hole>().Get();
        return result.Models;
    }


    /// <summary>
    /// This function will run a supabase query to return all of the holes associated with a teebox id
    /// </summary>
    /// <param name="teebox_id">The id of the teebox </param>
    /// <returns>Returns a list of holes from an id</returns>
    private static async Task<List<Hole>> GetHoles(int teebox_id) 
    {
        var result = await supabase!.From<Hole>().Filter("teebox_id", Operator.Equals, teebox_id).Get();
        return result.Models;
    }


};

using api_publicGolf.models;
using Newtonsoft.Json;
using static Postgrest.Constants;

namespace api_publicGolf.routes;

public static class CourseRoutesConfig {

    private static Supabase.Client? supabase;

    // All Course Routes
    public static void CourseRoutes(this IEndpointRouteBuilder app, Supabase.Client s)
    {
        supabase = s;

        const string routePrefix = "/api";

        app.MapGet($"{routePrefix}/course", async () =>
        {
            var result = await GetAllCourses();
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        });

        app.MapGet($"{routePrefix}/course/{{course_id}}", async (int course_id) => 
        {   
            var result = await GetCourse(course_id);
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        });

        app.MapGet($"{routePrefix}/course/{{course_id}}/teebox", async (int course_id) =>
        {
            var result = await GetTeebox(course_id);
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        });

        
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
        return result.Models;
    }


    /// <summary>
    /// This function will run a supabase query to return all of the teeboxes associated with a course id
    /// </summary>
    /// 
    /// <returns>Returns a list of courses from an id</returns>
    private static async Task<List<Teebox>> GetTeebox(int course_id) 
    {
        var result = await supabase!.From<Teebox>().Filter("course_id", Operator.Equals, course_id).Get();
        return result.Models;
    }


};
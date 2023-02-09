
using api_publicGolf.models;
using Newtonsoft.Json;
using static Postgrest.Constants;
using api_publicGolf;

namespace api_publicGolf.routes;

public static class CourseRoutesConfig {

    private static Database? database;

    const string CorsPolicyName = "_myCorsPolicy";

    // All Course Routes
    public static void CourseRoutes(this IEndpointRouteBuilder app)
    {
        const string routePrefix = "/api";

        // Get all courses
        app.MapGet($"{routePrefix}/course", () =>
        {
            database = new Database();
            database.closeConnection();
            return Results.Ok(database.GetAllCourses());

        }).RequireCors(CorsPolicyName);

        // Get a course from an id
        app.MapGet($"{routePrefix}/course/{{course_id}}", (int course_id) => 
        {   
            if(course_id <= 0) {
                return Results.BadRequest("bad request");
            }

            database = new Database();
            return Results.Ok(database.GetCourse(course_id));

        }).RequireCors(CorsPolicyName);

        // Get a list of teeboxes for the course
        app.MapGet($"{routePrefix}/course/{{course_id}}/teebox", (int course_id) =>
        {
            database = new Database();
            return Results.Ok(database.GetTeeboxes(course_id));
        }).RequireCors(CorsPolicyName);
    }
};
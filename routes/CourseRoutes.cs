
using api_publicGolf.models;
using api_publicGolf.helpers;

namespace api_publicGolf.routes;

public static class CourseRoutesConfig {

    private static Database database = new Database();

    const string CorsPolicyName = "_myCorsPolicy";

    // All Course Routes
    public static void CourseRoutes(this IEndpointRouteBuilder app)
    {
        const string routePrefix = "/api";
        
        app.MapGroup($"{routePrefix}/course").WithOpenApi();



        app.MapGet($"{routePrefix}/course", () 
            => Results.Ok(CourseHelper.GetAllCourses()))
                .RequireCors(CorsPolicyName).WithDescription("This will return all courses in the database");


        app.MapGet($"{routePrefix}/course/{{course_id}}", (int course_id) 
            => course_id <= 0 ? Results.BadRequest() : Results.Ok(CourseHelper.GetCourse(course_id)))
                .RequireCors(CorsPolicyName).WithDescription("This will return a course given an id");


        app.MapGet($"{routePrefix}/course/{{course_id}}/teebox", (int course_id) 
            => course_id <= 0 ? Results.BadRequest() : Results.Ok(CourseHelper.GetTeeboxes(course_id)))
                .RequireCors(CorsPolicyName).WithDescription("This will return a teebox given a course id");



        // PUT REQUESTS
        app.MapPut($"{routePrefix}/course/{{teebox_id}}/update/hole", (int teebox_id, Hole hole) 
            => Results.Ok(CourseHelper.UpdateHole(hole)))
                .RequireCors(CorsPolicyName).WithDescription("This will update a golf hole");

    }
};
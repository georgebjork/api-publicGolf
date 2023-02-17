
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
        

        app.MapGet($"{routePrefix}/course", () 
            => Results.Ok(CourseHelper.GetAllCourses()))
                .RequireCors(CorsPolicyName).WithDescription("This will return all courses in the database").WithOpenApi();




        app.MapGet($"{routePrefix}/course/{{course_id}}", (int course_id) 
            => course_id <= 0 ? Results.BadRequest() : Results.Ok(CourseHelper.GetCourse(course_id)))
                .RequireCors(CorsPolicyName).WithDescription("This will return a course given an id").WithOpenApi();



        app.MapGet($"{routePrefix}/course/{{course_id}}/teebox", (int course_id) 
            => course_id <= 0 ? Results.BadRequest() : Results.Ok(CourseHelper.GetTeeboxes(course_id)))
                .RequireCors(CorsPolicyName).WithDescription("This will return a course given an id").WithOpenApi();
    }
};
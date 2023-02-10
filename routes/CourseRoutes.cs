
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
        

        app.MapGet($"{routePrefix}/course", () =>
        {
            List<Course> courses = CourseHelper.GetAllCourses();

            return Results.Ok(courses);

        }).RequireCors(CorsPolicyName)
        .WithDescription("This will return all courses in the database").WithOpenApi();




        app.MapGet($"{routePrefix}/course/{{course_id}}", (int course_id) => 
        {   
            if(course_id <= 0) {
                return Results.BadRequest();
            }

            List<Course> courses = CourseHelper.GetCourse(course_id);

            return Results.Ok(courses);

        }).RequireCors(CorsPolicyName)
        .WithDescription("This will return a course given an id").WithOpenApi();



        app.MapGet($"{routePrefix}/course/{{course_id}}/teebox", (int course_id) =>
        {
            if(course_id <= 0) {
                return Results.BadRequest();
            }

            List<Teebox> teeboxes = CourseHelper.GetTeeboxes(course_id);

            return Results.Ok(teeboxes);

        }).RequireCors(CorsPolicyName)
        .WithDescription("This will return a list of teeboxes given a course id").WithOpenApi();
    }
};
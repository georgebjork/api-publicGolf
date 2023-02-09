using api_publicGolf.routes;

const string CorsPolicyName = "_myCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddPolicy(name: CorsPolicyName, builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Here is our api routes
app.CourseRoutes();

app.UseCors();
app.MapGet("/", () => "Welcome to the public golf api!");

app.Run();

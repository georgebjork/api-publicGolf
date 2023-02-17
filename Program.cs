using api_publicGolf.routes;

const string CorsPolicyName = "_myCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddPolicy(name: CorsPolicyName, builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

var app = builder.Build();

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/handle-errrors?view=aspnetcore-7.0#problem-details
app.UseStatusCodePages(async statusCodeContext 
    =>  await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
                 .ExecuteAsync(statusCodeContext.HttpContext));

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Here is our api routes
app.CourseRoutes();

// This will enable cors 
app.UseCors();

// Default route
app.MapGet("/api", () => "Welcome to the public golf api!");

// Exception route 
app.Map("/exception", () => { throw new InvalidOperationException("Sample Exception"); });

// This will be where our api starts on launch
app.Map("/", () => Results.Redirect("/swagger"));
app.Run();

using api_publicGolf.routes;
using dotenv.net;

var envVars = DotEnv.Read();

var url = envVars["SUPABASE_URL"];
var key = envVars["API_KEY"];

var options = new Supabase.SupabaseOptions{ AutoConnectRealtime = true };

var supabase = new Supabase.Client(url, key, options);
await supabase.InitializeAsync();

const string CorsPolicyName = "_myCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddPolicy(name: CorsPolicyName, builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Here is our api routes
app.CourseRoutes(supabase);

app.UseCors();
app.MapGet("/", () => "Welcome to the public golf api!");

app.Run();

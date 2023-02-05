using api_publicGolf.routes;
using dotenv.net;

var envVars = DotEnv.Read();

var url = envVars["SUPABASE_URL"];
var key = envVars["API_KEY"];

var options = new Supabase.SupabaseOptions{ AutoConnectRealtime = true };

var supabase = new Supabase.Client(url, key, options);
await supabase.InitializeAsync();

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Here is our api routes
app.CourseRoutes(supabase);

app.MapGet("/", () => "Welcome to the public golf api!");

app.Run();

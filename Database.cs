using Npgsql;
using dotenv.net;
using api_publicGolf.models;
using System.Data;

namespace api_publicGolf;

public class Database {

    // This holds our connection string 
    private readonly string connString;
    
    // This is our connection object
    private NpgsqlConnection connection;

    public Database() 
    {
        // Create a connection String
        connString = createConnectionString();

        // Create a connection to the database
        connection = initConnection();
    }
    


    /// <summary>
    /// This function will read our .env and get the username and password for the database and convert it to a connection string
    /// </summary>
    ///     
    /// <returns>String representing the connection string</returns>
    private String createConnectionString() 
    {
        var envVars = DotEnv.Read();
        var password = envVars["DB_PASSWORD"];
        var userId = envVars["DB_USERNAME"];

        return $"User Id={userId};Password={password};Server=db.xqnkfufucqnnmsljuzrz.supabase.co;Port=5432;Database=postgres";
    }



    /// <summary>
    /// This function will use our created connection string and init a connection to the database.
    /// </summary>
    ///     
    /// <returns>NpgsqlConnection object representing our database</returns>
    private NpgsqlConnection initConnection() => new NpgsqlConnection(connString);
    


    /// <summary>
    /// This function will close our connection to the database
    /// </summary>
    ///     
    /// <returns>void</returns>
    public void closeConnection() => connection.Close();



    /// <summary>
    /// This function will run a query on our database
    /// </summary>
    /// <param name="query">Query to run</param>   
    /// <returns>NpgsqlDataReader representing our database</returns>
    public NpgsqlDataReader query(String query) 
    {
        // If our connection is not open, lets open it.
        if(connection.State != ConnectionState.Open) { connection.Open(); }

        var command = new NpgsqlCommand(query, connection);

        return command.ExecuteReader();       
    }
}

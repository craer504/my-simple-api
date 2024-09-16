using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SimpleRegisterLoginLogout.Classes;
using SimpleRegisterLoginLogout.DBs;
using SimpleRegisterLoginLogout.Utility;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

DBSaver.LoadDBFromFile(UserDB.GetUserDB(), UserDB.DBFilePath);

app.UseEndpoints(endpoints =>
{
    endpoints.MapPost("/register", async (context) =>
    {
        string? body = await new StreamReader(context.Request.Body).ReadToEndAsync();
        Dictionary<string, StringValues> kvps = QueryHelpers.ParseQuery(body);
        string? username = string.Empty;
        string? password = string.Empty;

        if (Session.User != null)
        {
            await context.Response.WriteAsync($"Already logged in!\n");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        if (kvps.ContainsKey("username") && kvps.ContainsKey("password"))
        {
            username = kvps["username"][0]!;
            password = kvps["password"][0]!;
        }
        else
        {
            await context.Response.WriteAsync($"Username or password is missing!\n");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        if (!UserDB.IsUsernameRegistered(username))
        {
            User newUser = new User(Guid.NewGuid(), username, password);
            UserDB.AddUser(newUser);

            await context.Response.WriteAsync($"Created user: {username}\n");
        }
        else
        {
            await context.Response.WriteAsync($"Username already exists!\n");
        }

        DBSaver.SaveDBToFile(UserDB.GetUserDB(), UserDB.DBFilePath);
    });

    endpoints.MapPost("/login", async (context) =>
    {
        string? body = await new StreamReader(context.Request.Body).ReadToEndAsync();
        Dictionary<string, StringValues> kvps = QueryHelpers.ParseQuery(body);
        string? username = string.Empty;
        string? password = string.Empty;

        if (Session.User != null)
        {
            await context.Response.WriteAsync($"Already logged in!\n");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        if (kvps.ContainsKey("username") && kvps.ContainsKey("password"))
        {
            username = kvps["username"][0]!;
            password = kvps["password"][0]!;
        }
        else
        {
            await context.Response.WriteAsync($"Username or password is missing!\n");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        if (UserDB.IsUserRegistered(username, password))
        {
            Session.User = UserDB.GetUserByCredentials(username, password);

            await context.Response.WriteAsync($"Login succesful: {username}\n");
        }
        else
        {
            await context.Response.WriteAsync($"User not found!\n");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
    });

    endpoints.MapGet("/logout", async (context) =>
    {
        if (Session.UserLogout())
        {
            await context.Response.WriteAsync($"Logging out.\n");

            context.Response.StatusCode = StatusCodes.Status200OK;
        }
        else
        {
            await context.Response.WriteAsync("User not found.\n");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
    });

    endpoints.MapGet("/user", async (context) =>
    {
        if (Session.User != null)
        {
            await context.Response.WriteAsync(Session.User.Username);
        }
        else
        {
            await context.Response.WriteAsync($"Please log in!\n");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
    });

    endpoints.MapGet("/users", async (context) =>
    {
        if (Session.User != null)
        {
            await context.Response.WriteAsync(UserDB.GetAllUsersAsString());
        }
        else
        {
            await context.Response.WriteAsync($"Please log in!\n");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
    });
});

app.Run(async (context) =>
{
    await context.Response.WriteAsync($"Catch-all terminal middleware - route: {context.Request.Path}");
});

app.Run();

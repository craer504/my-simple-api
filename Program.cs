using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SimpleRegisterLoginLogout.Classes;
using SimpleRegisterLoginLogout.DBs;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseWhen(context => context.Request.Path == "/register", app =>
{
    app.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Registering new user.\n");

        await next(context);

        await context.Response.WriteAsync("Registration successful.\n");
    });
});

app.UseWhen(context => context.Request.Path == "/login", app =>
{
    app.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Attempting to log in.\n");

        await next(context);

        await context.Response.WriteAsync("Login successful.\n");
    });
});

app.UseWhen(context => context.Request.Path == "/logout", app =>
{
    app.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Attempting to log out.\n");

        await next(context);

        await context.Response.WriteAsync("Logout successful.\n");
    });
});

app.UseEndpoints(endpoints =>
{
    // Register a new user:
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
            User newUser = new User(username, password);
            UserDB.AddUser(newUser);

            await context.Response.WriteAsync($"Created user: {username}, {password}");
        }
        else
        {
            await context.Response.WriteAsync($"Username already exists!\n");
        }
    });

    // Login with an existing user:
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

            await context.Response.WriteAsync($"Login succesful: {username}");
        }
        else
        {
            await context.Response.WriteAsync($"User not found!\n");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
    });

    // Get user details:
    endpoints.MapGet("/user", async (context) =>
    {
        if (Session.User != null)
        {
            await context.Response.WriteAsync(Session.User.ToString()!);
        }
        else
        {
            await context.Response.WriteAsync($"Please log in!\n");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
    });

    // List all users:
    endpoints.MapGet("/users", async (context) =>
    {
        if (Session.User != null)
        {
            await context.Response.WriteAsync(UserDB.GetAllUsers());
        }
        else
        {
            await context.Response.WriteAsync($"Please log in!\n");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
    });
});

app.Run();

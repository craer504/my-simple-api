using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SimpleRegisterLoginLogout.Classes;
using SimpleRegisterLoginLogout.DBs;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

// Register a new user:
app.UseWhen(context => context.Request.Path == "/register", app =>
{
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
                User newUser = new User(username, password);
                UserDB.AddUser(newUser);

                await context.Response.WriteAsync($"Created user: {username}, {password}\n");
            }
            else
            {
                await context.Response.WriteAsync($"Username already exists!\n");
            }
        });

    });
});

// Login with an existing user:
app.UseWhen(context => context.Request.Path == "/login", app =>
{
    app.UseEndpoints(endpoints =>
    {
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

    });
});

// Logout currently logged in user:
app.UseWhen(context => context.Request.Path == "/logout", app =>
{
    app.UseEndpoints(endpoints =>
    {
        // Logout with current user:
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
    });
});

// Other functionality:
app.UseEndpoints(endpoints =>
{
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

app.Run(async (context) =>
{
    await context.Response.WriteAsync($"Catch-all terminal middleware - route: {context.Request.Path}");
});

app.Run();

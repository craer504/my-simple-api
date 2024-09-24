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
DBSaver.LoadDBFromFile(PostDB.GetPostDB(), PostDB.DBFilePath);

app.UseEndpoints(endpoints =>
{
    endpoints.MapPost("/register", async (context) =>
    {

        if (Session.User != null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Already logged in!\n");

            return;
        }

        string? body = await new StreamReader(context.Request.Body).ReadToEndAsync();
        Dictionary<string, StringValues> kvps = QueryHelpers.ParseQuery(body);
        string? username = string.Empty;
        string? password = string.Empty;

        if (kvps.ContainsKey("username") && kvps.ContainsKey("password"))
        {
            username = kvps["username"][0]!;
            password = kvps["password"][0]!;
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Username or password is missing!\n");

            return;
        }

        if (!UserDB.IsUsernameRegistered(username))
        {
            User newUser = new User(Guid.NewGuid(), username, password);
            UserDB.AddUser(newUser);

            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync($"Created user: {username}\n");
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Username already exists!\n");

            return;
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
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Already logged in!\n");

            return;
        }

        if (kvps.ContainsKey("username") && kvps.ContainsKey("password"))
        {
            username = kvps["username"][0]!;
            password = kvps["password"][0]!;
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Username or password is missing!\n");

            return;
        }

        if (UserDB.IsUserRegistered(username, password))
        {
            Session.User = UserDB.GetUserByCredentials(username, password);

            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync($"Login succesful: {username}\n");
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"User not found!\n");

            return;
        }
    });

    endpoints.MapGet("/logout", async (context) =>
    {
        if (Session.UserLogout())
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync($"Logging out.\n");
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("User not found.\n");

            return;
        }
    });

    endpoints.MapGet("/user", async (context) =>
    {
        if (Session.User == null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Please log in!\n");

            return;
        }

        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsync(Session.User.Username);
    });

    endpoints.MapGet("/users", async (context) =>
    {
        if (Session.User == null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Please log in!\n");

            return;
        }

        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsync(UserDB.GetAllUsersAsString());
    });

    endpoints.MapPost("/create-post", async (context) =>
    {
        if (Session.User == null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Please log in!\n");

            return;
        }

        string? body = await new StreamReader(context.Request.Body).ReadToEndAsync();
        Dictionary<string, StringValues> kvps = QueryHelpers.ParseQuery(body);
        string? title = string.Empty;
        string? mediaURL = string.Empty;

        if (kvps.ContainsKey("title"))
        {
            title = kvps["title"][0]!;

            if (kvps.ContainsKey("mediaURL"))
                mediaURL = kvps["mediaURL"][0]!;
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Title is missing!\n");

            return;
        }

        Post newPost = new Post(Guid.NewGuid(), title, mediaURL);
        PostDB.AddPost(newPost);

        await context.Response.WriteAsync($"Created post: {title}\n");

        DBSaver.SaveDBToFile(PostDB.GetPostDB(), PostDB.DBFilePath);
    });
});

app.Run(async (context) =>
{
    context.Response.StatusCode = StatusCodes.Status200OK;
    await context.Response.WriteAsync($"Catch-all terminal middleware - route: {context.Request.Path}");
});

app.Run();

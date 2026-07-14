
using LicenseManagerMinimalAPI.Data;
using LicenseManagerMinimalAPI.Middleware;
using LicenseManagerMinimalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.ComponentModel.DataAnnotations;

namespace LicenseManagerMinimalAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
           
            var builder = WebApplication.CreateBuilder(args);

            var columnOptions = new ColumnOptions(); // create column options for MSSqlServer sink
            
            // configure serilog
            Log.Logger = new LoggerConfiguration()
                       .MinimumLevel.Information() 
                       .WriteTo.Console() 
                       .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) 
                       .WriteTo.MSSqlServer(
                            connectionString: builder.Configuration.GetConnectionString("DefaultConnection"), 
                            sinkOptions: new MSSqlServerSinkOptions
                            {
                                TableName = "Logs", 
                                AutoCreateSqlTable = true 
                            },
                            columnOptions: columnOptions) 
                       .CreateLogger(); 

            builder.Host.UseSerilog();

            //add database service
            builder.Services.AddDbContext<AppDatabase>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //builder.Services.AddDbContext<AppDatabase>(op => op.UseInMemoryDatabase("Licenses"));

            // to show exceptions that may happen while developing 
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //adding problem details middleware
            builder.Services.AddProblemDetails();

            var app = builder.Build();
            Log.Information("Starting the application...");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseDeveloperExceptionPage();
            }
            //to convert any error can occur to problem details
            app.UseExceptionHandler();
            app.UseStatusCodePages();

            app.UseMiddleware<ExceptionMiddleware>(); // add custom exception handling middleware

            app.UseHttpsRedirection();

            app.MapGet("/license/error", () => {
                try
                {
                    throw new Exception("Database Erorr");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occurred while accessing the database");
                    return Results.Problem("An error occurred while accessing the database");
                }
            });

            app.MapGet("/license", (AppDatabase db) => { 
                Log.Information("Getting all licenses");
                return db.Licences.ToList();
            });
            app.MapGet("/license/active", GetActive);

            app.MapGet("/license/{id}", (AppDatabase db, int id) => db.Licences.FirstOrDefault(l => l.Id == id));

            app.MapGet("/license/search", (AppDatabase db, [FromQuery] string key) =>
            {
                Log.Information("Searching for license with key: {Key}", key);

                if (string.IsNullOrEmpty(key))
                {
                    Log.Warning("Search key is null or empty");
                    return Results.Problem(new ProblemDetails()
                    {
                        Title = "The key is required",
                        Status = StatusCodes.Status400BadRequest
                    });
                }

                var li = db.Licences.FirstOrDefault(l => l.Key.Equals(key));
                if (li != null)
                {
                    Log.Information("License found with key: {Key}", key);
                    return TypedResults.Ok(li);
                }
                else
                {
                    Log.Warning("License not found with key: {Key}", key);
                    return Results.Problem(detail: "The key not found", statusCode: StatusCodes.Status404NotFound);

                }
            });

            app.MapPost("/license" , (AppDatabase db , AppLicence licence) =>
            {
                Log.Information("Adding a new license");
                if (db.Licences.FirstOrDefault(l => l.Id == licence.Id) != null) { 
                    Log.Warning("License already exists");
                    return Results.BadRequest($"ID: The licesne is already exists.");
                    //return Results.BadRequest();
                }

                db.Licences.Add(licence);
                db.SaveChanges();
                Log.Information("License added successfully with ID: {Id}", licence.Id);

                // return Results.Ok();
                //return TypedResults.Ok(licence); // typedresults to return objects
                return TypedResults.Ok($"The licnse has been added with id {licence.Id}");
            }).WithParameterValidation();

            app.MapPut("/license", (AppDatabase db, AppLicence licence, int id) =>
            {
                Log.Information("Updating license with ID: {Id}", id);

                var li = db.Licences.FirstOrDefault(l => l.Id == id);
                if (li is not null)
                {
                   
                    li.UserNmae = licence.UserNmae;
                    li.Key = licence.Key;
                    li.IsActive = licence.IsActive;

                    db.SaveChanges();

                    Log.Information("License updated successfully");
                    return Results.Ok(li);
                }
                else
                {
                    Log.Warning("License not found");
                    return Results.NotFound();
                }
            });

            app.MapDelete("/license/{id}", (AppDatabase db, int id) =>
            {
                Log.Information("Deleting license with ID: {Id}", id);
                var li = db.Licences.FirstOrDefault(l => l.Id == id);
                if (li is not null)
                {
                    db.Remove(li);
                    db.SaveChanges();
                    return Results.NoContent();
                }
                else
                   // Results.NotFound();
                   return Results.NotFound($"The license not found");
            });

            app.Run();
        }

        public record struct LicenseID(
            [Required]
            [Range(1, int.MaxValue)]
            int id
        );
        public static List<AppLicence> GetActive(AppDatabase db)
        {
            return db.Licences.Where(l => l.IsActive).ToList();
        }
    }
}

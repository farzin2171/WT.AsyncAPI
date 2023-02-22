using Microsoft.EntityFrameworkCore;
using WT.AsyncProductAPI.Data;
using WT.AsyncProductAPI.DTO;
using WT.AsyncProductAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data source=RequestDB.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


//Start endpoint
app.MapPost("api/v1/products", async (AppDbContext context, ListingRequest request) =>
{
    if(request == null)
    {
        return Results.BadRequest();
    }

    request.RequestStatus = "ACCEPTED";
    request.EstimatedCompetionTime = "2023-02-23";

    await context.ListingRequests.AddAsync(request);
    await context.SaveChangesAsync();

    return Results.Accepted($"api/v1/productstatus/{request.RequestId}",request);
});

//Status endpoint
app.MapGet("api/v1/productstatus/{requestId}", (AppDbContext context, string requestId) =>
 {
     var lisingRequest=context.ListingRequests.FirstOrDefault(x => x.RequestId == requestId);
     if(lisingRequest == null)
     {
         return Results.NotFound();
     }

     var listingStatus = new ListingStatus
     {
         RequestStatus = lisingRequest.RequestStatus,
         ResourceUrl = String.Empty
     };

     if(lisingRequest.RequestStatus.ToUpper()== "COMPLETED")
     {
         listingStatus.ResourceUrl = $"api/v1/products/{Guid.NewGuid().ToString()}";
         //return Results.Ok(listingStatus);
         return Results.Redirect($"https://localhost:7281/{listingStatus.ResourceUrl}");
     }

     listingStatus.EstimatedComplitionTime = "2023-02-06";
     return Results.Ok(listingStatus);

 });

//Final endpoint

app.MapGet("api/v1/products/{requestId}", (string requestId) =>
{
    return Results.Ok("Tthis is Final call");
});


app.Run();

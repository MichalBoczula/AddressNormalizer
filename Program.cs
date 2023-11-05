using Microsoft.AspNetCore.Mvc;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/NormalizeAddress", async ([FromBody] string address) =>
{
    var url = "https://api.openai.com/v1/engines/text-davinci-003/completions";
    var apiKey = @"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";
    var prompt = @$"Normalize Address: {address}. Answer should be a json in form: city, postalCode, street, buildingNumber, flatNumber.";
    var maxTokens = 1000;

    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        var jsonContent = new StringContent($"{{\"prompt\": \"{prompt}\", \"max_tokens\": {maxTokens}}}", Encoding.UTF8, "application/json");

        using var response = await client.PostAsync(url, jsonContent);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
});

app.Run();

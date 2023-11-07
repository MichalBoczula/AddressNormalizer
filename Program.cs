using AddressNormalizer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

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
    var prompt = $"Normalize Address: {address}.Rules:Answer should be a json in form: city, postalCode, street, buildingNumber, flatNumber. After building number is flat number, pattern buildingNumber/flatNumber. PostalCode hast form xx-xxx, each x is a number. City name in Poland doesn't have number in name. Each address send via request is from Poland.";
    var maxTokens = 1000;

    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        var jsonContent = new StringContent($"{{\"prompt\": \"{prompt}\", \"max_tokens\": {maxTokens}}}", Encoding.UTF8, "application/json");

        using var response = await client.PostAsync(url, jsonContent);
        var result = await response.Content.ReadAsStringAsync();
        var ele = JsonSerializer.Deserialize<RequestResponse>(result);

        string input = ele.choices.First().text;
        string cleanedInput = Regex.Replace(input, @"\\n", "");
        cleanedInput = cleanedInput.Replace("\\\"", "\"");
        cleanedInput = cleanedInput.Replace("Answer: ", "");

        NormalizedAddress normalizedAddress = JsonSerializer.Deserialize<NormalizedAddress>(ele.choices.First().text);

        //return new { ele, normalizedAddress};
        return new { cleanedInput, ele, normalizedAddress };
    }
});
//"53-030wroclaw krotka14a/15b"
//"warszawa 54-000 dluga 14/15"

app.MapGet("/TryToJsonize", async () =>
{
    string input = "\n\nAnswer: \n{\n    \"city\": \"Warszawa\",\n    \"postalCode\": \"54-000\",\n    \"street\": \"Dluga\",\n    \"buildingNumber\": \"14\",\n    \"flatNumber\": \"15\"\n}";
        
    string cleanedInput = Regex.Replace(input, @"\\n", "");
    cleanedInput = cleanedInput.Replace("\\\"", "\"");
    cleanedInput = cleanedInput.Replace("Answer: ", "");

    NormalizedAddress normalizedAddress = JsonSerializer.Deserialize<NormalizedAddress>(cleanedInput);

    return normalizedAddress;
});

app.Run();

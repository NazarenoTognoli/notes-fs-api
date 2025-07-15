using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

// Carpeta base donde estarán los .txt
string txtFolder = Path.Combine(Directory.GetCurrentDirectory(), "TextFiles");
Directory.CreateDirectory(txtFolder);

// Crear un nuevo archivo .txt
app.MapPost("/files/{filename}", async (string filename, HttpRequest request) =>
{
    string filePath = Path.Combine(txtFolder, $"{filename}.txt");
    using var reader = new StreamReader(request.Body);
    var content = await reader.ReadToEndAsync();

    await File.WriteAllTextAsync(filePath, content);
    return Results.Created($"/files/{filename}", $"Archivo '{filename}.txt' creado.");
});

// Leer el contenido de un archivo .txt
app.MapGet("/files/{filename}", async (string filename) =>
{
    string filePath = Path.Combine(txtFolder, $"{filename}.txt");

    if (!File.Exists(filePath))
        return Results.NotFound("Archivo no encontrado.");

    var content = await File.ReadAllTextAsync(filePath);
    return Results.Ok(content);
});

// Actualizar el contenido de un archivo .txt
app.MapPut("/files/{filename}", async (string filename, HttpRequest request) =>
{
    string filePath = Path.Combine(txtFolder, $"{filename}.txt");

    if (!File.Exists(filePath))
        return Results.NotFound("Archivo no encontrado.");

    using var reader = new StreamReader(request.Body);
    var newContent = await reader.ReadToEndAsync();

    await File.WriteAllTextAsync(filePath, newContent);
    return Results.Ok($"Archivo '{filename}.txt' actualizado.");
});

// Eliminar un archivo .txt
app.MapDelete("/files/{filename}", (string filename) =>
{
    string filePath = Path.Combine(txtFolder, $"{filename}.txt");

    if (!File.Exists(filePath))
        return Results.NotFound("Archivo no encontrado.");

    File.Delete(filePath);
    return Results.Ok($"Archivo '{filename}.txt' eliminado.");
});

// Listar todos los archivos .txt
app.MapGet("/files", () =>
{
    var files = Directory.GetFiles(txtFolder, "*.txt")
                         .Select(Path.GetFileName)
                         .ToList();
    return Results.Ok(files);
});

app.Run();

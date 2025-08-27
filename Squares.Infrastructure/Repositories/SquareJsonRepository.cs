using Microsoft.Extensions.Logging;
using Squares.Domain.Models;
using Squares.Domain.Repositories;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Squares.Infrastructure.Repositories;

public class SquareJsonRepository : ISquareRepository
{
    private const string FILE_PATH = "squares.json";
    private readonly List<Square> _squares = new List<Square>();
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private readonly ILogger<SquareJsonRepository> _logger;
    private readonly JsonSerializerOptions _serializerOptions;

    public SquareJsonRepository(ILogger<SquareJsonRepository> logger)
    {
        _logger = logger;
        _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = false
        };

        LoadSquaresFromFile();
    }

    public async ValueTask<IEnumerable<Square>> GetAllSquares(CancellationToken ct)
    {
        if (!File.Exists(FILE_PATH))
        {
            return Enumerable.Empty<Square>();
        }

        await _semaphore.WaitAsync(ct);

        try
        {
            await using var stream = new FileStream(FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read);
            var squares = await JsonSerializer.DeserializeAsync<List<Square>>(stream, _serializerOptions, ct);
            return squares ?? Enumerable.Empty<Square>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all squares: {Message}", ex.Message);
            return _squares;
            
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async ValueTask<Square?> GetLastSquare(CancellationToken ct)
    {
        await _semaphore.WaitAsync(ct);
        try
        {
            return _squares.LastOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving last square: {Message}", ex.Message);
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async IAsyncEnumerable<Square> GetAllSquaresAsyncStream([EnumeratorCancellation] CancellationToken ct)
    {
        if (!File.Exists(FILE_PATH))
        {
            yield break;
        }

        await _semaphore.WaitAsync(ct);

        try
        {
            await using var stream = new FileStream(FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read);
            var squaresEnumerable = JsonSerializer.DeserializeAsyncEnumerable<Square>(stream, _serializerOptions, ct);

            await foreach (var square in squaresEnumerable.WithCancellation(ct))
            {
                ct.ThrowIfCancellationRequested();
                if (square != null)
                {
                    yield return square;
                }
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async ValueTask<Square> SaveNewSquare(Square square, CancellationToken ct)
    {
        await _semaphore.WaitAsync(ct);
        try
        {
            var newSquare = new Square(
                Color: square.Color,
                Position: square.Position
            );

            _squares.Add(newSquare);

            await WriteSquaresToFile(ct);
            _logger.LogInformation("Successfully saved new square with position {Position}", newSquare.Position);
            return newSquare;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving new square: {Message}", ex.Message);
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async ValueTask DeleteAllSquares(CancellationToken ct)
    {
        await _semaphore.WaitAsync(ct);
        try
        {
            _squares.Clear();
            await WriteSquaresToFile(ct);
            _logger.LogInformation("Successfully deleted all squares");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting all squares: {Message}", ex.Message);
            throw new InvalidOperationException($"Failed to delete all squares: {ex.Message}", ex);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private void LoadSquaresFromFile()
    {
        if (!File.Exists(FILE_PATH))
        {
            _logger.LogInformation("No squares.json file found. Starting with empty collection.");
            return;
        }

        try
        {
            var json = File.ReadAllText(FILE_PATH);
            var deserializedSquares = JsonSerializer.Deserialize<List<Square>>(json, _serializerOptions);
            if (deserializedSquares is not null)
            {
                _squares.Clear();
                _squares.AddRange(deserializedSquares);
                _logger.LogInformation("Successfully loaded {Count} squares from file", deserializedSquares.Count());
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error deserializing squares from file: {Message}", ex.Message);
            // Create a backup of the corrupted file for potential recovery
            CreateBackupFile();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error loading squares: {Message}", ex.Message);
        }
    }

    private async Task WriteSquaresToFile(CancellationToken ct)
    {
        try
        {
            string tempFilePath = $"{FILE_PATH}.tmp.json";
            
            await using (var stream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await JsonSerializer.SerializeAsync(stream, _squares, _serializerOptions, ct);
                await stream.FlushAsync(ct);
            }

            File.Move(tempFilePath, FILE_PATH, true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error writing squares to file: {Message}", ex.Message);
            throw;
        }
    }

    private void CreateBackupFile()
    {
        try
        {
            if (File.Exists(FILE_PATH))
            {
                string backupPath = $"{FILE_PATH}.bak.{DateTimeOffset.UtcNow:yyyyMMddHHmmss}";
                File.Copy(FILE_PATH, backupPath);
                _logger.LogInformation("Created backup of squares file at {BackupPath}", backupPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create backup file: {Message}", ex.Message);
        }
    }

    // Clean up resources
    ~SquareJsonRepository()
    {
        _semaphore?.Dispose();
    }
}
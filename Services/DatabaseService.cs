using System.Diagnostics;
using PersonalFinanceApp.Models;
using SQLite;
using Microsoft.Extensions.Logging;

namespace PersonalFinanceApp.Services;

public class DatabaseService
{
    private const string DbFileName = "personalfinance.db3";

    private SQLiteAsyncConnection? _connection;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    private readonly ILogger<DatabaseService> _logger;

    public DatabaseService(ILogger<DatabaseService> logger)
    {
        _logger = logger;
    }

    // З'єднання створюється один раз, таблиця створюється при першому зверненні
    private async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        if (_connection is not null)
            return _connection;

        await _initLock.WaitAsync();
        try
        {
            if (_connection is null)
            {
                var path = Path.Combine(FileSystem.AppDataDirectory, DbFileName);
                Debug.WriteLine($"[DB] Файл бази: {path}");

                var connection = new SQLiteAsyncConnection(path);
                await connection.CreateTableAsync<Account>();
                _connection = connection;
            }
            return _connection;
        }
        finally
        {
            _initLock.Release();
        }
    }
    public async Task<List<Account>> GetAllAsync()
    {
        var db = await GetConnectionAsync();
        return await db.Table<Account>().ToListAsync();
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        var db = await GetConnectionAsync();
        return await db.FindAsync<Account>(id);   // null, якщо запису немає
    }

    public async Task<int> InsertAsync(Account account)
    {
        var db = await GetConnectionAsync();
        return await db.InsertAsync(account);     // після вставки account.Id заповнюється
    }

    public async Task<int> UpdateAsync(Account account)
    {
        var db = await GetConnectionAsync();
        return await db.UpdateAsync(account);
    }

    public async Task<int> DeleteAsync(Account account)
    {
        var db = await GetConnectionAsync();
        return await db.DeleteAsync(account);
    }
}
using Microsoft.Extensions.Logging;
using PersonalFinanceApp.Models;
using SQLite;

namespace PersonalFinanceApp.Services;

public class DatabaseService
{
    private const string DbFileName = "personalfinance.db3";

    private readonly ILogger<DatabaseService> _logger;
    private SQLiteAsyncConnection? _connection;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    public DatabaseService(ILogger<DatabaseService> logger)
    {
        _logger = logger;
    }

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
                _logger.LogInformation("Файл бази даних: {Path}", path);

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

    // null означає збій; порожній список означає, що рахунків просто немає
    public async Task<List<Account>?> GetAllAsync()
    {
        try
        {
            _logger.LogDebug("Завантаження всіх рахунків");
            var db = await GetConnectionAsync();
            var items = await db.Table<Account>().ToListAsync();

            if (items.Count == 0)
                _logger.LogWarning("У базі немає жодного рахунку");
            else
                _logger.LogInformation("Завантажено рахунків: {Count}", items.Count);

            return items;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не вдалося завантажити список рахунків");
            return null;
        }
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        try
        {
            var db = await GetConnectionAsync();
            var account = await db.FindAsync<Account>(id);

            if (account is null)
                _logger.LogWarning("Рахунок з Id={Id} не знайдено", id);
            else
                _logger.LogDebug("Рахунок з Id={Id} завантажено", id);

            return account;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не вдалося отримати рахунок з Id={Id}", id);
            return null;
        }
    }

    public async Task<bool> InsertAsync(Account account)
    {
        try
        {
            var db = await GetConnectionAsync();
            await db.InsertAsync(account);
            _logger.LogInformation("Додано рахунок Id={Id}, Name={Name}", account.Id, account.Name);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не вдалося додати рахунок Name={Name}", account.Name);
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Account account)
    {
        try
        {
            var db = await GetConnectionAsync();
            var rows = await db.UpdateAsync(account);

            if (rows == 0)
            {
                _logger.LogWarning("Оновлення не виконано: рахунок Id={Id} не знайдено", account.Id);
                return false;
            }

            _logger.LogInformation("Оновлено рахунок Id={Id}", account.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не вдалося оновити рахунок Id={Id}", account.Id);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Account account)
    {
        try
        {
            var db = await GetConnectionAsync();
            var rows = await db.DeleteAsync(account);

            if (rows == 0)
            {
                _logger.LogWarning("Видалення не виконано: рахунок Id={Id} не знайдено", account.Id);
                return false;
            }

            _logger.LogInformation("Видалено рахунок Id={Id}", account.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не вдалося видалити рахунок Id={Id}", account.Id);
            return false;
        }
    }
}
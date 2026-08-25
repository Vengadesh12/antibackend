using MyBackend.Domain.Entities;

namespace MyBackend.Domain.Interfaces
{
    /// <summary>
    /// Specialized repository contract for tracking user login and logout audit sessions with timestamps and IP addresses.
    /// </summary>
    public interface IUserSessionRepository : IRepository<UserSession>
    {
        /// <summary>
        /// Records a new user login session with client IP address, user agent, and timestamp.
        /// </summary>
        Task<UserSession> RecordLoginAsync(int userId, string email, string userName, string ipAddress, string? userAgent = null, string? sessionToken = null);

        /// <summary>
        /// Records user logout timestamp, deactivates active sessions, and saves logout timestamp in database.
        /// </summary>
        Task<bool> RecordLogoutAsync(int userId, string? ipAddress = null, string? sessionToken = null, string? email = null);

        /// <summary>
        /// Retrieves the session history for a specific user ID.
        /// </summary>
        Task<List<UserSession>> GetUserSessionsAsync(int userId, int limit = 50);

        /// <summary>
        /// Retrieves the most recent active and logged-out user sessions across the workspace.
        /// </summary>
        Task<List<UserSession>> GetAllRecentSessionsAsync(int limit = 100);

        /// <summary>
        /// Retrieves all currently active (logged-in) sessions where IsActive is true and LogoutTime is null.
        /// </summary>
        Task<List<UserSession>> GetActiveSessionsAsync();

        /// <summary>
        /// Retrieves paginated sessions with optional text search and status filter.
        /// </summary>
        Task<(List<UserSession> Items, int TotalCount)> GetPagedSessionsAsync(string? search, string? status, int page, int pageSize);

        /// <summary>
        /// Terminates a specific active session by ID.
        /// </summary>
        Task<bool> TerminateSessionAsync(int sessionId);

        /// <summary>
        /// Terminates all active sessions for a specific user ID.
        /// </summary>
        Task<int> TerminateAllUserSessionsAsync(int userId);

        /// <summary>
        /// Retrieves quick aggregated statistics for user activities.
        /// </summary>
        Task<(int ActiveCount, int TodayLogins, int TodayLogouts, int TotalSessions)> GetActivityStatsAsync();
    }
}

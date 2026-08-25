using Microsoft.EntityFrameworkCore;
using MyBackend.Application.Common.Exceptions;
using MyBackend.Application.Common.Interfaces;
using MyBackend.Application.Contracts;
using MyBackend.Application.Interfaces;
using MyBackend.Domain.Entities;

namespace MyBackend.Application.Services
{
    public class SettingService : ISettingService
    {
        private readonly IApplicationDbContext _context;

        public SettingService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SettingsOverviewResponse> GetSettingsAsync(string? category, string? search)
        {
            var query = _context.SystemSettings.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(category) && category != "ALL")
            {
                query = query.Where(s => s.Category.ToLower() == category.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lower = search.ToLower();
                query = query.Where(s => s.SettingKey.ToLower().Contains(lower) ||
                                         s.Description.ToLower().Contains(lower) ||
                                         s.Category.ToLower().Contains(lower));
            }

            var settings = await query.OrderBy(s => s.Category).ThenBy(s => s.SettingKey).ToListAsync();

            var categoriesList = await _context.SettingCategories
                .Where(c => c.DeletedFlag == 1)
                .OrderBy(c => c.Id)
                .ToListAsync();

            var settingCounts = await _context.SystemSettings
                .GroupBy(s => s.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Category.ToLower(), x => x.Count);

            var categoryDtos = categoriesList.Select(c => new SettingCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Icon = c.Icon,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy,
                DeletedFlag = c.DeletedFlag,
                SettingsCount = settingCounts.TryGetValue(c.Name.ToLower(), out var count) ? count : 0
            }).ToList();

            var totalSettings = await _context.SystemSettings.CountAsync();

            return new SettingsOverviewResponse
            {
                SecurityLevel = "High (JWT & RBAC)",
                AlertChannels = 3,
                SessionTimeout = "24h",
                TotalSettings = totalSettings,
                TotalCategories = categoryDtos.Count,
                Settings = settings,
                Categories = categoryDtos
            };
        }

        public async Task<List<SettingCategoryDto>> GetCategoriesAsync()
        {
            var categories = await _context.SettingCategories
                .Where(c => c.DeletedFlag == 1)
                .OrderBy(c => c.Id)
                .ToListAsync();

            var settingCounts = await _context.SystemSettings
                .GroupBy(s => s.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Category.ToLower(), x => x.Count);

            return categories.Select(c => new SettingCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Icon = c.Icon,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy,
                DeletedFlag = c.DeletedFlag,
                SettingsCount = settingCounts.TryGetValue(c.Name.ToLower(), out var count) ? count : 0
            }).ToList();
        }

        public async Task<SettingCategoryDto> CreateCategoryAsync(CreateSettingCategoryRequest request, string callerName)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new BadRequestException("Category name is required.");
            }

            var trimmedName = request.Name.Trim();

            var existing = await _context.SettingCategories
                .FirstOrDefaultAsync(c => c.DeletedFlag == 1 && c.Name.ToLower() == trimmedName.ToLower());

            if (existing != null)
            {
                throw new BadRequestException($"Category '{trimmedName}' already exists in database.");
            }

            var category = new SettingCategory
            {
                Name = trimmedName,
                Description = string.IsNullOrWhiteSpace(request.Description) ? $"{trimmedName} configuration and parameters" : request.Description.Trim(),
                Icon = string.IsNullOrWhiteSpace(request.Icon) ? "Tune" : request.Icon.Trim(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = callerName,
                DeletedFlag = 1
            };

            _context.SettingCategories.Add(category);
            await _context.SaveChangesAsync();

            return new SettingCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Icon = category.Icon,
                CreatedAt = category.CreatedAt,
                CreatedBy = category.CreatedBy,
                DeletedFlag = category.DeletedFlag,
                SettingsCount = 0
            };
        }

        public async Task<SettingCategoryDto?> UpdateCategoryAsync(int id, UpdateSettingCategoryRequest request)
        {
            var category = await _context.SettingCategories.FirstOrDefaultAsync(c => c.Id == id && c.DeletedFlag == 1);
            if (category == null) return null;

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var trimmed = request.Name.Trim();
                var dup = await _context.SettingCategories.FirstOrDefaultAsync(c => c.Id != id && c.DeletedFlag == 1 && c.Name.ToLower() == trimmed.ToLower());
                if (dup != null)
                {
                    throw new BadRequestException($"Another category with name '{trimmed}' already exists.");
                }
                category.Name = trimmed;
            }

            if (request.Description != null)
            {
                category.Description = request.Description.Trim();
            }

            if (!string.IsNullOrWhiteSpace(request.Icon))
            {
                category.Icon = request.Icon.Trim();
            }

            await _context.SaveChangesAsync();

            var count = await _context.SystemSettings.CountAsync(s => s.Category.ToLower() == category.Name.ToLower());

            return new SettingCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Icon = category.Icon,
                CreatedAt = category.CreatedAt,
                CreatedBy = category.CreatedBy,
                DeletedFlag = category.DeletedFlag,
                SettingsCount = count
            };
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.SettingCategories.FirstOrDefaultAsync(c => c.Id == id && c.DeletedFlag == 1);
            if (category == null) return false;

            var protectedCategories = new[] { "general", "security", "notifications", "rbac", "sessions" };
            if (protectedCategories.Contains(category.Name.ToLower()))
            {
                throw new BadRequestException($"Category '{category.Name}' is a core system category and cannot be deleted.");
            }

            category.DeletedFlag = 0;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateSettingsBulkAsync(UpdateSettingsBulkRequest request, string callerName)
        {
            foreach (var kvp in request.Settings)
            {
                var setting = await _context.SystemSettings.FirstOrDefaultAsync(s => s.SettingKey == kvp.Key);
                if (setting != null)
                {
                    setting.SettingValue = kvp.Value;
                    setting.UpdatedAt = DateTime.UtcNow;
                    setting.UpdatedBy = callerName;
                }
                else
                {
                    _context.SystemSettings.Add(new SystemSetting
                    {
                        SettingKey = kvp.Key,
                        SettingValue = kvp.Value,
                        Category = "General",
                        Description = kvp.Key,
                        DataType = "string",
                        UpdatedAt = DateTime.UtcNow,
                        UpdatedBy = callerName
                    });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<SystemSetting> CreateSettingAsync(CreateSettingRequest request, string callerName)
        {
            if (string.IsNullOrWhiteSpace(request.SettingKey))
            {
                throw new BadRequestException("Setting key is required.");
            }

            var existing = await _context.SystemSettings.FirstOrDefaultAsync(s => s.SettingKey == request.SettingKey.Trim());
            if (existing != null)
            {
                throw new BadRequestException($"Setting with key '{request.SettingKey}' already exists.");
            }

            var categoryName = string.IsNullOrWhiteSpace(request.Category) ? "General" : request.Category.Trim();

            var catExists = await _context.SettingCategories.AnyAsync(c => c.DeletedFlag == 1 && c.Name.ToLower() == categoryName.ToLower());
            if (!catExists)
            {
                _context.SettingCategories.Add(new SettingCategory
                {
                    Name = categoryName,
                    Description = $"{categoryName} workspace settings",
                    Icon = "Tune",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = callerName,
                    DeletedFlag = 1
                });
            }

            var setting = new SystemSetting
            {
                SettingKey = request.SettingKey.Trim(),
                SettingValue = request.SettingValue.Trim(),
                Category = categoryName,
                Description = request.Description.Trim(),
                DataType = string.IsNullOrWhiteSpace(request.DataType) ? "string" : request.DataType.Trim(),
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = callerName
            };

            _context.SystemSettings.Add(setting);
            await _context.SaveChangesAsync();
            return setting;
        }

        public async Task<SystemSetting?> UpdateSettingAsync(int id, UpdateSettingRequest request, string callerName)
        {
            var setting = await _context.SystemSettings.FirstOrDefaultAsync(s => s.Id == id);
            if (setting == null) return null;

            setting.SettingKey = string.IsNullOrWhiteSpace(request.SettingKey) ? setting.SettingKey : request.SettingKey.Trim();
            setting.SettingValue = request.SettingValue ?? string.Empty;
            setting.Category = string.IsNullOrWhiteSpace(request.Category) ? setting.Category : request.Category.Trim();
            setting.Description = request.Description ?? setting.Description;
            setting.DataType = string.IsNullOrWhiteSpace(request.DataType) ? setting.DataType : request.DataType.Trim();
            setting.UpdatedAt = DateTime.UtcNow;
            setting.UpdatedBy = callerName;

            await _context.SaveChangesAsync();
            return setting;
        }

        public async Task<bool> DeleteSettingAsync(int id)
        {
            var setting = await _context.SystemSettings.FirstOrDefaultAsync(s => s.Id == id);
            if (setting == null) return false;

            _context.SystemSettings.Remove(setting);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

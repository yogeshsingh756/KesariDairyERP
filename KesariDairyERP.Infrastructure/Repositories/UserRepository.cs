using KesariDairyERP.Application.DTOs.Users;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;
using KesariDairyERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KesariDairyERP.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetUserWithRoleAsync(string username)
        {
            return await _db.Users
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u =>
                    u.Username == username &&
                    u.IsActive &&
                    !u.IsDeleted);
        }
        public async Task<(List<User> Users, int TotalCount)> GetPagedAsync(
     int pageNumber,
     int pageSize,
     string? search)
        {
            var query = _db.Users
                .Include(u => u.UserRole)
                .ThenInclude(ur => ur.Role)
                .Where(u => !u.IsDeleted);

            // SEARCH
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();

                query = query.Where(u =>
                    u.FullName.ToLower().Contains(search) ||
                    u.Username.ToLower().Contains(search) ||
                    u.Email.ToLower().Contains(search) ||
                    u.UserRole.Role.RoleName.ToLower().Contains(search) ||
                    u.MobileNumber.ToLower().Contains(search)
                );
            }

            var total = await query.CountAsync();

            var users = await query
                .OrderByDescending(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, total);
        }

        public async Task CreateUserWithRoleAsync(User user, long roleId)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            _db.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = roleId
            });

            await _db.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(UpdateUserDto dto)
        {
            var user = await _db.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Id == dto.Id && !u.IsDeleted);

            if (user == null)
                throw new Exception("User not found");

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.MobileNumber = dto.MobileNumber;
            user.Gender = dto.Gender;
            user.Address = dto.Address;
            user.IsActive = dto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            if (user.UserRole.RoleId != dto.RoleId)
            {
                user.UserRole.RoleId = dto.RoleId;
            }

            await _db.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(long userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            user.IsDeleted = true;
            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
        public async Task<User?> GetByIdAsync(long id)
        {
            return await _db.Users
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }
    }
}

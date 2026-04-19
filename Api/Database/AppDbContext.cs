using Api.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Database;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<TargetSymbol> TargetSymbols => Set<TargetSymbol>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
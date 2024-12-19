using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NotifyMe.Persistence;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext(options);
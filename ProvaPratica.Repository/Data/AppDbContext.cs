using Microsoft.EntityFrameworkCore;
using System;
using ProvaPratica.Domain.Models;
using ProvaPratica.Repository.Mappings;
using Microsoft.Extensions.Configuration;


namespace ProvaPratica.Repository.Data
{
    public partial class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            _connectionString = GetConnectionString(configuration);
        }

        private string GetConnectionString(IConfiguration configuration)
        {
            var defaultConnection = configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrWhiteSpace(defaultConnection))
                return defaultConnection;

            var envConnection = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");
            if (!string.IsNullOrWhiteSpace(envConnection))
                return envConnection;

            throw new Exception("Não há ConnectionString.");
        }

        public DbSet<UsuarioModel> Usuario { get; set; }
        public DbSet<PostModel> Post { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMapping());
            modelBuilder.ApplyConfiguration(new PostMapping());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString, options =>
            {
                options.MigrationsHistoryTable("__EFMigrationsHistoryProvaPratica");
            });
        }

        public override int SaveChanges()
        {
            Timestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            Timestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void Timestamps()
        {
            ChangeTracker.DetectChanges();
            var entries = ChangeTracker.Entries<BaseModel>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var now = DateTime.Now;
            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.CriadoEm = now;
                }
                else
                {
                    entry.Property(nameof(BaseModel.CriadoEm)).IsModified = false;
                }
                entity.AtualizadoEm = now;
            }
        }
    }
}

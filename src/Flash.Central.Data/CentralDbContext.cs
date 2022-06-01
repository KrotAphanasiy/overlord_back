using DigitalSkynet.DotnetCore.DataAccess.StaticData;
using Flash.Central.Data.StaticData;
using Flash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Flash.Central.Foundation.Base.Entities;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;

namespace Flash.Central.Data
{
    /// <summary>
    /// Class. Inherits DbContext
    /// </summary>
    public class CentralDbContext : DbContext, IDataProtectionKeyContext
    {
        public CentralDbContext(DbContextOptions<CentralDbContext> options)
            : base(options)
        { }
        /// <summary>
        /// Configures the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Camera>()
                .HasIndex(x => x.IsDeleted);
            modelBuilder.Entity<Camera>() // TODO: for debug only, to remove before prod
                .HasDataProvider<CameraDataProvider, Camera>();

            modelBuilder.Entity<CameraRegion>()
                .HasIndex(x => x.IsDeleted);

            modelBuilder.Entity<CameraRegion>()
                .HasOne(x => x.WatchingTerminal);

            modelBuilder.Entity<Terminal>() // TODO: for debug only, to remove before prod
               .HasDataProvider<TerminalDataProvider, Terminal>();

            modelBuilder.Entity<CameraRegion>() // TODO: for debug only, to remove before prod
                .HasDataProvider<CameraRegionDataProvider, CameraRegion>();

            modelBuilder.Entity<GasStation>()
                .HasIndex(x => x.IsDeleted);
            modelBuilder.Entity<GasStation>()
                .HasIndex(x => x.Name);
            modelBuilder.Entity<GasStation>() // TODO: for debug only, to remove before prod
                .HasDataProvider<GasStationDataProvider, GasStation>();

            modelBuilder.Entity<RecognitionEvent>()
                .HasIndex(x => x.IsDeleted);
            modelBuilder.Entity<RecognitionEvent>()
                .HasIndex(x => x.Timestamp);

            modelBuilder.Entity<DetectionEvent>()
                .HasIndex(x => x.IsDeleted);
            modelBuilder.Entity<DetectionEvent>()
                .HasIndex(x => x.Timestamp);

            modelBuilder.Entity<Visit>()
                .HasIndex(x => x.IsDeleted);
            modelBuilder.Entity<Visit>()
                .HasIndex(x => x.PlateNumber);
            modelBuilder.Entity<Visit>()
                .HasIndex(x => x.Start);
        }

        #region Modification Tracking

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <summary> Track entites with modification dates </summary>
        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity baseEntity)
                {
                    var now = DateTime.UtcNow;

                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            baseEntity.UpdatedDate = now;
                            break;

                        case EntityState.Added:
                            baseEntity.CreatedDate = now;
                            baseEntity.UpdatedDate = now;
                            break;
                    }
                }
            }
        }

        #endregion

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }
}

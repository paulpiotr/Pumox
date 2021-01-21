using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pumox.Core.Database.Data.EntityTypeConfiguration;
using Pumox.Core.Models;

namespace Pumox.Core.Database.Data
{
    public class PumoxCoreDatabaseContext : DbContext
    {
        #region private readonly log4net.ILog _log4net
        /// <summary>
        /// private readonly log4net.ILog _log4net
        /// private readonly log4net.ILog _log4net
        /// </summary>
        private readonly log4net.ILog _log4net = Log4netLogger.Log4netLogger.GetLog4netInstance(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region private static readonly AppSettings appSettings...
        /// <summary>
        /// Instancja do klasy modelu ustawień jako AppSettings
        /// Instance to the settings model class as AppSettings
        /// </summary>
        private readonly Models.AppSettings _appSettings = new Models.AppSettings();
        #endregion

        #region private static readonly MemoryCacheEntryOptions memoryCacheEntryOptions
        /// <summary>
        /// Opcje wpisu pamięci podręcznej
        /// Memory Cache Entry Options
        /// </summary>
        private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
        #endregion

        //#region public PumoxCoreDatabaseContext()
        ///// <summary>
        ///// Konstruktor Klasy kontekstu bazy danych
        ///// Constructor Database Context Classes
        ///// </summary>
        //public PumoxCoreDatabaseContext()
        //{
        //    CheckAndMigrate();
        //}
        //#endregion

        #region public virtual DbSet<Company> Company
        /// <summary>
        /// Model danych Pumox.Core.Models.Company
        /// Pumox.Core.Models.Company data model
        /// </summary>
        public virtual DbSet<Company> Company { get; set; }
        #endregion

        #region public virtual DbSet<Employee> Employee
        /// <summary>
        /// Model danych Pumox.Core.Models.Employee
        /// Pumox.Core.Models.Employee data model
        /// </summary>
        public virtual DbSet<Employee> Employee { get; set; }
        #endregion

        #region public PumoxCoreDatabaseContext(DbContextOptions<PumoxCoreDatabaseContext> options)
        /// <summary>
        /// Konstruktor klasy kontekstu bazy danych api wykazu podatników VAT
        /// Constructor database context classes api list of VAT taxpayers
        /// </summary>
        /// <param name="options">
        /// Opcje połączenia da bazy danych options AS DbContextOptions<PumoxCoreDatabaseContext>
        /// Connection options will give the options AS DbContextOptions<PumoxCoreDatabaseContext>
        /// </param>
        public PumoxCoreDatabaseContext(DbContextOptions<PumoxCoreDatabaseContext> options)
            : base(options)
        {
            CheckAndMigrate();
        }
        #endregion

        #region protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        /// <summary>
        /// Zdarzenie wyzwalające konfigurację bazy danych
        /// Database configuration triggering event
        /// </summary>
        /// <param name="optionsBuilder">
        /// Fabryka budowania połączenia do bazy danych optionsBuilder jako DbContextOptionsBuilder
        /// Factory building connection to the database optionsBuilder AS DbContextOptionsBuilder
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                //#if DEBUG
                //                ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
                //                {
                //                    builder.AddFilter(level => level == LogLevel.Debug).AddConsole();
                //                });
                //                optionsBuilder.UseLoggerFactory(loggerFactory);
                //#endif
                if (!optionsBuilder.IsConfigured)
                {
                    //Console.WriteLine($"\n\n\n { optionsBuilder.Options.ContextType } { _appSettings.GetConnectionString() } \n\n\n");
                    optionsBuilder.UseSqlServer(_appSettings.GetConnectionString());
                }
                CheckAndMigrate();
            }
            catch (Exception e)
            {
                _log4net.Error(string.Format("\n{0}\n{1}\n{2}\n{3}\n", e.GetType(), e.InnerException?.GetType(), e.Message, e.StackTrace), e);
            }
        }
        #endregion

        #region public void CheckAndMigrate()
        /// <summary>
        /// Sprawdź ostatnią datę migracji bazy danych lub wymuś wykonanie, jeśli opcja CheckAndMigrate jest zaznaczona i wykonaj migrację bazy danych.
        /// Check the latest database migration date or force execution if CheckAndMigrate is selected and perform database migration.
        /// </summary>
        public void CheckAndMigrate()
        {
            Task.Run(async () =>
            {
                await CheckAndMigrateAsync();
            }).Wait();
        }
        #endregion

        #region public async Task CheckAndMigrateAsync()
        /// <summary>
        /// Sprawdź ostatnią datę migracji bazy danych lub wymuś wykonanie, jeśli opcja CheckAndMigrate jest zaznaczona i wykonaj migrację bazy danych.
        /// Check the latest database migration date or force execution if CheckAndMigrate is selected and perform database migration.
        /// </summary>
        /// <returns>
        /// async Task
        /// async Task
        /// </returns>
        public async Task CheckAndMigrateAsync()
        {
            DateTime? lastMigrateDateTime = null;
            try
            {
                lastMigrateDateTime = await _appSettings.AppSettingsRepository.GetValueAsync<DateTime>(_appSettings, "LastMigrateDateTime");
                var isCheckAndMigrate = await _appSettings.AppSettingsRepository.GetValueAsync<bool>(_appSettings, "CheckAndMigrate");
                var dateTimeDiffDays = (DateTime.Now - (DateTime)lastMigrateDateTime).Days;
                if ((isCheckAndMigrate || dateTimeDiffDays >= 1) && (await Database.GetPendingMigrationsAsync()).Any())
                {
                    try
                    {
#if DEBUG
                        _log4net.Debug($"Try Check And Migrate Async...");
#endif
                        await NetAppCommon.Helpers.EntityContextHelper.RunMigrationAsync<PumoxCoreDatabaseContext>(this);
#if DEBUG
                        _log4net.Debug($"Ok");
#endif
                    }
                    catch (Exception e)
                    {
                        _log4net.Warn(string.Format("\n{0}\n{1}\n{2}\n{3}\n", e.GetType(), e.InnerException?.GetType(), e.Message, e.StackTrace), e);
                    }
                    _appSettings.LastMigrateDateTime = DateTime.Now;
                    await _appSettings.AppSettingsRepository.MergeAndSaveAsync(_appSettings);
                }
            }
            catch (Exception e)
            {
                _log4net.Warn(string.Format("\n{0}\n{1}\n{2}\n{3}\n", e.GetType(), e.InnerException?.GetType(), e.Message, e.StackTrace), e);
            }
            finally
            {
                if (null == lastMigrateDateTime || lastMigrateDateTime == DateTime.MinValue)
                {
                    _appSettings.LastMigrateDateTime = DateTime.Now;
                    await _appSettings.AppSettingsRepository.MergeAndSaveAsync(_appSettings);
                }
            }
        }
        #endregion

        #region protected override void OnModelCreating(ModelBuilder modelBuilder)
        /// <summary>
        /// Zdarzenie wyzwalające tworzenie modelu bazy danych
        /// The event that triggers the creation of the database model
        /// </summary>
        /// <param name="modelBuilder">
        /// Fabryka budowania modelu bazy danych modelBuilder jako ModelBuilder
        /// ModelBuilder database model building as ModelBuilder
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }
        #endregion
    }
}

using System;
using System.IO;
using System.Reflection;
using NetAppCommon.AppSettings.Models;
using NetAppCommon.AppSettings.Models.Base;

namespace Pumox.Core.Database.Models
{
    public partial class AppSettings : AppSettingsBaseModel
    {
        ///Important !!!
        #region AppSettingsModel()
        public AppSettings()
        {
            try
            {
                var memoryCacheProvider = NetAppCommon.Helpers.Cache.MemoryCacheProvider.GetInstance();
                var filePathKey = string.Format("{0}{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, ".FilePath");
                var filePath = memoryCacheProvider.Get(filePathKey);
                if (null == filePath)
                {
                    AppSettingsRepository.MergeAndCopyToUserDirectory(this);
                    memoryCacheProvider.Put(filePathKey, FilePath, TimeSpan.FromDays(1));
                }
                if (null != UserProfileDirectory && null != FileName)
                {
                    FilePath = (string)(filePath ?? Path.Combine(UserProfileDirectory, FileName));
                }
                var useGlobalDatabaseConnectionSettingsKey = string.Format("{0}{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, ".UseGlobalDatabaseConnectionSettings");
                var useGlobalDatabaseConnectionSettings = memoryCacheProvider.Get(useGlobalDatabaseConnectionSettingsKey);
                if (null == useGlobalDatabaseConnectionSettings)
                {
                    memoryCacheProvider.Put(useGlobalDatabaseConnectionSettingsKey, UseGlobalDatabaseConnectionSettings, TimeSpan.FromDays(1));
                    if (UseGlobalDatabaseConnectionSettings)
                    {
                        var appSettingsModel = new AppSettingsModel();
                        ConnectionString = appSettingsModel.ConnectionString;
                        AppSettingsRepository.MergeAndSave(this);
                    }
                }
            }
            catch (Exception e)
            {
                _log4net.Error(string.Format("\n{0}\n{1}\n{2}\n{3}\n", e.GetType(), e.InnerException?.GetType(), e.Message, e.StackTrace), e);
            }
        }
        #endregion

        ///Important !!!
        #region public static AppSettings GetAppSettings()
        /// <summary>
        /// Pobierz statyczną referencję do instancji AppSettingsBaseModel
        /// Get a static reference to the AppSettingsBaseModel instance
        /// </summary>
        /// <returns>
        /// Statyczna referencja do instancji AppSettingsBaseModel
        /// A static reference to the AppSettingsBaseModel instance
        /// </returns>
        public static AppSettings GetAppSettings()
        {
            return new AppSettings();
        }
        #endregion

        #region private readonly log4net.ILog log4net
        /// <summary>
        /// Instancja do klasy Log4netLogger
        /// Instance to Log4netLogger class
        /// </summary>
        private readonly log4net.ILog _log4net = Log4netLogger.Log4netLogger.GetLog4netInstance(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region protected new string _fileName = FILENAME;
#if DEBUG
        protected const string FILENAME = "pumox.core.database.json";
#else
        protected const string FILENAME = "pumox.core.database.json";
#endif

        protected new string _fileName = FILENAME;

        public override string FileName
        {
            get => _fileName;
            protected set
            {
                if (value != _fileName)
                {
                    _fileName = value;
                    OnPropertyChanged("FileName");
                }
            }
        }
        #endregion

        #region protected new string _connectionStringName = CONNECTIONSTRINGNAME;
#if DEBUG
        protected const string CONNECTIONSTRINGNAME = "PumoxCoreDatabaseContext";
#else
        protected const string CONNECTIONSTRINGNAME = "PumoxCoreDatabaseContext";
#endif

        protected new string _connectionStringName = CONNECTIONSTRINGNAME;

        public override string ConnectionStringName
        {
            get => _connectionStringName;
            set
            {
                if (value != _connectionStringName)
                {
                    _connectionStringName = value;
                }
            }
        }
        #endregion
    }
}

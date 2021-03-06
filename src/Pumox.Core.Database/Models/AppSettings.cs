#region using

using System;
using System.IO;
using System.Reflection;
using log4net;
using NetAppCommon.AppSettings.Models;
using NetAppCommon.AppSettings.Models.Base;
using NetAppCommon.Helpers.Cache;

#endregion

namespace Pumox.Core.Database.Models
{
    public class AppSettings : AppSettingsBaseModel
    {
        ///Important !!!

        #region AppSettingsModel()

        public AppSettings()
        {
            try
            {
                var memoryCacheProvider = MemoryCacheProvider.GetInstance();
                var filePathKey = string.Format("{0}{1}", MethodBase.GetCurrentMethod()?.DeclaringType.FullName,
                    ".FilePath");
                var filePath = memoryCacheProvider.Get(filePathKey);
                if (null == filePath)
                {
#if DEBUG
                    try
                    {
                        var removeFilePath = Path.Combine(UserProfileDirectory, FileName);
                        if (File.Exists(removeFilePath))
                        {
                            File.Delete(removeFilePath);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
#endif
                    AppSettingsRepository.MergeAndCopyToUserDirectory(this);
                    memoryCacheProvider.Put(filePathKey, FilePath, TimeSpan.FromDays(1));
                }

                if (null != UserProfileDirectory && null != FileName)
                {
                    FilePath = (string)(filePath ?? Path.Combine(UserProfileDirectory, FileName));
                }

                var useGlobalDatabaseConnectionSettingsKey = string.Format("{0}{1}",
                    MethodBase.GetCurrentMethod()?.DeclaringType.FullName, ".UseGlobalDatabaseConnectionSettings");
                var useGlobalDatabaseConnectionSettings =
                    memoryCacheProvider.Get(useGlobalDatabaseConnectionSettingsKey);
                if (null == useGlobalDatabaseConnectionSettings)
                {
                    memoryCacheProvider.Put(useGlobalDatabaseConnectionSettingsKey, UseGlobalDatabaseConnectionSettings,
                        TimeSpan.FromDays(1));
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
                _log4Net.Error(
                    string.Format("\n{0}\n{1}\n{2}\n{3}\n", e.GetType(), e.InnerException?.GetType(), e.Message,
                        e.StackTrace), e);
            }
        }

        #endregion

        ///Important !!!

        #region public static AppSettings GetAppSettings()

        /// <summary>
        ///     Pobierz statyczną referencję do instancji AppSettingsBaseModel
        ///     Get a static reference to the AppSettingsBaseModel instance
        /// </summary>
        /// <returns>
        ///     Statyczna referencja do instancji AppSettingsBaseModel
        ///     A static reference to the AppSettingsBaseModel instance
        /// </returns>
        public static AppSettings GetAppSettings() => new();

        #endregion

        #region private readonly ILog _log4Net

        /// <summary>
        ///     Instancja do klasy Log4netLogger
        ///     Instance to Log4netLogger class
        /// </summary>
        private readonly ILog _log4Net =
            Log4netLogger.Log4netLogger.GetLog4netInstance(MethodBase.GetCurrentMethod()?.DeclaringType);

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

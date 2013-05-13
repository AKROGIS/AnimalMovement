using System;
using System.Globalization;
using System.Linq;
using System.Diagnostics;

namespace DataModel
{
    public static class Settings
    {
        private const string SystemUsername = "system";
        // When adding new persistent settings, remember to modify the dbo.Settings_Update stored procedure
        private const string ProjectKey = "project";
        private const string FilterKey = "filter_projects";
        private const string SpeciesKey = "species";
        private const string FormatKey = "file_format";
        private const string ParameterFormatKey = "parameter_file_format";
        private const string ModelKey = "collar_model";
        private const string ManufacturerKey = "collar_manufacturer";
        private const string WantsEmailKey = "wants_email";
        private const string IgnoreCtnSuffixKey = "ignore_ctn_suffix";

        private const string SystemEmailKey = "sa_email";
        private const string SystemEmailPasswordKey = "sa_email_password";
        private const string SystemDbaContact = "dba_contact";

        #region getters

        public static string GetSystemDbaContact()
        {
            return GetSystemDefault(SystemDbaContact);
        }

        public static string GetSystemEmail()
        {
            return GetSystemDefault(SystemEmailKey);
        }

        public static string GetSystemEmailPassword()
        {
            return GetSystemDefault(SystemEmailPasswordKey);
        }

        public static string GetSystemDefault(string key)
        {
            var db = new SettingsDataContext();
            Setting setting = db.Settings.FirstOrDefault(s => s.Username == SystemUsername && s.Key == key);
            return setting == null ? null : setting.Value;
        }

        public static string GetDefaultProject()
        {
            return GetUsersDefault(ProjectKey);
        }

        public static bool GetDefaultProjectFilter()
        {
            return GetUsersDefault(FilterKey) == true.ToString(CultureInfo.InvariantCulture);
        }

        public static string GetDefaultSpecies()
        {
            return GetUsersDefault(SpeciesKey);
        }

        public static char? GetDefaultFileFormat()
        {
            string format = GetUsersDefault(FormatKey);
            return format == null ? (char?)null : format[0];
        }

        public static char? GetDefaultParameterFileFormat()
        {
            string format = GetUsersDefault(ParameterFormatKey);
            return format == null ? (char?)null : format[0];
        }

        public static string GetDefaultCollarModel(string manufacturer)
        {
            return GetUsersDefault(ModelKey + "_" + manufacturer);
        }

        public static string GetDefaultCollarManufacturer()
        {
            return GetUsersDefault(ManufacturerKey);
        }

        public static bool GetWantsEmail()
        {
            //default is to get email, unless explicitly and correctly denied
            string setting = GetUsersDefault(WantsEmailKey);
            return (setting == null || setting != false.ToString(CultureInfo.InvariantCulture));
        }

        public static bool GetIgnoreCtnSuffix()
        {
            //default is to get email, unless explicitly and correctly denied
            string setting = GetUsersDefault(IgnoreCtnSuffixKey);
            return (setting == null || setting != false.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region setters

        public static void SetDefaultProject(string project)
        {
            SetUsersDefault(ProjectKey, project);
        }

        public static void SetDefaultProjectFilter(bool filter)
        {
            SetUsersDefault(FilterKey, filter.ToString(CultureInfo.InvariantCulture));
        }

        public static void SetDefaultSpecies(string species)
        {
            SetUsersDefault(SpeciesKey, species);
        }

        public static void SetDefaultFileFormat(char format)
        {
            SetUsersDefault(FormatKey, format.ToString(CultureInfo.InvariantCulture));
        }

        public static void SetDefaultParameterFileFormat(char format)
        {
            SetUsersDefault(ParameterFormatKey, format.ToString(CultureInfo.InvariantCulture));
        }

        public static void SetDefaultCollarModel(string manufacturer, string model)
        {
            SetUsersDefault(ModelKey + "_" + manufacturer, model);
        }

        public static void SetDefaultCollarManufacturer(string manufacturer)
        {
            SetUsersDefault(ManufacturerKey, manufacturer);
        }

        public static void SetWantsEmail(bool wantsEmail)
        {
            SetUsersDefault(WantsEmailKey, wantsEmail.ToString(CultureInfo.InvariantCulture));
        }

        public static void SetIgnoreCtnSuffix(bool ignoreCtnSuffix)
        {
            SetUsersDefault(IgnoreCtnSuffixKey, ignoreCtnSuffix.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region Getting/Setting implementation

        private static string GetUsersDefault(string key)
        {
            var db = new SettingsDataContext();
            var user = GetUserName();
            Setting setting = db.Settings.FirstOrDefault(s => s.Username == user && s.Key == key);
            return setting == null ? null : setting.Value;
        }

        private static void SetUsersDefault(string key, string value)
        {
            var db = new SettingsDataContext();
            try
            {
                db.Settings_Update(key, value);
            }
            catch (Exception ex)
            {
                Debug.Print("Unable to save settings (u:{0},k:{1},v:{2}, exception:{3}", GetUserName(), key, value,
                            ex.Message);
            }
        }

        private static string GetUserName()
        {
            return Environment.UserDomainName + "\\" + Environment.UserName;
        }

        #endregion


        public static bool PiWantsEmails(string address)
        {
            var database = new AnimalMovementDataContext();
            var pi = database.ProjectInvestigators.FirstOrDefault(p => p.Email == address);
            if (pi == null)
                return false;
            var db = new SettingsDataContext();
            var setting = db.Settings.FirstOrDefault(s => s.Username == pi.Login && s.Key == WantsEmailKey);
            //default is to get email, unless explicitly and correctly denied
            return (setting == null || setting.Value != false.ToString(CultureInfo.InvariantCulture));
        }
    }
}

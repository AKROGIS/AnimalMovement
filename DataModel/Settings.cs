using System;
using System.Globalization;
using System.Linq;
using System.Diagnostics;

namespace DataModel
{
    public static class Settings
    {
        // When adding new persistent settings, remember to modify the dbo.Settings_Update stored procedure
        private const string ProjectKey = "project";
        private const string FilterKey = "filter_projects";
        private const string SpeciesKey = "species";
        private const string FormatKey = "file_format";
        private const string ParameterFormatKey = "parameter_file_format";
        private const string ModelKey = "collar_model";
        private const string ManufacturerKey = "collar_manufacturer";

        #region getters

        public static string GetSystemDefault(string key)
        {
            var db = new SettingsDataContext();
            Setting setting = db.Settings.FirstOrDefault(s => s.Username == "system" && s.Key == key);
            return setting == null ? null : setting.Value;
        }

        public static string GetDefaultProject()
        {
            return GetUsersDefault(ProjectKey);
        }

        public static bool GetDefaultProjectFilter()
        {
            return GetUsersDefault(FilterKey) == "True";
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

        public static string GetDefaultCollarModel()
        {
            return GetUsersDefault(ModelKey);
        }

        public static string GetDefaultCollarManufacturer()
        {
            return GetUsersDefault(ManufacturerKey);
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

        public static void SetDefaultCollarModel(string model)
        {
            SetUsersDefault(ModelKey, model);
        }

        public static void SetDefaultCollarManufacturer(string manufacturer)
        {
            SetUsersDefault(ManufacturerKey, manufacturer);
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
    }
}

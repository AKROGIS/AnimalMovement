using System;
using System.Globalization;
using System.Linq;
using System.Diagnostics;
using DataModel;

namespace AnimalMovement
{
    internal static class Settings
    {
        private const string ProjectKey = "project";
        private const string FilterKey = "filter_projects";
        private const string SpeciesKey = "species";
        private const string FormatKey = "file_format";
        private const string ParameterFormatKey = "parameter_file_format";
        private const string ModelKey = "collar_model";
        private const string ManufacturerKey = "collar_manufacturer";

        #region getters

        internal static string GetDefaultProject()
        {
            return GetUsersDefault(ProjectKey);
        }

        internal static bool GetDefaultProjectFilter()
        {
            return GetUsersDefault(FilterKey) == "True";
        }

        internal static string GetDefaultSpecies()
        {
            return GetUsersDefault(SpeciesKey);
        }

        internal static char? GetDefaultFileFormat()
        {
            string format = GetUsersDefault(FormatKey);
            return format == null ? (char?)null : format[0];
        }

        internal static char? GetDefaultParameterFileFormat()
        {
            string format = GetUsersDefault(ParameterFormatKey);
            return format == null ? (char?)null : format[0];
        }

        internal static string GetDefaultCollarModel()
        {
            return GetUsersDefault(ModelKey);
        }

        internal static string GetDefaultCollarManufacturer()
        {
            return GetUsersDefault(ManufacturerKey);
        }

        #endregion

        #region setters

        internal static void SetDefaultProject(string project)
        {
            SetUsersDefault(ProjectKey, project);
        }

        internal static void SetDefaultProjectFilter(bool filter)
        {
            SetUsersDefault(FilterKey, filter.ToString(CultureInfo.InvariantCulture));
        }

        internal static void SetDefaultSpecies(string species)
        {
            SetUsersDefault(SpeciesKey, species);
        }

        internal static void SetDefaultFileFormat(char format)
        {
            SetUsersDefault(FormatKey, format.ToString(CultureInfo.InvariantCulture));
        }

        internal static void SetDefaultParameterFileFormat(char format)
        {
            SetUsersDefault(ParameterFormatKey, format.ToString(CultureInfo.InvariantCulture));
        }

        internal static void SetDefaultCollarModel(string model)
        {
            SetUsersDefault(ModelKey, model);
        }

        internal static void SetDefaultCollarManufacturer(string manufacturer)
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

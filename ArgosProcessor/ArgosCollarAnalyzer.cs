using System;
using System.Collections.Generic;
using System.Linq;
using Telonics;
using DataModel;

namespace ArgosProcessor
{
    internal class ArgosCollarAnalyzer
    {
        #region Private Fields

        //I have this long collection of private fields to cache results from all methods.
        //various properties and methods use or report these values.  Since this object is
        //immutable, these fields ensure that I only do each database query once.
        private readonly HashSet<string> _filePlatforms;
        private IEnumerable<string> _unknownPlatforms;
        private IEnumerable<string> _ambiguousPlatforms;
        private Dictionary<Collar, string> _collarsWithProblems;
        private Dictionary<string, Collar> _argosCollars;
        private Dictionary<string, List<Collar>> _sharedArgosCollars;
        private HashSet<Collar> _allUnambiguousCollars;
        private IEnumerable<Collar> _validCollars;
        private IEnumerable<Collar> _unambiguousSharedCollars;
        private Dictionary<string, IProcessor> _processors;
        private Dictionary<string, List<Byte[]>> _tpfFiles;
        private Dictionary<string, TimeSpan> _gen3periods;
        //FIXME - Should some of these IEnumerables be fixed types to avoid multiple delayed executions

        #endregion

        #region Public API

        public ArgosCollarAnalyzer(ArgosFile file, AnimalMovementDataContext database)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            if (database == null)
                throw new ArgumentNullException("database");
            File = file;
            Database = database;

            _filePlatforms = new HashSet<string>(File.GetPlatforms());
        }


        public ArgosFile File { get; private set; }


        public AnimalMovementDataContext Database { get; private set; }


        public IEnumerable<string> UnknownPlatforms
        {
            get { return _unknownPlatforms ?? (_unknownPlatforms = GetUnknownPlatforms()); }
        }


        public IEnumerable<string> AmbiguousPlatforms
        {
            get { return _ambiguousPlatforms ?? (_ambiguousPlatforms = GetAmbiguousPlatforms()); }
        }


        public Dictionary<Collar, string> CollarsWithProblems
        {
            get { return _collarsWithProblems ?? (_collarsWithProblems = GetCollarsWithProblems()); }
        }


        public IEnumerable<Collar> ValidCollars
        {
            get { return _validCollars ?? (_validCollars = GetValidCollars()); }
        }


        public Func<string, DateTime, string> GetCollarSelector()
        {
            return GetCollarIdFromTransmission;
        }


        public string GetCollarIdFromTransmission(string argosId, DateTime transmissionTime)
        {
            if (String.IsNullOrEmpty(argosId))
                throw new ArgumentNullException("argosId", "No Argos Id provided when one was expected");

            if (_argosCollars == null)
                _argosCollars = GetCollarsWithoutSharedArgosId();
            if (_argosCollars.ContainsKey(argosId))
                return _argosCollars[argosId].CollarId;

            if (_sharedArgosCollars == null)
                _sharedArgosCollars = GetCollarsWithSharedArgosId();
            if (_sharedArgosCollars.ContainsKey(argosId))
            {
                var collar = _sharedArgosCollars[argosId].SelectWithDate(transmissionTime);
                if (collar != null)
                    return collar.CollarId;
                var msg1 = String.Format("Unable to select a unique collar for platform Id {0} on {1}" +
                                         "\n  potential collars are {2}." +
                                         "\n  To correct this problem, add a disposal date to one or more of these collars.",
                                         argosId, transmissionTime, String.Join(", ",
                                                                                _sharedArgosCollars[argosId].Select(
                                                                                    c => c.ToString())));
                throw new InvalidOperationException(msg1);
            }
            var msg2 = String.Format("Unable find a collar for platform Id {0}", argosId);
            throw new InvalidOperationException(msg2);
        }


        public Func<string, IProcessor> GetProcessorSelector()
        {
            return GetProcessorFromTelonicsId;
        }


        public IProcessor GetProcessorFromTelonicsId(string telonicId)
        {
            if (String.IsNullOrEmpty(telonicId))
                    throw new ArgumentNullException("telonicId", "No Telonic Id (CTN) provided when one was expected");

            //This will be called multiple times from the client, so cache the dictionary
            if (_processors == null)
                _processors = InitializeProcessors();
            if (!_processors.ContainsKey(telonicId))
                throw new ArgumentOutOfRangeException("telonicId", "Internal Error, No processor for Telonics Id "+ telonicId);
            return _processors[telonicId];
        }


        /// <summary>
        /// Get the code for the file format produced by the processor used by a collar
        /// </summary>
        /// <param name="collar">A unique record in the Collars database table </param>
        /// <returns>The primary key (Char) for a unique record in the database table LookupCollarFileFormats</returns>
        public char GetFileFormatForCollar(Collar collar)
        {
            if (collar == null)
                throw new ArgumentNullException("collar", "No collar provided when one was expected");

            // Expand this switch as additional models/file formats are supported 
            switch (collar.CollarModel)
            {
                case "TelonicsGen3":
                    return 'D';
                case "TelonicsGen4":
                    return 'C';
                default:
                    var msg = String.Format("Unsupported model ({0} for collar {1}", collar.CollarModel, collar.CollarId);
                    throw new ArgumentOutOfRangeException(msg);
            }
        }

        #endregion

        #region Private Methods

        private IEnumerable<string> GetUnknownPlatforms()
        {
            if (_unknownPlatforms == null)
            {
                var databasePlatforms = from collar in Database.Collars
                                        where collar.CollarManufacturer == "Telonics" && collar.AlternativeId != null
                                        select collar.AlternativeId;
                _unknownPlatforms = _filePlatforms.Except(databasePlatforms);
            }
            return _unknownPlatforms;
        }


        private IEnumerable<string> GetAmbiguousPlatforms()
        {
            if (_ambiguousPlatforms == null)
            {
                if (_sharedArgosCollars == null)
                    _sharedArgosCollars = GetCollarsWithSharedArgosId();
                if (_unambiguousSharedCollars == null)
                    _unambiguousSharedCollars = GetUnambiguousSharedCollars();
                var ambiguousCollars = _sharedArgosCollars.Values.SelectMany(c => c).Except(_unambiguousSharedCollars);
                _ambiguousPlatforms = ambiguousCollars.Select(c => c.AlternativeId);
            }
            return _ambiguousPlatforms;
        }


        private Dictionary<Collar, string> GetCollarsWithProblems()
        {
            if (_collarsWithProblems == null)
            {
                if (_allUnambiguousCollars == null)
                    _allUnambiguousCollars = GetAllUnambiguousCollars();
                _collarsWithProblems = GetCollarsWithProblems(_allUnambiguousCollars);
            }
            return _collarsWithProblems;
        }


        private IEnumerable<Collar> GetValidCollars()
        {
            if (_validCollars == null)
            {
                if (_allUnambiguousCollars == null)
                    _allUnambiguousCollars = GetAllUnambiguousCollars();
                if (_collarsWithProblems == null)
                    _collarsWithProblems = GetCollarsWithProblems();
                _validCollars = _allUnambiguousCollars.Except(_collarsWithProblems.Select(e => e.Key));
            }
            return _validCollars;
        }


        //Get a dictionary yielding a list of Collars with the same Platform Id
        private Dictionary<string, List<Collar>> GetCollarsWithSharedArgosId()
        {
            var collarsByArgos = from collar in Database.Collars
                                 where collar.CollarManufacturer == "Telonics" && collar.AlternativeId != null && _filePlatforms.Contains(collar.AlternativeId)
                                 group collar by collar.AlternativeId;

            var groups = from grouping in collarsByArgos
                            where grouping.Count() > 1
                            select grouping;
            var results = new Dictionary<string, List<Collar>>();
            foreach (var group in groups)
                results[group.Key] = group.ToList();
            return results;
        }

        //FIXME - merge these two methods

        //Get a dictionary yielding a single collar for a platform Id
        private Dictionary<string, Collar> GetCollarsWithoutSharedArgosId()
        {
            var collarsByArgos = from collar in Database.Collars
                                 where collar.CollarManufacturer == "Telonics" && collar.AlternativeId != null && _filePlatforms.Contains(collar.AlternativeId)
                                 group collar by collar.AlternativeId;

            var singleton = from grouping in collarsByArgos
                           where grouping.Count() == 1 
                           select grouping;
            var results = new Dictionary<string, Collar>();
            foreach (var single in singleton)
                results[single.Key] = single.First();
            return results;
        }


        private HashSet<Collar> GetAllUnambiguousCollars()
        {
            if (_argosCollars == null)
                _argosCollars = GetCollarsWithoutSharedArgosId();
            if (_unambiguousSharedCollars == null)
                _unambiguousSharedCollars = GetUnambiguousSharedCollars();
            return new HashSet<Collar>(_argosCollars.Values.Concat(_unambiguousSharedCollars));
        }


        private IEnumerable<Collar> GetUnambiguousSharedCollars()
        {
            if (_sharedArgosCollars == null)
                _sharedArgosCollars = GetCollarsWithSharedArgosId();
            return _sharedArgosCollars.SelectMany(e =>
            {
                if (e.Value.IsAmbiguous()) return new List<Collar>();
                return e.Value;
            });
        }


        private Dictionary<string, IProcessor> InitializeProcessors()
        {
            var processors = new Dictionary<string, IProcessor>();

            if (_validCollars == null)
                _validCollars = GetValidCollars();
            foreach (var collar in _validCollars)
            {
                switch (collar.CollarModel)
                {
                    case "TelonicsGen3":
                        processors[collar.CollarId] = new Gen3Processor(Gen3period(collar.CollarId));
                        break;
                    case "TelonicsGen4":
                        processors[collar.CollarId] = new Gen4Processor(Gen4tpfFile(collar.CollarId));
                        break;
                    default:
                        var msg = String.Format("Unsupported model ({0} for collar {1}", collar.CollarModel, collar.CollarId);
                        throw new ArgumentOutOfRangeException(msg);
                }
            }
            return processors;
        }


        private Dictionary<Collar, string> GetCollarsWithProblems(IEnumerable<Collar> collars)
        {
            // known problems:
            //  1) not Telonics - reported as an unknown platform - not checked here
            //  2) Collar is not a known gen
            //  3) for Gen3: no delay, or an active PPF file.
            //  4) For Gen4: No tpf file or multiple tpf files

            var results = new Dictionary<Collar, string>();
            foreach (var collar in collars)
            {
                if (collar.CollarModel == "TelonicsGen3")
                {
                    if (_gen3periods == null)
                        _gen3periods = GetPeriods();
                    if (!_gen3periods.ContainsKey(collar.CollarId))
                        results[collar] = "Gen3 Collar does not have a period between fixes specified";
                    else if (HasGen3parameterFile(collar.CollarId))
                        results[collar] = 
                                 "Warning: PPF file for this collar is not used. Suggest deactiving the PPF file, or use Telonics ADC-T03 to process this collar";
                }
                else if (collar.CollarModel == "TelonicsGen4")
                {
                    if (_tpfFiles == null)
                        _tpfFiles = GetTpfFiles();
                    if (_tpfFiles.ContainsKey(collar.CollarId))
                    {
                        if (_tpfFiles[collar.CollarId].Count > 1)
                            results[collar] = "Gen4 Collar has multiple TPF files.  Suggest creating multiple collars one for each TPF files";
                    }
                    else
                        results[collar] = "Gen4 Collar does not have a TPF file";
                }
                else
                    results[collar] = "Model must be one of (TelonicsGen3, TelonicsGen4)";
            }
            return results;
        }


        private byte[] Gen4tpfFile(string telonicId)
        {
            if (_tpfFiles == null)
                _tpfFiles = GetTpfFiles();
            return _tpfFiles[telonicId].First();
        }

        private Dictionary<string, List<Byte[]>> GetTpfFiles()
        {
            if (_allUnambiguousCollars == null)
                _allUnambiguousCollars = GetAllUnambiguousCollars();
            var query = from parameter in Database.CollarParameters
                        //FIXME - add database field for parameter file status and check here
                        where parameter.CollarParameterFile.Format == 'A' && _allUnambiguousCollars.Contains(parameter.Collar)
                        group parameter.CollarParameterFile.Contents by parameter.Collar.CollarId;

            var results = new Dictionary<string, List<Byte[]>>();
            foreach (var item in query)
                results[item.Key] = item.Cast<Byte[]>().ToList();
            return results;
        }


        private TimeSpan Gen3period(string telonicId)
        {
            if (_gen3periods == null)
                _gen3periods = GetPeriods();
            return _gen3periods[telonicId];
        }

        private Dictionary<string, TimeSpan> GetPeriods()
        {
            if (_allUnambiguousCollars == null)
                _allUnambiguousCollars = GetAllUnambiguousCollars();
            var query = from collar in Database.Collars
                        where collar.CollarModel == "TelonicsGen3" && _allUnambiguousCollars.Contains(collar) && collar.DownloadInfo != null
                        select new {collar.CollarId, Minutes=collar.DownloadInfo};
            //FIXME - put new field in database for Gen 3 Period
            var results = new Dictionary<string, TimeSpan>();
            foreach (var item in query)
            {
                int minutes;
                if (Int32.TryParse(item.Minutes, out minutes))
                    results[item.CollarId] = TimeSpan.FromMinutes(minutes);
            }
            return results;
        }


        private bool HasGen3parameterFile(string telonicId)
        {
            if (_allUnambiguousCollars == null)
                _allUnambiguousCollars = GetAllUnambiguousCollars();
            var query = from parameter in Database.CollarParameters
                        //FIXME - add database field for parameter file status and check here
                        where parameter.CollarParameterFile.Format == 'B' && _allUnambiguousCollars.Contains(parameter.Collar)
                        select parameter.CollarId;
            return query.Contains(telonicId);
        }

        #endregion

    }


    public static class Extensions
    {
        public static Collar SelectWithDate(this List<Collar> collars, DateTime date)
        {
            //Find the collar with the closest future disposal date.
            //Null disposal date is valid and indicates disposal date is max date
            //Exception on duplicate closest disposal dates, or no future disposal date.
            if (collars == null || collars.Count == 0)
                throw new InvalidOperationException("Selecting from similar collars: Error - no collars have been provided");

            var pairs = (from collar in collars
                         let timeSpan =
                             collar.DisposalDate.HasValue ? collar.DisposalDate.Value - date : DateTime.MaxValue - date
                         where timeSpan >= TimeSpan.Zero
                         orderby timeSpan
                         select new {timeSpan, collar}).ToList();

            if (pairs.Count == 0)
            {
                var msg = String.Format("There are no undisposed collars with Argos Id {0} at time {1}",
                                        collars[0].AlternativeId, date);
                throw new InvalidOperationException(msg);
            }
            if (pairs.Count > 1 && pairs[0].timeSpan == pairs[1].timeSpan)
            {
                var msg = String.Format("Error: collar {0} and {1} (both with Argos Id {2}) have identical disposal dates ({3}).",
                                        pairs[0].collar, pairs[1].collar, pairs[0].collar.AlternativeId, pairs[0].collar.DisposalDate);
                throw new InvalidOperationException(msg);
            }
            return pairs[0].collar;
        }

        public static Boolean IsAmbiguous(this List<Collar> collars)
        {
            if (collars == null || collars.Count == 0)
                throw new  InvalidOperationException("Determining ambiguity of similar collars: Error - no collars have been provided");

            if (collars.Count == 1)
                return false;

            var uniqueCount = (from collar in collars
                               select collar.DisposalDate).Distinct().Count();

            //few dates than collars implies duplicate dates which implies ambiguity
            return uniqueCount < collars.Count;
        }
    }
}

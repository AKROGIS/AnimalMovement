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

        //These private fields cache results for all the properties.
        //This object is immutable, so I only need to calculate each property once.
        private List<string> _unknownPlatforms;
        private List<string> _ambiguousPlatforms;
        private List<Collar> _validCollars;
        private Dictionary<Collar, string> _collarsWithProblems;
        private Dictionary<string, List<Collar>> _argosCollars;
        private Dictionary<string, Collar> _uniqueArgosCollars;
        private Dictionary<string, List<Collar>> _sharedArgosCollars;
        private HashSet<Collar> _unambiguousSharedCollars;
        private HashSet<Collar> _allUnambiguousCollars;
        private Dictionary<string, IProcessor> _processors;
        private HashSet<string> _telonicGen3CollarsWithParameterFile;
        private Dictionary<string, TimeSpan> _gen3periods;
        private Dictionary<string, List<Byte[]>> _tpfFiles;

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
            FilePlatforms = new HashSet<string>(File.GetPlatforms());
        }


        public ArgosFile File { get; private set; }

        public AnimalMovementDataContext Database { get; private set; }

        public IEnumerable<string> UnknownPlatforms
        {
            get { return (_unknownPlatforms ?? (_unknownPlatforms = GetUnknownPlatforms())).ToArray(); }
        }


        public IEnumerable<string> AmbiguousPlatforms
        {
            get { return (_ambiguousPlatforms ?? (_ambiguousPlatforms = GetAmbiguousPlatforms())).ToArray(); }
        }


        public IEnumerable<Collar> ValidCollars
        {
            get { return (_validCollars ?? (_validCollars = GetValidCollars())).ToArray(); }
        }


        public Dictionary<Collar, string> CollarsWithProblems
        {
            get
            {
                if (_collarsWithProblems == null)
                    _collarsWithProblems = GetCollarsWithProblems();
                return _collarsWithProblems.ToDictionary(pair => pair.Key, pair => pair.Value);
            }
        }


        public Func<string, DateTime, string> CollarSelector
        {
            get {return GetCollarIdFromTransmission;}
        }


        public Func<string, IProcessor> ProcessorSelector
        {
            get { return GetProcessorFromTelonicsId; }
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

        #region 'private' methods, accessible through public property

        private string GetCollarIdFromTransmission(string argosId, DateTime transmissionTime)
        {
            if (String.IsNullOrEmpty(argosId))
                throw new ArgumentNullException("argosId", "No Argos Id provided when one was expected");

            if (UniqueArgosCollars.ContainsKey(argosId))
                return UniqueArgosCollars[argosId].CollarId;

            if (SharedArgosCollars.ContainsKey(argosId))
            {
                var collar = SharedArgosCollars[argosId].SelectWithDate(transmissionTime);
                if (collar != null)
                    return collar.CollarId;
                var msg1 = String.Format("Unable to select a unique collar for platform Id {0} on {1}" +
                                         "\n  potential collars are {2}." +
                                         "\n  To correct this problem, add a disposal date to one or more of these collars.",
                                         argosId, transmissionTime, String.Join(", ",
                                                                                SharedArgosCollars[argosId].Select(
                                                                                    c => c.ToString())));
                throw new InvalidOperationException(msg1);
            }
            var msg2 = String.Format("Unable find a collar for platform Id {0}", argosId);
            throw new InvalidOperationException(msg2);
        }


        private IProcessor GetProcessorFromTelonicsId(string telonicId)
        {
            if (String.IsNullOrEmpty(telonicId))
                    throw new ArgumentNullException("telonicId", "No Telonic Id (CTN) provided when one was expected");
            if (!Processors.ContainsKey(telonicId))
                throw new ArgumentOutOfRangeException("telonicId", "Internal Error, No processor for Telonics Id "+ telonicId);
            return Processors[telonicId];
        }

        #endregion

        #region Private Properties

        private HashSet<string> FilePlatforms { get; set; }

        private Dictionary<string, List<Collar>> ArgosCollars
        {
            get { return _argosCollars ?? (_argosCollars = GetArgosCollars()); }
        }

        private Dictionary<string, Collar> UniqueArgosCollars
        {
            get { return _uniqueArgosCollars ?? (_uniqueArgosCollars = GetUniqueArgosCollars()); }
        }

        private Dictionary<string, List<Collar>> SharedArgosCollars
        {
            get { return _sharedArgosCollars ?? (_sharedArgosCollars = GetSharedArgosCollars()); }
        }

        private IEnumerable<Collar> UnambiguousSharedCollars
        {
            get { return _unambiguousSharedCollars ?? (_unambiguousSharedCollars = GetUnambiguousSharedCollars()); }
        }

        private HashSet<Collar> AllUnambiguousCollars
        {
            get { return _allUnambiguousCollars ?? (_allUnambiguousCollars = GetAllUnambiguousCollars()); }
        }

        private Dictionary<string, IProcessor> Processors
        {
            get { return _processors ?? (_processors = GetProcessors()); }
        }

        private HashSet<string> TelonicGen3CollarsWithParameterFile
        {
            get { return _telonicGen3CollarsWithParameterFile ?? (_telonicGen3CollarsWithParameterFile = GetTelonicGen3CollarsWithParameterFile()); }
        }

        private Dictionary<string, TimeSpan> Gen3periods
        {
            get { return _gen3periods ?? (_gen3periods = GetGen3Periods()); }
        }

        private Dictionary<string, List<Byte[]>> TpfFiles
        {
            get { return _tpfFiles ?? (_tpfFiles = GetTpfFiles()); }
        }
        
        #endregion

        #region Property initializers (only called once)

        private List<string> GetUnknownPlatforms()
        {
            var databasePlatforms = from collar in Database.Collars
                                    where collar.CollarManufacturer == "Telonics" && collar.AlternativeId != null
                                    select collar.AlternativeId;
            return FilePlatforms.Except(databasePlatforms).ToList();
        }


        private List<string> GetAmbiguousPlatforms()
        {
            var collars = new HashSet<Collar>(SharedArgosCollars.Values.SelectMany(c => c));
            collars.ExceptWith(UnambiguousSharedCollars);
            return collars.Select(c => c.AlternativeId).ToList();
        }


        private List<Collar> GetValidCollars()
        {
            return AllUnambiguousCollars.Except(CollarsWithProblems.Select(e => e.Key)).ToList();
        }


        private Dictionary<Collar, string> GetCollarsWithProblems()
        {
            // known problems:
            //  1) not Telonics - reported as an unknown platform - not checked here
            //  2) Collar is not a known gen
            //  3) for Gen3: no delay, or an active PPF file.
            //  4) For Gen4: No tpf file or multiple tpf files

            var results = new Dictionary<Collar, string>();
            foreach (var collar in AllUnambiguousCollars)
            {
                if (collar.CollarModel == "TelonicsGen3")
                {
                    if (!Gen3periods.ContainsKey(collar.CollarId))
                        results[collar] = "Gen3 Collar does not have a period between fixes specified";
                    else if (TelonicGen3CollarsWithParameterFile.Contains(collar.CollarId))
                        results[collar] = 
                                 "Warning: PPF file for this collar is not used. Suggest deactiving the PPF file, or use Telonics ADC-T03 to process this collar";
                }
                else if (collar.CollarModel == "TelonicsGen4")
                {
                    if (TpfFiles.ContainsKey(collar.CollarId))
                    {
                        if (TpfFiles[collar.CollarId].Count > 1)
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


        //Get a dictionary yielding a list of Collars for each Platform Id (most lists will have only one item)
        private Dictionary<string, List<Collar>> GetArgosCollars()
        {
            var argosCollars = from collar in Database.Collars
                               where
                                   collar.CollarManufacturer == "Telonics" && collar.AlternativeId != null &&
                                   FilePlatforms.Contains(collar.AlternativeId)
                               group collar by collar.AlternativeId;
            return argosCollars.ToDictionary(group => group.Key, group => group.ToList());
        }


        private Dictionary<string, Collar> GetUniqueArgosCollars()
        {
            return ArgosCollars.Where(pair => pair.Value.Count == 1).ToDictionary(pair => pair.Key, pair => pair.Value[0]);
        }


        private Dictionary<string, List<Collar>> GetSharedArgosCollars()
        {
            return ArgosCollars.Where(pair => pair.Value.Count > 1).ToDictionary(pair => pair.Key, pair => pair.Value);
        }


        private HashSet<Collar> GetUnambiguousSharedCollars()
        {
            var collars = SharedArgosCollars.SelectMany(e => e.Value.IsAmbiguous() ? new List<Collar>() : e.Value);
            return new HashSet<Collar>(collars);
        }


        private HashSet<Collar> GetAllUnambiguousCollars()
        {
            return new HashSet<Collar>(UniqueArgosCollars.Values.Concat(UnambiguousSharedCollars));
        }


        private Dictionary<string, IProcessor> GetProcessors()
        {
            var processors = new Dictionary<string, IProcessor>();

            foreach (var collar in ValidCollars)
            {
                switch (collar.CollarModel)
                {
                    case "TelonicsGen3":
                        processors[collar.CollarId] = new Gen3Processor(Gen3periods[collar.CollarId]);
                        break;
                    case "TelonicsGen4":
                        processors[collar.CollarId] = new Gen4Processor(TpfFiles[collar.CollarId].First());
                        break;
                    default:
                        var msg = String.Format("Unsupported model ({0} for collar {1}", collar.CollarModel, collar.CollarId);
                        throw new ArgumentOutOfRangeException(msg);
                }
            }
            return processors;
        }


        private HashSet<string> GetTelonicGen3CollarsWithParameterFile()
        {
            var query = from parameter in Database.CollarParameters
                        //FIXME - add database field for parameter file status and check here
                        where parameter.CollarParameterFile.Format == 'B' && AllUnambiguousCollars.Contains(parameter.Collar)
                        select parameter.CollarId;
            return new HashSet<string>(query);
        }


        private Dictionary<string, TimeSpan> GetGen3Periods()
        {
            var query = from collar in Database.Collars
                        where collar.CollarModel == "TelonicsGen3" && AllUnambiguousCollars.Contains(collar) && collar.Gen3Period != null
                        select collar;
            return query.ToDictionary(item => item.CollarId, item => TimeSpan.FromMinutes(item.Gen3Period.GetValueOrDefault()));
        }


        private Dictionary<string, List<Byte[]>> GetTpfFiles()
        {
            var query = from parameter in Database.CollarParameters
                        //FIXME - add database field for parameter file status and check here
                        where parameter.CollarParameterFile.Format == 'A' && AllUnambiguousCollars.Contains(parameter.Collar)
                        group parameter.CollarParameterFile.Contents by parameter.Collar.CollarId;

            var results = new Dictionary<string, List<Byte[]>>();
            foreach (var item in query)
                results[item.Key] = item.Cast<Byte[]>().ToList();
            return results;
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

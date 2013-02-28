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
        private List<Collar> _allUnambiguousCollars;
        private Dictionary<string, IProcessor> _processors;
        private HashSet<string> _telonicGen3CollarsWithParameterFile;
        private Dictionary<string, TimeSpan> _gen3periods;
        private Dictionary<string, List<Byte[]>> _tpfFiles;

        #endregion

        #region Public API

        /// <summary>
        /// Provides the analysis necessary to map Argos email data to the Animal Movements database 
        /// </summary>
        /// <param name="file">Argos email file with messages to be processed</param>
        /// <param name="database">Animal Movement Database Context</param>
        public ArgosCollarAnalyzer(ArgosFile file, AnimalMovementDataContext database)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            if (database == null)
                throw new ArgumentNullException("database");
            File = file;
            Database = database;
            FilePlatforms = File.GetPlatforms().ToList();
        }

        /// <summary>
        /// Argos email file with messages to be processed
        /// </summary>
        public ArgosFile File { get; private set; }

        /// <summary>
        /// Animal Movement Database Context
        /// </summary>
        public AnimalMovementDataContext Database { get; private set; }

        /// <summary>
        /// Argos Platform Ids which are in the Argos file, but not the database
        /// </summary>
        public IEnumerable<string> UnknownPlatforms
        {
            get { return (_unknownPlatforms ?? (_unknownPlatforms = GetUnknownPlatforms())).ToArray(); }
        }

        /// <summary>
        /// Argos Platform Ids which are used in more that once collar with overlapping active (not disposed) dates
        /// </summary>
        public IEnumerable<string> AmbiguousPlatforms
        {
            get { return (_ambiguousPlatforms ?? (_ambiguousPlatforms = GetAmbiguousPlatforms())).ToArray(); }
        }

        /// <summary>
        /// Database collars that are matched one to one with Platform Ids in the Argos file
        /// </summary>
        public IEnumerable<Collar> ValidCollars
        {
            get { return (_validCollars ?? (_validCollars = GetValidCollars())).ToArray(); }
        }

        /// <summary>
        /// Collection of collars that are unambiguously share an Argos Id
        /// </summary>
        public HashSet<Collar> UnambiguousSharedCollars
        {
            get { return _unambiguousSharedCollars ?? (_unambiguousSharedCollars = GetUnambiguousSharedCollars()); }
        }

        /// <summary>
        /// Dictionary of collars (in Argos file) that have issues, and a description of the issue
        /// </summary>
        public Dictionary<Collar, string> CollarsWithProblems
        {
            get
            {
                if (_collarsWithProblems == null)
                    _collarsWithProblems = GetCollarsWithProblems();
                return _collarsWithProblems.ToDictionary(pair => pair.Key, pair => pair.Value);
            }
        }

        /// <summary>
        /// Function that the client can use to get a Telonics Id from an Argos Id and a transmission date
        /// </summary>
        public Func<string, DateTime, string> CollarSelector
        {
            get {return GetCollarIdFromTransmission;}
        }

        /// <summary>
        /// Function that the client can use to get a class that can process the messages for a collar (by Telonics Id)
        /// </summary>
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
                case "Gen3":
                    return 'D';
                case "Gen4":
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

            if (UnknownPlatforms.Contains(argosId))
                return null;
            if (AmbiguousPlatforms.Contains(argosId))
                return null;

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

        private List<string> FilePlatforms { get; set; }

        private Dictionary<string, List<Collar>> ArgosCollars
        {
            get { return _argosCollars ?? (_argosCollars = GetArgosCollars()); }
        }

        private Dictionary<string, Collar> UniqueArgosCollars
        {
            get { return _uniqueArgosCollars ?? (_uniqueArgosCollars = GetUniqueArgosCollars()); }
        }

        /// <summary>
        /// Dictionary yielding a list of Collars for each Platform Id (most lists will have only one item)
        /// </summary>
        private Dictionary<string, List<Collar>> SharedArgosCollars
        {
            get { return _sharedArgosCollars ?? (_sharedArgosCollars = GetSharedArgosCollars()); }
        }

        private List<Collar> AllUnambiguousCollars
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
                                    where collar.CollarManufacturer == "Telonics" && collar.ArgosId != null
                                    select collar.ArgosId;
            return FilePlatforms.Except(databasePlatforms).ToList();
        }


        private List<string> GetAmbiguousPlatforms()
        {
            var collars = new HashSet<Collar>(SharedArgosCollars.Values.SelectMany(c => c));
            collars.ExceptWith(UnambiguousSharedCollars);
            return collars.Select(c => c.ArgosId).Distinct().ToList();
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
                if (collar.CollarModel == "Gen3")
                {
                    if (!Gen3periods.ContainsKey(collar.CollarId))
                        results[collar] = "Gen3 Collar does not have a period between fixes specified";
                    else if (TelonicGen3CollarsWithParameterFile.Contains(collar.CollarId))
                        results[collar] =
                            "PPF file for this collar is not used.\n" +
                            "  Suggest 1) deactiving the PPF file\n" +
                            ",      or 2) use Telonics ADC-T03 to process this collar";
                }
                else if (collar.CollarModel == "Gen4")
                {
                    if (TpfFiles.ContainsKey(collar.CollarId))
                    {
                        if (TpfFiles[collar.CollarId].Count > 1)
                            results[collar] = "Gen4 Collar has multiple TPF files.\n" +
                                              "  Suggest 1) creating multiple collars one for each TPF files\n" +
                                              "       or 2) make all but one of the TPF files inactive.";
                    }
                    else
                        results[collar] = "Gen4 Collar does not have a TPF file";
                }
                else
                    results[collar] = "Model must be one of (Gen3, Gen4)";
            }
            return results;
        }


        private Dictionary<string, List<Collar>> GetArgosCollars()
        {
            var argosCollars = from collar in Database.Collars
                               where
                                   collar.CollarManufacturer == "Telonics" && collar.ArgosId != null &&
                                   FilePlatforms.Contains(collar.ArgosId)
                               group collar by collar.ArgosId;
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


        private List<Collar> GetAllUnambiguousCollars()
        {
            return UniqueArgosCollars.Values.Concat(UnambiguousSharedCollars).ToList();
        }


        private Dictionary<string, IProcessor> GetProcessors()
        {
            var processors = new Dictionary<string, IProcessor>();

            string tdcExe = Settings.GetSystemDefault("tdc_exe");
            string batchFile = Settings.GetSystemDefault("tdc_batch_file_format");

            foreach (var collar in ValidCollars)
            {
                switch (collar.CollarModel)
                {
                    case "Gen3":
                        processors[collar.CollarId] = new Gen3Processor(Gen3periods[collar.CollarId]);
                        break;
                    case "Gen4":
                        var processor = new Gen4Processor(TpfFiles[collar.CollarId].First());
                        if (!String.IsNullOrEmpty(tdcExe))
                            processor.TdcExecutable = tdcExe;
                        if (!String.IsNullOrEmpty(batchFile))
                            processor.BatchFileTemplate = batchFile;
                        processors[collar.CollarId] = processor;
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
                        where
                            parameter.CollarParameterFile.Format == 'B' &&
                            AllUnambiguousCollars.Contains(parameter.Collar) &&
                            parameter.CollarParameterFile.Status == 'A'
                        select parameter.CollarId;
            return new HashSet<string>(query);
        }


        private Dictionary<string, TimeSpan> GetGen3Periods()
        {
            var query = from collar in Database.Collars
                        where collar.CollarModel == "Gen3" && AllUnambiguousCollars.Contains(collar) && collar.Gen3Period != null
                        select collar;
            return query.ToDictionary(item => item.CollarId, item => TimeSpan.FromMinutes(item.Gen3Period.GetValueOrDefault()));
        }


        private Dictionary<string, List<Byte[]>> GetTpfFiles()
        {
            var query = from parameter in Database.CollarParameters
                        where
                            parameter.CollarParameterFile.Format == 'A' &&
                            AllUnambiguousCollars.Contains(parameter.Collar) &&
                            parameter.CollarParameterFile.Status == 'A'
                        group parameter.CollarParameterFile.Contents.ToArray() by parameter.Collar.CollarId;

            return query.ToDictionary(item => item.Key, item => item.ToList());
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Telonics;
using DataModel;

namespace ArgosProcessor
{
    internal class ArgosCollarAnalyzer
    {
        private HashSet<string> _filePlatforms;
        private IEnumerable<string> _unknownPlatforms;
        private IEnumerable<string> _ambiguousPlatforms;
        private Dictionary<Collar, string> _collarsWithProblems;



        public ArgosFile File { get; private set; }
        public AnimalMovementDataContext Database { get; private set; }

        public ArgosCollarAnalyzer(ArgosFile file, AnimalMovementDataContext database)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            if (database == null)
                throw new ArgumentNullException("database");
            File = file;
            Database = database;

            _filePlatforms = new HashSet<string>(File.GetPlatforms());

            var allids = File.GetPlatforms().ToArray();
            var gen3ids = allids.Intersect(from collar in Database.Collars
                                           where collar.CollarModel == "TelonicsGen3"
                                           select collar.AlternativeId).ToArray();
            var gen4ids = allids.Intersect(from collar in Database.Collars
                                           where collar.CollarModel == "TelonicsGen4"
                                           select collar.AlternativeId).ToArray();
            if (gen3ids.Length > 0)
            {
                var gen34ids = gen3ids.Intersect(gen4ids).ToArray();
                if (gen34ids.Length > 0)
                {
                    var startDates = new Dictionary<string, DateTime>();
                    var endDates = new Dictionary<string, DateTime>();
                    //FIXME set dates by querying database deployments
                    Func<string, DateTime, Boolean> func =
                        (i, d) => startDates.ContainsKey(i) && startDates[i] < d && d < endDates[i];
                }
            }
        }

        internal IEnumerable<string> UnknownPlatforms
        {
            get
            {
                if (_unknownPlatforms == null)
                    DoAnalysis();
                return _unknownPlatforms;
            }
        }

        internal IEnumerable<string> AmbiguousPlatforms
        {
            get
            {
                if (_ambiguousPlatforms == null)
                    DoAnalysis();
                return _ambiguousPlatforms;
            }
        }

        internal Dictionary<Collar, string> CollarsWithProblems
        {
            get
            {
                if (_collarsWithProblems == null)
                    DoAnalysis();
                return _collarsWithProblems;
            }
        }

        internal void DoAnalysis()
        {
            _unknownPlatforms = GetUnknownPlatforms();
            _ambiguousPlatforms = GetAmbiguousPlatforms();
            _collarsWithProblems = GetCollarsWithProblems();
        }


        private IEnumerable<string> GetUnknownPlatforms()
        {
            var databasePlatforms = from collar in Database.Collars
                                    where collar.CollarManufacturer == "Telonics" && collar.AlternativeId != null
                                    select collar.AlternativeId;
            return _filePlatforms.Except(databasePlatforms);
        }

        private IEnumerable<string> GetAmbiguousPlatforms()
        {
            var collarsByArgos = from collar in Database.Collars
                                 where collar.CollarManufacturer == "Telonics" && collar.AlternativeId != null
                                 group collar.CollarId by collar.AlternativeId;

            var problems = from grouping in collarsByArgos
                           where grouping.Count() > 1 && _filePlatforms.Contains(grouping.Key)  && DatesOverlap(grouping)
                           select grouping.Key;
            return problems;
        }


        private Dictionary<Collar, string> GetCollarsWithProblems()
        {
            // problems: not Telonics, no parameters,
            throw new NotImplementedException();
        }

        internal Collar[] GetValidCollars()
        {
            throw new NotImplementedException();
        }

        internal Func<string, IProcessor> GetProcessorSelector()
        {
            //returns a function that given a Telonics CTN will return the appropriate IProcessor
            throw new NotImplementedException();
        }

        internal char GetCollarFormat(Collar collar)
        {
            throw new NotImplementedException();
        }


        #region Private Methods

        private bool DatesOverlap(IGrouping<string, string> grouping)
        {
            //FIXME - check deployment dates 
            return true;
        }
        #endregion
    }
}

/*

*/

using System;
using System.Collections.Generic;
using System.Linq;
using Telonics;
using DataModel;

namespace ArgosProcessor
{
    internal class ArgosCollarAnalyzer
    {
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

        internal string[] GetUnknownPlatforms()
        {
            throw new NotImplementedException();
        }

        internal string[] GetAmbiguousPlatforms()
        {
            throw new NotImplementedException();
        }

        internal Dictionary<Collar, string> GetCollarsWithProblems()
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
    }
}

/*

*/

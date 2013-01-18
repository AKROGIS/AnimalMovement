using System;
using System.Collections.Generic;
using System.Linq;
using DataModel;

namespace ArgosProcessor
{
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
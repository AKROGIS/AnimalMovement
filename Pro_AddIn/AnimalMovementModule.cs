using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;

namespace AnimalMovement
{
    internal class AnimalMovementModule : Module
    {
        private static AnimalMovementModule _this = null;

        /// <summary>
        /// Retrieves the singleton instance to this module
        /// </summary>
        public static AnimalMovementModule Current
        {
            get
            {
                return _this ?? (_this = (AnimalMovementModule)FrameworkApplication.FindModule("AnimalMovement_Module"));
            }
        }

        #region Overrides
        /// <summary> 
        /// Called by Framework when ArcGIS Pro is closing
        /// </summary>
        /// <returns>False to prevent Pro from closing, otherwise True</returns>
        protected override bool CanUnload()
        {
            return true;
        }

        #endregion Overrides

    }
}

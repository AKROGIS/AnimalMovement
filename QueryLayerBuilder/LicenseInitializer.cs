using ESRI.ArcGIS;
using System;

namespace QueryLayerBuilder
{
    internal partial class LicenseInitializer
    {
        public LicenseInitializer()
        {
            ResolveBindingEvent += BindingArcGisRuntime;
        }

        static void BindingArcGisRuntime(object sender, EventArgs e)
        {
            if (RuntimeManager.Bind(ProductCode.Desktop))
                return;
            // Failed to bind, announce and force exit
            Console.WriteLine("Invalid ArcGIS runtime binding. Application will shut down.");
            Environment.Exit(1);
        }
    }
}
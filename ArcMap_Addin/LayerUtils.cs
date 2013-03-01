using System;
using System.Collections.Generic;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

namespace ArcMap_Addin
{
    static class LayerUtils
    {
        
        /// <summary>
        /// Gets all layers in a map document of a particular type 
        /// </summary>
        /// <param name="doc">The map document to search</param>
        /// <param name="type">A GUID type string for the layer type. see http://help.arcgis.com/en/sdk/10.0/arcobjects_net/componenthelp/index.html#/Loop_Through_Layers_of_Specific_UID_Snippet/00490000005w000000/ </param>
        /// <returns>a list (possibly empty) of layer that were found to match the type requested</returns>
        public static IEnumerable<ILayer> GetAllLayers(IMxDocument doc, string type)
        {
            if (doc == null)
                throw new ArgumentNullException("doc");
            if (type == null)
                throw new ArgumentNullException("type");

            UID uid = new UIDClass();
            uid.Value = type;
            IMaps maps = doc.Maps;
            for (int i = 0; i < maps.Count; i++)
            {
                IMap map = maps.Item[i];
                IEnumLayer elayers = map.Layers[uid];
                ILayer layer = elayers.Next();
                while (layer != null)
                {
                    yield return layer;
                    layer = elayers.Next();
                }
            }
        }

        #region layer naming methods
        /// <summary>
        /// Gets the full path name of the layer (including ancestor group layers and data frame)
        /// </summary>
        /// <param name="doc">The map document that this layer is in</param>
        /// <param name="layer">The ILayer whose name we want</param>
        /// <param name="mapSeparator">A character string used to data frame name from the group/layer names</param>
        /// <param name="layerSeparator">A character string used to the group names from the layer name</param>
        /// <returns>null if the layer does not exist in the map document, full name otherwise</returns>
        public static string GetFullName(IMxDocument doc, ILayer layer, string mapSeparator = ":", string layerSeparator = "/")
        {
            IMaps maps = doc.Maps;
            for (int i = 0; i < maps.Count; i++)
            {
                IMap map = maps.Item[i];
                string name = GetFullName(map, layer, layerSeparator);
                if (name != null)
                {
                    return map.Name + mapSeparator + name;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the full path name of the layer (including ancestor group layers) relative to the IMap
        /// </summary>
        /// <param name="parent">The data frame that this layer is in</param>
        /// <param name="layer">The ILayer whose full path name we want</param>
        /// <param name="separator">A character string used to separate names in path</param>
        /// <returns>null if the layer does not exist in IMap, full name otherwise</returns>
        public static string GetFullName(IMap parent, ILayer layer, string separator = "/")
        {
            for (int i = 0; i < parent.LayerCount; i++)
            {
                if (parent.Layer[i] == layer)
                    return layer.Name;

                var compositeLayer = parent.Layer[i] as ICompositeLayer;
                if (compositeLayer == null)
                    continue;
                string name = GetFullName(compositeLayer, layer, separator);
                if (name != null)
                {
                    return parent.Layer[i].Name + separator + name;
                }
            }
            return null;
        }

        private static string GetFullName(ICompositeLayer parent, ILayer layer, string separator)
        {
            for (int i = 0; i < parent.Count; i++)
            {
                if (parent.Layer[i] == layer)
                    return layer.Name;

                var compositeLayer = parent.Layer[i] as ICompositeLayer;
                if (compositeLayer == null)
                    continue;
                string name = GetFullName(compositeLayer, layer, separator);
                if (name != null)
                {
                    return parent.Layer[i].Name + separator + name;
                }
            }
            return null;
        }
        #endregion
    }
}

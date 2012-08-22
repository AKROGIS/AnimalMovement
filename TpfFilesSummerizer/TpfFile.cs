using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TpfFilesSummerizer
{
    internal class TpfFile
    {
        private string[] _fileContents;

        public string FilePath { get; private set; }

        public static TpfFile LoadFromPath(string path)
        {
            var tpfFile = new TpfFile(path);
            tpfFile.Load();
            return tpfFile;
        }

        public TpfFile(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            FilePath = path;
        }

        public void Load()
        {
            _fileContents = File.ReadAllLines(FilePath);
        }

        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(FilePath); }
        }

        public IEnumerable<Collar> ParseForCollars()
        {
            string owner = GetOwner();
            string[] ids = GetIds();
            string[] frequencies = GetFrequencies();
            string[] argosIds = GetArgosIds();
            if (ids.Length != frequencies.Length || 
                ids.Length != argosIds.Length || 
                frequencies.Length != argosIds.Length)
                throw new InvalidOperationException("Indeterminant number of collars in file " + Name);
            return ids.Select((id, index) => new Collar
                                            {
                                                Ctn = id,
                                                ArgosId = argosIds[index],
                                                Frequency = GetFrequency(frequencies[index]),
                                                Owner = owner,
                                                TpfFile = this
                                            });
        }

        public string GetOwner()
        {
            return GetTpfData("sections.customer.parameters.customer");
        }

        private string[] GetFrequencies()
        {
            return GetTpfData("sections.units.parameters.vhfFrequencyList").Split();
        }

        private string[] GetArgosIds()
        {
            return GetTpfData("sections.units.parameters.decimalArgosIdList").Split();
        }

        private string[] GetIds()
        {
            return GetTpfData("sections.units.parameters.ctnList").Split();
        }

        private string GetTpfData(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (_fileContents == null)
                throw new InvalidOperationException("You must Load() the Tpf file before extracting properties");

            var value = from line in _fileContents
                        where line.StartsWith(key + " ")
                        select line.Replace(key + " ", "");
            var data = value.FirstOrDefault();
            if (String.IsNullOrEmpty(data))
                throw new FormatException("No value found for Key: " + key + " not found in file "+ Name);
            if (data[0] == '{')
                //should read until next '}', but we stop at the newline, and assume the '}' is at the end o fthe line
                return data.Replace("{","").Replace("}","").Trim();
            return data.Trim();
        }

        private Double GetFrequency(string s)
        {
            int f;
            if (Int32.TryParse(s, out f))
                return f / 1000000.0;
            throw new FormatException("Frequency (" + s + ") in " + Name + " is not in the expected format");
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

namespace DataModel
{
    public partial class CollarParameterFile
    {
        public override string ToString()
        {
            return string.Format("{0} (id={1})",FileName, FileId);
        }
    }
}

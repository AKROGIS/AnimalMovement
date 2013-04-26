namespace DataModel
{
    public partial class Animal
    {
        public override string ToString()
        {
            return Project.ProjectName + "/" + _AnimalId;
        }
    }
}

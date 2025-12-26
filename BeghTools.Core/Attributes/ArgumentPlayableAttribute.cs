
namespace BeghTools.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ArgumentPlayableAttribute : Attribute
    {
        public string ArgumentName { get; }
        public string ArgumentDescription { get;}
        public string ArgumentIcon { get; }

        public ArgumentPlayableAttribute(string argumentName, string argumentDescription, string argumentIcon)
        {
            ArgumentName = argumentName;
            ArgumentDescription = argumentDescription;
            ArgumentIcon = argumentIcon;
        }
    }
}

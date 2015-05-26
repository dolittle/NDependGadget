using Bifrost.Concepts;

namespace Web
{
    public class BuildIdentifier : ConceptAs<string>
    {
        public static implicit operator BuildIdentifier(string value)
        {
            return new BuildIdentifier { Value = value };
        }
    }
}

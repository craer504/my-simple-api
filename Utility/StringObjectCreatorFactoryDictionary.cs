using SimpleRegisterLoginLogout.Interfaces;

namespace SimpleRegisterLoginLogout.Utility
{
    public static class StringObjectCreatorFactoryDictionary
    {
        // Add types and corresponding factories to this dictionary in Program.cs:
        public static Dictionary<Type, IStringParseObjectCreatable> FactoryDictionary = new() { };

        public static void AddTypeToDictionary(Type t, IStringParseObjectCreatable spoc)
        {
            FactoryDictionary.Add(t, spoc);
        }
    }
}

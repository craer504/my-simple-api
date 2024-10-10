using System.IO;
using System.Text;
using SimpleRegisterLoginLogout.Classes;
using SimpleRegisterLoginLogout.DBs;
using SimpleRegisterLoginLogout.Factories;
using SimpleRegisterLoginLogout.Interfaces;

namespace SimpleRegisterLoginLogout.Utility
{
    public static class DBSaver
    {
        // Need to set factory dictionary for type matching during db load operation:
        public static Dictionary<Type, IStringParseObjectCreatable> FactoryDictionary { get; set; } = new()
        {
            {typeof(User), new UserFactory()},
            {typeof(Post), new PostFactory()},
            {typeof(UserPost), new UserPostFactory()},
        };

        public static void LoadDBFromFile<T>(List<T> db, string filePath)
        {
            if (filePath == string.Empty)
                return;

            db.Clear();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string currentData = string.Empty;

                // No factory registered for the type T:
                if (!FactoryDictionary.TryGetValue(typeof(T), out var factory))
                    return;

                while ((currentData = reader.ReadLine()!) != null)
                {
                    var obj = (T)factory.ParseStringCreateObject(currentData);

                    db.Add(obj);
                }
            }
        }

        public static async void SaveDBToFile<T>(List<T> db, string filePath)
        {
            if (filePath == string.Empty || db.Count == 0)
                return;

            StringBuilder sb = new();

            for (int i = 0; i < db.Count; i++)
            {
                sb.AppendLine((i + 1) + " - " + db.ElementAt(i)!.ToString());
            }

            string contents = sb.ToString();

            // WriteAllText overwrites so I don't have to equalize list with my db:
            await File.WriteAllTextAsync(filePath, contents);
        }
    }
}

using Newtonsoft.Json;
using WorkWithData.Domain.models;

namespace Data.services
{
    public static class JsonFileManager
    {
        public static void Save(List<CoffeeTransaction> data, string path)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                string json = JsonConvert.SerializeObject(data, settings);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public static List<CoffeeTransaction> Load(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return new List<CoffeeTransaction>();
                }

                List<CoffeeTransaction> data = JsonConvert.DeserializeObject<List<CoffeeTransaction>>(File.ReadAllText(path));

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}

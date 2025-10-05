using Newtonsoft.Json;

namespace Data.services
{
    public static class JsonFileManager
    {
        public static void Save<T>(List<T> data, string path)
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

        public static List<T> Load<T>(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return new List<T>();
                }

                List<T> data = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}

using System.Xml.Serialization;
using WorkWithData.Domain.models;

namespace Data.services
{
    public static class XmlFileManager
    {
        public static void Save(List<CoffeeTransaction> data, string path)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<CoffeeTransaction>));
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    serializer.Serialize(fs, data);
                }
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

                XmlSerializer serializer = new XmlSerializer(typeof(List<CoffeeTransaction>));
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    List<CoffeeTransaction> data = (List<CoffeeTransaction>)serializer.Deserialize(fs);
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}

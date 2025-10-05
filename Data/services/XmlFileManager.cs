using System.Xml.Serialization;

namespace Data.services
{
    public static class XmlFileManager
    {
        public static void Save<T>(List<T> data, string path)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
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

        public static List<T> Load<T>(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return new List<T>();
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    List<T> data = (List<T>)serializer.Deserialize(fs);
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

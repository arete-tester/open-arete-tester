using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AreteTester.Core
{
    public class LoadSave
    {
        public static void Save(Type type, object o, string filename)
        {
            bool serializable = true;

            try
            {
                XmlSerializer ser = new XmlSerializer(type);
                using (StringWriter writer = new StringWriter())
                {
                    ser.Serialize(writer, o);
                    string serializedValue = writer.ToString();
                }
            }
            catch
            {
                serializable = false;
            }

            if (serializable)
            {
                XmlSerializer serializer = new XmlSerializer(type);
                using (TextWriter txtWriter = new StreamWriter(filename))
                {
                    serializer.Serialize(txtWriter, o);
                }
            }
        }

        public static object Load(Type type, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using (StreamReader reader = new StreamReader(filename))
            {
                return serializer.Deserialize(reader);
            }
        }
    }
}

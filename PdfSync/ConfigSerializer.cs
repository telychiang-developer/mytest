using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace iSTMC.Common
{
   public class ConfigSerializer<T>
   {
      private XmlSerializer s = null;
      private Type type = null;

      public ConfigSerializer()
      {
         this.type = typeof(T);
         this.s = new XmlSerializer(this.type);

      }

      public T Deserialize(TextReader reader)
      {
         var st = s.GetType();
         var stm = st.GetMethod("Deserialize", new Type[] { typeof(TextReader) });
         T o = (T)stm.Invoke(s, new object[] { reader });

         //T o = (T)s.Deserialize(reader);
         reader.Close();
         return o;
      }

      public T Deserialize(string xml)
      {
         TextReader reader = new StringReader(xml);
         return Deserialize(reader);
      }

      public T Deserialize(XmlDocument doc)
      {
         TextReader reader = new StringReader(doc.OuterXml);
         return Deserialize(reader);
      }

      private TextWriter WriterSerialize(T table)
      {
         TextWriter w = new StringWriter();
         this.s = new XmlSerializer(this.type);

         XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
         ns.Add("", "");

         s.Serialize(w, table, ns);
         w.Flush();
         return w;
      }

      public string StringSerialize(T table)
      {
         TextWriter w = WriterSerialize(table);
         string xml = w.ToString();
         w.Close();
         return xml.Trim();
      }

      public string StringSerialize(T table, Encoding encoding)
      {
         MemoryStream ms = new MemoryStream();
         StreamWriter sw = new StreamWriter(ms, encoding);

         this.s = new XmlSerializer(this.type);

         XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
         ns.Add("", "");

         s.Serialize(sw, table, ns);
         sw.Flush();

         ms.Seek(0, SeekOrigin.Begin);
         StreamReader sr = new StreamReader(ms, encoding);
         string xml = sr.ReadToEnd();
         ms.Close();
         sw.Close();
         return xml.Trim();
      }

      public XmlDocument Serialize(T table)
      {
         string xml = StringSerialize(table);
         XmlDocument doc = new XmlDocument();
         doc.PreserveWhitespace = true;
         doc.LoadXml(xml);
         return doc;
      }

      public static T ReadFile(string file)
      {
         ConfigSerializer<T> serializer = new ConfigSerializer<T>();
         try
         {
            //string xml = string.Empty;
            //using (StreamReader reader = new StreamReader(file))
            //{
            //   xml = reader.ReadToEnd();
            //   reader.Close();
            //}
            if (System.IO.File.Exists(file))
            {
               string xml = System.IO.File.ReadAllText(file);
               return serializer.Deserialize(xml);
            }
            else
               return default(T);
         }
         catch(Exception ex)
         {
            return default(T);
         }
      }

      public static T ReadXml(string xml)
      {
         ConfigSerializer<T> serializer = new ConfigSerializer<T>();
         try
         {
            return serializer.Deserialize(xml);
         }
         catch { }
         return default(T);
      }

      public static bool WriteFile(string file, T table)
      {
         bool ok = false;
         ConfigSerializer<T> serializer = new ConfigSerializer<T>();
         try
         {
            string xml = serializer.Serialize(table).OuterXml;
            using (StreamWriter writer = new StreamWriter(file, false))
            {
               writer.Write(xml.Trim());
               writer.Flush();
               writer.Close();
            }
            ok = true;
         }
         catch { }
         return ok;
      }

      public static string WriteXml(T table)
      {
         ConfigSerializer<T> serializer = new ConfigSerializer<T>();
         try
         {
            return serializer.Serialize(table).OuterXml;
         }
         catch { }
         return "";
      }
   }
}

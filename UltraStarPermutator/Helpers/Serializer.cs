using System;
using System.IO;
using System.Xml.Serialization;

namespace UltraStarPermutator
{
    internal class Serializer
    {
        internal static byte[]? Serialize(ProjectModel projectModel)
        {
            byte[]? response = default;
            XmlSerializer x = new XmlSerializer(projectModel.GetType());

            using (MemoryStream memoryStream = new MemoryStream())
            {
                x.Serialize(memoryStream, projectModel);

                response = memoryStream.ToArray();
            }

            return response;
        }

        internal static ProjectModel? Deserialize(byte[] fileBytes)
        {
            ProjectModel? response = default;

            XmlSerializer x = new XmlSerializer(typeof(ProjectModel));

            using (MemoryStream memoryStream = new MemoryStream(fileBytes))
            {
                response = x.Deserialize(memoryStream) as ProjectModel;
            }

            return response;
        }
    }
}
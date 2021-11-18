using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UAssetAPI.Extensions
{
    /// <summary>
    /// Extension for json operations
    /// </summary>
    public static class JsonExtensions
    {
        public static void UpdateFromFModelJObject(this Export export, JObject objData)
        {
            if (export is NormalExport normal)
            {

            }
        }

        /// <summary>
        /// Update export properties from FModel json
        /// </summary>
        /// <param name="asset">Asset</param>
        /// <param name="jsonFile">Json file path</param>
        public static void UpdateFromFModelJson(this UAsset asset, string jsonFile)
        {
            if (!File.Exists(jsonFile))
                throw new FileNotFoundException($"File \"{jsonFile}\" does not exist");

            string json = File.ReadAllText(jsonFile);
            JToken data = JToken.Parse(json);

            if (!(data is JArray))
                throw new Exception("Wrong json structure. Expected objects array.");

            int curExport = -1;
            foreach (JToken obj in data)
            {
                curExport++;

                // Skip not object items
                if (!(obj is JObject))
                    continue;

                if (curExport >= asset.ExportCount)
                    break;

                asset.Exports[curExport].UpdateFromFModelJObject((JObject)obj);
            }
        }
    }
}

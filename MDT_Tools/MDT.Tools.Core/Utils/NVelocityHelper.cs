using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;

namespace MDT.Tools.Core.Utils
{
    public class NVelocityHelper
    {

        public NVelocityHelper(string path)
        {
            ExtendedProperties extendedProperties=new ExtendedProperties();
            extendedProperties.AddProperty(RuntimeConstants.INPUT_ENCODING, "UTF-8");
            extendedProperties.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "UTF-8");
            extendedProperties.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file"); ;
            extendedProperties.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, path);
            Velocity.Init(extendedProperties);
        }

        public  string GenByTemplate(string path,Dictionary<string, object> vars)
        {
            VelocityContext context = new VelocityContext();
            if (vars != null)
            {
                foreach (KeyValuePair<string, object> kvp in vars)
                {
                    context.Put(kvp.Key, kvp.Value);
                }
            }
            StringWriter writer = new StringWriter();
            Template template = Velocity.GetTemplate(path);
            template.Merge(context, writer);
            string str= writer.GetStringBuilder().ToString();
            Console.WriteLine(str);
            return str;
        }

    }
}

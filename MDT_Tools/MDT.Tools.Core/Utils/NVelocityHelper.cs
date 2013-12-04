using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NVelocity;
using NVelocity.App;

namespace MDT.Tools.Core.Utils
{
    public class NVelocityHelper
    {

        static NVelocityHelper()
        {
            Velocity.Init();
        }

        public static string GenByTemplate(string path, Dictionary<string, object> vars)
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
            return writer.GetStringBuilder().ToString();
        }

    }
}

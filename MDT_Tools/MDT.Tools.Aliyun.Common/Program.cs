using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.Aliyun.Common.Oss;

namespace MDT.Tools.Aliyun.Common
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

           
             OssHelper ossHelper=new OssHelper();
             ossHelper.OssConfig = new OssConfig() { AccessId = "8rtKfQTKlHlgDNHc", AccessKey = "dVRZLKfLS9SYNnv5bwLolTfv9cJqoS", BucketName = "neapmet" };


             ossHelper.MutiPartUpload("MDT.Tools.Aliyun.Common.exe.config", "aa");

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.StackTrace);
            }
            Console.ReadLine();
        }
    }
}

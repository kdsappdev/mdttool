using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Aliyun.OpenServices.OpenStorageService;

namespace MDT.Tools.Aliyun.Common.Oss
{
    public class OssHelper
    {
        private OssClient ossClient = null;
        private OssConfig ossConfig = null;
        public OssConfig OssConfig
        {
            get { return ossConfig; }
            set
            {
                ossConfig = value;
                if (ossConfig != null)
                { 
                    ossClient = new OssClient(ossConfig.AccessId, ossConfig.AccessKey);
                    
                }
            }
        }

        public void Delete(string prefix)
        {
            OssClient client = new OssClient(OssConfig.AccessId, OssConfig.AccessKey);
            ObjectListing ol = null;
            do
            {
                ol = client.ListObjects(ossConfig.BucketName, prefix);
                foreach (var v in ol.ObjectSummaries)
                {
                    client.DeleteObject(ossConfig.BucketName, v.Key);
                    Console.WriteLine(v.Key + " 删除成功.");
                }
            } while (ol.IsTrunked);

        }

        public void DownLoad(string prefix,string dir)
        {
            //OssClient client = new OssClient(OssConfig.AccessId, OssConfig.AccessKey);
            //ObjectListing ol = null;
            //do
            //{
            //    ol = client.ListObjects(ossConfig.BucketName, prefix);
            //    foreach (var v in ol.ObjectSummaries)
            //    {
            //       client.
            //    }
            //} while (ol.IsTrunked);

        }

        public void UpLoad(string fileName,string key)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    MutiPartUpload(fileName, key);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(key + " 上传失败.");
                

            }
           
        }
        public void UpLoad(Stream stream, string key)
        {
            try
            {
               
                ObjectMetadata metadata = new ObjectMetadata();

                ossClient.PutObject(ossConfig.BucketName, key, stream, metadata);
                Console.WriteLine(key + " 上传成功.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(key + " 上传失败.");


            }

        }
        public void UpLoad(FileInfo fileInfo, string key)
        {
            try
            {
                FileStream fileStream = fileInfo.OpenRead();
                ObjectMetadata metadata = new ObjectMetadata();
                 
                ossClient.PutObject(ossConfig.BucketName, key, fileStream, metadata);
                Console.WriteLine(key + " 上传成功.");
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(key + " 上传失败.");
                 

            }
           
        }


        public void MutiPartUpload(string fileName, string key)
        {


            Console.WriteLine("开始上传:" + key);
            InitiateMultipartUploadRequest initRequest =
                            new InitiateMultipartUploadRequest(ossConfig.BucketName, key);
            InitiateMultipartUploadResult initResult = ossClient.InitiateMultipartUpload(initRequest);


            // 设置每块为 5M 
            int partSize = 1024 * 1024 * 5;

            FileInfo partFile = new FileInfo(fileName);

            // 计算分块数目 
            int partCount = (int)(partFile.Length / partSize);
            if (partFile.Length % partSize != 0)
            {
                partCount++;

            }
            Console.WriteLine("数据分块上传，一共:{0}块",partCount);
            // 新建一个List保存每个分块上传后的ETag和PartNumber 
            List<PartETag> partETags = new List<PartETag>();

            for (int i = 0; i < partCount; i++)
            {
                // 获取文件流 
                FileStream fis = new FileStream(partFile.FullName, FileMode.Open);

                // 跳到每个分块的开头 
                long skipBytes = partSize * i;
                fis.Position = skipBytes;
                //fis.skip(skipBytes); 

                // 计算每个分块的大小 
                long size = partSize < partFile.Length - skipBytes ?
                        partSize : partFile.Length - skipBytes;

                // 创建UploadPartRequest，上传分块 
                UploadPartRequest uploadPartRequest = new UploadPartRequest(ossConfig.BucketName, key, initResult.UploadId);
                uploadPartRequest.InputStream = fis;
                uploadPartRequest.PartSize = size;
                uploadPartRequest.PartNumber = (i + 1);
                UploadPartResult uploadPartResult = ossClient.UploadPart(uploadPartRequest);

                // 将返回的PartETag保存到List中。 
                partETags.Add(uploadPartResult.PartETag);

                // 关闭文件 
                fis.Close();
                Console.WriteLine("第{0}块,上传完毕", i+1);
            }

            CompleteMultipartUploadRequest completeReq = new CompleteMultipartUploadRequest(ossConfig.BucketName, key, initResult.UploadId);
            foreach (PartETag partETag in partETags)
            {
                completeReq.PartETags.Add(partETag);
            }
            //  红色标注的是与JAVA的SDK有区别的地方 

            //完成分块上传 
            Console.WriteLine("合并数据块开始");
            CompleteMultipartUploadResult completeResult = ossClient.CompleteMultipartUpload(completeReq);
            Console.WriteLine("合并数据块结束");
            // 返回最终文件的MD5，用于用户进行校验 
           
            Console.WriteLine(key + " 上传成功.");

        } 
 
    }
}

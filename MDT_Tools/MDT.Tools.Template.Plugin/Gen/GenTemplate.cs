using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Common;
using MDT.Tools.Template.Plugin.Model;
using MDT.Tools.Template.Plugin.Utils;

namespace MDT.Tools.Template.Plugin.Gen
{
    public class GenTemplate : AbstractHandler
    {
        public TemplateParas TemplateParas { get; set; }

        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                base.process(drTables, dsTableColumns, dsTablePrimaryKeys);
                SaveFileEncoding = Encoding.GetEncoding(TemplateParas.SaveFileEncoding);
                CodeLanguage = TemplateParas.CodeLanguage;
                OutPut = TemplateParas.SaveFilePath;
                setEnable(false);
                setStatusBar("");
                string[] strs = null;

                if (drTables != null && dsTableColumns != null)
                {
                    
                    if (!TemplateParas.IsShowGenCode)
                    {
                        FileHelper.DeleteDirectory(OutPut);
                        setStatusBar(string.Format("正在生成{0}模板代码", TemplateParas.TemplateName));
                        setProgreesEditValue(0);
                        setProgress(0);
                        setProgressMax(drTables.Length);
                    }
                   
                    for (int i = 0; i < tableInfos.Count; i++)
                    {

                        if (!TemplateParas.IsShowGenCode)
                        {
                            setStatusBar(string.Format("正在生成{0}模板{1}的代码,共{2}个代码，已生成了{3}个代码", TemplateParas.TemplateName,
                                                       tableInfos[i].TableName,
                                                       drTables.Length, i ));
                            setProgress(1);
                        }
                         
                            
                            GenCode(tableInfos[i]);
                         
                    }

                }

                if (!TemplateParas.IsShowGenCode)
                {
                    setStatusBar(string.Format("{0}模板代码生成成功", TemplateParas.TemplateName));
                    openDialog();
                }
            }
            catch (Exception ex)
            {
                setStatusBar(string.Format("{0}模板代码生成失败[{1}]", TemplateParas.TemplateName, ex.Message));

            }
            finally
            {
                setEnable(true);
            }
        }


        private readonly NVelocityHelper nVelocityHelper = new NVelocityHelper(FilePathHelper.TemplatesPath);
        public void GenCode(TableInfo tableInfo)
        {

            string tableName = tableInfo.TableName;
 
            string path = string.Format(@"{0}", TemplateParas.TemplateName);
            var dic = GetNVelocityVars();
            dic.Add("tableInfo", tableInfo);
          
            string str = nVelocityHelper.GenByTemplate(path, dic);


            string title = tableName + "." + TemplateParas.CodeLanguage;

            if (TemplateParas.IsShowGenCode)
            {
                CodeShow(title, str);
            }
            else
            {

                FileHelper.Write(TemplateParas.SaveFilePath + title, new[] { str }, SaveFileEncoding);
            }
        }

       public GenTemplate()
       {
           AddContextMenu();
       }
    }
}

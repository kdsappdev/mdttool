using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Common;
using MDT.Tools.Fix.Common.Model;
using MDT.Tools.Fix.Common.Utils;
using MDT.Tools.Template.Plugin.Model;
using MDT.Tools.Template.Plugin.Utils;
using Message = MDT.Tools.Fix.Common.Model.Message;
namespace MDT.Tools.Template.Plugin.Gen
{
    public class GenTemplate : AbstractHandler
    {
        public TemplateParas TemplateParas { get; set; }
        public List<FieldDic> FieldDics { get; set; }

        public void process(object[] o)
        {
            try
            {
               
                SaveFileEncoding = Encoding.GetEncoding(TemplateParas.SaveFileEncoding);
                CodeLanguage = TemplateParas.CodeLanguage;
                OutPut = TemplateParas.SaveFilePath;
                setEnable(false);
                setStatusBar("");
                string[] strs = null;
                if (!TemplateParas.IsShowGenCode)
                {
                    FileHelper.DeleteDirectory(OutPut);
                    setStatusBar(string.Format("正在生成{0}模板代码", TemplateParas.TemplateName));
                    setProgreesEditValue(0);
                    setProgress(0);
                    setProgressMax(o.Length);
                }
                if (TemplateParas.IsAutoGenSaveFileName)
                {
                    for (int i = 0; i < o.Length; i++)
                    {
                        if (!TemplateParas.IsShowGenCode)
                        {
                            setStatusBar(string.Format("正在生成{0}模板,共{1}个代码，已生成了{2}个代码", TemplateParas.TemplateName,
                                                       o.Length, i));
                            setProgress(1);
                        }
                        GenCode(o[i]);
                    }
                }
                else
                {
                    if (!TemplateParas.IsShowGenCode)
                    {
                        setProgress(100);
                    }
                     GenCode(o);   
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
                    if (TemplateParas.IsAutoGenSaveFileName)
                    {
                        for (int i = 0; i < tableInfos.Count; i++)
                        {
                            if (!TemplateParas.IsShowGenCode)
                            {
                                setStatusBar(string.Format("正在生成{0}模板{1}的代码,共{2}个代码，已生成了{3}个代码",
                                                           TemplateParas.TemplateName,
                                                           tableInfos[i].TableName,
                                                           drTables.Length, i));
                                setProgress(1);
                            }
                            GenCode(tableInfos[i]);

                        }
                    }
                    else
                    {
                        if (!TemplateParas.IsShowGenCode)
                        {
                            setProgress(100);
                        }
                        GenCode();
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

        /// <summary>
        /// 逐个生成文件
        /// </summary>
        /// <param name="o"></param>
        public void GenCode(object o)
        {
            NVelocityHelper nVelocityHelper = new NVelocityHelper(FilePathHelper.TemplatesPath);
            Header header = o as Header;
            Trailer trailer = o as Trailer;
            Message message = o as Message;
            Component component = o as Component;
            FieldDic field = o as FieldDic;
            string tableName = "";

            string path = string.Format(@"{0}", TemplateParas.TemplateName);
            var dic = new Dictionary<string, object>();
            if (header != null)
            {
                tableName = "header";
                dic.Add("header", header);
            }
            if (trailer != null)
            {
                tableName = "trailer";
                dic.Add("trailer", trailer);
            }
            if (message != null)
            {
                tableName = message.Name;
                dic.Add("message", message);
            }
            if (component != null)
            {
                tableName = component.Name;
                dic.Add("component", component);
            }
            if (field != null)
            {
                tableName = field.Name;
                dic.Add("field", field);
            }
            
            FixHelper.FieldDics = FieldDics;
            dic.Add("FixHelper",new FixHelper());
            dic.Add("codeGenHelper", new CodeGenHelper());
            string str = nVelocityHelper.GenByTemplate(path, dic);


            string title = tableName + "." + (TemplateParas.CodeLanguage+"").ToLower();

            if (!TemplateParas.IsAutoGenSaveFileName)
            {
                title = TemplateParas.SaveFileName;
            }

            if (TemplateParas.IsShowGenCode)
            {
                CodeShow(title, str);
            }

            else
            {

                FileHelper.Write(TemplateParas.SaveFilePath + title, new[] { str }, SaveFileEncoding);
            }
        }
        /// <summary>
        /// 生成指定单一文件
        /// </summary>
        /// <param name="o"></param>
        public void GenCode(object[] o)
        {
            NVelocityHelper nVelocityHelper = new NVelocityHelper(FilePathHelper.TemplatesPath);
            Header header = o[0] as Header;
            Trailer trailer = o[0] as Trailer;
            Message message = o[0] as Message;
            Component component = o[0] as Component;
            FieldDic field = o[0] as FieldDic;
            string tableName = "";

            string path = string.Format(@"{0}", TemplateParas.TemplateName);
            var dic = new Dictionary<string, object>();
            if (header != null)
            {
                tableName = "header";
                dic.Add("header", o[0]);
            }
            if (trailer != null)
            {
                tableName = "trailer";
                dic.Add("trailer", o[0]);
            }
            if (message != null)
            {
                tableName = message.Name;
                dic.Add("messages", o);
            }
            if (component != null)
            {
                tableName = component.Name;
                dic.Add("components", o);
            }
            if (field != null)
            {
                tableName = field.Name;
                dic.Add("fields", o);
            }
            
            FixHelper.FieldDics = FieldDics;
            dic.Add("FixHelper", new FixHelper());
            dic.Add("codeGenHelper", new CodeGenHelper());
            string str = nVelocityHelper.GenByTemplate(path, dic);
            string title = tableName + "." + (TemplateParas.CodeLanguage + "").ToLower();

            if (!TemplateParas.IsAutoGenSaveFileName)
            {
                title = TemplateParas.SaveFileName;
            }

            if (TemplateParas.IsShowGenCode)
            {
                CodeShow(title, str);
            }
            else
            {
                FileHelper.Write(TemplateParas.SaveFilePath + title, new[] { str }, SaveFileEncoding);
            }
        }

      
        public void GenCode(TableInfo tableInfo)
        {
            NVelocityHelper nVelocityHelper = new NVelocityHelper(FilePathHelper.TemplatesPath);
            string tableName = tableInfo.TableName;

            string path = string.Format(@"{0}", TemplateParas.TemplateName);
            var dic = GetNVelocityVars();
            dic.Add("tableInfo", tableInfo);

            string str = nVelocityHelper.GenByTemplate(path, dic);


            string title = tableName + "." + (TemplateParas.CodeLanguage+"").ToLower();
            if (!TemplateParas.IsAutoGenSaveFileName)
            {
                title = TemplateParas.SaveFileName;
            }
            if (TemplateParas.IsShowGenCode)
            {
                CodeShow(title, str);
            }
            else
            {

                FileHelper.Write(TemplateParas.SaveFilePath + title, new[] { str }, SaveFileEncoding);
            }
        }


        public void GenCode()
        {
            NVelocityHelper nVelocityHelper = new NVelocityHelper(FilePathHelper.TemplatesPath);
            string path = string.Format(@"{0}", TemplateParas.TemplateName);
            var dic = GetNVelocityVars();
            string str = nVelocityHelper.GenByTemplate(path, dic);
            string title = "";
            if (!TemplateParas.IsAutoGenSaveFileName)
            {
                title = TemplateParas.SaveFileName;
            }
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

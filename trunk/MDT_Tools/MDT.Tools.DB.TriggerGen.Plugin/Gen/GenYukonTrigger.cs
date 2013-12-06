using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.DB.Common;
using System.Data;
using System.Windows.Forms;
using MDT.Tools.DB.TriggerGen.Plugin.Utils;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.DB.TriggerGen.Plugin.Gen
{
    public class GenYukonTrigger: IGenTrigger
    {

        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            foreach (DataRow drTable in drTables)
            {
                DataRow[] drTableColumns = dsTableColumns.Tables[dbName + DBtablesColumns].Select("TABLE_NAME = '" + drTable["name"].ToString() + "'", "COLUMN_ID ASC");

                string tableName = (drTable["name"] as string).Trim().ToUpper();
                this.TriggerName = TriggerInfo.YukonTriggerName.Replace("$TABLENAME", tableName);
                string temp = GenCode(drTable, drTableColumns, tableName, this.TriggerName);
                display(temp);
            }
        }

        private  string GenCode(DataRow drTable, DataRow[] drTableColumns,string tableName,string triggerName)
        {
            StringBuilder sb = new StringBuilder();          
            bool isExistAuditAction = false;
            foreach (DataRow dr in drTableColumns)
            {
                string columnName = dr["COLUMN_NAME"] as string;
                if ("AUDIT_ACTION".Equals(columnName))
                {
                    isExistAuditAction = true;
                    break;
                }
            }
            genYukonTG(drTableColumns, sb, tableName, triggerName, isExistAuditAction);
            
            return sb.ToString();
        }

        private void genYukonTG(System.Data.DataRow[] drTableColumns, StringBuilder sb, string tableName, string triggerName, bool isExistAuditAction)
        {
            sb.AppendFormat("--Yukon TG").AppendFormat("\r\n");
            #region MQ触发器

            sb.AppendFormat("CREATE OR REPLACE TRIGGER").AppendFormat(" ").AppendFormat(triggerName, tableName).AppendFormat("\r\n");
            if (!isExistAuditAction)
            {
                sb.AppendFormat("\t").AppendFormat("AFTER INSERT or UPDATE or DELETE").AppendFormat("\r\n");
            }
            else
            {
                sb.AppendFormat("\t").AppendFormat("AFTER INSERT").AppendFormat("\r\n");
            }
            sb.AppendFormat("\t").AppendFormat("ON ").AppendFormat(tableName).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("REFERENCING NEW AS NEW OLD AS OLD").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("FOR EACH ROW").AppendFormat("\r\n");
            sb.AppendFormat("DECLARE").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("v_action   VARCHAR (1)    := '';").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("v_msg      VARCHAR (2000) := '';").AppendFormat("\r\n");
            sb.AppendFormat("BEGIN").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("/*").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Function : This trigger will create bbox Aq automatic and call a procedure which send this message .").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Create Date : ").AppendFormat(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Create By : MDT_Tools .").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("*/").AppendFormat("\r\n");

            if (isExistAuditAction)
            {
                sb.AppendFormat("\t").AppendFormat("IF :NEW.AUDIT_ACTION = 'INSERT'").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("THEN").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("v_action := 'I';").AppendFormat("\r\n");

                sb.AppendFormat("\t").AppendFormat("ELSIF :NEW.AUDIT_ACTION = 'UPDATE'").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("THEN").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("v_action := 'U';").AppendFormat("\r\n");

                sb.AppendFormat("\t").AppendFormat("ELSIF :NEW.AUDIT_ACTION = 'DELETE'").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("THEN").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("v_action := 'D';").AppendFormat("\r\n");

                sb.AppendFormat("\t").AppendFormat("END IF;").AppendFormat("\r\n");

                //sb.AppendFormat("\t\t").AppendFormat("IF :NEW.AUDIT_CURRENT = 'Y' AND :NEW.AUDIT_ISHISTORY = 'N'").AppendFormat("\r\n");
                //sb.AppendFormat("\t\t").AppendFormat("THEN").AppendFormat("\r\n");

                sb.AppendFormat("\t\t").AppendFormat("v_msg :=").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("'ACTION:'").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("|| v_action").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("|| CHR (10)").AppendFormat("\r\n");

                for (int i = 0; i < drTableColumns.Length; i++)
                {
                    DataRow dr = drTableColumns[i];
                    string columnName = dr["COLUMN_NAME"] as string;
                    sb.AppendFormat("\t\t\t").AppendFormat("|| '").AppendFormat(columnName).AppendFormat(":'").AppendFormat("\r\n");
                    if (i != drTableColumns.Length - 1)
                    {
                        sb.AppendFormat("\t\t\t").AppendFormat("|| ").AppendFormat(getDataRowValue(dr, columnName, ":NEW.")).AppendFormat("\r\n");
                        sb.AppendFormat("\t\t\t").AppendFormat("|| CHR (10)").AppendFormat("\r\n");
                    }
                    else
                    {
                        sb.AppendFormat("\t\t\t").AppendFormat("|| ").AppendFormat(getDataRowValue(dr, columnName, ":NEW.")).AppendFormat(";").AppendFormat("\r\n");
                    }
                }
            }
            else
            {
                sb.AppendFormat("\t").AppendFormat("IF DELETING").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("THEN").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("v_action := 'D';").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("v_msg :=").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("'ACTION:'").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("|| v_action").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("|| CHR (10)").AppendFormat("\r\n");


                for (int i = 0; i < drTableColumns.Length; i++)
                {
                    DataRow dr = drTableColumns[i];
                    string columnName = dr["COLUMN_NAME"] as string;
                    sb.AppendFormat("\t\t\t").AppendFormat("|| '").AppendFormat(columnName).AppendFormat(":'").AppendFormat("\r\n");
                    if (i != drTableColumns.Length - 1)
                    {
                        sb.AppendFormat("\t\t\t").AppendFormat("|| ").AppendFormat(getDataRowValue(dr, columnName, ":OLD.")).AppendFormat("\r\n");
                        sb.AppendFormat("\t\t\t").AppendFormat("|| CHR (10)").AppendFormat("\r\n");
                    }
                    else
                    {
                        sb.AppendFormat("\t\t\t").AppendFormat("|| ").AppendFormat(getDataRowValue(dr, columnName, ":OLD.")).AppendFormat(";").AppendFormat("\r\n");
                    }
                }

                sb.AppendFormat("\t").AppendFormat("ELSIF INSERTING OR UPDATING").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("THEN").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("IF INSERTING").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("THEN").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("v_action := 'I';").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat(" ELSIF UPDATING").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("THEN").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("v_action := 'U';").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("END IF;").AppendFormat("\r\n");

                sb.AppendFormat("\r\n");

                sb.AppendFormat("\t\t").AppendFormat("v_msg :=").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t\t").AppendFormat("'ACTION:'").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("|| v_action").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("|| CHR (10)").AppendFormat("\r\n");

                for (int i = 0; i < drTableColumns.Length; i++)
                {
                    DataRow dr = drTableColumns[i];
                    string columnName = dr["COLUMN_NAME"] as string;
                    sb.AppendFormat("\t\t\t").AppendFormat("|| '").AppendFormat(columnName).AppendFormat(":'").AppendFormat("\r\n");
                    if (i != drTableColumns.Length - 1)
                    {
                        sb.AppendFormat("\t\t\t").AppendFormat("|| ").AppendFormat(getDataRowValue(dr, columnName, ":NEW.")).AppendFormat("\r\n");
                        sb.AppendFormat("\t\t\t").AppendFormat("|| CHR (10)").AppendFormat("\r\n");
                    }
                    else
                    {
                        sb.AppendFormat("\t\t\t").AppendFormat("|| ").AppendFormat(getDataRowValue(dr, columnName, ":NEW.")).AppendFormat(";").AppendFormat("\r\n");
                    }
                }
                sb.AppendFormat("\t").AppendFormat("END IF;").AppendFormat("\r\n");
            }

            sb.AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("enqueue_bbox_trigger ('").AppendFormat(tableName).AppendFormat("', 'BBOX_USE', v_msg);").AppendFormat("\r\n");
            sb.AppendFormat("END ").AppendFormat(";").AppendFormat("\r\n");

            #endregion
        }

        private string getDataRowValue(DataRow dr, string columnName, string head)
        {
            string result;
            string dateType = dr["DATA_TYPE"] as string;
            switch (dateType)
            {
                case "DATE":
                    result = "TO_CHAR(" + head + columnName + ",'YYYY-MM-DD HH24:MI:SS')";
                    break;
                case "CLOB":
                    result = "TO_CHAR(" + head + columnName + ")";
                    break;
                default:
                    result = head + columnName;
                    break;
            }

            return result;
        }

    }
}

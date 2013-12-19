using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.DB.Common;
using System.Data;
using MDT.Tools.DB.TriggerGen.Plugin.Utils;

namespace MDT.Tools.DB.TriggerGen.Plugin.Gen
{
    public class GenMQTrigger : IGenTrigger
    {
        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            foreach (DataRow drTable in drTables)
            {
                DataRow[] drTableColumns = dsTableColumns.Tables[dbName + DBtablesColumns].Select("TABLE_NAME = '" + drTable["name"].ToString() + "'", "COLUMN_ID ASC");

                string tableName = (drTable["name"] as string).Trim().ToUpper();
                this.TriggerName = TriggerInfo.YukonTriggerName.Replace("$TABLENAME", tableName);

                string temp = GenCode(drTable, drTableColumns);
                display(temp,TriggerName);
            }     
        }

        private  string GenCode(DataRow drTable, DataRow[] drTableColumns)
        {
            StringBuilder sb = new StringBuilder();
            string tableName = (drTable["name"] as string).Trim().ToUpper();
            string prodcedureName = TriggerInfo.MQProdcedureName.Replace("$TABLENAME", tableName);
            string triggerName = TriggerInfo.MQTriggerName.Replace("$TABLENAME", tableName);
            string topic = TriggerInfo.MQTopicName.Replace("$TABLENAME", tableName);
            bool isExistAuditAction = false;//是否是AuditAction结构表
            bool isExistLocation = false;
            foreach (DataRow dr in drTableColumns)
            {
                string columnName = dr["COLUMN_NAME"] as string;
                if ("AUDIT_ACTION".Equals(columnName))
                {
                    isExistAuditAction = true;
                }
                if ("LOCATION".Equals(columnName))
                {
                    isExistLocation = true;
                }
            }
            if (isExistAuditAction)
            {
                gen2(drTableColumns, sb, tableName, prodcedureName, triggerName, topic, isExistLocation);
            }
            else
            {
                gen1(drTableColumns, sb, tableName, prodcedureName, triggerName, topic, isExistLocation);
            }
            //string title = Utils.CodeGenHelper.StrFirstToUpperRemoveUnderline(drTable["name"] as string + "MQ TG SP") + ".sql";
            //SqlForm mf = new SqlForm(goi.CodeGenForDskGui.mdiPanel1.MdiForm, sb.ToString(), title, goi.DbConfigInfo);
            //mf.Show();
            return sb.ToString();
        }

        private  void gen1(DataRow[] drTableColumns, StringBuilder sb, string tableName, string prodcedureName, string triggerName, string topic, bool isExistLoation)
        {
            #region MQ存储过程
            sb.AppendFormat("CREATE OR REPLACE PROCEDURE").AppendFormat(" ").AppendFormat(prodcedureName, tableName).AppendFormat(" (").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("p_table    IN   VARCHAR2,").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("p_action   IN   VARCHAR2,").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("m_audit_current IN VARCHAR2,").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("m_audit_ishistory IN VARCHAR2,").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("p_row      IN   ").AppendFormat(tableName).AppendFormat("%ROWTYPE").AppendFormat("\r\n");
            sb.AppendFormat(")").AppendFormat("\r\n");
            sb.AppendFormat("IS").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("v_msg     VARCHAR2 (4000)     := '';").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("p_count   NUMBER;").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("p_name    VARCHAR2 (100 BYTE);").AppendFormat("\r\n");
            sb.AppendFormat("BEGIN").AppendFormat("\r\n");
            //注释
            sb.AppendFormat("\t").AppendFormat("/*").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Function : This procedure will crate ATSFeedMessage automatic and send message to AMQII .").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Notice : This procedure crate automatic , so please do not modify this procedure outside .").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Create Date : ").AppendFormat(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Create By : CodeGenForDsk .").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("*/").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- Start Create ATSFeedMessage").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("SELECT 'ATSFEEDMEESAGEVERSION:0.1' || CHR(23) INTO v_msg FROM DUAL;").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- Start Create ATSFeedMessage Header").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'ATSFEEDHEADER:Header' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'Primary:' || p_table || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'Source:MQ' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'Action:' || p_action || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'Code:0000' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'Comment:Success' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- End Of Create ATSFeedMessage Header").AppendFormat("\r\n");

            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- Start Create ATSFeedMessage Body").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" SELECT v_msg || 'TABLE:' || UPPER(p_table) || CHR(23)    INTO v_msg    FROM DUAL;").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            foreach (DataRow dr in drTableColumns)
            {
                string columnName = dr["COLUMN_NAME"] as string;
                if ("AUDIT_ACTION".Equals(columnName) || "AUDIT_CURRENT".Equals(columnName) || "AUDIT_ISHISTORY".Equals(columnName))
                {
                    continue;
                }
                sb.AppendFormat("\t").AppendFormat("IF p_row.").AppendFormat(columnName).AppendFormat(" IS NOT NULL").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("THEN").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("   SELECT    v_msg").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("   || '").AppendFormat(StrProperty(columnName).ToUpper()).AppendFormat("\"'").AppendFormat("\r\n");
                string dateType = dr["DATA_TYPE"] as string;
                if ("DATE".Equals(dateType))
                {
                    sb.AppendFormat("\t").AppendFormat("   || TO_CHAR(p_row.").AppendFormat(columnName).AppendFormat(",'YYYY-MM-DD HH24:MI:SS'").AppendFormat("\r\n");
                }
                else
                {
                    sb.AppendFormat("\t").AppendFormat("   || p_row.").AppendFormat(columnName).AppendFormat("\r\n");
                }
                sb.AppendFormat("\t").AppendFormat("   || CHR (23)").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("   INTO v_msg").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("   FROM DUAL;").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("END IF;").AppendFormat("\r\n");
                sb.AppendFormat("\r\n");
            }
            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'AUDITACTION\"' || p_action || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'AUDITCURRENT\"' || m_audit_current || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'AUDITISHISTORY\"' || m_audit_ishistory || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("-- End Of Create ATSFeedMessage Body").AppendFormat("\r\n");

            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- Start Create ATSFeedMessage Trailer").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'ATSFEEDTRAILER:Trailer' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" SELECT v_msg || 'SignatureLength' || CHR (38) || '0' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'Signature\"' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'CheckSum' || CHR (38) || '196' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- End Of Create ATSFeedMessage Trailer").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- End Of Create ATSFeedMessage").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- Start Of Send Message").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("enqueue_message (").AppendFormat(isExistLoation ? topic.Replace("$LOCATION", "p_row.LOCATION") : topic.Replace("$LOCATION", "'PINGO'")).AppendFormat(",").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("'External',").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(");").AppendFormat("\r\n");
            sb.AppendFormat("EXCEPTION").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("WHEN OTHERS").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("THEN").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("\t").AppendFormat("RAISE;").AppendFormat("\r\n");
            sb.AppendFormat("END ").AppendFormat(";").AppendFormat("\r\n");

            #endregion
            sb.AppendFormat("--MQ TG").AppendFormat("\r\n");
            #region MQ触发器

            sb.AppendFormat("CREATE OR REPLACE TRIGGER").AppendFormat(" ").AppendFormat(triggerName, tableName).AppendFormat("\r\n");
            sb.AppendFormat("AFTER INSERT or UPDATE or DELETE").AppendFormat("\r\n");
            sb.AppendFormat("ON ").AppendFormat(tableName).AppendFormat(" FOR EACH ROW").AppendFormat("\r\n");
            sb.AppendFormat("DECLARE").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("m_audit_action").AppendFormat("\t").AppendFormat("VARCHAR2 (60 BYTE);").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("m_audit_current").AppendFormat("\t").AppendFormat("VARCHAR2 (60 BYTE);").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("m_audit_ishistory").AppendFormat("\t").AppendFormat("VARCHAR2 (60 BYTE);").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("p_row").AppendFormat("\t").AppendFormat(tableName).AppendFormat("%ROWTYPE;").AppendFormat("\r\n");
            sb.AppendFormat("BEGIN").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("/*").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Function : This trigger will call a procedure which will create ATSFeedMessage automatic and send message to AMQII .").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Notice : This trigger crate automatic , so please do not modify this trigger outside .").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Create Date : ").AppendFormat(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Create By : CodeGenForDsk .").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("*/").AppendFormat("\r\n");
            foreach (DataRow dr in drTableColumns)
            {
                string columnName = dr["COLUMN_NAME"] as string;
                sb.AppendFormat("\t").AppendFormat("p_row.").AppendFormat(columnName).AppendFormat(" :=").AppendFormat(" :new.").AppendFormat(columnName).AppendFormat(";").AppendFormat("\r\n");

            }
            #region Insert
            sb.AppendFormat("\t").AppendFormat("IF INSERTING").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("THEN").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("m_audit_action :='INSERT';").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("m_audit_current :='Y';").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("m_audit_ishistory :='N';").AppendFormat("\r\n");
            #endregion
            #region Update
            sb.AppendFormat("\t").AppendFormat("ELSIF UPDATING").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("THEN").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("m_audit_action :='UPDATE';").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("m_audit_current :='Y';").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("m_audit_ishistory :='N';").AppendFormat("\r\n");
            #endregion

            #region Delete
            sb.AppendFormat("\t").AppendFormat("ELSIF DELETING").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("THEN").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("m_audit_action :='DELETE';").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("m_audit_current :='N';").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("m_audit_ishistory :='Y';").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("END IF;").AppendFormat("\r\n");
            #endregion
            sb.AppendFormat("\t").AppendFormat(prodcedureName).AppendFormat("('").AppendFormat(tableName).AppendFormat("',m_audit_action,m_audit_current,m_audit_ishistory").AppendFormat(",p_row);").AppendFormat("\r\n");
            sb.AppendFormat("END ").AppendFormat(";").AppendFormat("\r\n");

            #endregion
        }

        private  void gen2(DataRow[] drTableColumns, StringBuilder sb, string tableName, string prodcedureName, string triggerName, string topic, bool isExistLoation)
        {
            #region MQ存储过程
            sb.AppendFormat("CREATE OR REPLACE PROCEDURE").AppendFormat(" ").AppendFormat(prodcedureName, tableName).AppendFormat(" (").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("p_table    IN   VARCHAR2,").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("p_action   IN   VARCHAR2,").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("p_row      IN   ").AppendFormat(tableName).AppendFormat("%ROWTYPE").AppendFormat("\r\n");
            sb.AppendFormat(")").AppendFormat("\r\n");
            sb.AppendFormat("IS").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("v_msg     VARCHAR2 (4000)     := '';").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("p_count   NUMBER;").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("p_name    VARCHAR2 (100 BYTE);").AppendFormat("\r\n");
            sb.AppendFormat("BEGIN").AppendFormat("\r\n");
            //注释
            sb.AppendFormat("\t").AppendFormat("/*").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Function : This procedure will crate ATSFeedMessage automatic and send message to AMQII .").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Notice : This procedure crate automatic , so please do not modify this procedure outside .").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Create Date : ").AppendFormat(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Create By : CodeGenForDsk .").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("*/").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- Start Create ATSFeedMessage").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("SELECT 'ATSFEEDMEESAGEVERSION:0.1' || CHR(23) INTO v_msg FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("-- Start Create ATSFeedMessage Header").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'ATSFEEDHEADER:Header' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'Primary:' || p_table || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'Source:MQ' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'Action:' || p_action || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'Code:0000' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'Comment:Success' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");

            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- End Of Create ATSFeedMessage Header").AppendFormat("\r\n");

            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- Start Create ATSFeedMessage Body").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" SELECT v_msg || 'TABLE:' || UPPER(p_table) || CHR(23)    INTO v_msg    FROM DUAL;").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            foreach (DataRow dr in drTableColumns)
            {
                string columnName = dr["COLUMN_NAME"] as string;
                sb.AppendFormat("\t").AppendFormat("IF p_row.").AppendFormat(columnName).AppendFormat(" IS NOT NULL").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("THEN").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("   SELECT    v_msg").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("   || '").AppendFormat(StrProperty(columnName).ToUpper()).AppendFormat("\"'").AppendFormat("\r\n");
                string dateType = dr["DATA_TYPE"] as string;
                if ("DATE".Equals(dateType))
                {
                    sb.AppendFormat("\t").AppendFormat("   || TO_CHAR(p_row.").AppendFormat(columnName).AppendFormat(",'YYYY-MM-DD HH24:MI:SS')").AppendFormat("\r\n");
                }
                else
                {
                    sb.AppendFormat("\t").AppendFormat("   || p_row.").AppendFormat(columnName).AppendFormat("\r\n");
                }
                sb.AppendFormat("\t").AppendFormat("   || CHR (23)").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("   INTO v_msg").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("   FROM DUAL;").AppendFormat("\r\n");
                sb.AppendFormat("\t").AppendFormat("END IF;").AppendFormat("\r\n");
                sb.AppendFormat("\r\n");
            }

            sb.AppendFormat("\t").AppendFormat("-- End Of Create ATSFeedMessage Body").AppendFormat("\r\n");

            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- Start Create ATSFeedMessage Trailer").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'ATSFEEDTRAILER:Trailer' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" SELECT v_msg || 'SignatureLength' || CHR (38) || '0' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'Signature\"' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("SELECT v_msg || 'CheckSum' || CHR (38) || '196' || CHR (23)").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  INTO v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("  FROM DUAL;").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- End Of Create ATSFeedMessage Trailer").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- End Of Create ATSFeedMessage").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("-- Start Of Send Message").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("enqueue_message (").AppendFormat(isExistLoation ? topic.Replace("$LOCATION", "p_row.LOCATION") : topic.Replace("$LOCATION", "'PINGO'")).AppendFormat(",").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("'External',").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("v_msg").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(");").AppendFormat("\r\n");
            sb.AppendFormat("EXCEPTION").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("WHEN OTHERS").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("THEN").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("\t").AppendFormat("RAISE;").AppendFormat("\r\n");
            sb.AppendFormat("END ").AppendFormat(";").AppendFormat("\r\n");

            #endregion
            sb.AppendFormat("--MQ TG").AppendFormat("\r\n");
            #region MQ触发器

            sb.AppendFormat("CREATE OR REPLACE TRIGGER").AppendFormat(" ").AppendFormat(triggerName, tableName).AppendFormat("\r\n");
            sb.AppendFormat("AFTER INSERT or UPDATE or DELETE").AppendFormat("\r\n");
            sb.AppendFormat("ON ").AppendFormat(tableName).AppendFormat(" FOR EACH ROW").AppendFormat("\r\n");
            sb.AppendFormat("DECLARE").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("p_row").AppendFormat("\t").AppendFormat(tableName).AppendFormat("%ROWTYPE;").AppendFormat("\r\n");
            sb.AppendFormat("BEGIN").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("/*").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Function : This trigger will call a procedure which will create ATSFeedMessage automatic and send message to AMQII .").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Notice : This trigger crate automatic , so please do not modify this trigger outside .").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Create Date : ").AppendFormat(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("* Create By : CodeGenForDsk .").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("*/").AppendFormat("\r\n");
            foreach (DataRow dr in drTableColumns)
            {
                string columnName = dr["COLUMN_NAME"] as string;
                sb.AppendFormat("\t").AppendFormat("p_row.").AppendFormat(columnName).AppendFormat(" :=").AppendFormat(" :new.").AppendFormat(columnName).AppendFormat(";").AppendFormat("\r\n");
            }
            sb.AppendFormat("\t").AppendFormat(prodcedureName).AppendFormat("('").AppendFormat(tableName).AppendFormat("',p_row.Audit_Action").AppendFormat(",p_row);").AppendFormat("\r\n");
            sb.AppendFormat("END ").AppendFormat(";").AppendFormat("\r\n");

            #endregion
        }

        private  string StrProperty(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = StrFirstToUpperRemoveUnderline(str);
            }
            return str;
        }
        private  string StrFirstToUpperRemoveUnderline(string str)
        {
            string temp = str;
            if (!string.IsNullOrEmpty(str))
            {
                temp = temp.ToLower();
                string[] temps = temp.ToLower().Split(new char[] { '_' });
                temp = "";
                foreach (string s in temps)
                {
                    temp += StrFirstToUpper(s);
                }
            }
            return temp;
        }

        private  string StrFirstToUpper(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str[0].ToString().ToUpper() + str.Substring(1, str.Length - 1);
            }
            return str;
        }
    }
}

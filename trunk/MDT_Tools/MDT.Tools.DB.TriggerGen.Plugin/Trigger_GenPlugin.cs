using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.DB.Common;
using MDT.Tools.Core.Plugin;
using System.Threading;
using System.Windows.Forms;
using System.Data;
using MDT.Tools.DB.TriggerGen.Plugin.Gen;
using MDT.Tools.DB.TriggerGen.Plugin.Utils;

namespace MDT.Tools.DB.TriggerGen.Plugin
{
    public class Trigger_GenPlugin : DBSubPlugin
    {
        #region 插件信息

        private int _tag = 5;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 15; }
        }

        public override string PluginName
        {
            get { return "Trigger生成插件"; }
        }

        public override string Description
        {
            get { return "根据数据库表结构生成Trigger."; }
        }

        public override string Author
        {
            get { return "xyb"; }
        }

        #endregion

        private readonly ToolStripMenuItem _tsiYukonTriggerGen = new ToolStripMenuItem();
        private readonly ToolStripMenuItem _tsiMQTriggerGen = new ToolStripMenuItem();
        protected override void AddContextMenu()
        {
            if (Application.MainContextMenu.InvokeRequired)
            {
                Action method = new Action(AddContextMenu);
                this.Application.MainContextMenu.Invoke(method, null);
            }
            else
            {
                base.AddContextMenu();
                this._tsiGen.Text = "Trigger生成";
                this._tsiYukonTriggerGen.Text = "Java Aq触发器生成";
                this._tsiMQTriggerGen.Text = "Atf 触发器生成";
                this._tsiGen.Enabled = false;

                this._tsiGen.DropDownItems.AddRange(new[] {this._tsiYukonTriggerGen ,this._tsiMQTriggerGen});
                this._tsiYukonTriggerGen.Click += new EventHandler(_tsiYukonTriggerGen_Click);
                this._tsiMQTriggerGen.Click += new EventHandler(_tsiMQTriggerGen_Click);
            }
        }

        void _tsiMQTriggerGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

            TriggerParams tp = new TriggerParams();
            tp.DrTable = drTable;
            tp.Trigger = TriggerType.MQ;
            //ThreadPool.QueueUserWorkItem(o => GenTrigger(tp));
            GenTrigger(tp);
        }

        void _tsiYukonTriggerGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];
           
            TriggerParams tp = new TriggerParams();
            tp.DrTable = drTable;
            tp.Trigger = TriggerType.YuKon;
            //ThreadPool.QueueUserWorkItem(o => GenTrigger(tp));
            GenTrigger(tp);
        }

        private void GenTrigger(TriggerParams triggerParams)
        {
            IGenTrigger genTrigger=null ;
            switch (triggerParams.Trigger)
            {
                case TriggerType.YuKon:
                    genTrigger = new GenYukonTrigger();
                    break;
                case TriggerType.MQ:
                    genTrigger = new GenMQTrigger();                    
                    break;
                case TriggerType.None:
                default:
                    genTrigger = new GenNone();
                    break;
            }
          
            process(triggerParams.DrTable, genTrigger);
        }
    }  
}

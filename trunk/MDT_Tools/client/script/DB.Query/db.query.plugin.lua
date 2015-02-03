--��������Lua�ļ�
require("script\\CLRPackage")
--������򼯺������ռ�
import("System")
import("System.Windows.Forms")
import("System.Drawing")
import("System.Collections.Generic")
import("System.IO")
import("System.Text")
import("System.Data")
import("System.Threading")
import("MDT.Tools.Core","MDT.Tools.Core.UI")
import("MDT.Tools.Core","MDT.Tools.Core.Utils")
import("MDT.ThirdParty.Controls","WeifenLuo.WinFormsUI.Docking")
import("MDT.ThirdParty.Controls","ICSharpCode.TextEditor")
import("MDT.ThirdParty.Controls","ICSharpCode.TextEditor.Document")

import("MDT.Tools.Core","MDT.Tools.Core.Resources")
import("MDT.Tools.DB.Common","MDT.Tools.DB.Common")
import("MDT.Tools.Core","DNCCFrameWork.DataAccess")
--��ȡ��Form
application=getApplication()

--�����Ϣ
local tag=51
local pluginKey=51
local pluginName='�鿴���ݿ����ݲ��'
local description='�鿴��ǰ���ݿ�ı�������Ϣ'
local author='�׵�˧'
local version='1.0.0.0'
--�������:��ʼ��
function init()
	return tag,pluginKey,pluginName,description,author,version
end
--������ť

setcolumnTSMI=ToolStripMenuItem()

--�������:����
function load()
	debug(string.format("%d %s", pluginKey,"load"))--������־	     
	setcolumnTSMI.Text="Sql��ѯ����"
	setcolumnTSMI.Image=Image.FromFile("script\\db.query\\dbquery.ico")
	setcolumnTSMI.Click:Add(setcolumnTSMI_click)--����Click�¼�
	
	getObject(43,"tsmiTool").DropDownItems:Add(setcolumnTSMI)
	subscribe("BroadCastDBEnable",pluginKey)
	
end
--��ť�¼�
function setcolumnTSMI_click(sender,args)

 local dbName=getObject(1,"DBCurrentDBName")
	local splitContainer1=SplitContainer()
	
	 splitContainer1.Dock = DockStyle.Fill
	splitContainer1.Orientation = Orientation.Horizontal
	splitContainer1.SplitterDistance = 30
	
	
	
	btnSave= Button()
	btnSave.Image = Resources.start
	btnSave.Text="ִ��"
	btnSave.Click:Add(btnSave_Click)
	
	tbSql=TextEditorControl()
 
	 
	 tbSql.Dock = DockStyle.Fill;
            
            --tbSql.LineViewerStyle = LineViewerStyle.FullRow
         
           
             tbSql.AllowCaretBeyondEOL = false;
            tbSql.ShowEOLMarkers = false;
            tbSql.ShowHRuler = false;
            tbSql.ShowInvalidLines = false;
            tbSql.ShowSpaces = false;
            tbSql.ShowTabs = false;
            tbSql.ShowVRuler = false;
            tbSql.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("TSQL")
	 
	local tableLayoutPanel2=TableLayoutPanel()
	tableLayoutPanel2.ColumnCount = 6
	tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Absolute, 84))
	tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Absolute, 300))
	tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Absolute, 131))
	tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Percent, 100))
	
	tableLayoutPanel2.Dock = DockStyle.Fill
	tableLayoutPanel2.RowCount = 2
	tableLayoutPanel2.RowStyles:Add(RowStyle(SizeType.Absolute, 35))
	tableLayoutPanel2.RowStyles:Add(RowStyle(SizeType.Percent, 100))
	tableLayoutPanel2.Controls:Add(btnSave, 0, 0)
	tableLayoutPanel2.Controls:Add(tbSql, 0, 1)
	tableLayoutPanel2:SetColumnSpan(tbSql, tableLayoutPanel2.ColumnCount)
	 
	 
	 gv=DataGridView()
	gv.AutoGenerateColumns=true	
	gv.ReadOnly=true
	gv.Dock=DockStyle.Fill
	gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
	gv.AllowUserToAddRows = false;
    gv.AllowUserToDeleteRows = false;
	
	




local dc=DockContent()
dc.Text=dbName..setcolumnTSMI.Text

	
	 splitContainer1.Panel1.Controls:Add(tableLayoutPanel2)
 
     splitContainer1.Panel2.Controls:Add(gv)

dc.Controls:Add(splitContainer1)
dc:show(application.Panel)

 
end
 

function btnSave_Click(sender,args)
local suc,err=pcall(function()	 
local originalEncoding=getObject(1,"OriginalEncoding")
local targetEncoding=getObject(1,"TargetEncoding")

local dbConnectionString=getObject(1,"DBCurrentDBConnectionString")
local dbType=getObject(1,"DBCurrentDBType")
	 
local db = DbFactory(dbConnectionString,DBType.GetDbProviderString(dbType)).IDbHelper
local temp = DataSet()
local sql=tbSql.Text
sql=EncodingHelper.ConvertEncoder(targetEncoding, originalEncoding,sql)
local strs=String[1]
strs[0]="Table1"
db:Fill(sql, temp,strs)

temp=DBFileHelper.ConvertDataSet(temp,originalEncoding,targetEncoding)
gv.DataMember = strs[0]
gv.DataSource=temp
CallCtrlWithThreadSafety.RefreshGridViewDataSource(gv,gv)
end)
if not suc then
		MessageBox.Show(application.MainMenu, err.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information)
	end
end

function unload()
	
end

--�������:�㲥���֮�乲�����Ϣ
function onNotify(name,o)
	if(name=="BroadCastDBEnable") then
	setcolumnTSMI.Enabled=o
	
	end
end

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
import("MDT.Tools.Core","MDT.Tools.Core.Resources")
import("MDT.Tools.DB.Common","MDT.Tools.DB.Common")
import("MDT.Tools.Core","DNCCFrameWork.DataAccess")
--��ȡ��Form
application=getApplication()

--�����Ϣ
local tag=50
local pluginKey=50
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
    _dbContextMenuStrip = getObject(1, "CmcSubPlugin")
	setcolumnTSMI.Text="�鿴����"
	_dbContextMenuStrip.Items:Insert(3, setcolumnTSMI)
	setcolumnTSMI.Click:Add(setcolumnTSMI_click)--����Click�¼�
	subscribe("BroadCast_CheckTableNumberIsGreaterThan0",pluginKey)
	
end
--��ť�¼�
function setcolumnTSMI_click(sender,args)
	local drs = getObject(1, "DBCurrentCheckTable");
	local originalEncoding=getObject(1,"OriginalEncoding")
	local targetEncoding=getObject(1,"TargetEncoding")
	local dbName=getObject(1,"DBCurrentDBName")
	local dbConnectionString=getObject(1,"DBCurrentDBConnectionString")
	local dbType=getObject(1,"DBCurrentDBType")
	local count=drs.Length-1
	 db = DbFactory(dbConnectionString,DBType.GetDbProviderString(dbType)).IDbHelper
	for i=0,count do
	local dr=drs[i]
	local tableLayoutPanel1=TableLayoutPanel()
	tableLayoutPanel1.ColumnCount=1
	tableLayoutPanel1.RowCount = 2
	tableLayoutPanel1.Dock=DockStyle.Fill
	tableLayoutPanel1.ColumnStyles:Add(ColumnStyle(SizeType.Percent, 100))
	tableLayoutPanel1.RowStyles:Add(RowStyle(SizeType.Absolute, 35))
	tableLayoutPanel1.RowStyles:Add(RowStyle(SizeType.Percent, 100))
	local btnRefersh= Button()
	btnRefersh.Image = Resources.start
	btnRefersh.Text="ˢ��"
	btnRefersh.Click:Add(btnRefersh_Click)
	
	 local tableName=getDataRowValue(dr,"NAME")
	
	local tableLayoutPanel2=TableLayoutPanel()
	tableLayoutPanel2.ColumnCount = 6
	tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Absolute, 84))
	tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Absolute, 300))
	tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Absolute, 131))
	tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Percent, 100))
	
	tableLayoutPanel2.Dock = DockStyle.Fill
	tableLayoutPanel2.RowCount = 1
	tableLayoutPanel2.RowStyles:Add(RowStyle(SizeType.Percent, 100))
	tableLayoutPanel2.Controls:Add(btnRefersh, 0, 0)
	 
	
	 
	 
	local gv=DataGridView()
	gv.AutoGenerateColumns=true	
	gv.Dock=DockStyle.Fill
	gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
	gv.AllowUserToAddRows = false;
    gv.AllowUserToDeleteRows = false;
	gv.ReadOnly=true
local obj= Object[4]
obj[0]=tableName
obj[1]=gv
obj[2]=originalEncoding
obj[3]=targetEncoding
 btnRefersh.Tag=obj



local dc=DockContent()
dc.Text=dbName.."["..tableName.."]"..setcolumnTSMI.Text

tableLayoutPanel1.Controls:Add(tableLayoutPanel2,0,0)
tableLayoutPanel1.Controls:Add(gv, 0, 1)
dc.Controls:Add(tableLayoutPanel1)
dc:show(application.Panel)
pcall(select_data,btnRefersh)

end
end



function select_data(btnRefersh)
local suc,err=pcall(function()	 
local tableName=btnRefersh.Tag[0]
local temp = DataSet()
local sql="select * from "..tableName.." where rownum>0 and rownum<2000"
local strs=String[1]
strs[0]=tableName
db:Fill(sql, temp,strs)
local originalEncoding=btnRefersh.Tag[2]
local targetEncoding=btnRefersh.Tag[3]
temp=DBFileHelper.ConvertDataSet(temp,originalEncoding,targetEncoding)
local gv=btnRefersh.Tag[1]
gv.DataMember = tableName;
gv.DataSource=temp
CallCtrlWithThreadSafety.RefreshGridViewDataSource(gv,gv)
end)
if not suc then
		MessageBox.Show(application.MainMenu, err.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information)
	end
end
 

function btnRefersh_Click(sender,args)
pcall(select_data,sender)
end

function unload()
	
end

--�������:�㲥���֮�乲�����Ϣ
function onNotify(name,o)
	if(name=="BroadCastDBEnable") then
	setcolumnTSMI.Enabled=o
	
	end
end

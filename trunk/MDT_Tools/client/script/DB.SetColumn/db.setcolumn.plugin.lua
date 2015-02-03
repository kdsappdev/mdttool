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

--��ȡ��Form
application=getApplication()

--�����Ϣ
local tag=45
local pluginKey=42
local pluginName='ά�����ݿ��е��ֶβ��'
local description='ά����ǰ���ݿ�������ֶ���Ϣ'
local author='�׵�˧'
local version='1.0.0.1'
--�������:��ʼ��
function init()
	return tag,pluginKey,pluginName,description,author,version
end
--������ť

setcolumnTSMI=ToolStripMenuItem()

--�������:����
function load()
	debug(string.format("%d %s", pluginKey,"load"))--������־
	setcolumnTSMI.Text="���ֶ�ά��"
	setcolumnTSMI.Image=Image.FromFile("script\\db.setcolumn\\dbsetcolumn.ico")
	setcolumnTSMI.Click:Add(setcolumnTSMI_click)--����Click�¼�
	
	getObject(43,"tsmiTool").DropDownItems:Add(setcolumnTSMI)
	subscribe("BroadCastDBEnable",pluginKey)
	
end
--��ť�¼�
function setcolumnTSMI_click(sender,args)
	originalEncoding=getObject(1,"OriginalEncoding")
	targetEncoding=getObject(1,"TargetEncoding")
	tableLayoutPanel1=TableLayoutPanel()
	tableLayoutPanel1.ColumnCount=1
	tableLayoutPanel1.RowCount = 2
	tableLayoutPanel1.Dock=DockStyle.Fill
	tableLayoutPanel1.ColumnStyles:Add(ColumnStyle(SizeType.Percent, 100))
	tableLayoutPanel1.RowStyles:Add(RowStyle(SizeType.Absolute, 35))
	tableLayoutPanel1.RowStyles:Add(RowStyle(SizeType.Percent, 100))
	btnSave= Button()
	btnSave.Image = Resources.start
	btnSave.Text="����"
	btnSave.Click:Add(btnSave_Click)
	lbSearch=Label()
	lbSearch.Dock=DockStyle.Fill
	lbSearch.Text = "���ٲ�ѯ:"
    lbSearch.TextAlign =ContentAlignment.MiddleRight
	
	tbSearch=TextBox()
	tbSearch.Dock=DockStyle.Fill
	tbSearch.TextChanged:Add(tbSearch_TextChanged);
	tableLayoutPanel2=TableLayoutPanel()
	
	tableLayoutPanel2.ColumnCount = 6
	tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Absolute, 84))
	tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Absolute, 300))
	tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Absolute, 131))
	tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Percent, 100))
	
	tableLayoutPanel2.Dock = DockStyle.Fill
	tableLayoutPanel2.RowCount = 1
	tableLayoutPanel2.RowStyles:Add(RowStyle(SizeType.Percent, 100))
	tableLayoutPanel2.Controls:Add(lbSearch, 0, 0)
	tableLayoutPanel2.Controls:Add(tbSearch, 1, 0)
	dbName=getObject(1,"DBCurrentDBName")
	dbTablesColumns=getObject(1,"DBtablesColumns")
	colName = DataGridViewTextBoxColumn()
	colName.DataPropertyName = "COLUMN_NAME"
	colName.HeaderText = "����"
	colName.Name = "colName"
	colName.ReadOnly = true
	
	colTableName = DataGridViewTextBoxColumn()
	colTableName.DataPropertyName = "COLUMN_TABLENAMES"
	colTableName.HeaderText = "��������"
	colTableName.Name = "colTableName"
	colTableName.ReadOnly = true
	
	colComment = DataGridViewTextBoxColumn()
	colComment.DataPropertyName = "COMMENTS"
	colComment.HeaderText = "��ע"
	colComment.Name = "colComment"
	
	gv=DataGridView()
	gv.AutoGenerateColumns=false
	gv.Columns:Add(colName)
	gv.Columns:Add(colTableName)
	gv.Columns:Add(colComment)
	gv.Dock=DockStyle.Fill
	gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
	gv.CellValueChanged:Add(gv_CellValueChanged)
	
	

local dc=DockContent()
dc.Text=dbName..setcolumnTSMI.Text

tableLayoutPanel1.Controls:Add(tableLayoutPanel2,0,0)
tableLayoutPanel1.Controls:Add(gv, 0, 1)
dc.Controls:Add(tableLayoutPanel1)
dc:show(application.Panel)
pcall(bindGridView)
end

function bindGridView(o)
	dataTable=DataTable("setColumn")
	dataTable.Columns:Add("COLUMN_NAME")
	dataTable.Columns:Add("COLUMN_TABLENAMES")
	dataTable.Columns:Add("COMMENTS")
	str=dbName..dbTablesColumns
	path= Application.StartupPath .."\\data\\"..dbName.."_TABLE_SETCOLUMN.data"
	if(File.Exists(path))then
	dataTable:ReadXml(path)
	end
columns=getObject(1,"DBCurrentDBAllTablesColumns")
local dv=columns.Tables[str].DefaultView
local distinctTable=columns.Tables[str]--getDistinctDataTable(dv,true,{"COLUMN_NAME"})
local count=distinctTable.Rows.Count-1

for i=0,count do
	local newRow=dataTable:NewRow()
	local temp=distinctTable.Rows[i]
	local columnName=getDataRowValue(temp,"COLUMN_NAME")
	local tableName=getDataRowValue(temp,"TABLE_NAME")
	local comments=getDataRowValue(temp,"COMMENTS")	
	local drs=dataTable:Select("COLUMN_NAME ='"..columnName.."'")
	if (drs.Length==0) then
		setDataRowValue(newRow,"COLUMN_NAME",columnName)
		setDataRowValue(newRow,"COLUMN_TABLENAMES",tableName)
		setDataRowValue(newRow,"COMMENTS",comments)
		dataTable.Rows:Add(newRow)
	else
	 
	local dr=drs[0]
	local tempTableName=getDataRowValue(dr,"COLUMN_TABLENAMES")	 
	if (not string.find(tempTableName,tableName)) then	 
	tempTableName=tempTableName..","..tableName
	setDataRowValue(dr,"COLUMN_TABLENAMES",tempTableName)
	end
	end
end

pcall(tbSearch_TextChanged)
end

function tbSearch_TextChanged(sender, e)
local dv=dataTable.DefaultView
dv.Sort="COLUMN_NAME ASC"
if(not String.IsNullOrEmpty(tbSearch.Text)) then
dv.RowFilter="COLUMN_NAME like '"..tbSearch.Text.."*' Or COMMENTS like '"..tbSearch.Text.."*'"
else
dv.RowFilter=null
end
gv.DataSource=dv
CallCtrlWithThreadSafety.RefreshGridViewDataSource(gv,gv)
end

function gv_CellValueChanged(sender, e)
	local columnName=getDataGridViewRowCellValue(gv,e.RowIndex,0)
	local columnComment=getDataGridViewRowCellValue(gv,e.RowIndex,e.ColumnIndex)
	local drs= columns.Tables[str]:Select("COLUMN_NAME ='"..columnName.."'")
	local count=drs.Length-1
	for i=0,count do
		local dr=drs[i]
		local tempComment=getDataRowValue(dr,"COMMENTS")
		--if((columnComment==tempComment) or (String.IsNullOrEmpty(tempComment)) ) then
		setDataRowValue(dr,"COMMENTS",columnComment)
		--end
	end

local path= Application.StartupPath .."\\data\\"..dbName.."_TABLE_SETCOLUMN.data"
FileHelper.CreateDirectory(path)
dataTable:WriteXml(path, XmlWriteMode.WriteSchema)
registerObject(pluginKey,"SetColumn",dataTable)
DBFileHelper.WriteXml(columns)
end

function btnSave_Click(sender,args)
	local path= Application.StartupPath .."\\data\\"..dbName.."_TABLE_SETCOLUMN.data"
	FileHelper.CreateDirectory(path)
	dataTable:WriteXml(path, XmlWriteMode.WriteSchema)
	registerObject(pluginKey,"SetColumn",dataTable)
	MessageBox.Show(btnSave,"����ɹ�","��ʾ", MessageBoxButtons.OK,
	MessageBoxIcon.Information)
end

function unload()
	
end

--�������:�㲥���֮�乲�����Ϣ
function onNotify(name,o)
	if(name=="BroadCastDBEnable") then
	setcolumnTSMI.Enabled=o
	
	end
end

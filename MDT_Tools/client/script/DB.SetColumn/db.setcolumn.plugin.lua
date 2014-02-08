--导入依赖Lua文件
require("script\\CLRPackage")
--导入程序集和命名空间
import("System")
import("System.Windows.Forms")
import("System.Drawing")
import("System.Collections.Generic")
import("MDT.Tools.Core","MDT.Tools.Core.UI") 
import("MDT.Tools.Core","MDT.Tools.Core.Utils") 
import("MDT.ThirdParty.Controls","WeifenLuo.WinFormsUI.Docking")
import("MDT.Tools.Core","MDT.Tools.Core.Resources")
import("MDT.Tools.DB.Common")
import("System.IO")
import("System.Text")
import("System.Data")

--获取主Form
application=getApplication()

--插件信息 
local tag=42
local pluginKey=42
local pluginName='维护数据库中的字段插件'
local description='维护当前数据库的所有字段信息'
local author='孔德帅'

--插件方法:初始化
function init()
return tag,pluginKey,pluginName,description,author
end
--创建按钮

setcolumnTSMI=ToolStripMenuItem()

--插件方法:加载
function load()
debug(string.format("%d %s", pluginKey,"load"))--调试日志
setcolumnTSMI.Text="表字段维护"
setcolumnTSMI.Click:Add(setcolumnTSMI_click)--增加Click事件
 
application.MainTool.Items:Insert(3, setcolumnTSMI)

end
--按钮事件
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
	 btnSave.Text="保存"
	 btnSave.Click:Add(btnSave_Click)	 
	 tableLayoutPanel2=TableLayoutPanel()
	 
     tableLayoutPanel2.ColumnCount = 6
     tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Absolute, 84))
     tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Absolute, 14))
     tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Absolute, 131))
     tableLayoutPanel2.ColumnStyles:Add(ColumnStyle(SizeType.Percent, 100))
     --tableLayoutPanel2.Controls:Add(btnSave, 0, 0)
	 tableLayoutPanel2.Dock = DockStyle.Fill
     tableLayoutPanel2.RowCount = 1
     tableLayoutPanel2.RowStyles:Add(RowStyle(SizeType.Percent, 100))	 
	dbName=getObject(1,"DBCurrentDBName")
	dbTablesColumns=getObject(1,"DBtablesColumns")
	colName = DataGridViewTextBoxColumn()
	colName.DataPropertyName = "COLUMN_NAME"
	colName.HeaderText = "列名"
    colName.Name = "colName"
    colName.ReadOnly = true
    colComment = DataGridViewTextBoxColumn()
	colComment.DataPropertyName = "COMMENTS"
    colComment.HeaderText = "备注"
    colComment.Name = "colComment"
	
	 gv=DataGridView()
	gv.AutoGenerateColumns=false
	gv.Columns:Add(colName)
	gv.Columns:Add(colComment)
	gv.Dock=DockStyle.Fill
	gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
	gv.CellValueChanged:Add(gv_CellValueChanged)
            
	dataTable=DataTable("setColumn")
	dataTable.Columns:Add("COLUMN_NAME")
	dataTable.Columns:Add("COMMENTS")
	str=dbName..dbTablesColumns
	path= Application.StartupPath .."\\data\\"..dbName.."_TABLE_SETCOLUMN.data"
	if(File.Exists(path))then
	dataTable:ReadXml(path)
	end
	columns=getObject(1,"DBCurrentDBAllTablesColumns")
	local dv=columns.Tables[str].DefaultView	
	local distinctTable=getDistinctDataTable(dv,true,{"COLUMN_NAME"})
	local count=distinctTable.Rows.Count-1
for i=0,count do
	local newRow=dataTable:NewRow()	
	local temp=distinctTable.Rows[i]
	local columnName=getDataRowValue(temp,"COLUMN_NAME")
	local drs=dataTable:Select("COLUMN_NAME ='"..columnName.."'")
	if (drs.Length==0) then
	setDataRowValue(newRow,"COLUMN_NAME",columnName)
	dataTable.Rows:Add(newRow)
	end
end	

	dv=dataTable.DefaultView
	dv.Sort="COLUMN_NAME ASC"
	
	gv.DataSource=dv
	gv:Refresh()
	
	local dc=DockContent()
	dc.Text=dbName..setcolumnTSMI.Text
	
	tableLayoutPanel1.Controls:Add(tableLayoutPanel2,0,0)
	tableLayoutPanel1.Controls:Add(gv, 0, 1)
	dc.Controls:Add(tableLayoutPanel1)
	dc:show(application.Panel)	 
end

function gv_CellValueChanged(sender, e)
	local columnName=getDataGridViewRowCellValue(gv,e.RowIndex,0)
	local columnComment=getDataGridViewRowCellValue(gv,e.RowIndex,e.ColumnIndex)
	--MessageBox.Show(columnComment)
	--columnComment= EncodingHelper.ConvertEncoder(Encoding.GetEncoding(targetEncoding),Encoding.GetEncoding(originalEncoding),columnComment)
	--MessageBox.Show(columnComment)
	--columnComment= EncodingHelper.ConvertEncoder(Encoding.ASCII,Encoding.GetEncoding(targetEncoding),columnComment)
	--MessageBox.Show(columnComment)
	local drs= columns.Tables[str]:Select("COLUMN_NAME ='"..columnName.."'")
	local count=drs.Length-1	
	for i=0,count do
	local dr=drs[i]
	local tempComment=getDataRowValue(dr,"COMMENTS")
	if((columnComment==tempComment) or (String.IsNullOrEmpty(tempComment)) ) then
	setDataRowValue(dr,"COMMENTS",columnComment)
	end
	end
	
local path= Application.StartupPath .."\\data\\"..dbName.."_TABLE_SETCOLUMN.data"
FileHelper.CreateDirectory(path)
dataTable:WriteXml(path, XmlWriteMode.WriteSchema)
registerObject("SetColumn",dataTable)

	 DBFileHelper.WriteXml(columns)
end

function btnSave_Click(sender,args)
local path= Application.StartupPath .."\\data\\"..dbName.."_TABLE_SETCOLUMN.data"
FileHelper.CreateDirectory(path)
dataTable:WriteXml(path, XmlWriteMode.WriteSchema)
registerObject("SetColumn",dataTable)
MessageBox.Show(btnSave,"保存成功","提示", MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
end

function unload()

end

--插件方法:广播插件之间共享的信息
function onNotify(name,o)

end

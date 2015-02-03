--导入依赖Lua文件
require("script\\CLRPackage")
require("script\\CLRForm")
--导入程序集和命名空间
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
--获取主Form
application=getApplication()

--插件信息
local tag=81
local pluginKey=81
local pluginName='Json格式化插件'
local description='格式化Json字符串'
local author='孔德帅'
local version='1.0.0.0'
--插件方法:初始化
function init()
	return tag,pluginKey,pluginName,description,author,version
end
--创建按钮

jsonTSMI=ToolStripMenuItem()

--插件方法:加载
function load()
	debug(string.format("%d %s", pluginKey,"load"))--调试日志	     
	jsonTSMI.Text="Json格式化"
	jsonTSMI.Image=Image.FromFile("script\\json.format\\json.ico")
	jsonTSMI.Click:Add(jsonTSMI_click)--增加Click事件
	 
	getObject(43,"tsmiTool").DropDownItems:Add(jsonTSMI)
	 
	
end
--按钮事件
function jsonTSMI_click(sender,args)	
	btnSave= Button()
	btnSave.Image = Resources.start
	btnSave.Text="格式化"
	btnSave.Click:Add(btnSave_Click)
	
	tbFormat=TextEditorControl()
 
	 
	 tbFormat.Dock = DockStyle.Fill;
            
            tbFormat.LineViewerStyle = LineViewerStyle.FullRow
         
           
             tbFormat.AllowCaretBeyondEOL = false;
            tbFormat.ShowEOLMarkers = false;
            tbFormat.ShowHRuler = false;
            tbFormat.ShowInvalidLines = false;
            tbFormat.ShowSpaces = false;
            tbFormat.ShowTabs = false;
            tbFormat.ShowVRuler = false;
            tbFormat.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("JSON")
	 
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
	tableLayoutPanel2.Controls:Add(tbFormat, 0, 1)
	tableLayoutPanel2:SetColumnSpan(tbFormat, tableLayoutPanel2.ColumnCount)
	 
 
	
	




local dc=DockContent()
dc.Text=jsonTSMI.Text

 

dc.Controls:Add(tableLayoutPanel2)
dc:show(application.Panel)

 
end
 

function btnSave_Click(sender,args)
local suc,err=pcall(function()	 

local sql=tbFormat.Text
tbFormat.Text=JsonHelper.JsonFormat(sql) 
end)
if not suc then
		MessageBox.Show(application.MainMenu, err.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
	end
end

function unload()
	
end

--插件方法:广播插件之间共享的信息
function onNotify(name,o)
	 
end

--导入依赖Lua文件
require("script\\CLRPackage")
--导入程序集和命名空间
import("System.IO")
import("System.Text")
import("System.Data")
import("System")
import("System.Windows.Forms")
import("System.Drawing")
import("System.Collections.Generic")
import("MDT.ThirdParty.Controls","WeifenLuo.WinFormsUI.Docking")
import("MDT.Tools.Core","MDT.Tools.Core.UI")
import("System.Threading")
import("System.Diagnostics")
--获取主Form
Application=getApplication()

--插件信息
local tag=53
local pluginKey=53
local pluginName='帮助'
local description='帮助信息'
local author='孔德帅'
local version='1.0.0.0'
--插件方法:初始化
function init()
	return tag,pluginKey,pluginName,description,author,version
end
--创建一个窗口
--explorer = ToolWindow()
--创建按钮
--toolTSMI=ToolStripMenuItem()
tsmiMdtPluginHelper=ToolStripMenuItem()
tsmiFAQHelper=ToolStripMenuItem()
--插件方法:加载
function load()
	debug(string.format("%d %s", pluginKey,"load"))--调试日志
	 
	 
	 tsmiFAQHelper.Text="常见问题解答"
	tsmiFAQHelper.Click:Add(tsmiFAQHelper_Click)
	
	tsmiMdtPluginHelper.Text="Atf插件开发教程"
	tsmiMdtPluginHelper.Click:Add(tsmiMdtPluginHelper_Click)
	
	getObject(43,"tsmiHelper").DropDownItems:Insert(2,tsmiMdtPluginHelper)
	getObject(43,"tsmiHelper").DropDownItems:Insert(3,tsmiFAQHelper)
 
	 
end
function tsmiMdtPluginHelper_Click(sender,e)
	Process.Start("http://doc.teamlinker.net/display/product/MDT+Smart+Kit")
end

function tsmiFAQHelper_Click(sender,e)

	Process.Start("control\\FAQ.docx")
	 
end

function showFile(fileName,filePath)
	code=Code()
	code.Text=fileName
	file=StreamReader(string.format("%s%s",filePath,fileName),Encoding.GetEncoding("gbk"))
	code.CodeContent=file:ReadToEnd()
	file:Close()
	code:show(Application.Panel)
end

function unload()
	--MessageBox.Show('unload')
end

--插件方法:广播插件之间共享的信息
function onNotify(name,o)
	--MessageBox.Show(name)
end

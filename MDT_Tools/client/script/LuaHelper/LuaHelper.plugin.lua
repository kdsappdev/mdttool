--导入依赖Lua文件
require("script\\CLRPackage")
--导入程序集和命名空间
import("System.Windows.Forms")
import("System.Drawing")
import("MDT.ThirdParty.Controls","WeifenLuo.WinFormsUI.Docking")
import("MDT.Tools.Core","MDT.Tools.Core.UI") 
import("System.IO")
import("System.Text")
--获取主Form
Application=getApplication()

--插件信息 
local tag=41
local pluginKey=41
local pluginName='test'
local description='test'
local author='孔德帅'

--插件方法:初始化
function init()
return tag,pluginKey,pluginName,description,author
end
--创建一个窗口
--explorer = ToolWindow()
--创建按钮
--toolTSMI=ToolStripMenuItem()
testLuaTSMI=ToolStripMenuItem()
--插件方法:加载
function load()
debug(string.format("%d %s", pluginKey,"load"))--调试日志
--explorer.Text="Lua插件"
--explorer.CloseButton=False
--explorer.CloseButtonVisible=False
--explorer.Icon= Icon("script\\test\\lua.ico")
--toolTSMI.Text="工具(&T)"
testLuaTSMI.Text="Lua帮助文档"
testLuaTSMI.Click:Add(testLuaTSMI_click)--增加Click事件
--toolTSMI.DropDownItems:Add(testLuaTSMI)

 Application.MainMenu.Items["tsmiHelper"].DropDownItems:Insert(1, testLuaTSMI);--主Form上面帮助菜单增加一个"Lua帮助文档"菜单
--MessageBox.Show("load")
--explorer:show(Application.Panel, DockState.DockRight)
--subscribe('BroadCastCheckFixNumberIsGreaterThan0',pluginKey)--订阅消息
end
--按钮事件
function testLuaTSMI_click(sender,args)
	showFile("LuaHelper.plugin.lua","script\\LuaHelper\\")
	showFile("readme.txt","script\\")
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

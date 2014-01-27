--导入依赖Lua文件
require("script\\CLRPackage")
--导入程序集和命名空间
import("System.Windows.Forms")
import("System.Drawing")
import("MDT.ThirdParty.Controls","WeifenLuo.WinFormsUI.Docking")
import("MDT.Tools.Core","MDT.Tools.Core.UI") 

--获取主Form
Application=getApplication()

--插件信息 
local tag=41
local pluginKey=41
local pluginName='test'
local description='test'
local author='孔德帅'

--插件方法:初始化
function initPlugin ()
return tag,pluginKey,pluginName,description,author
end
--创建一个窗口
explorer = ToolWindow()
--插件方法:加载
function load()

debug(string.format("%d %s", pluginKey,"load"))
explorer.Text="Lua插件"
explorer.CloseButton=False
explorer.CloseButtonVisible=False
explorer.Icon= Icon("script\\test\\lua.ico")

--MessageBox.Show("load")
explorer:show(Application.Panel, DockState.DockRight)
subscribe('BroadCastCheckFixNumberIsGreaterThan0',pluginKey)
end

function unload()
MessageBox.Show('unload')
end

--插件方法:广播插件之间共享的信息
function onNotify(name,o)
MessageBox.Show(name)
end

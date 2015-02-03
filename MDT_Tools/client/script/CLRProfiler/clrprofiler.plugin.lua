--导入依赖Lua文件
require("script\\CLRPackage")
--导入程序集和命名空间
import("System")
import("System.Windows.Forms")
import("System.Drawing")
import("System.Collections.Generic")
import("System.IO")
import("System.Text")
import("System.Data")
import("System.Threading")
import("System.Diagnostics")
--获取主Form
application=getApplication()

--插件信息
local tag=92
local pluginKey=92
local pluginName='CLRProfiler'
local description='CLRProfiler 检查.net程序内存问题'
local author='孔德帅'
local version='1.0.0.0'
--插件方法:初始化
function init()
	return tag,pluginKey,pluginName,description,author,version
end
--创建按钮

clrprofilerTSMI=ToolStripMenuItem()

--插件方法:加载
function load()
	debug(string.format("%d %s", pluginKey,"load"))--调试日志	     
	clrprofilerTSMI.Text="CLRProfiler" 
	clrprofilerTSMI.Click:Add(clrprofilerTSMI_click)--增加Click事件
	 
	getObject(43,"tsmiTool").DropDownItems:Add(clrprofilerTSMI)
	 
	
end
--按钮事件
function clrprofilerTSMI_click(sender,args)	 
	Process.Start("bin\\CLRProfiler.exe")
end
 

function unload()
	
end

--插件方法:广播插件之间共享的信息
function onNotify(name,o)
	 
end

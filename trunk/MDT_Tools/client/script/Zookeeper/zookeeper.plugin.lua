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
local tag=93
local pluginKey=93
local pluginName='Zookeeper'
local description='Zookeeper 检查Zookeeper注册情况'
local author='刘方龙'
local version='1.0.0.0'
--插件方法:初始化
function init()
	return tag,pluginKey,pluginName,description,author,version
end
--创建按钮

zookeeperTSMI=ToolStripMenuItem()

--插件方法:加载
function load()
	debug(string.format("%d %s", pluginKey,"load"))--调试日志	     
	zookeeperTSMI.Text="Zookeeper" 
	zookeeperTSMI.Click:Add(zookeeperTSMI_click)--增加Click事件
	 
	getObject(43,"tsmiTool").DropDownItems:Add(zookeeperTSMI)
	 
	
end
--按钮事件
function zookeeperTSMI_click(sender,args)	

 local ps =   Process()
            ps.StartInfo.UseShellExecute = true
            ps.StartInfo.FileName = "run.bat"
            ps.StartInfo.WorkingDirectory = "bin\\build"
           
            ps:Start()
			
	 
	 
end
 

function unload()
	
end

--插件方法:广播插件之间共享的信息
function onNotify(name,o)
	 
end

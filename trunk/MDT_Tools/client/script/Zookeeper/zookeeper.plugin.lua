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
import("System.Diagnostics")
--��ȡ��Form
application=getApplication()

--�����Ϣ
local tag=93
local pluginKey=93
local pluginName='Zookeeper'
local description='Zookeeper ���Zookeeperע�����'
local author='������'
local version='1.0.0.0'
--�������:��ʼ��
function init()
	return tag,pluginKey,pluginName,description,author,version
end
--������ť

zookeeperTSMI=ToolStripMenuItem()

--�������:����
function load()
	debug(string.format("%d %s", pluginKey,"load"))--������־	     
	zookeeperTSMI.Text="Zookeeper" 
	zookeeperTSMI.Click:Add(zookeeperTSMI_click)--����Click�¼�
	 
	getObject(43,"tsmiTool").DropDownItems:Add(zookeeperTSMI)
	 
	
end
--��ť�¼�
function zookeeperTSMI_click(sender,args)	

 local ps =   Process()
            ps.StartInfo.UseShellExecute = true
            ps.StartInfo.FileName = "run.bat"
            ps.StartInfo.WorkingDirectory = "bin\\build"
           
            ps:Start()
			
	 
	 
end
 

function unload()
	
end

--�������:�㲥���֮�乲�����Ϣ
function onNotify(name,o)
	 
end

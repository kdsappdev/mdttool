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
local tag=91
local pluginKey=91
local pluginName='MSE'
local description='MSE ���.net����פ�߳�'
local author='�׵�˧'
local version='1.0.0.0'
--�������:��ʼ��
function init()
	return tag,pluginKey,pluginName,description,author,version
end
--������ť

mseTSMI=ToolStripMenuItem()

--�������:����
function load()
	debug(string.format("%d %s", pluginKey,"load"))--������־	     
	mseTSMI.Text="MSE" 
	mseTSMI.Click:Add(mseTSMI_click)--����Click�¼�
	 
	getObject(43,"tsmiTool").DropDownItems:Add(mseTSMI)
	 
	
end
--��ť�¼�
function mseTSMI_click(sender,args)	 
	Process.Start("bin\\MSE.exe")
end
 

function unload()
	
end

--�������:�㲥���֮�乲�����Ϣ
function onNotify(name,o)
	 
end

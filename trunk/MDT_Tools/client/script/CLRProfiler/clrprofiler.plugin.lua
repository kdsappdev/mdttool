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
local tag=92
local pluginKey=92
local pluginName='CLRProfiler'
local description='CLRProfiler ���.net�����ڴ�����'
local author='�׵�˧'
local version='1.0.0.0'
--�������:��ʼ��
function init()
	return tag,pluginKey,pluginName,description,author,version
end
--������ť

clrprofilerTSMI=ToolStripMenuItem()

--�������:����
function load()
	debug(string.format("%d %s", pluginKey,"load"))--������־	     
	clrprofilerTSMI.Text="CLRProfiler" 
	clrprofilerTSMI.Click:Add(clrprofilerTSMI_click)--����Click�¼�
	 
	getObject(43,"tsmiTool").DropDownItems:Add(clrprofilerTSMI)
	 
	
end
--��ť�¼�
function clrprofilerTSMI_click(sender,args)	 
	Process.Start("bin\\CLRProfiler.exe")
end
 

function unload()
	
end

--�������:�㲥���֮�乲�����Ϣ
function onNotify(name,o)
	 
end

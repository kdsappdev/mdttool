--��������Lua�ļ�
require("script\\CLRPackage")
--������򼯺������ռ�
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
--��ȡ��Form
Application=getApplication()

--�����Ϣ
local tag=53
local pluginKey=53
local pluginName='����'
local description='������Ϣ'
local author='�׵�˧'
local version='1.0.0.0'
--�������:��ʼ��
function init()
	return tag,pluginKey,pluginName,description,author,version
end
--����һ������
--explorer = ToolWindow()
--������ť
--toolTSMI=ToolStripMenuItem()
tsmiMdtPluginHelper=ToolStripMenuItem()
tsmiFAQHelper=ToolStripMenuItem()
--�������:����
function load()
	debug(string.format("%d %s", pluginKey,"load"))--������־
	 
	 
	 tsmiFAQHelper.Text="����������"
	tsmiFAQHelper.Click:Add(tsmiFAQHelper_Click)
	
	tsmiMdtPluginHelper.Text="Atf��������̳�"
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

--�������:�㲥���֮�乲�����Ϣ
function onNotify(name,o)
	--MessageBox.Show(name)
end

--��������Lua�ļ�
require("script\\CLRPackage")
--������򼯺������ռ�
import("System.Windows.Forms")
import("System.Drawing")
import("MDT.ThirdParty.Controls","WeifenLuo.WinFormsUI.Docking")
import("MDT.Tools.Core","MDT.Tools.Core.UI") 
import("System.IO")
import("System.Text")
--��ȡ��Form
Application=getApplication()

--�����Ϣ 
local tag=41
local pluginKey=41
local pluginName='test'
local description='test'
local author='�׵�˧'

--�������:��ʼ��
function init()
return tag,pluginKey,pluginName,description,author
end
--����һ������
--explorer = ToolWindow()
--������ť
--toolTSMI=ToolStripMenuItem()
testLuaTSMI=ToolStripMenuItem()
--�������:����
function load()
debug(string.format("%d %s", pluginKey,"load"))--������־
--explorer.Text="Lua���"
--explorer.CloseButton=False
--explorer.CloseButtonVisible=False
--explorer.Icon= Icon("script\\test\\lua.ico")
--toolTSMI.Text="����(&T)"
testLuaTSMI.Text="Lua�����ĵ�"
testLuaTSMI.Click:Add(testLuaTSMI_click)--����Click�¼�
--toolTSMI.DropDownItems:Add(testLuaTSMI)

 Application.MainMenu.Items["tsmiHelper"].DropDownItems:Insert(1, testLuaTSMI);--��Form��������˵�����һ��"Lua�����ĵ�"�˵�
--MessageBox.Show("load")
--explorer:show(Application.Panel, DockState.DockRight)
--subscribe('BroadCastCheckFixNumberIsGreaterThan0',pluginKey)--������Ϣ
end
--��ť�¼�
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

--�������:�㲥���֮�乲�����Ϣ
function onNotify(name,o)
--MessageBox.Show(name)
end

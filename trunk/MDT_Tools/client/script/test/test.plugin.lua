--��������Lua�ļ�
require("script\\CLRPackage")
--������򼯺������ռ�
import("System.Windows.Forms")
import("System.Drawing")
import("MDT.ThirdParty.Controls","WeifenLuo.WinFormsUI.Docking")
import("MDT.Tools.Core","MDT.Tools.Core.UI") 

--��ȡ��Form
Application=getApplication()

--�����Ϣ 
local tag=41
local pluginKey=41
local pluginName='test'
local description='test'
local author='�׵�˧'

--�������:��ʼ��
function initPlugin ()
return tag,pluginKey,pluginName,description,author
end
--����һ������
explorer = ToolWindow()
--�������:����
function load()

debug(string.format("%d %s", pluginKey,"load"))
explorer.Text="Lua���"
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

--�������:�㲥���֮�乲�����Ϣ
function onNotify(name,o)
MessageBox.Show(name)
end

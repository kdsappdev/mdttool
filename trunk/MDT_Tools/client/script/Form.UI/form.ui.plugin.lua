require("script\\CLRPackage")
import("System")
import("System.Windows.Forms")
import("System.Drawing")
import("System.Collections.Generic")
import("MDT.Tools.Core","MDT.Tools.Core.Resources")
import("MDT.ThirdParty.Controls","WeifenLuo.WinFormsUI.Docking")
import("MDT.ThirdParty.Controls","KnightsWarriorAutoupdater")
import("MDT Smart Kit","MDT.Tools.UI")
import("System.Text")
import("System.Threading")
import("System.Diagnostics")
import("System.Configuration")
application=getApplication()

local tag=0
local pluginKey=43
local pluginName='UI���'
local description='form������ṩ�Ļ�������'
local author='�׵�˧'
local version='1.0.0.0'


function init()
	return tag,pluginKey,pluginName,description,author,version
end
--�˵���
--����
tsmiHelper=ToolStripMenuItem()
tsmiCheckUpdate=ToolStripMenuItem()
toolStripSeparator1=ToolStripSeparator()
tsmiAbout=ToolStripMenuItem()
--����
tsmiWindow=ToolStripMenuItem()
tsmiCloseAllDocument=ToolStripMenuItem()

--������
tsbCloseAllDocment=ToolStripButton()
tsbExit=ToolStripButton()
--״̬��
tsslMessage=ToolStripStatusLabel()
tspbLoadDbProgress=ToolStripProgressBar()

function load()
	
	tsmiHelper.Text = "����(&H)"
	tsmiHelper.DropDownItems:Add(tsmiCheckUpdate)
	tsmiHelper.DropDownItems:Add(toolStripSeparator1)
	tsmiHelper.DropDownItems:Add(tsmiAbout)
	
	tsmiWindow.DropDownItems:Add(tsmiCloseAllDocument)
	tsmiWindow.Text = "����(&W)"
	
	
	tsmiCheckUpdate.Text="������"
	tsmiCheckUpdate.Click:Add(tsmiCheckUpdate_Click)
	
	tsmiCloseAllDocument.Image = Resources.closeAllDocment
	
	
	tsmiAbout.Text = "����"
	tsmiAbout.Click:Add(tsmiAbout_Click)
	
	tsmiCloseAllDocument.Text = "�ر������ĵ�(&L)"
	tsmiCloseAllDocument.Click:Add(tsmiCloseAllDocument_Click)
	
	
	application.MainMenu.Items:Add(tsmiWindow)
	application.MainMenu.Items:Add(tsmiHelper)
	
	
	tsbCloseAllDocment.Text="�ر������ĵ�"
	tsbCloseAllDocment.Click:Add(tsmiCloseAllDocument_Click)
	tsbCloseAllDocment.Image = Resources.closeAllDocment
	
	tsbExit.Name="tsbExit"
	tsbExit.Text="�˳�"
	tsbExit.Image = Resources.exit
	tsbExit.Click:Add(tsbExit_Click)
	
	application.MainTool.Items:Add(tsbCloseAllDocment)
	application.MainTool.Items:Add(tsbExit)
	
	
	
	tspbLoadDbProgress.AutoSize = false
	tspbLoadDbProgress.Visible = false
	tspbLoadDbProgress.Width = 250
	
	tsslMessage.AutoSize = false
	tsslMessage.TextAlign = ContentAlignment.MiddleLeft
	tsslMessage.Width = application.StatusBar.Width - tspbLoadDbProgress.Width - 20
	application.StatusBar.SizeChanged:Add(StatusBarSizeChanged)
	application.StatusBar.Items:Insert(0, tsslMessage)
	application.StatusBar.Items:Insert(1, tspbLoadDbProgress)
	
	
	
	registerObject(pluginKey,"tsmiWindow",tsmiWindow)
	
	registerObject(pluginKey,"tsmiHelper",tsmiHelper)
	
	registerObject(pluginKey,"tsbExit",tsbExit)
	registerObject(pluginKey,"tsslMessage",tsslMessage)
	registerObject(pluginKey,"tspbLoadDbProgress",tspbLoadDbProgress)
	
end

function  StatusBarSizeChanged(sender,e)
	tsslMessage.Width = application.StatusBar.Width - tspbLoadDbProgress.Width - 20
	
end

function tsmiCheckUpdate_Click(sender,e)
	ThreadPool.QueueUserWorkItem(checkUpdate)
end

function checkUpdate()
	
	local suc,err=pcall(function()
		local autoUpdater = AutoUpdater()
		local isUpdate= autoUpdater:IsUpdate()
		if (isUpdate) then
			local dr = MessageBox.Show(application.MainMenu, "��鵽���°棬�Ƿ�����?","��ʾ",MessageBoxButtons.YesNo,MessageBoxIcon.Information)
			if (dr == DialogResult.Yes) then
				Process.Start("MDT.Tools.AutoUpdater.exe", "true")
				pcall(tsmiExit_Click)
			end
		else
			MessageBox.Show(application.MainMenu, "�Ѿ������°汾", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information)
		end
	end)
	
	if not suc then
		MessageBox.Show(application.MainMenu, "���ʧ��[" .. err .."]", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information)
	end
	
end

function tsbExit_Click(sender,e)
	application.PluginManager:Unloading();
	Application.Exit()
end
function tsmiAbout_Click(sender,e)
	local aboutDialog = AboutDialog()
	aboutDialog:ShowDialog()
end
function tsmiCloseAllDocument_Click(sender,e)
	documents = application.Panel:DocumentsToArray()
	count=documents.Length-1
	for i=0,count do
		documents[i].DockHandler:Close()
	end
end

function unload()
	
end


function onNotify(name,o)
	
end
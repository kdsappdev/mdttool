require("script\\CLRPackage")
import("System")
import("System.Windows.Forms")
import("System.Drawing")
import("System.Text")
import("System.Threading")
import("MDT.ThirdParty.Controls","KnightsWarriorAutoupdater")
import("System.Diagnostics")
import("System.Configuration")
application=getApplication()

local tag=44
local pluginKey=44
local pluginName='框架升级'
local description='框架中增加自动升级程序'
local author='孔德帅'

local version='1.0.0.0'


function init()
	return tag,pluginKey,pluginName,description,author,version
end


function load()
	ThreadPool.QueueUserWorkItem(checkUpdate)
end

function checkUpdate()
	
	local suc,err=pcall(function()
		local autoUpdater = AutoUpdater()
		local isUpdate= autoUpdater:IsUpdate()
		
		if (isUpdate) then
			local dr = MessageBox.Show(application.MainMenu, "检查到有新版，是否升级?","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Information)
			if (dr == DialogResult.Yes) then
				Process.Start("MDT.Tools.AutoUpdater.exe", "true")
				
				getObject(43,"tsbExit"):PerformClick();
			end
			
		end
	end)	
end

function unload()
	
end


function onNotify(name,o)
	
end
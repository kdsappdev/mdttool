local tag=41
local pluginKey=41
local pluginName='test'
local description='test'
local author='孔德帅'

function initPlugin ()
return tag,pluginKey,pluginName,description,author
end

function load()
showMessage('加载')
subscribe('BroadCastCheckFixNumberIsGreaterThan0',pluginKey)
end

function unload()
showMessage('unload')
end

function onNotify(name,o)
showMessage(name)
end

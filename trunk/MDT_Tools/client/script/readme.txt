MDT Smart Kit通过Lua脚本编写插件

Lua插件接口：
1.初始化
	local tag=41--插件的顺序
	local pluginKey=41--插件Id号
	local pluginName='test'--插件名称
	local description='test'--插件描述
	local author='孔德帅'--插件编写者

	--插件方法:初始化
	function init()
		return tag,pluginKey,pluginName,description,author
	end
2.加载
	function load()
	--逻辑代码
	end
3.卸载
	function unload()
	--逻辑代码
	end
4.通知
	--插件方法:广播插件之间共享的信息
	function onNotify(name,o)
	--逻辑代码
	end

Lua引擎提供的方法

	registerObject(name, obj) - 注册插件之间共享的信息 name:信息key, obj:信息内容

	getObject(pluginKey, name) - 获取插件之间共享的信息 pluginKey:信息key, name:信息内容

	removeObject(name) - 移除插件之间共享的信息 name:信息key

	unsubscribe(name, luaPluginKey) - 退订插件之间时时改变的信息 name:信息key, luaPluginKey:Lua插件的key

	subscribe(name, luaPluginKey) - 订阅插件之间时时改变的信息 name:信息key, luaPluginKey:Lua插件的key

	broadcast(name, o) - 广播插件之间时时改变的信息 name:信息key, o:信息内容

	getPluginShareKey(name, pluginKey) - 获取插件共享的key name:信息key, pluginKey:Lua插件的key
	getApplication() - 获取IApplication

	debug(str) - 调试日志记录 str:日志内容

	warn(str) - 警告日志记录 str:日志内容

	error(ex) - 错误日志记录 ex:日志内容sssss
注意：
	1.编写的脚本要以ACSII编码保存，否则涉及中文的，就会乱码，并且文件的结尾要以plugin.lua命名，如:xx.plugin.lua

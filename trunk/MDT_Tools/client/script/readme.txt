MDT Smart Kit通过Lua脚本编写插件

Lua插件接口：
1.初始化
	local tag=41--插件的顺序
	local pluginKey=41--插件Id号
	local pluginName='test'--插件名称
	local description='test'--插件描述
	local author='孔德帅'--插件编写者
	local version='1.0.0.0'--版本号
	--插件方法:初始化
	function init()
		return tag,pluginKey,pluginName,description,author,version
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

registerObject(Int32 pluginKey, System.String name, System.Object obj) - 注册插件之间共享的信息 pluginKey:插件key(Int32), name:信息key(String), obj:信息内容(Object)

getObject(Int32 pluginKey, System.String name) - 获取插件之间共享的信息 pluginKey:信息key(Int32), name:信息内容(String)

removeObject(System.String name) - 移除插件之间共享的信息 name:信息key(String)

unsubscribe(System.String name, Int32 pluginKey) - 退订插件之间时时改变的信息 name:信息key(String), pluginKey:Lua插件的key(Int32)

subscribe(System.String name, Int32 pluginKey) - 订阅插件之间时时改变的信息 name:信息key(String), pluginKey:Lua插件的key(Int32)

broadcast(System.String name, System.Object o) - 广播插件之间时时改变的信息 name:信息key(String), o:信息内容(Object)

getPluginShareKey(System.String name, Int32 pluginKey) - 获取插件共享的key name:信息key(String), pluginKey:Lua插件的key(Int32)

getApplication() - 获取IApplication

debug(System.Object str) - 调试日志记录 str:日志内容(Object)

warn(System.Object str) - 警告日志记录 str:日志内容(Object)

error(System.Object ex) - 错误日志记录 ex:日志内容(Object)

getDataRowValue(System.Data.DataRow dr, System.String columnName) - 获取DataRowValue dr:DataRow(DataRow), columnName:列名(String)

setDataRowValue(System.Data.DataRow dr, System.String columnName, System.Object value) - 设置DataRowValue dr:DataRow(DataRow), columnName:列名(String), value:列value(Object)

getDistinctDataTable(System.Data.DataView dv, Boolean isDistinct, LuaInterface.LuaTable columnNames) - 获取getDistinctDataTable dv:DataTable(DataView), isDistinct:是否Distinct(Boolean), columnNames:列名(LuaTable)

getDataGridViewRowCellValue(System.Windows.Forms.DataGridView dgv, Int32 rowIndex, Int32 columnIndex) - 获取getDataGridViewRowCellValue dgv:DataGridView(DataGridView), rowIndex:rowIndex(Int32), columnIndex:columnIndex(Int32)

注意：
	1.编写的脚本要以ACSII编码保存，否则涉及中文的，就会乱码，并且文件的结尾要以plugin.lua命名，如:xx.plugin.lua

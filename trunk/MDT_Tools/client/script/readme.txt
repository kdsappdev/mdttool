MDT Smart Kitͨ��Lua�ű���д���

Lua����ӿڣ�
1.��ʼ��
	local tag=41--�����˳��
	local pluginKey=41--���Id��
	local pluginName='test'--�������
	local description='test'--�������
	local author='�׵�˧'--�����д��
	local version='1.0.0.0'--�汾��
	--�������:��ʼ��
	function init()
		return tag,pluginKey,pluginName,description,author,version
	end
2.����
	function load()
	--�߼�����
	end
3.ж��
	function unload()
	--�߼�����
	end
4.֪ͨ
	--�������:�㲥���֮�乲�����Ϣ
	function onNotify(name,o)
	--�߼�����
	end

Lua�����ṩ�ķ���

registerObject(Int32 pluginKey, System.String name, System.Object obj) - ע����֮�乲�����Ϣ pluginKey:���key(Int32), name:��Ϣkey(String), obj:��Ϣ����(Object)

getObject(Int32 pluginKey, System.String name) - ��ȡ���֮�乲�����Ϣ pluginKey:��Ϣkey(Int32), name:��Ϣ����(String)

removeObject(System.String name) - �Ƴ����֮�乲�����Ϣ name:��Ϣkey(String)

unsubscribe(System.String name, Int32 pluginKey) - �˶����֮��ʱʱ�ı����Ϣ name:��Ϣkey(String), pluginKey:Lua�����key(Int32)

subscribe(System.String name, Int32 pluginKey) - ���Ĳ��֮��ʱʱ�ı����Ϣ name:��Ϣkey(String), pluginKey:Lua�����key(Int32)

broadcast(System.String name, System.Object o) - �㲥���֮��ʱʱ�ı����Ϣ name:��Ϣkey(String), o:��Ϣ����(Object)

getPluginShareKey(System.String name, Int32 pluginKey) - ��ȡ��������key name:��Ϣkey(String), pluginKey:Lua�����key(Int32)

getApplication() - ��ȡIApplication

debug(System.Object str) - ������־��¼ str:��־����(Object)

warn(System.Object str) - ������־��¼ str:��־����(Object)

error(System.Object ex) - ������־��¼ ex:��־����(Object)

getDataRowValue(System.Data.DataRow dr, System.String columnName) - ��ȡDataRowValue dr:DataRow(DataRow), columnName:����(String)

setDataRowValue(System.Data.DataRow dr, System.String columnName, System.Object value) - ����DataRowValue dr:DataRow(DataRow), columnName:����(String), value:��value(Object)

getDistinctDataTable(System.Data.DataView dv, Boolean isDistinct, LuaInterface.LuaTable columnNames) - ��ȡgetDistinctDataTable dv:DataTable(DataView), isDistinct:�Ƿ�Distinct(Boolean), columnNames:����(LuaTable)

getDataGridViewRowCellValue(System.Windows.Forms.DataGridView dgv, Int32 rowIndex, Int32 columnIndex) - ��ȡgetDataGridViewRowCellValue dgv:DataGridView(DataGridView), rowIndex:rowIndex(Int32), columnIndex:columnIndex(Int32)

ע�⣺
	1.��д�Ľű�Ҫ��ACSII���뱣�棬�����漰���ĵģ��ͻ����룬�����ļ��Ľ�βҪ��plugin.lua��������:xx.plugin.lua

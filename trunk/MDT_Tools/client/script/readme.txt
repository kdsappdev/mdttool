MDT Smart Kitͨ��Lua�ű���д���

Lua����ӿڣ�
1.��ʼ��
	local tag=41--�����˳��
	local pluginKey=41--���Id��
	local pluginName='test'--�������
	local description='test'--�������
	local author='�׵�˧'--�����д��

	--�������:��ʼ��
	function init()
		return tag,pluginKey,pluginName,description,author
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

	registerObject(name, obj) - ע����֮�乲�����Ϣ name:��Ϣkey, obj:��Ϣ����

	getObject(pluginKey, name) - ��ȡ���֮�乲�����Ϣ pluginKey:��Ϣkey, name:��Ϣ����

	removeObject(name) - �Ƴ����֮�乲�����Ϣ name:��Ϣkey

	unsubscribe(name, luaPluginKey) - �˶����֮��ʱʱ�ı����Ϣ name:��Ϣkey, luaPluginKey:Lua�����key

	subscribe(name, luaPluginKey) - ���Ĳ��֮��ʱʱ�ı����Ϣ name:��Ϣkey, luaPluginKey:Lua�����key

	broadcast(name, o) - �㲥���֮��ʱʱ�ı����Ϣ name:��Ϣkey, o:��Ϣ����

	getPluginShareKey(name, pluginKey) - ��ȡ��������key name:��Ϣkey, pluginKey:Lua�����key
	getApplication() - ��ȡIApplication

	debug(str) - ������־��¼ str:��־����

	warn(str) - ������־��¼ str:��־����

	error(ex) - ������־��¼ ex:��־����sssss
ע�⣺
	1.��д�Ľű�Ҫ��ACSII���뱣�棬�����漰���ĵģ��ͻ����룬�����ļ��Ľ�βҪ��plugin.lua��������:xx.plugin.lua

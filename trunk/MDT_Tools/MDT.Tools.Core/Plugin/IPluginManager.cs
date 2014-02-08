using System;
using System.Collections.Generic;
using System.Text;
using MDT.Tools.Core.Plugin.WindowsPlugin;

namespace MDT.Tools.Core.Plugin
{
    /// <summary>
    /// IPluginManager
    /// 
    /// �������ӿ�  ���ڼ���/ж�أ�������ֲ��
    /// 
    /// �޸ļ�¼
    ///   
    ///         2010.8.8 �汾��1.0 �׵�˧ ���
    /// 
    /// �汾��1.0
    /// 
    /// <author>
    ///        <name>�׵�˧</name>
    ///        <date>2010.8.8</date>
    /// </author> 
    /// </summary>
    public interface IPluginManager
    {
        #region ����

        IForm Application { get; set; }
        /// <summary>
        /// CopyToMemory �Ƿ񽫲���������ڴ�����
        /// </summary>
        bool CopyToMemory { get;set;}

        /// <summary>
        /// AddinList �Ѽ��صĲ���б�
        /// </summary>
        IList<IPlugin> PluginList { get;} //������ΪIPlugin

        #endregion

        #region ���� ���� ж�� ���

        #region
        void Init();
        void Loading();
        void Unloading();
        #endregion

        #region ���ز��
        /// <summary>
        /// LoadDefault ���ص�ǰĿ¼����Ŀ¼�µ�������Ч���
        /// </summary>
        void LoadDefault(string pluginSign);

        /// <summary>
        /// LoadAllAddins ����ָ��Ŀ¼�µ����в��
        /// </summary>      
        void LoadAllPlugins(string pluginFolderPath, bool searchChildFolder, string pluginSign);       
        #endregion

        #region ж�ز��
        /// <summary>
        /// Clear ��������Ѿ����صĲ��
        /// </summary>
        void Clear();

        /// <summary>
        /// DynRemoveAddin ж��ָ���Ĳ��
        /// </summary>       
        void DynRemovePlugin(int pluginKey);
        #endregion

        #region ����ָ�����
        /// <summary>
        /// EnablePlugin ����ָ���Ĳ��
        /// </summary>       
        void EnablePlugin(int pluginKey);
        #endregion

        #region ָֹͣ�����
        /// <summary>
        /// DisEnablePlugin ����ָ���Ĳ��
        /// </summary> 
        void DisEnablePlugin(int pluginKey);

        #endregion

        #region ͨ�����Key��ȡָ�����
        IPlugin GetPlugin(int pluginKey);
        #endregion

        #region �Ƿ����ָ�����
        bool ContainsPlugin(int pluginKey);       
        #endregion

        #endregion

        #region �¼� ����ı�����       
        event PluginChanged PluginChanged;
        #endregion
    }
    public delegate void PluginChanged();
}

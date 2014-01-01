using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.UI;
using WeifenLuo.WinFormsUI.Docking;

namespace MDT.Tools.Core.Plugin.WindowsPlugin
{
    /// <summary>
    /// IForm
    /// 
    /// Window���Ӧ����������ӿڣ�IPlugin�����loading()�и��ݵ�ǰ�����ṩ��Ӧ�÷���ӿ���������������WinForm����
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
    public interface IForm : IApplication
    {
       // ToolStripPanel LeftToolPanel { get;}
       // ToolStripPanel RightToolPanel { get;}
        ToolStrip MainTool { get; }
       // ToolStripPanel BottomToolPanel { get;}
        IPluginManager PluginManager { get;}
        MenuStrip MainMenu { get;}
        StatusStrip StatusBar { get;}
        DockPanel Panel { get; }         
        ContextMenuStrip MainContextMenu { get; }

        
        void RegisterObject(string name, object obj);
        object GetObject(string name);
        void Remove(string name);

        void Subscribe(string name, IPlugin plugin);
        void Unsubscribe(string name,IPlugin plugin);
        void BroadCast(string name, object o);
    }
}

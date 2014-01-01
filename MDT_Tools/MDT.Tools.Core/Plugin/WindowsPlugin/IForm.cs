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
    /// Window插件应用容器服务接口，IPlugin插件在loading()中根据当前容器提供的应用服务接口来创建插件，针对WinForm程序
    /// 
    /// 修改纪录
    ///   
    ///         2010.8.8 版本：1.0 孔德帅 添加
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    ///        <name>孔德帅</name>
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

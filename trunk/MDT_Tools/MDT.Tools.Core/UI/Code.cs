using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MDT.ThirdParty.Controls.Util;
using WeifenLuo.WinFormsUI.Docking;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Actions;
namespace MDT.Tools.Core.UI
{
    public partial class Code : DockContent
    {


        public Code()
        {
            InitializeComponent();
            tbCode.AllowCaretBeyondEOL = false;
            tbCode.ShowEOLMarkers = false;
            tbCode.ShowHRuler = false;
            tbCode.ShowInvalidLines = false;
            tbCode.ShowSpaces = false;
            tbCode.ShowTabs = false;
            tbCode.ShowVRuler = false; 
        }
        XmlFolding xmlFolding=new XmlFolding();
        private string codeLanguage = "C#";
        public string CodeLanguage
        {
            set { codeLanguage = value;
            tbCode.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(codeLanguage);
            
            }
        }

        public string CodeContent
        {
            get { return tbCode.Text; }
            set
            {
                if (codeLanguage.ToLower() == "xml")
                {
                    xmlFolding.ActiveEditor = tbCode;
                    xmlFolding.FormatXml(value);
                }
                else
                {
                    tbCode.Text = value;

                }
            }
        }

    }
}

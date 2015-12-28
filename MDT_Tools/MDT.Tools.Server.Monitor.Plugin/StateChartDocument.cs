/*
 *  Copyright ?Northwoods Software Corporation, 2000-2011. All Rights
 *  Reserved.
 *
 *  Restricted Rights: Use, duplication, or disclosure by the U.S.
 *  Government is subject to restrictions as set forth in subparagraph
 *  (c) (1) (ii) of DFARS 252.227-7013, or in FAR 52.227-19, or in FAR
 *  52.227-14 Alt. III, as applicable.
 */

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using Northwoods.Go;
using Northwoods.Go.Xml;

namespace MDT.Tools.Server.Monitor.Plugin
{
    [Serializable]
    public class StateChartDocument : GoDocument
    {
        public StateChartDocument() { }

        // whenever the document's Name is changed, update the nameTextBox in the MainForm
        public override String Name
        {
            set
            {
                if (base.Name != value)
                {
                    base.Name = value;

                }
            }
        }

        // keep track of the file where this document is stored
        public virtual String Path
        {
            get { return myPath; }
            set
            {
                String old = myPath;
                if (old != value)
                {
                    myPath = value;
                    RaiseChanged(ChangedPath, 0, null, 0, old, NullRect, 0, value, NullRect);
                }
            }
        }

        public static GoBasicNode NewNode()
        {
            GoBasicNode state = new GoBasicNode();
            state.LabelSpot = GoObject.Middle;
            state.Shape = new GoRoundedRectangle();
            state.Shape.FillShapeHighlight(Color.FromArgb(80, 180, 240), Color.FromArgb(255, 255, 255));
            state.Editable = false;
            state.Text = "state";

            // state.Brush = new SolidBrush(Color.Black);
            state.Shape.PenColor = Color.White;
            state.Shape.PenWidth = 2;
            state.Label.EditableWhenSelected = false;
            state.Label.Editable = false;
            state.Label.Multiline = true;
            state.Port.IsValidDuplicateLinks = false;
            state.Port.IsValidSelfNode = false;
            state.Port.Editable = false;

            return state;
        }



        public static GoComment NewComment()
        {
            GoComment comment = new GoComment();
            comment.Text = "";
            comment.Width = 50;
            comment.Height = 100;
            comment.Label.Multiline = true;
            comment.Label.Editable = false;
            comment.Label.EditableWhenSelected = true;
            return comment;
        }

        public virtual int NumLinksBetween(IGoPort a, IGoPort b)
        {
            int count = 0;
            foreach (IGoLink l in a.DestinationLinks)
            {
                if (l.ToPort == b)
                    count++;
            }
            return count;
        }

        public int Version
        {
            get { return 4; }
            set
            {
                if (value != this.Version)
                    throw new NotSupportedException("For simplicity, this sample application does not handle different versions of saved documents");
            }
        }

        // TODO: adapt the XML elements and attributes to match your classes and their properties
        private static void InitReaderWriter(GoXmlReaderWriterBase rw)
        {
            GoXmlBindingTransformer.DefaultTracingEnabled = true;  // for debugging, check your Output window (trace listener)
            GoXmlBindingTransformer t;

            t = new GoXmlBindingTransformer("statechart", new StateChartDocument());
            t.AddBinding("version", "Version", GoXmlBindingFlags.RethrowsExceptions);  // let exception from Version setter propagate out
            t.AddBinding("name", "Name");
            t.AddBinding("path", "Path");
            rw.AddTransformer(t);

            t = new GoXmlBindingTransformer("comment", NewComment());
            t.IdAttributeUsedForSharedObjects = true;  // each GoComment gets a unique ID
            t.AddBinding("label", "Text");
            t.AddBinding("center", "Center");  // last property, since it depends on content/alignment
            AddBrushBindings(t, "", "Shape");
            AddPenBindings(t, "pen", "Shape");
            AddFontBindings(t, "label", "Label");
            rw.AddTransformer(t);

            t = new GoXmlBindingTransformer("state", NewNode());
            t.IdAttributeUsedForSharedObjects = true;  // each GraphNode gets a unique ID
            t.HandlesNamedPorts = true;  // generate attributes for each of the named ports, specifying their IDs
            t.AddBinding("label", "Text");
            t.AddBinding("loc", "Location");
            AddBrushBindings(t, "", "Shape");
            AddPenBindings(t, "pen", "Shape");
            AddFontBindings(t, "label", "Label");
            //t.AddBinding("userFlags", "UserFlags");
            rw.AddTransformer(t);

         

            t = new GoXmlBindingTransformer("transition", new AnimatedLink());
            t.AddBinding("from", "FromPort");
            t.AddBinding("to", "ToPort");
            t.AddBinding("curviness", "Curviness");
            t.AddBinding("brush", "RealLink.BrushColor");
            t.AddBinding("adjusted", "RealLink.AdjustingStyle");
            t.AddBinding("points", "RealLink.Points");
            t.AddBinding("label", "Text");
            //t.AddBinding("userFlags", "UserFlags");
            AddPenBindings(t, "pen", "RealLink");
            AddFontBindings(t, "label", "MidLabel");
            rw.AddTransformer(t);
        }

        private static void AddBrushBindings(GoXmlBindingTransformer t, string xmlprefix, string objprefix)
        {
            t.AddBinding(xmlprefix + "color", objprefix + ".BrushColor");
            t.AddBinding(xmlprefix + "midcolor", objprefix + ".BrushMidColor");
            t.AddBinding(xmlprefix + "forecolor", objprefix + ".BrushForeColor");
            t.AddBinding(xmlprefix + "midfraction", objprefix + ".BrushMidFraction");
            t.AddBinding(xmlprefix + "brushstyle", objprefix + ".BrushStyle");
        }

        private static void AddFontBindings(GoXmlBindingTransformer t, string xmlprefix, string objprefix)
        {
            t.AddBinding(xmlprefix + "familyname", objprefix + ".FamilyName");
            t.AddBinding(xmlprefix + "fontsize", objprefix + ".FontSize");
            t.AddBinding(xmlprefix + "alignment", objprefix + ".Alignment");
            t.AddBinding(xmlprefix + "bold", objprefix + ".Bold");
            t.AddBinding(xmlprefix + "italic", objprefix + ".Italic");
            t.AddBinding(xmlprefix + "multiline", objprefix + ".Multiline");
            t.AddBinding(xmlprefix + "strikethru", objprefix + ".StrikeThrough");
            t.AddBinding(xmlprefix + "underline", objprefix + ".Underline");
            t.AddBinding(xmlprefix + "textcolor", objprefix + ".TextColor");
        }
        private static void AddPenBindings(GoXmlBindingTransformer t, string xmlprefix, string objprefix)
        {
            t.AddBinding(xmlprefix + "color", objprefix + ".PenColor");
            t.AddBinding(xmlprefix + "width", objprefix + ".PenWidth");
            t.AddBinding(xmlprefix + "dashstyle", objprefix + ".Pen.DashStyle");
        }

        public void StoreXml(FileStream ofile)
        {
            GoXmlWriter xw = new GoXmlWriter();
            InitReaderWriter(xw);
            xw.NodesGeneratedFirst = true;
            xw.Objects = this;
            xw.Generate(ofile);
        }
        public static StateChartDocument LoadXml(string ifile)
        {
            StateChartDocument state = null;
            if (File.Exists(ifile))
            {
                FileStream fs = File.OpenRead(ifile);
                state = LoadXml(fs);
            }
            return state;
        }

        public static StateChartDocument LoadXml(Stream ifile)
        {

            GoXmlReader xr = new GoXmlReader();
            InitReaderWriter(xr);
            StateChartDocument doc = xr.Consume(ifile) as StateChartDocument;
            if (doc == null) return null;
            return doc;
        }

        // handle undo and redo for the additional document state
        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            switch (e.Hint)
            {
                case ChangedPath:
                    this.Path = (String)e.GetValue(undo);
                    break;
                default:
                    base.ChangeValue(e, undo);
                    break;
            }
        }

        public const int ChangedPath = GoDocument.LastHint + 1;

        private String myPath = "";
    }



}

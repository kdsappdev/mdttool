using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MDT.Tools.MetaDesinger.Plugin.UI
{
    public class CListBoxItem : Entity
    {
        public string CnItem { get; set; }
        public string DisItem { get; set; }
        protected Rectangle location;
        public Rectangle Location
        {
            get { return location; }
            set { location = value; }
        }

        public RectangleF FK;
        public RectangleF Item;
        protected ConnectorCollection connectors = new ConnectorCollection();
        public override void Paint(System.Drawing.Graphics g)
        {
            string drawString = DisItem;
            string fk = "□";
            if (IsSelected)
            {
                fk = "☑";
            }
            Font fkFont = new Font(font.FontFamily, 13);
            var fkSize = g.MeasureString(fk, fkFont);
            FK = new RectangleF(location.Location, fkSize);
            Item = new RectangleF(location.X + fkSize.Width, location.Y, location.Width, location.Height);
            g.DrawString(fk, fkFont, new SolidBrush(site.ForeColor), FK, StringFormat.GenericDefault);

            g.DrawString(drawString, font, new SolidBrush(site.ForeColor), Item,

StringFormat.GenericDefault);
        }
        public bool isCheck(Point p)
        {
            return FK.Contains(p);
        }

        public override bool Hit(System.Drawing.Point p)
        {
            return false;
        }

        public override void Invalidate()
        {
            site.Invalidate(Location);
        }

        public override void Move(System.Drawing.Point p)
        {

            location.X += p.X;
            location.Y += p.Y;
            for (int k = 0; k < this.connectors.Count; k++)
            {
                connectors[k].Move(p);
            }
            this.Invalidate();
        }

        public Connector HitConnector(Point p)
        {
            for (int k = 0; k < connectors.Count; k++)
            {
                if (connectors[k].Hit(p))
                {
                    connectors[k].hovered = true;
                    connectors[k].Invalidate();
                    return connectors[k];
                }
                else
                {
                    connectors[k].hovered = false;
                    connectors[k].Invalidate();

                }


            }
            return null;

        }
    }
}

using NVelocity.Context;
using NVelocity.Runtime.Parser;
using NVelocity.Runtime.Parser.Node;
using System;
using System.IO;
using System.Text;
namespace NVelocity.Runtime.Directive
{
    public class Macro : Directive
    {
        private static bool debugMode;
        public override int Type
        {
            get
            {
                return 1;
            }
        }
        public override string Name
        {
            get
            {
                return "macro";
            }
        }
        public override bool Render(IInternalContextAdapter context, TextWriter writer, INode node)
        {
            return true;
        }
        public override void Init(IRuntimeServices rs, IInternalContextAdapter context, INode node)
        {
            base.Init(rs, context, node);
        }
        public static void ProcessAndRegister(IRuntimeServices rs, Token t, INode node, string sourceTemplate)
        {
            int numChildren = node.GetNumChildren();
            if (numChildren < 2)
            {
                rs.Log.Error("#macro error : Velocimacro must have name as 1st argument to #macro(). #args = " + numChildren);
                throw new MacroParseException("First argument to #macro() must be  macro name.", sourceTemplate, t);
            }
            int type = node.GetChild(0).Type;
            if (type != 9)
            {
                throw new MacroParseException("First argument to #macro() must be a token without surrounding ' or \", which specifies the macro name.  Currently it is a " + ParserTreeConstants.jjtNodeName[type], sourceTemplate, t);
            }
            string[] argArray = Macro.GetArgArray(node, rs);
            rs.AddVelocimacro(argArray[0], node.GetChild(numChildren - 1), argArray, sourceTemplate);
        }
        private static string[] GetArgArray(INode node, IRuntimeServices rsvc)
        {
            int num = node.GetNumChildren();
            num--;
            string[] array = new string[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = node.GetChild(i).FirstToken.Image;
                if (i > 0 && array[i].StartsWith("$"))
                {
                    array[i] = array[i].Substring(1, array[i].Length - 1);
                }
            }
            if (Macro.debugMode)
            {
                StringBuilder stringBuilder = new StringBuilder("Macro.getArgArray() : nbrArgs=");
                stringBuilder.Append(num).Append(" : ");
                Macro.MacroToString(stringBuilder, array);
                rsvc.Log.Debug(stringBuilder);
            }
            return array;
        }
        public static StringBuilder MacroToString(StringBuilder buf, string[] argArray)
        {
            StringBuilder stringBuilder = (buf == null) ? new StringBuilder() : buf;
            stringBuilder.Append('#').Append(argArray[0]).Append("( ");
            for (int i = 1; i < argArray.Length; i++)
            {
                stringBuilder.Append(' ').Append(argArray[i]);
            }
            stringBuilder.Append(" )");
            return stringBuilder;
        }
    }
}

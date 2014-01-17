using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Core.Lua
{
    public class LuaFuncDescriptor
    {
        private String FunctionName;
        private String FunctionDoc;
        private Dictionary<string, string> FunctionParameters;
       
        private String FunctionDocString;

        public LuaFuncDescriptor(String strFuncName, String strFuncDoc, Dictionary<string, string> pParams)
        {
            FunctionName = strFuncName;
            FunctionDoc = strFuncDoc;
            FunctionParameters = pParams;
        

            String strFuncHeader = strFuncName + "(%params%) - " + strFuncDoc;
            String strFuncBody = "\n\n";
            String strFuncParams = "";

            Boolean bFirst = true;

            foreach (var strFuncParam in FunctionParameters)
            {
                if (!bFirst)
                    strFuncParams += ", ";

                strFuncParams += strFuncParam.Key;
                strFuncBody += "\t" + strFuncParam.Key + "\t\t" + strFuncParam.Value + "\n";

                bFirst = false;
            }
            for (int i = 0; i < FunctionParameters.Count; i++)
            {
               
            }

            strFuncBody = strFuncBody.Substring(0, strFuncBody.Length - 1);
            if (bFirst)
                strFuncBody = strFuncBody.Substring(0, strFuncBody.Length - 1);

            FunctionDocString = strFuncHeader.Replace("%params%", strFuncParams) + strFuncBody;
        }

        public String getFuncName()
        {
            return FunctionName;
        }

        public String getFuncDoc()
        {
            return FunctionDoc;
        }

        

        public String getFuncHeader()
        {
            if (FunctionDocString.IndexOf("\n") == -1)
                return FunctionDocString;

            return FunctionDocString.Substring(0, FunctionDocString.IndexOf("\n"));
        }

        public String getFuncFullDoc()
        {
            return FunctionDocString;
        }
    }
}

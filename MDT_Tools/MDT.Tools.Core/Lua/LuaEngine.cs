using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using LuaInterface;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Core.Lua
{
    public class LuaEngine : ILuaEngine
    {
        private LuaInterface.Lua luavm = new LuaInterface.Lua();

        public void Dispose()
        {
            luavm.Dispose();
        }


        public void BindLuaFunctions(object luaAPIClass)
        {
            registerLuaFunctions(luaAPIClass);
        }

        public object[] DoFile(string luaFileName)
        {
            
            return luavm.DoFile(luaFileName);
        }

        public object[] DoString(string luaStr)
        {
            return luavm.DoString(luaStr);
        }

        public object[] Invoke(string luaFunction, params object[] args)
        {
            object[] os=null;
            LuaFunction fun = luavm.GetFunction(luaFunction);
            if (fun != null)
            {
                os=fun.Call(args);
            }
            return os;
        }

        public Dictionary<string, LuaFuncDescriptor> pLuaFuncs = new Dictionary<string, LuaFuncDescriptor>();

        public void registerLuaFunctions(Object pTarget)
        {
            // Sanity checks
            if (luavm == null || pLuaFuncs == null)
                return;

            // Get the target type
            Type pTrgType = pTarget.GetType();

            // ... and simply iterate through all it's methods
            foreach (MethodInfo mInfo in pTrgType.GetMethods())
            {
                // ... then through all this method's attributes
                foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))
                {
                    // and if they happen to be one of our AttrLuaFunc attributes
                    if (attr.GetType() == typeof(AttrLuaFunc))
                    {
                        AttrLuaFunc pAttr = (AttrLuaFunc)attr;
                        Dictionary<string, string> pParams = new Dictionary<string, string>();

                        // Get the desired function name and doc string, along with parameter info
                        string strFName = pAttr.getFuncName();
                        string strFDoc = pAttr.getFuncDoc();
                        string[] pPrmDocs = pAttr.getFuncParams();

                        // Now get the expected parameters from the MethodInfo object
                        ParameterInfo[] pPrmInfo = mInfo.GetParameters();

                        // If they don't match, someone forgot to add some documentation to the
                        // attribute, complain and go to the next method
                        if (pPrmDocs != null && (pPrmInfo.Length != pPrmDocs.Length))
                        {
                            Console.WriteLine("Function " + mInfo.Name + " (exported as " +
                                              strFName + ") argument number mismatch. Declared " +
                                              pPrmDocs.Length + " but requires " +
                                              pPrmInfo.Length + ".");
                            break;
                        }

                        // Build a parameter <-> parameter doc hashtable
                        for (int i = 0; i < pPrmInfo.Length; i++)
                        {
                            pParams.Add(pPrmInfo[i].Name, pPrmDocs[i]);
                        }

                        // Get a new function descriptor from this information
                        LuaFuncDescriptor pDesc = new LuaFuncDescriptor(strFName, strFDoc, pParams);

                        // Add it to the global hashtable
                        pLuaFuncs.Add(strFName, pDesc);

                        // And tell the VM to register it.
                        luavm.RegisterFunction(strFName, pTarget, mInfo);
                    }
                }
            }
        }

        public override string ToString()
        {
            string str = "\n\r";
            foreach (KeyValuePair<string, LuaFuncDescriptor> luaFuncDescriptor in pLuaFuncs)
            {
                str += luaFuncDescriptor.Value.getFuncFullDoc()+"\n\r";
            }
            return str;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.Core.Lua;

namespace MDT.Tools.Core.Utils
{
    public class LuaHelper
    {
        private static ILuaEngine luaEngine=new LuaEngine();
        public static void BindLuaFunctions(object luaAPIClass)
        {
            luaEngine.BindLuaFunctions(luaAPIClass);
        }

        public static ILuaEngine GetLuaEngine()
        {
            return luaEngine;
        }
    }
}

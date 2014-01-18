using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.Core.Lua;

namespace MDT.Tools.Core.Utils
{
    public class LuaHelper
    {

        public static ILuaEngine CreateLuaEngine()
        {
            ILuaEngine luaEngine = new LuaEngine();
            return luaEngine;
        }
         
    }
}

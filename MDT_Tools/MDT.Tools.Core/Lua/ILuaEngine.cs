using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Core.Lua
{
    public interface ILuaEngine : IDisposable
    {
        void BindLuaFunctions(object luaAPIClass);
        object[] DoFile(string luaFileName);
        object[] DoString(string luaStr);
        object[] Invoke(string luaFunction, params object[] args);
    }
}

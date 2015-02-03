using System;
using com.adaptiveMQ2.message;

namespace MDT.Tools.CEDA.Plugin
{
   
    public class EventHelper
    {
          public event Action<Message> OnCedaMessage;
    }
}
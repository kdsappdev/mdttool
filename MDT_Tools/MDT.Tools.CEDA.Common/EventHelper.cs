using System;
using com.adaptiveMQ2.message;

namespace MDT.Tools.CEDA.Common
{
   
    public class EventHelper
    {
          public event Action<Message> OnCedaMessage;
    }
}


namespace MDT.Tools.Aliyun.Upload.Plugin.DataMemory
{
    public class XMLDataMemory:IDataMemory
    {
        private UserDataManager umd = new UserDataManager();
        public string GetData(string key)
        {
            UserDataObject udo = umd.GetUserObject( key);
            if (udo != null)
            {
                return udo.Value;
            }
            return "";
        }
        public void SetData(string key, string value)
        {
            UserDataObject udo = new UserDataObject();
            udo.Key = key;
            udo.Value = value;
            umd.SaveUserObject(udo);
        }
    }
}

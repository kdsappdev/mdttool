namespace MDT.Tools.CEDA.Plugin.DataMemory
{
    /// <summary>
    /// 记忆接口，存储本地信息
    /// </summary>
    /// <remarks>
    /// 2013.12.10: 创建. deshuai.kong <br/>
    /// </remarks>
    public interface IDataMemory
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">数据Key</param>
        /// <returns>数据字符串</returns>
        string GetData(string key);
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">数据Key</param>
        /// <param name="value">数据字符串</param>
        void SetData(string key, string value);
    }
}

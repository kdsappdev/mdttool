using System.Windows.Forms;

namespace MDT.Tools.DB.Ibatis.Plugin.Utils
{
    public class ConstHelper
    {
        //public const string DBConfigName = "Test";
        //public const string Tables = "_TABLES";
        //public const string TablesColumns = "_TABLES_COLUMNS";
        //public const string Views = "_VIEWS";
        //public const string TablesPrimaryKeys = "_TABLES_PK";
        public const string PoTail = ".java";
        public const string PoExampleTail = "Example.java";
        public const string DaoTail = "DAO.java";
        public const string DaoImplTail = "DAOImpl.java";
        public const string SqlMapTail = "_SqlMap.xml";
        public const string PoPackage = "ats.common.model.po";
        public const string DaoPackage = "ats.common.model.dao";

        public static readonly string TemplatesPath = Application.StartupPath + "\\templates\\";
        //public static readonly string ModelConfigPath = Application.StartupPath + "\\control\\ATSModelConfig_ORA.xml";
        public static readonly string ModelConfigPath = Application.StartupPath + "\\control\\MDT_ORA.xml";


        public static readonly string PoPath = Application.StartupPath + @"\data\model\po\";
        public static readonly string DaoPath = Application.StartupPath + @"\data\model\dao\";

    }
}
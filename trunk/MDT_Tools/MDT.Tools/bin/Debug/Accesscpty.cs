using System;
namespace 
{
	/// <summary>
	/// Accesscpty
	/// </summary>
	/// <remarks>
	/// 2013.11.30: 创建. Csharp代码生成插件(V1.0.0.0) <br/>
	/// </remarks>
	[Serializable]
	public class Accesscpty
	{
		#region 字段

		private decimal _id = 0M;
		private string _atsuserShortname;
		private string _accessmode;
		private decimal? _accessrightread;
		private decimal? _accessrightwrite;
		private decimal? _oca;
		private string _location = "";

		#endregion

		#region 属性

		public decimal Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public string AtsuserShortname
		{
			get { return _atsuserShortname; }
			set { _atsuserShortname = value; }
		}

		public string Accessmode
		{
			get { return _accessmode; }
			set { _accessmode = value; }
		}

		public decimal? Accessrightread
		{
			get { return _accessrightread; }
			set { _accessrightread = value; }
		}

		public decimal? Accessrightwrite
		{
			get { return _accessrightwrite; }
			set { _accessrightwrite = value; }
		}

		public decimal? Oca
		{
			get { return _oca; }
			set { _oca = value; }
		}

		public string Location
		{
			get { return _location; }
			set { _location = value; }
		}

		#endregion

	}
}
//#define 指令不能用于声明常量值
//#define PASS_OK     (100)
//#define PASS_NO		(102)
//#define ERR_RANGE_STR	(-100)

using System;
using DevExpress.Xpo;

namespace DataProcess.Controls
{
	static class Constants
    {
		public const int PASS_OK = 100;
		public const int PASS_NO = 102;
		public const int ERR_RANGE_STR = -100;
	}


	public class DetermineDataInterval
    {
		// 判断一个浮点数是否落在范围串中
		//	范围串符合数学上区间的定义：[a,b]、(a,b)、(-x, 100).....
		//	“∞”用“x”表示
		//
		public  static int InRange(string pBuf, double fVal)
		{
			double fMin = 0.0, fMax = 0.0;
			string strRange = pBuf;
			strRange.Replace(" ", "");  //去掉里面的空格


			//int nPosComma = strRange.Find(",");
			int nPosComma = strRange.IndexOf(",");
			if (nPosComma == -1)
				return Constants.ERR_RANGE_STR;

			//取出左值串
			string strLeft = strRange.Substring(1, nPosComma - 1);
			if (strLeft == String.Empty)
				return Constants.ERR_RANGE_STR;

			if (strLeft == "-x" || strLeft == "x")
				fMin = -9.9e10;
			else
				fMin = float.Parse(strLeft);

			//取出右值串
			string strRight = strRange.Substring(nPosComma + 1, strRange.Length - 1 - nPosComma - 1);
			if (strRight == String.Empty)
				return Constants.ERR_RANGE_STR;

			if (strRight == "+x" || strRight == "x")
				fMax = +9.9e10;
			else
				fMax = float.Parse(strRight);

			//判断是小括号“(”还是中括号“[”
			bool b1, b2;
			b1 = b2 = false;
			if (strRange.Substring(0,1) == "(")
				b1 = (fVal > fMin) ? true : false;
			else if (strRange.Substring(0, 1) == "[")
				b1 = (fVal >= fMin) ? true : false;
			else
				return Constants.ERR_RANGE_STR;


			if (strRange.Substring(strRange.Length-1,1) == ")")
				b2 = (fVal < fMax) ? true : false;
			else if (strRange.Substring(strRange.Length - 1, 1) == "]")
				b2 = (fVal <= fMax) ? true : false;
			else
				return Constants.ERR_RANGE_STR;

			if (b1 && b2)
				return Constants.PASS_OK; //落在
			else
				return Constants.PASS_NO; //没有落在
		}
	}

}
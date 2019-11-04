using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Common.Utils;

namespace zTest
{
	public class TestSynchroTwoValues
	{

		public static void Test()
		{
			
			int i1 = 2;
			int i2 = 2;
			Assert.IsTrue(CUtilReflex.IsEqualValues((object) i1, (object) i2));

			i1 = 2;
			i2 = 3;
			Assert.IsFalse(CUtilReflex.IsEqualValues((object)i1, (object)i2));

			long l1 = 2;
			long l2 = 2;

			Assert.IsTrue(CUtilReflex.IsEqualValues((object) l1, (object) l2));

			l1 = 2;
			l2 = 3;

			Assert.IsFalse(CUtilReflex.IsEqualValues((object)l1, (object)l2));

			decimal d1 = 2.01m;
			decimal d2 = 2.01m;

			Assert.IsTrue(CUtilReflex.IsEqualValues((object)d1, (object)d2));

			d1 = 2.01m;
			d2 = 2.02m;

			Assert.IsFalse(CUtilReflex.IsEqualValues((object)d1, (object)d2));

			double dbl1 = 2.01;
			double dbl2 = 2.01;

			Assert.IsTrue(CUtilReflex.IsEqualValues((object)dbl1, (object)dbl2));

			 dbl1 = 2.01;
			 dbl2 = 2.02;

			 Assert.IsFalse(CUtilReflex.IsEqualValues((object)dbl1, (object)dbl2));

			string st1 = "ab";
			string st2 = "ab";

			Assert.IsTrue(CUtilReflex.IsEqualValues((object)st1, (object)st2));

			st1 = "ab";
			st2 = "aB";

			Assert.IsFalse(CUtilReflex.IsEqualValues((object)st1, (object)st2));
		}

	}
}

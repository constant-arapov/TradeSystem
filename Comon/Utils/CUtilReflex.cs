using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;


namespace Common.Utils
{
    public static class CUtilReflex
    {
        public static bool SetPropertyValue(object classConstainsProperty,
                                            PropertyInfo property, 
                                             object value)
        {

            object convValue =ConvertValue(property.PropertyType, value);
            if (convValue != null)
            {


                property.SetValue(classConstainsProperty,
                                    convValue,
                                    null);

                return true;
            }
            return false;
        }



        public static object ConvertValue(Type typeProprty, object value)
        {
            string stPropType = typeProprty.Name;
            string stValue = value.ToString();


            if (stPropType == "String")
                return stValue;
            else if (stPropType == "Double")
                return Convert.ToDouble(stValue);

            else if (stPropType == "Int16")
                return Convert.ToInt16(stValue);
            else if (stPropType == "Int32")
                return Convert.ToInt32(stValue);
            else if (stPropType == "Int64")
                return Convert.ToInt64(stValue);
            else if (stPropType == "Boolean")
                return Convert.ToBoolean(stValue);
            //added 2018-04-09
            else if (stPropType == "Decimal")
                return CUtilConv.ToDecimal(stValue);



            else
                return null;
        }



		public static bool IsEqualValues(object src, object dst)
		{
			string stSrcType = src.GetType().Name;
			string stDstType = dst.GetType().Name;

			if (stSrcType != stDstType)
				throw new ApplicationException("CUtilReflex.SynchroToValues. Types are notequal");

			
			if (stSrcType == "String")
			{
				string srcValue = src.ToString();
				string dstValue = dst.ToString();

				if (srcValue != dstValue)
					return false;

				
			}
			else if (stSrcType == "Double")
			{
				double srcValue = Convert.ToDouble(src);
				double dstValue = Convert.ToDouble(dst);

				if (srcValue != dstValue)
					return false;

				
			}
			else if (stSrcType == "Int16")
			{
				Int16 srcValue = Convert.ToInt16(src);
				Int16 dstValue = Convert.ToInt16(dst);

				if (srcValue != dstValue)
					return false;

				
			}
			else if (stSrcType == "Int32")
			{
				Int32 srcValue = Convert.ToInt32(src);
				Int32 dstValue = Convert.ToInt32(dst);

				if (srcValue != dstValue)
					return false;

				
			}
			else if (stSrcType == "Int64")
			{
				Int64 srcValue = Convert.ToInt64(src);
				Int64 dstValue = Convert.ToInt64(dst);

				if (srcValue != dstValue)
					return false;

				
			}
			else if (stSrcType == "Boolean")
			{
				bool srcValue = Convert.ToBoolean(src);
				bool dstValue = Convert.ToBoolean(dst);

				if (srcValue != dstValue)
					return false;

				
			}
			else if (stSrcType == "Decimal")
			{
				decimal srcValue = Convert.ToDecimal(src);
				decimal dstValue = Convert.ToDecimal(dst);

				if (srcValue != dstValue)
					return false;

				
			}

            //2018-09-06
            else if (stSrcType == "DateTime")
            {
                DateTime srcValue = Convert.ToDateTime(src);
                DateTime dstValue = Convert.ToDateTime(dst);

                if (srcValue != dstValue)
                    return false;


            }


			else
			{
				throw new ApplicationException("CUtilReflex.SynchroToValues. Types not supported");
			}

			return true;
		}



        public static FieldInfo GetDependencyPropertyField(string name, Type type)
        {
            string dpName = name + "Property";
            
            return GetField(dpName, type);

        }

        public static FieldInfo GetField(string name, Type type)
        {
            if (type == null)
                return null;

            return type.GetField(name);
        }


        public static List<PropertyInfo> GetPropertiesList(string stType)
        {
           
            return Type.GetType(stType).GetProperties().ToList();

        }

        public static List<FieldInfo> GetFiedsList(string stType)
        {

            return Type.GetType(stType).GetFields().ToList();

        }


        public static List<MemberInfo> GetMembersList(string stType)
        {

            return Type.GetType(stType).GetMembers().ToList();

        }

    }
}

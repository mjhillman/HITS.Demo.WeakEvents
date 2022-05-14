using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace HITS.Extensions.String
{
    /// <summary>
    /// This class provides utility methods for converting string values to other data types
    /// Some of this code was taken from ...
    /// https://msdn.microsoft.com/en-us/library/system.web.webpages.stringextensions(v=vs.111).aspx
    /// with some changes.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// This method checks if a string value is DBNull, Null or Whitespace.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>true if value is DBNull, Null or Whitespace; otherwise, false.</returns>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// This method strips all non-digit characters from a string.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>new string</returns>
        public static string GetDigits(this string value)
        {
            return new string(value.Where(c => char.IsDigit(c)).ToArray());
        }

        /// <summary>
        /// This method returns just the letters (upper and lower case) from a string.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>new string</returns>
        public static string GetLetters(this string value)
        {
            return new string(value.Where(c => char.IsLetter(c)).ToArray());
        }

        /// <summary>
        /// This method returns just the letters and white space from a string.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>new string</returns>
        public static string GetLettersAndSpaces(this string value)
        {
            return new string(value.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)).ToArray());
        }

        /// <summary>
        /// This method returns just the letters and digits from a string.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>new string</returns>
        public static string GetLetterOrDigit(this string value)
        {
            return new string(value.Where(c => char.IsLetterOrDigit(c)).ToArray());
        }

        /// <summary>
        /// This method will return only the characters of the string containing letter, numbers whitespace and punctuation.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>new string</returns>
        public static string GetSafeString(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "";
            byte[] bytes = Encoding.Default.GetBytes(value);
            value = Encoding.UTF8.GetString(bytes);
            return new string(value.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || char.IsPunctuation(c) || char.IsControl(c)).ToArray());
        }

        /// <summary>
        /// This method returns string.Empty if the value is DBNull, Null or Whitespace.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>string.Empty if value is DBNull, Null or Whitespace; otherwise, the value.</returns>
        public static string AsString(this string value)
        {
            return value.IsEmpty() ? string.Empty : value;
        }

        /// <summary>
        /// This method converts a string to an integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>
        /// When you use instance method syntax to call this method, omit the parameter
        /// If you pass a value that cannot be converted to the type, the method returns 0.
        /// </remarks>
        public static int AsInt(this string value)
        {
            return value.AsInt(0);
        }

        /// <summary>
        /// This method converts a string to an integer and specifies a default value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if value is null or invalid.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static int AsInt(this string value, int defaultValue)
        {
            int result;
            if (!int.TryParse(value, out result))
                return defaultValue;
            return result;
        }

        /// <summary>
        /// This method converts a string to a long.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>
        /// When you use instance method syntax to call this method, omit the parameter
        /// If you pass a value that cannot be converted to the type, the method returns 0.
        /// </remarks>
        public static long AsLong(this string value)
        {
            return value.AsLong(0);
        }

        /// <summary>
        /// This method converts a string to a long and specifies a default value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if value is null or invalid.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static long AsLong(this string value, int defaultValue)
        {
            long result;
            if (!long.TryParse(value, out result))
                return defaultValue;
            return result;
        }

        /// <summary>
        /// This method converts a string to a Decimal.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>
        /// When you use instance method syntax to call this method, omit the parameter
        /// If you pass a value that cannot be converted to the type, the method returns 0.
        /// </remarks>
        public static Decimal AsDecimal(this string value)
        {
            return value.As<Decimal>();
        }

        /// <summary>
        /// This method converts a string to a Decimal number and specifies a default value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if value is null or invalid.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static Decimal AsDecimal(this string value, Decimal defaultValue)
        {
            decimal result;
            if (!decimal.TryParse(value, out result))
                return defaultValue;
            return result;
        }

        /// <summary>
        /// This method converts a string to a Double.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>
        /// When you use instance method syntax to call this method, omit the parameter
        /// If you pass a value that cannot be converted to the type, the method returns 0.
        /// </remarks>
        public static Double AsDouble(this string value)
        {
            return value.As<Double>();
        }

        /// <summary>
        /// This method converts a string to a Double and specifies a default value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if value is null or invalid.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static Double AsDouble(this string value, Double defaultValue)
        {
            Double result;
            if (!Double.TryParse(value, out result))
                return defaultValue;
            return result;
        }

        /// <summary>
        /// This method converts a string to a Single.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>
        /// When you use instance method syntax to call this method, omit the parameter
        /// If you pass a value that cannot be converted to the type, the method returns 0.
        /// </remarks>
        public static Single AsSingle(this string value)
        {
            return value.As<Single>();
        }

        /// <summary>
        /// This method converts a string to a Single and specifies a default value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if value is null or invalid.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static Single AsSingle(this string value, Single defaultValue)
        {
            Single result;
            if (!Single.TryParse(value, out result))
                return defaultValue;
            return result;
        }

        /// <summary>
        /// This method converts a string to a Float.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>
        /// When you use instance method syntax to call this method, omit the parameter
        /// If you pass a value that cannot be converted to the type, the method returns 0.
        /// </remarks>
        public static float AsFloat(this string value)
        {
            return value.AsFloat(0.0f);
        }

        /// <summary>
        /// This method converts a string to a Float and specifies a default value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if value is null or invalid.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static float AsFloat(this string value, float defaultValue)
        {
            float result;
            if (!float.TryParse(value, out result))
                return defaultValue;
            return result;
        }

        /// <summary>
        /// This method converts a string to a DateTime.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>
        /// When you use instance method syntax to call this method, omit the parameter
        /// If you pass a value that cannot be converted to the type, the method returns the default DateTime.
        /// </remarks>
        public static DateTime AsDateTime(this string value)
        {
            return value.AsDateTime(new DateTime());
        }

        /// <summary>
        /// This method converts a string to a DateTime and specifies a default value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if value is null or invalid.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static DateTime AsDateTime(this string value, DateTime defaultValue)
        {
            DateTime result;
            if (!DateTime.TryParse(value, out result))
                return defaultValue;
            return result;
        }

        /// <summary>
        /// This method converts a string to a bool.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>
        /// When you use instance method syntax to call this method, omit the parameter
        /// If you pass a value that cannot be converted to the type, the method returns the false.
        /// </remarks>
        public static bool AsBool(this string value)
        {
            return value.AsBool(false);
        }

        /// <summary>
        /// This method converts a string to a bool and specifies a default value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if value is null or invalid.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static bool AsBool(this string value, bool defaultValue)
        {
            bool result;
            if (!bool.TryParse(value, out result))
                return defaultValue;
            return result;
        }

        /// <summary>
        /// This method converts a string to it's base64 representation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>base64 encoded string</returns>
        public static string AsBase64Encoded(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(value);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// This method converts a base64 value back to a string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AsBase64Decoded(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            byte[] base64EncodedBytes = System.Convert.FromBase64String(value);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// This method converts a string to a strongly typed value of the specified data type.
        /// </summary>
        /// <typeparam name="TValue">The data type to convert to.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static TValue As<TValue>(this string value)
        {
            return value.As<TValue>(default(TValue));
        }

        /// <summary>
        /// This method converts a string to the specified data type and specifies a default value.
        /// </summary>
        /// <typeparam name="TValue">The data type to convert to.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if value is null.</param>
        /// <returns>The converted value</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static TValue As<TValue>(this string value, TValue defaultValue)
        {
            try
            {
                TypeConverter converter1 = TypeDescriptor.GetConverter(typeof(TValue));
                if (converter1.CanConvertFrom(typeof(string)))
                    return (TValue)converter1.ConvertFrom((object)value);
                TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(string));
                if (converter2.CanConvertTo(typeof(TValue)))
                    return (TValue)converter2.ConvertTo((object)value, typeof(TValue));
            }
            catch
            {
            }
            return defaultValue;
        }

        /// <summary>
        /// This method checks whether a string can be converted to the Boolean (true/false) type.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>true if value can be converted to the specified type; otherwise, false.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static bool IsBool(this string value)
        {
            bool result;
            return bool.TryParse(value, out result);
        }

        /// <summary>
        /// This method checks whether a string can be converted to an integer.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>true if value can be converted to the specified type; otherwise, false.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static bool IsInt(this string value)
        {
            int result;
            return int.TryParse(value, out result);
        }

        /// <summary>
        /// This method checks whether a string can be converted to an long.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>true if value can be converted to the specified type; otherwise, false.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static bool IsLong(this string value)
        {
            long result;
            return long.TryParse(value, out result);
        }

        /// <summary>
        /// This method checks whether a string can be converted to a Decimal.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>true if value can be converted to the specified type; otherwise, false.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static bool IsDecimal(this string value)
        {
            decimal result;
            return decimal.TryParse(value, out result);
        }

        /// <summary>
        /// This method checks whether a string can be converted to a float.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>true if value can be converted to the specified type; otherwise, false.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static bool IsFloat(this string value)
        {
            float result;
            return float.TryParse(value, out result);
        }

        /// <summary>
        /// This method checks whether a string contains only digit characters.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>true if value is all digits; otherwise, false.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static bool IsAllDigits(this string value)
        {
            if (value.IsEmpty()) return false;

            foreach (char c in value)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        /// <summary>
        /// This method checks whether a string can be converted to a single.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>true if value can be converted to the specified type; otherwise, false.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static bool IsSingle(this string value)
        {
            Single result;
            return Single.TryParse(value, out result);
        }

        /// <summary>
        /// This method checks whether a string can be converted to a double.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>true if value can be converted to the specified type; otherwise, false.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static bool IsDouble(this string value)
        {
            double result;
            return double.TryParse(value, out result);
        }

        /// <summary>
        /// This method checks whether a string can be converted to a DateTime.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>true if value can be converted to the specified type; otherwise, false.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static bool IsDateTime(this string value)
        {
            DateTime result;
            return DateTime.TryParse(value, out result);
        }

        /// <summary>
        /// This method checks whether a string can be converted to the specified data type.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>true if value can be converted to the specified type; otherwise, false.</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter.</remarks>
        public static bool Is<TValue>(this string value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(TValue));
            if (converter != null)
            {
                try
                {
                    if (value != null)
                    {
                        if (!converter.CanConvertFrom((ITypeDescriptorContext)null, value.GetType()))
                        {
                            return false;
                        }
                    }
                    converter.ConvertFrom((ITypeDescriptorContext)null, CultureInfo.CurrentCulture, (object)value);
                    return true;
                }
                catch
                {
                }
            }
            return false;
        }

        /// <summary>
        /// This method checks whether a string contains a valid email address.
        /// </summary>
        /// <param name="value">The string value to test.</param>
        /// <returns>true if a valid email address</returns>
        public static bool IsValidEmailAddress(this string value)
        {
            try
            {
                if (!value.Contains("@")) return false;
                var emailChecked = new System.Net.Mail.MailAddress(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// This method formats an XML document.
        /// </summary>
        /// <param name="doc">the XML document</param>
        /// <returns>formatted XML string</returns>
        public static string AsPrettyXml(this XmlDocument doc)
        {
            StringWriter stringWriter = new StringWriter();

            try
            {
                // Format the XML text.
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
                xmlTextWriter.Formatting = Formatting.Indented;
                doc.WriteTo(xmlTextWriter);
                return stringWriter.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
            finally
            {
                stringWriter.Dispose();
            }
        }

        /// <summary>
        /// This method formats an XML string.
        /// </summary>
        /// <param name="xmlString">the XML string</param>
        /// <returns>formatted XML string</returns>
        public static string AsPrettyXml(this string xmlString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            return AsPrettyXml(doc);
        }

        /// <summary>
        /// This method formats an JSON string.
        /// </summary>
        /// <param name="json">the JSON string</param>
        /// <returns>formatted JSON string</returns>
        /// <remarks>https://stackoverflow.com/questions/4580397/json-formatter-in-c/24782322#24782322</remarks>
        public static string AsPrettyJson(this string json)
        {
            if (json.IsEmpty()) return json;

            const string INDENT_STRING = "    ";
            int indentation = 0;
            int quoteCount = 0;

            try
            {
                var result =
                    from ch in json
                    let quotes = ch == '"' ? quoteCount++ : quoteCount
                    let lineBreak = ch == ',' && quotes % 2 == 0 ? ch + Environment.NewLine + string.Concat(Enumerable.Repeat(INDENT_STRING, indentation)) : null
                    let openChar = ch == '{' || ch == '[' ? ch + Environment.NewLine + string.Concat(Enumerable.Repeat(INDENT_STRING, ++indentation)) : ch.ToString()
                    let closeChar = ch == '}' || ch == ']' ? Environment.NewLine + string.Concat(Enumerable.Repeat(INDENT_STRING, --indentation)) + ch : ch.ToString()
                    select lineBreak == null
                                ? openChar.Length > 1
                                    ? openChar
                                    : closeChar
                                : lineBreak;

                return string.Concat(result);
            }
            catch (Exception)
            {
                return json;
            }
        }

        /// <summary>
        /// This method will convert a json string to a POCO object.
        /// </summary>
        /// <typeparam name="T">the object type</typeparam>
        /// <param name="jsonString">the json string</param>
        /// <returns>POCO object of type T</returns>
        /// <remarks>See the method Serialize in the ObjectExtensions class</remarks>
        public static T Deserialize<T>(this string jsonString)
        {
            if (jsonString.IsEmpty()) return default(T);
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        /// <summary>
        /// This method will convert a POCO object to json
        /// </summary>
        /// <param name="dataToSerialize">the POCO object</param>
        /// <returns>json string</returns>
        /// <remarks>When you use instance method syntax to call this method, omit the parameter</remarks> 
        public static string Serialize(this object dataToSerialize)
        {
            if (dataToSerialize == null) return null;
            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(dataToSerialize, options);
        }

        /// <summary>
        /// This method replaces carriage return and line feeds with a space.
        /// </summary>
        /// <param name="value">the string value to trim</param>
        /// <returns>string without carriage returns and line feeds</returns>
        public static string TrimCrLf(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.Replace("\\r\\n", " ")
                            .Replace("\\r", " ")
                            .Replace("\\n", " ")
                            .Replace("\r\n", " ")
                            .Replace("\r", " ")
                            .Replace("\n", " ")
                            .Replace("\\", string.Empty)
                            .Replace("  ", " ")
                            .Trim();
            }

            return value;
        }

    }
}

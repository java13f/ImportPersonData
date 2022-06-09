using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ImportPersonDataLib.Tests")]
namespace ImportPersonDataLib
{

    public class Utils
    {
               
        internal string GetNameTable(string sql)
        {
            string query = sql.ToLower();

            try
            {
                string[] arr = query.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                int index = Array.IndexOf(arr, "from");

                if (index == -1)
                {
                    throw new ArgumentException("Method: GetNameTable; Error: Ошибка определения оператора 'from'");
                }
                                
                if (arr.Length -1 <= index)
                {
                    throw new ArgumentException("Method: GetNameTable; Error: Не указана таблица.");
                }

                string nameTable = arr[index + 1];
                                                            
                return nameTable;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Method: GetNameTable; Error: Ошибка определения имени таблицы. {ex.Message}");
            }            
        }

        /// <summary>
        /// Отделить цифру от букву в номере дома/квартиры
        /// </summary>
        /// <param name="num">131А</param>
        /// <param name="partC">return 131</param>
        /// <param name="partB">return А</param>
        internal void SeparatNumberAndLetter(string num, out int partC, out string partB)
        {
            if (string.IsNullOrWhiteSpace(num))
            {
                throw new ArgumentException("Method: SeparatNumberAndLetter; Error: Не указан аргумент.");
            }

            string tmpC = "";
            partB = "";
            bool isNumber = true;

            foreach (var item in num)
            {
                if (string.IsNullOrWhiteSpace(item.ToString()))
                {
                    continue;
                }

                if (isNumber && Char.IsNumber(item))
                {
                    tmpC += item;
                }
                else
                {
                    isNumber = false;
                    partB += item.ToString();
                }
            }


            if (tmpC.Length == 0)
            {
                throw new ArgumentException("Method: SeparatNumberAndLetter; Error: Не указана числовая часть.");
            }
            partC = int.Parse(tmpC);


            //Проверить чтоб первый символ был, либо цифра, либо буква
            partB = RemoveSpecialCharFromStartAndEnd(partB);
        }


        /// <summary>
        /// Удалить специальный символ с начала и конца строки
        /// </summary>
        /// <returns></returns>
        private string RemoveSpecialCharFromStartAndEnd(string str)
        {

            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }

            if (!char.IsLetterOrDigit(str[0]))
            {
                str = str.Remove(0, 1);
                str = RemoveSpecialCharFromStartAndEnd(str);
            }

            var lastIndex = str.Length - 1;
            if (!char.IsLetterOrDigit(str[lastIndex]))
            {
                str = str.Remove(lastIndex, 1);
                str = RemoveSpecialCharFromStartAndEnd(str);
            }


            return str;
        }

    }
}

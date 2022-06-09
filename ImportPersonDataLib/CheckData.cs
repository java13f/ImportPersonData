using ImportPersonDataLib.Interfaces;
using ImportPersonDataLib.Models;
using System.Collections.Generic;

namespace ImportPersonDataLib
{
    public class CheckData : ICheckData
    {
        public bool CheckList(List<Person> persons)
        {
            //Обязательные поля к заполнению: Ф, И, ДР, Район, Город, Улица, Дом 
            List<string> errors;
            bool isError = false;

            foreach (var p in persons)
            {
                errors = new List<string>();

                if (string.IsNullOrWhiteSpace(p.Surname))
                {
                    errors.Add("Поле \"Фамилия\" не заполнено.");
                }
                if (string.IsNullOrWhiteSpace(p.Name))
                {
                    errors.Add("Поле \"Имя\" не заполнено.");
                }
                if (p.DateBirth == null)
                {
                    errors.Add("Поле \"Дата рождения\" не заполнено.");
                }
                if (string.IsNullOrWhiteSpace(p.Rayon))
                {
                    errors.Add("Поле \"Район\" не заполнено.");
                }
                if (string.IsNullOrWhiteSpace(p.Gorod))
                {
                    errors.Add("Поле \"Город\" не заполнено.");
                }
                if (string.IsNullOrWhiteSpace(p.Street))
                {
                    errors.Add("Поле \"Улица\" не заполнено.");
                }
                if (string.IsNullOrWhiteSpace(p.Dom))
                {
                    errors.Add("Поле \"Дом\" не заполнено.");
                }

                if (errors.Count > 0)
                {
                    p.ErrorMessage = string.Join('#', errors);
                    isError = true;
                }
            }

            if (isError)
            {
                return false;
            }

            return true;
        }

        

    }
}

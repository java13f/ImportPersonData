# Импорт данных

### Алгоритм выполнения:

- Библиотека ImportPersonDataLib
- Класс ImportPersonDataFromDb, конструктор принимает два параметра: sql запрос и строку подключения
- В Web версии sql запрос и строка подключения находятся в файле appsettings.json
- Метод Import()
    - Метод AddFields(): добавляет два поля в базу данных (isImport, errorMessage)
	- Метод CheckDataBeforeImport(): проверяет заполнение обязательных полей (фамилия, имя, дата рождения, район, город, улица, дом). Если   поле не заполнено, то в поле ErrorMessage записывается ошибка
	- Метод ExecuteImport()
		- Получение записей у которых поле isImport is null
		- Поиск записи в таблице Addresses 
		- Если адресс не существует, то пытаемся получить idGorodRayon, idStreet, idDom, idKv
		- Если не существует одного из ид (idGorodRayon, idStreet, idDom, idKv), то добавляем запись в соответствующую таблицу (GorodRayon, Streets, Dom, Kv)
		- Получив idGorodRayon, idStreet, idDom, idKv добавляем запись в таблицу Addresses
		- Получив idAddress добавляем запись в основную таблицу
		- В случае возникновения ошибку, записываем лог файл (/Документы/logfile.txt)

- [x] Юнит тесты

# ������ ������

### �������� ����������:

- ���������� ImportPersonDataLib
- ����� ImportPersonDataFromDb, ����������� ��������� ��� ���������: sql ������ � ������ �����������
- � Web ������ sql ������ � ������ ����������� ��������� � ����� appsettings.json
- ����� Import()
    - ����� AddFields(): ��������� ��� ���� � ���� ������ (isImport, errorMessage)
	- ����� CheckDataBeforeImport(): ��������� ���������� ������������ ����� (�������, ���, ���� ��������, �����, �����, �����, ���). ����   ���� �� ���������, �� � ���� ErrorMessage ������������ ������
	- ����� ExecuteImport()
		- ��������� ������� � ������� ���� isImport is null
		- ����� ������ � ������� Addresses 
		- ���� ������ �� ����������, �� �������� �������� idGorodRayon, idStreet, idDom, idKv
		- ���� �� ���������� ������ �� �� (idGorodRayon, idStreet, idDom, idKv), �� ��������� ������ � ��������������� ������� (GorodRayon, Streets, Dom, Kv)
		- ������� idGorodRayon, idStreet, idDom, idKv ��������� ������ � ������� Addresses
		- ������� idAddress ��������� ������ � �������� �������
		- � ������ ������������� ������, ���������� ��� ���� (/���������/logfile.txt)

- [x] ���� �����

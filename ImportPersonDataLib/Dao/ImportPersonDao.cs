using ImportPersonDataLib.Interfaces;
using ImportPersonDataLib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;


namespace ImportPersonDataLib.Dao
{
    public class ImportPersonDao : Utils, IImportPersonDao
    {
        private readonly string connectionString;
        private readonly string sql;

        public ImportPersonDao(string sql, string connectionString)
        {
            this.sql = sql;
            this.connectionString = connectionString;
        }

        public void AddPersonToDatabase(Person person, int idAddress)
        {

            using (var context = new ImportPersonContext(connectionString))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        AddPerson(context, person, idAddress);
                        AddFlagIsImport(context, person);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Method: AddPersonsToDatabase; Error: {ex.Message}");
                    }
                }
            }
        }

        private void AddPerson(ImportPersonContext context, Person person, int idAddress)
        {
            string sql = "insert into tbFIO(surname, name, o_name, dateBirth, idAdres) " +
                         " values(@surname, @name, @oname, @dateBirth, @idAddress)";

            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName="@surname", Value=person.Surname},
                new SqlParameter{ ParameterName="@name", Value=person.Name},
                new SqlParameter{ ParameterName="@oname", Value=person.Oname??""},
                new SqlParameter{ ParameterName="@dateBirth", Value=person.DateBirth},
                new SqlParameter{ ParameterName="@idAddress", Value=idAddress}
            };

            context.Database.ExecuteSqlCommand(sql, sqlParams.ToArray());
        }


        private void AddFlagIsImport(ImportPersonContext context, Person person)
        {
            string nameTable = GetNameTable(this.sql);
                        
            string sql = "Update " + nameTable +
                         " SET isImport = 1 WHERE 1=1 ";
            
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            StringBuilder sqlCondition = new StringBuilder();

            sqlCondition.Append(sql);
            SetParameters(person, sqlCondition, sqlParams);

            context.Database.ExecuteSqlCommand(sqlCondition.ToString(), sqlParams.ToArray());

            sqlParams.Clear();
            sqlCondition.Clear();
        }


        public void ClearFieldErrorMessage()
        {
            string nameTable = GetNameTable(this.sql);

            var sql = "update " + nameTable +
                " SET ErrorMessage = null WHERE ErrorMessage is not null";
            try
            {
                using (var context = new ImportPersonContext(connectionString))
                {
                    context.Database.ExecuteSqlCommand(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Method: ClearFieldErrorMessage; Error: {ex.Message}");
            }
        }


        private int GetIdGorodRayon(ImportPersonContext context, Person person)
        {
            string sql = " select id from tbRayonGorod " +
                        " where rayon_adrRUS = @rayon_adrRUS and gorod_adrRUS = @gorod_adrRUS ";
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@rayon_adrRUS", Value = person.Rayon},
                new SqlParameter{ ParameterName = "@gorod_adrRUS", Value = person.Gorod}
            };

            var result = context.Database.SqlQuery<int>(sql, sqlParams.ToArray()).ToList();
            int idGoroRayon = result.Count == 0 ? 0 : result[0];
            return idGoroRayon;
        }

        private int GetIdStreet(ImportPersonContext context, Person person)
        {
            string sql = " select id from tbStreet " +
                         " where geonimRUS = @geonimRUS and streetRUS = @streetRUS";
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@geonimRUS", Value = person.Geonim??""},
                new SqlParameter{ ParameterName = "@streetRUS", Value = person.Street}
            };

            var result = context.Database.SqlQuery<int>(sql, sqlParams.ToArray()).ToList();
            int idStreet = result.Count == 0 ? 0 : result[0];
            return idStreet;
        }


        private int GetIdDom(ImportPersonContext context, Person person)
        {
            string sql = " select ID from tbNomerDoma " +
                         " where NomerDoma = @NomerDoma";
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@NomerDoma", Value = person.Dom}
            };

            var result = context.Database.SqlQuery<int>(sql, sqlParams.ToArray()).ToList();
            int idDom = result.Count == 0 ? 0 : result[0];
            return idDom;
        }


        private int? GetIdKv(ImportPersonContext context, Person person)
        {
            if (string.IsNullOrEmpty(person.Kv))
            {
                return null;
            }

            string sql = "select ID from tbNomerKvartira " +
                         "where NomerKvartira = @NomerKvartira";
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@NomerKvartira", Value = person.Kv}
            };

            var result = context.Database.SqlQuery<int>(sql, sqlParams.ToArray()).ToList();
            int idKv = result.Count == 0 ? 0 : result[0];
            return idKv;
        }


        public int GetIdAddress(Person person)
        {
            int cnt = 0;
            if (cnt == 2)
            {
                throw new ArgumentException("Method: GetIdAddress; Error: Ошибка добавления адреса.");
            }

            using (var context = new ImportPersonContext(connectionString))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        int idAddress = GetIdAddress(context, person);
                        if (idAddress != 0)
                        {
                            return idAddress;
                        }

                        int idGorodRayon = GetIdGorodRayon(context, person);
                        int idStreet = GetIdStreet(context, person);
                        int idNomerDom = GetIdDom(context, person);
                        int? idNomerKvartira = GetIdKv(context, person);

                        if (idGorodRayon == 0)
                        {
                            AddRayon(context, person.Rayon, person.Gorod);
                        }
                        if (idStreet == 0)
                        {
                            AddStreet(context, person.Geonim, person.Street);
                        }
                        if (idNomerDom == 0)
                        {
                            SeparatNumberAndLetter(person.Dom, out int domC, out string domB);
                            AddDom(context, person.Dom, domC, domB);
                        }
                        if (idNomerKvartira == 0)
                        {
                            SeparatNumberAndLetter(person.Kv, out int kvC, out string kvB);
                            AddKv(context, person.Kv, kvC, kvB);
                        }

                        //Добавляем запись в tbAdres
                        AddAddress(context, person);

                        transaction.Commit();

                        cnt++;
                        //рекурсия
                        return GetIdAddress(person);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Method: GetIdAddress; Error: {ex.Message}");
                    }
                }
            }
        }

        private int GetIdAddress(ImportPersonContext context, Person person)
        {
            string sql = " SELECT id FROM tbAdres " +
                "where " +
                "idGorodRayon = (select id from tbRayonGorod where rayon_adrRUS = @rayon and gorod_adrRUS = @gorod) " +
                "and idStreet = (select top(1) id from tbStreet where geonimRUS = @geonim and streetRUS = @street) " +
                "and idNomerDoma = (select id from tbNomerDoma where NomerDoma = @dom) " +
                (person.Kv == null ? " and idNomerKvartira is null "
                                   : " and idNomerKvartira = (select id from tbNomerKvartira where NomerKvartira = @kv)");

            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@rayon", Value = person.Rayon},
                new SqlParameter{ ParameterName = "@gorod", Value = person.Gorod},
                new SqlParameter{ ParameterName = "@geonim", Value = person.Geonim??""},
                new SqlParameter{ ParameterName = "@street", Value = person.Street},
                new SqlParameter{ ParameterName = "@dom", Value = person.Dom}
            };

            if (person.Kv != null)
            {
                sqlParams.Add(new SqlParameter { ParameterName = "@kv", Value = person.Kv });
            }

            
            var result = context.Database.SqlQuery<int>(sql, sqlParams.ToArray()).ToList();

            return result.Count == 0 ? 0 : result[0];
        }

        private void AddRayon(ImportPersonContext context, string rayon, string gorod)
        {
            string sql = " insert into tbRayonGorod(rayon_adr, gorod_adr, rayon_adrRUS, gorod_adrRUS, pochta_index) " +
                         " values(@rayon, @gorod, @rayon, @gorod, 91000) ";

            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@rayon", Value = rayon},
                new SqlParameter{ ParameterName = "@gorod", Value = gorod}
            };

            context.Database.ExecuteSqlCommand(sql, sqlParams.ToArray());
        }

        private void AddStreet(ImportPersonContext context, string geonim, string street)
        {
            string sql = " insert into tbStreet(geonim_adr, street_adr, geonimRUS, streetRUS) " +
                        " VALUES(@geonim, @street, @geonim, @street)";

            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@geonim", Value = geonim??""},
                new SqlParameter{ ParameterName = "@street", Value = street}
            };

            context.Database.ExecuteSqlCommand(sql, sqlParams.ToArray());
        }

        private void AddDom(ImportPersonContext context, string dom, int domC, string domB)
        {
            string sql = "insert into tbNomerDoma(NomerDoma, NomerDomaC, NomerDomaB) " +
                         "values(@dom, @domC, @domB)";

            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@dom", Value = dom},
                new SqlParameter{ ParameterName = "@domC", Value = domC},
                new SqlParameter{ ParameterName = "@domB", Value = domB},
            };

            context.Database.ExecuteSqlCommand(sql, sqlParams.ToArray());
        }

        private void AddKv(ImportPersonContext context, string kv, int kvC, string kvB)
        {
            string sql = "insert into tbNomerKvartira(NomerKvartira, NomerKvartiraC, NomerKvartiraB) " +
                         "values(@kv, @kvC, @kvB)";

            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@kv", Value = kv},
                new SqlParameter{ ParameterName = "@kvC", Value = kvC},
                new SqlParameter{ ParameterName = "@kvB", Value = kvB},
            };

            context.Database.ExecuteSqlCommand(sql, sqlParams.ToArray());
        }

        private void AddAddress(ImportPersonContext context, Person person)
        {

            string idNomerKvartira = person.Kv == null ? "" : ", idNomerKvartira ";

            string sql = "  INSERT INTO tbAdres(idGorodRayon, idStreet, idNomerDoma" + idNomerKvartira + ") " +
                    "VALUES( " +
                    "(select id from tbRayonGorod where rayon_adrRUS = @rayon and gorod_adrRUS = @gorod), " +
                    "(select top(1) id from tbStreet where geonimRUS = @geonim and streetRUS = @street), " +
                    "(select id from tbNomerDoma where NomerDoma = @dom) " +
                     (person.Kv == null ? "" : ",(select id from tbNomerKvartira where NomerKvartira = @kv) ") +
                    ")";

            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@rayon", Value = person.Rayon},
                new SqlParameter{ ParameterName = "@gorod", Value = person.Gorod},
                new SqlParameter{ ParameterName = "@geonim", Value = person.Geonim??""},
                new SqlParameter{ ParameterName = "@street", Value = person.Street},
                new SqlParameter{ ParameterName = "@dom", Value = person.Dom}
            };

            if (person.Kv != null)
            {
                sqlParams.Add(new SqlParameter { ParameterName = "@kv", Value = person.Kv });
            }

            context.Database.ExecuteSqlCommand(sql, sqlParams.ToArray());
        }

        public List<Person> GetPersons()
        {
            try
            {
                using (var context = new ImportPersonContext(connectionString))
                {
                    RemoveSpaces(context);
                    
                    var result = context.Database.SqlQuery<Person>(sql).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Method: GetPersons; Error: {ex.Message}");
            }
        }


        private void RemoveSpaces(ImportPersonContext context)
        {
            string nameTable = GetNameTable(sql);

            string sqlUpdate = $" UPDATE {nameTable} " +
                "SET surname = LTRIM(RTRIM(surname)), " +
                "name = LTRIM(RTRIM(name)), " +
                "oname = LTRIM(RTRIM(oname)), " +
                "rayon = LTRIM(RTRIM(rayon)), " +
                "gorod = LTRIM(RTRIM(gorod)), " +
                "geonim = LTRIM(RTRIM(geonim)), " +
                "street = LTRIM(RTRIM(street)), " +
                "dom = LTRIM(RTRIM(dom)), " +
                "kv = LTRIM(RTRIM(kv))";

            context.Database.ExecuteSqlCommand(sqlUpdate);
        }


        public void WriteErrors(List<Person> persons)
        {
            string nameTable = GetNameTable(this.sql);
                        
            string sql = "Update " + nameTable +
                " SET ErrorMessage=@ErrorMessage " +
                "WHERE 1=1 ";
            
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            StringBuilder sqlCondition = new StringBuilder();
            
            try
            {
                using (var context = new ImportPersonContext(connectionString))
                {
                    foreach (var p in persons)
                    {
                        sqlCondition.Append(sql);
                        sqlParams.Add(new SqlParameter { ParameterName = "@ErrorMessage", Value = p.ErrorMessage });
                        SetParameters(p, sqlCondition, sqlParams);                                                                      

                        context.Database.ExecuteSqlCommand(sqlCondition.ToString(), sqlParams.ToArray());

                        sqlParams.Clear();
                        sqlCondition.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Method: WriteErrors; Error: {ex.Message}");
            }
        }


        internal void SetParameters(Person p, StringBuilder sqlCondition, List<SqlParameter> sqlParams)
        {
            if (p.Num != null)
            {
                sqlCondition.Append(" and num = @num ");
                sqlParams.Add(new SqlParameter { ParameterName = "@num", Value = p.Num });
            }

            if (!string.IsNullOrEmpty(p.Surname))
            {
                sqlCondition.Append(" and surname=@surname ");
                sqlParams.Add(new SqlParameter { ParameterName = "@surname", Value = p.Surname });
            }

            if (!string.IsNullOrEmpty(p.Name))
            {
                sqlCondition.Append(" and name=@name ");
                sqlParams.Add(new SqlParameter { ParameterName = "@name", Value = p.Name });
            }

            if (!string.IsNullOrEmpty(p.Oname))
            {
                sqlCondition.Append(" and oname=@oname");
                sqlParams.Add(new SqlParameter { ParameterName = "@oname", Value = p.Oname });
            }

            sqlCondition.Append(" and dateBirth=@dateBirth ");
            sqlParams.Add(new SqlParameter { ParameterName = "@dateBirth", Value = p.DateBirth });

            if (sqlParams.Count < 3)
            {
                throw new ArgumentException("Method: SetParameters; Error: Входных параметров меньше 3.");
            }
        }


        
    }
}


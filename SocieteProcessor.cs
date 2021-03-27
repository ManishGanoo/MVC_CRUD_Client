using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DdLibrary.Models;
using DdLibrary.DataAccess;

namespace DdLibrary.BusinessLogic
{
    public static class SocieteProcessor
    {
        public static int CreateSociete(int societeid, string detailsociete, string societeadresse, string contactperson)
        {
            SocieteModel data = new SocieteModel
            {
                SocieteId = societeid,
                DetailSociete = detailsociete,
                SocieteAdresse = societeadresse,
                ContactPerson = contactperson

            };

            string sql = @"INSERT into dbo.Societe VALUES (@DetailSociete,@SocieteAdresse,@ContactPerson);";

            return SqlDataAccess.CRUD(sql, data);
        }

        public static int UpdateSociete(int societeid, string detailsociete, string societeadresse, string contactperson)
        {
            string sql = @"Update dbo.Societe SET DetailSociete ='" + @detailsociete + "', SocieteAdresse = '" + @societeadresse + "', ContactPerson = '" + @contactperson + "' Where SocieteId = " + @societeid + ";";
            return SqlDataAccess.RunSQL(sql);
        }

        public static int DeleteSociete(int societeid)
        {
            string sql = @"delete from dbo.Societe where SocieteId = " + @societeid + ";";
            return SqlDataAccess.RunSQL(sql);
        }

        public static List<SocieteModel> LoadSociete()
        {
            string sql = @"Select SocieteId, DetailSociete ,SocieteAdresse, ContactPerson from dbo.Societe;";

            return SqlDataAccess.LoadData<SocieteModel>(sql);
        }

    }
}

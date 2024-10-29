using Documentor.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentor.Repository
{
    public class repDocument : IDisposable
    {
        private SqlConnection? activeCon;
        private readonly Connexion connexion;

        public repDocument()
        {
            connexion = new Connexion();
        }

        public MDocument GetById(int Id)
        {
            try
            {
                MDocument? document = null;

                using (activeCon = connexion.GetConnexion())
                {
                    SqlCommand GetDocumentById = activeCon.CreateCommand();
                    GetDocumentById.CommandText = "SELECT * FROM Documents WHERE id_document = @id";
                    SqlParameter id = GetDocumentById.Parameters.Add("@id", SqlDbType.Int);
                    id.Value = Id;

                    using (SqlDataReader reader = GetDocumentById.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            document = new MDocument
                            (
                                reader.GetInt32(0),
                                reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                                reader.GetInt32(2),
                                reader.GetString(3),
                                reader.GetDateTime(4),
                                reader.GetDateTime(5)
                            );
                        }
                    }

                    if (document == null)
                    {
                        throw new Exception("Le document avec l'ID spécifié n'a pas été trouvé.");
                    }

                    return document;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

        public List<MDocument> GetAll()
        {
            List<MDocument> documents = new List<MDocument>();

            try
            {
                using (activeCon = connexion.GetConnexion())
                {
                    SqlCommand GetDocuments = activeCon.CreateCommand();
                    GetDocuments.CommandText = "SELECT * FROM Documents";
                    using (SqlDataReader reader = GetDocuments.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MDocument document = new MDocument
                            (
                                reader.GetInt32(0),
                                reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                                reader.GetInt32(2),
                                reader.GetString(3),
                                reader.GetDateTime(4),
                                reader.GetDateTime(5)
                            );

                            documents.Add(document);
                        }
                    }

                    return documents;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

        public void Create(MDocument newDocument)
        {
            using (activeCon = connexion.GetConnexion())
            {
                SqlCommand AddDocument = activeCon.CreateCommand();
                AddDocument.CommandText = "INSERT INTO Documents (size_id, document_name) VALUES (@sizeId, @name)";

                SqlParameter sizeId = AddDocument.Parameters.Add("@sizeId", SqlDbType.Int);
                sizeId.Value = newDocument.SizeId;
                SqlParameter name = AddDocument.Parameters.Add("@name", SqlDbType.NVarChar);
                name.Value = newDocument.Name;

                AddDocument.ExecuteNonQuery();
            }
        }

        public void Update(MDocument document)
        {
            using (activeCon = connexion.GetConnexion())
            {
                SqlCommand UpdateDocument = activeCon.CreateCommand();
                UpdateDocument.CommandText = "UPDATE Documents SET document_name = @name WHERE id_document = @id";

                SqlParameter name = UpdateDocument.Parameters.Add("name", SqlDbType.NVarChar);
                name.Value = document.Name;
                SqlParameter id = UpdateDocument.Parameters.Add("id", SqlDbType.Int);
                id.Value = document.Id;

                UpdateDocument.ExecuteNonQuery();
            }
        }

        public void Delete(MDocument documentToDelete)
        {
            using (activeCon = connexion.GetConnexion())
            {
                SqlCommand DeleteDocument = activeCon.CreateCommand();
                DeleteDocument.CommandText = "DELETE FROM Documents WHERE id_document = @id";

                SqlParameter id = DeleteDocument.Parameters.Add("@id", SqlDbType.Int);
                id.Value = documentToDelete.Id;

                DeleteDocument.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (activeCon != null)
                {
                    activeCon.Close();
                    activeCon.Dispose();
                    activeCon = null;
                }
            }
        }
    }
}

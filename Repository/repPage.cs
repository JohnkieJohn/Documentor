using Documentor.Models;
using Documentor.Tools;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Documentor.Repository
{
    public class repPage : IDisposable
    {
        private SqlConnection? activeCon;
        private readonly Connexion connexion;

        public repPage()
        {
            this.connexion = new Connexion();
        }

        public List<MPage> GetAll(int documentId)
        {
            var pages = new List<MPage>();
            try
            {
                using (activeCon = connexion.GetConnexion())
                {
                    SqlCommand GetPages = activeCon.CreateCommand();
                    GetPages.CommandText = "SELECT * FROM Pages WHERE document_id = @id";
                    SqlParameter id = GetPages.Parameters.Add("@id", SqlDbType.Int);
                    id.Value = documentId;

                    using (SqlDataReader reader = GetPages.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MPage page = new MPage
                            (
                                reader.GetInt32(0),
                                reader.GetInt32(1),
                                reader.GetInt32(2)
                            );

                            pages.Add(page);
                        }
                    }
                    return pages;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

        public void Create(MPage newPage)
        {
            try
            {
                using (activeCon = connexion.GetConnexion())
                {
                    SqlCommand AddPage = activeCon.CreateCommand();
                    AddPage.CommandText =
                        "INSERT INTO Pages VALUES (@document_id, @page_number); SELECT SCOPE_IDENTITY();";

                    SqlParameter documentId = AddPage.Parameters.Add("@documentId", SqlDbType.Int);
                    documentId.Value = newPage.DocumentId;
                    SqlParameter pageNumber = AddPage.Parameters.Add("@page_number", SqlDbType.Int);
                    pageNumber.Value = newPage.Number;

                    var newPageId = AddPage.ExecuteScalar();
                    if (newPageId != null)
                    {
                        newPage.Id = Convert.ToInt32(newPageId);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

        public void Update(MPage page)
        {
            try
            {
                using (activeCon = connexion.GetConnexion())
                {
                    SqlCommand UpdatePage = activeCon.CreateCommand();
                    UpdatePage.CommandText =
                        "UPDATE Pages SET page_number = @number WHERE id_page = @id";

                    SqlParameter number = UpdatePage.Parameters.Add("@number", SqlDbType.VarChar);
                    number.Value = page.Number;
                    SqlParameter id = UpdatePage.Parameters.Add("@id", SqlDbType.Int);
                    id.Value = page.Id;

                    UpdatePage.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

        public void Delete(MPage pageToDelete)
        {
            try
            {
                using (activeCon = connexion.GetConnexion())
                {
                    SqlCommand DeletePage = activeCon.CreateCommand();
                    DeletePage.CommandText = "DELETE FROM Pages WHERE id_page = @id";

                    SqlParameter id = DeletePage.Parameters.Add("@id", SqlDbType.Int);
                    id.Value = pageToDelete.Id;

                    DeletePage.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
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

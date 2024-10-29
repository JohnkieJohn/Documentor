using Documentor.Interfaces;
using Documentor.Models;
using Documentor.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using Documentor.Tools;

namespace Documentor.Repository
{
    public class repElement : IDisposable
    {
        private SqlConnection? activeCon;
        private readonly Connexion connexion;

        private readonly repPosition repPosition;
        private readonly repSize repSize;

        public repElement()
        {
            this.connexion = new Connexion();
            this.repPosition = new repPosition();
            this.repSize = new repSize();
        }

        public MElement GetDefault()
        {
            try
            {
                MElement defaultElement;

                using (activeCon = connexion.GetConnexion())
                {
                    defaultElement = new MElement
                    (
                        repSize.GetById(1, activeCon),
                        repPosition.GetById(1, activeCon)
                    );
                }
                return defaultElement;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

        public List<MElement> GetAll(int pageId)
        {
            var elements = new List<MElement>();
            MSize size;
            MPosition position;
            try
            {
                using (activeCon = connexion.GetConnexion())
                {
                    SqlCommand GetElements = activeCon.CreateCommand();
                    GetElements.CommandText = "SELECT * FROM Elements LEFT JOIN Sizes ON id_size = size_id LEFT JOIN Positions ON id_position = position_id WHERE page_id = @id";
                    SqlParameter id = GetElements.Parameters.Add("@id", SqlDbType.Int);
                    id.Value = pageId;

                    using (SqlDataReader reader = GetElements.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MElement element = new MElement
                            (
                                reader.GetInt32(0),
                                reader.GetInt32(1),
                                reader.GetInt32(2),
                                size = new MSize
                                (
                                    reader.GetInt32(7),
                                    reader.GetDouble(8),
                                    reader.GetDouble(9),
                                    reader.GetInt32(10)
                                ),
                                position = new MPosition
                                (
                                    reader.GetInt32(11),
                                    reader.GetDouble(12),
                                    reader.GetDouble(13)
                                ),
                                reader.GetString(5),
                                reader.GetString(6)
                            );

                            elements.Add(element);
                        }
                    }
                    return elements;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

        public void Create(MElement newElement)
        {
            try
            {
                using (activeCon = connexion.GetConnexion())
                {
                    repSize.Create(newElement.Size, activeCon);
                    repPosition.Create(newElement.Position, activeCon);

                    SqlCommand AddElement = activeCon.CreateCommand();
                    AddElement.CommandText =
                        "INSERT INTO Elements VALUES (@controlId, @pageId, @sizeId, @positionId, @name, @content); SELECT SCOPE_IDENTITY();";

                    SqlParameter controlId = AddElement.Parameters.Add("@controlId", SqlDbType.Int);
                    controlId.Value = newElement.ControlId;
                    SqlParameter pageId = AddElement.Parameters.Add("@pageId", SqlDbType.Int);
                    pageId.Value = newElement.PageId;
                    SqlParameter sizeId = AddElement.Parameters.Add("@sizeId", SqlDbType.Int);
                    sizeId.Value = newElement.Size.Id;
                    SqlParameter positionId = AddElement.Parameters.Add("@positionId", SqlDbType.Int);
                    positionId.Value = newElement.Position.Id;
                    SqlParameter name = AddElement.Parameters.Add("@name", SqlDbType.NVarChar);
                    name.Value = newElement.Name;
                    SqlParameter content = AddElement.Parameters.Add("@content", SqlDbType.NVarChar);
                    content.Value = newElement.Content;

                    var newElementId = AddElement.ExecuteScalar();
                    if (newElementId != null)
                    {
                        newElement.Id = Convert.ToInt32(newElementId);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

        public void Update(MElement element)
        {
            try
            {
                using (activeCon = connexion.GetConnexion())
                {
                    repPosition.Update(element.Position, activeCon);
                    repSize.Update(element.Size, activeCon);

                    SqlCommand UpdateElement = activeCon.CreateCommand();
                    UpdateElement.CommandText =
                        "UPDATE Elements SET element_name = @name, element_content = @content WHERE id_element = @id";

                    SqlParameter name = UpdateElement.Parameters.Add("@name", SqlDbType.VarChar);
                    name.Value = element.Name;
                    SqlParameter content = UpdateElement.Parameters.Add("@content", SqlDbType.VarChar);
                    content.Value = element.Content;
                    SqlParameter id = UpdateElement.Parameters.Add("@id", SqlDbType.Int);
                    id.Value = element.Id;

                    UpdateElement.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

        public void Delete(MElement elementToDelete)
        {
            try
            {
                using (activeCon = connexion.GetConnexion())
                {
                    SqlCommand DeleteElement = activeCon.CreateCommand();
                    DeleteElement.CommandText = "DELETE FROM Elements WHERE id_element = @id";

                    SqlParameter id = DeleteElement.Parameters.Add("@id", SqlDbType.Int);
                    id.Value = elementToDelete.Id;

                    DeleteElement.ExecuteNonQuery();

                    repSize.Delete(elementToDelete.Size, activeCon);
                    repPosition.Delete(elementToDelete.Position, activeCon);
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

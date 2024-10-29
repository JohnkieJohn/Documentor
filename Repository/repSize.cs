using Documentor.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Documentor.Interfaces;
using Documentor.Views;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace Documentor.Repository
{
    public class repSize : IPropertiesRepository<MSize>
    {
        public MSize GetById(int Id, SqlConnection activeCon)
        {
            try
            {
                MSize? size = null;

                SqlCommand GetSizeById = activeCon.CreateCommand();
                GetSizeById.CommandText = "SELECT * FROM Sizes WHERE id_size = @id";
                SqlParameter id = GetSizeById.Parameters.Add("@id", SqlDbType.Int);
                id.Value = Id;

                using (SqlDataReader reader = GetSizeById.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        size = new MSize
                         (
                            reader.GetInt32(0),
                            reader.GetDouble(1),
                            reader.GetDouble(2),
                            reader.GetInt32(3)
                        );
                    };
                }

                if (size == null)
                {
                    throw new Exception("La dimension avec l'ID spécifié n'a pas été trouvé.");
                }

                return size;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            } 
        }

        public void Create(MSize newSize, SqlConnection activeCon)
        {
            try
            {
                SqlCommand AddSize = activeCon.CreateCommand();
                AddSize.CommandText = "INSERT INTO Sizes VALUES (@width, @height, @padding); SELECT SCOPE_IDENTITY();";

                SqlParameter width = AddSize.Parameters.Add("@width", SqlDbType.Float);
                width.Value = newSize.Width;
                SqlParameter height = AddSize.Parameters.Add("@height", SqlDbType.Float);
                height.Value = newSize.Height;
                SqlParameter padding = AddSize.Parameters.Add("@padding", SqlDbType.Int);
                padding.Value = newSize.Padding;

                var newSizeId = AddSize.ExecuteScalar();
                if (newSizeId != null)
                {
                    newSize.Id = Convert.ToInt32(newSizeId);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

        public void Update(MSize size, SqlConnection activeCon)
        {
            try
            {
                SqlCommand UpdateSize = activeCon.CreateCommand();
                UpdateSize.CommandText = "UPDATE Sizes SET size_width = @width, size_height = @height, size_padding = @padding WHERE id_size = @id";

                SqlParameter width = UpdateSize.Parameters.Add("@width", SqlDbType.Float);
                width.Value = size.Width;
                SqlParameter height = UpdateSize.Parameters.Add("@height", SqlDbType.Float);
                height.Value = size.Height;
                SqlParameter padding = UpdateSize.Parameters.Add("@padding", SqlDbType.Int);
                padding.Value = size.Padding;
                SqlParameter id = UpdateSize.Parameters.Add("@id", SqlDbType.Int);
                id.Value = size.Id;

                UpdateSize.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

        public void Delete(MSize sizeToDelete, SqlConnection activeCon)
        {
            try
            {
                SqlCommand DeleteSize = activeCon.CreateCommand();
                DeleteSize.CommandText = "DELETE FROM Sizes WHERE id_size = @id";

                SqlParameter id = DeleteSize.Parameters.Add("@id", SqlDbType.Int);
                id.Value = sizeToDelete.Id;

                DeleteSize.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }
    }
}

using Documentor.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Documentor.Interfaces;
using Documentor.Tools;
using System.Diagnostics;

namespace Documentor.Repository
{
    public class repPosition : IPropertiesRepository<MPosition>
    {
        public MPosition GetById(int Id, SqlConnection activeCon)
        {
            try
            {
                MPosition? position = null;

                SqlCommand GetPositionById = activeCon.CreateCommand();
                GetPositionById.CommandText = "SELECT * FROM Positions WHERE id_position = @id";
                SqlParameter id = GetPositionById.Parameters.Add("@id", SqlDbType.Int);
                id.Value = Id;

                using (SqlDataReader reader = GetPositionById.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        position = new MPosition
                        (
                            reader.GetInt32(0),
                            reader.GetDouble(1),
                            reader.GetDouble(2)
                        );
                    };
                }

                if (position == null)
                {
                    throw new Exception("La position avec l'ID spécifié n'a pas été trouvé.");
                }

                return position;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

        public void Create(MPosition newPosition, SqlConnection activeCon)
        {
            try
            {
                SqlCommand AddPosition = activeCon.CreateCommand();
                AddPosition.CommandText = "INSERT INTO Positions VALUES (@top, @left); SELECT SCOPE_IDENTITY();";

                SqlParameter top = AddPosition.Parameters.Add("top", SqlDbType.Float);
                top.Value = newPosition.Top;
                SqlParameter left = AddPosition.Parameters.Add("left", SqlDbType.Float);
                left.Value = newPosition.Left;

                var newPositionId = AddPosition.ExecuteScalar();
                if (newPositionId != null)
                {
                    newPosition.Id = Convert.ToInt32(newPositionId);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

        public void Update(MPosition position, SqlConnection activeCon)
        {
            try
            {
                SqlCommand UpdatePosition = activeCon.CreateCommand();
                UpdatePosition.CommandText = "UPDATE Positions SET position_top = @top, position_left = @left WHERE id_position = @id";

                SqlParameter top = UpdatePosition.Parameters.Add("top", SqlDbType.Float);
                top.Value = position.Top;
                SqlParameter left = UpdatePosition.Parameters.Add("left", SqlDbType.Float);
                left.Value = position.Left;
                SqlParameter id = UpdatePosition.Parameters.Add("@id", SqlDbType.Int);
                id.Value = position.Id;

                UpdatePosition.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

        public void Delete(MPosition positionToDelete, SqlConnection activeCon)
        {
            try
            {
                SqlCommand DeletePosition = activeCon.CreateCommand();
                DeletePosition.CommandText = "DELETE FROM Positions WHERE id_position = @id";

                SqlParameter id = DeletePosition.Parameters.Add("@id", SqlDbType.Int);
                id.Value = positionToDelete.Id;

                DeletePosition.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Une exception s'est produite : " + ex.Message);
                throw;
            }
        }

    }
}

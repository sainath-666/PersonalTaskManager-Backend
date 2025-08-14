using System.Data.SqlClient;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Data
{
    public class CartRepository
    {
        private readonly string _connectionString;

        public CartRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<CartItem> GetCart(string sessionId)
        {
            var cartItems = new List<CartItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM CartItems WHERE SessionId = @SessionId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SessionId", sessionId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cartItems.Add(new CartItem
                    {
                        CartItemId = (int)reader["CartItemId"],
                        SessionId = reader["SessionId"].ToString(),
                        ProductId = (int)reader["ProductId"],
                        Quantity = (int)reader["Quantity"],
                        AddedDate = (DateTime)reader["AddedDate"]
                    });
                }
            }

            return cartItems;
        }

        public void AddToCart(CartItem cartItem)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO CartItems (SessionId, ProductId, Quantity) 
                                 VALUES (@SessionId, @ProductId, @Quantity)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SessionId", cartItem.SessionId);
                cmd.Parameters.AddWithValue("@ProductId", cartItem.ProductId);
                cmd.Parameters.AddWithValue("@Quantity", cartItem.Quantity);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateQuantity(int cartItemId, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE CartItems SET Quantity = @Quantity WHERE CartItemId = @CartItemId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CartItemId", cartItemId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveFromCart(int cartItemId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM CartItems WHERE CartItemId = @CartItemId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CartItemId", cartItemId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

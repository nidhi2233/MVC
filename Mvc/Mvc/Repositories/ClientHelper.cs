using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mvc.Models;
using Npgsql;

namespace Mvc.Repositories
{
    public class ClientHelper
    {
        public NpgsqlConnection conn;

        public ClientHelper(NpgsqlConnection connection)
        {
            conn = connection;

            if (conn != null)
            {

                Console.WriteLine("clickent side connection done ..");

            }
        }

        public List<ClientClass> showdata()
        {

            try
            {
                List<ClientClass> clients = new List<ClientClass>();
                conn.Open();
                string query = "select s.c_sid,s.c_name,s.c_qty,s.c_price,s.c_size,s.c_img,s.c_gender,s.c_color,b.c_bname from t_shoes as s join brand as b on s.c_bid=b.c_bid where s.c_qty>1;";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    var reader = cmd.ExecuteReader();

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ClientClass client = new ClientClass
                            {

                                c_sid = Convert.ToInt32(reader["c_sid"]),
                                c_name = reader["c_name"].ToString(),
                                c_color = reader["c_color"].ToString(),
                                c_gender = reader["c_gender"].ToString(),
                                c_size = Convert.ToInt32(reader["c_size"]),
                                c_qty = Convert.ToInt32(reader["c_qty"]),
                                c_price = Convert.ToInt32(reader["c_price"]),
                                c_bname = reader["c_bname"].ToString(),
                                path = reader["c_img"].ToString(),

                            };
                            clients.Add(client);
                        }
                    }

                }
                return clients;
            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

        }

        public int addorder(purchesClass model)
        {
            try
            {
                int row;
                int mainrow = 0;
                conn.Open();
                string query = "insert into t_order(c_sid,c_qty)values(@c_sid,@c_qty)";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@c_sid", model.c_sid);
                    cmd.Parameters.AddWithValue("@c_qty", model.c_qty);

                    row = cmd.ExecuteNonQuery();

                    if (row > 0)
                    {

                        string query2 = "update t_shoes set c_qty=(c_qty-@c_qty) where c_sid=@c_sid";
                        using (NpgsqlCommand cmd2 = new NpgsqlCommand(query2, conn))
                        {
                            cmd2.Parameters.AddWithValue("@c_sid", model.c_sid);
                            cmd2.Parameters.AddWithValue("@c_qty", model.c_qty);
                            mainrow = cmd2.ExecuteNonQuery();
                            Console.WriteLine("===" + mainrow);

                            return mainrow;

                        }

                    }

                    return mainrow;

                }
            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<CartClass> orders()
        {
            List<CartClass> carts = new List<CartClass>();
            try
            {
                conn.Open();
                string query = "select s.c_sid,s.c_name,s.c_price,s.c_size,s.c_img,s.c_gender,s.c_color,b.c_bname,o.c_qty,o.c_oid,o.c_cancel from t_shoes as s join brand as b on s.c_bid=b.c_bid join t_order as o on s.c_sid=o.c_sid";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    var reader = cmd.ExecuteReader();

                    if (reader != null)
                    {
                        int subtotal = 0;
                        int grandtotal = 0;
                        while (reader.Read())
                        {

                            if (!bool.Parse(reader["c_cancel"].ToString()))
                            {
                                subtotal = Convert.ToInt32(reader["c_qty"]) * Convert.ToInt32(reader["c_price"]);
                                grandtotal += subtotal;

                            }
                            //Console.WriteLine(reader["c_cancel"].ToString());

                            CartClass cart = new CartClass
                            {
                                c_oid = Convert.ToInt32(reader["c_oid"]),
                                c_sid = Convert.ToInt32(reader["c_sid"]),
                                c_name = reader["c_name"].ToString(),
                                c_qty = Convert.ToInt32(reader["c_qty"]),
                                c_price = Convert.ToInt32(reader["c_price"]),
                                c_size = Convert.ToInt32(reader["c_size"]),
                                path = reader["c_img"].ToString(),
                                c_bname = reader["c_bname"].ToString(),
                                c_gender = reader["c_gender"].ToString(),
                                c_color = reader["c_color"].ToString(),
                                cancel = bool.Parse(reader["c_cancel"].ToString()),
                                total = subtotal,
                                grandtotal = grandtotal,

                            };
                            carts.Add(cart);

                        }
                    }
                }
                return carts;

            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public int cancel(int id)
        {
            int result = 0;
            int qty = 0;
            int sid = 0;
            int updateqty = 0;
            int cancel = 0;

            try
            {
                conn.Open();
                string query = "select c_qty,c_sid from t_order where c_oid=@id";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                qty = Convert.ToInt32(reader["c_qty"]);
                                sid = Convert.ToInt32(reader["c_sid"]);
                            }
                        }
                    }

                }
                if (qty > 0 && sid > 0)
                {
                    Console.WriteLine("qty = " + qty);
                    Console.WriteLine("sid = " + sid);

                    string query2 = "update t_shoes set  c_qty=(c_qty+@c_qty) where c_sid=@c_sid";

                    using (NpgsqlCommand cmd2 = new NpgsqlCommand(query2, conn))
                    {
                        cmd2.Parameters.AddWithValue("@c_qty", qty);
                        cmd2.Parameters.AddWithValue("@c_sid", sid);
                        updateqty = cmd2.ExecuteNonQuery();



                    }
                    if (updateqty > 0)
                    {

                        Console.WriteLine("qty is update");

                        string query3 = "update  t_order set c_cancel=@cancel where c_oid=@id";
                        using (NpgsqlCommand cmd3 = new NpgsqlCommand(query3, conn))
                        {
                            cmd3.Parameters.AddWithValue("@cancel", true);
                            cmd3.Parameters.AddWithValue("@id", id);

                            cancel = cmd3.ExecuteNonQuery();

                            if (cancel > 0)
                            {
                                Console.WriteLine(" order is cancel ");
                            }

                        }



                    }





                }
                return 0;
            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Mvc.Models;
using Npgsql;

namespace Mvc.Repositories
{
    public class ProductHelper
    {
        public NpgsqlConnection conn;
        public static string oldimgpath = "";

        public ProductHelper(NpgsqlConnection connection)
        {
            conn = connection;

            if (conn != null)
            {
                Console.WriteLine("connection is domne");
            }
        }

        public List<ProductClass> getalldata()
        {

            try
            {
                List<ProductClass> products = new List<ProductClass>();
                conn.Open();
                string query = "select s.c_sid,s.c_qty,s.c_name,s.c_size,s.c_color,s.c_qty,s.c_price,s.c_date,s.c_img,s.c_bid,b.c_bname from t_shoes as s join brand as b on b.c_bid=s.c_bid;";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    var reader = cmd.ExecuteReader();

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var product = new ProductClass
                            {
                                c_sid = Convert.ToInt32(reader["c_sid"]),
                                c_name = reader["c_name"].ToString(),
                                c_size = Convert.ToInt32(reader["c_size"]),
                                c_color = reader["c_color"].ToString(),
                                c_qty = Convert.ToInt32(reader["c_qty"]),
                                c_price = Convert.ToInt32(reader["c_price"]),
                                c_date = DateTime.Parse(reader["c_date"].ToString()),
                                c_bid = Convert.ToInt32(reader["c_bid"]),
                                img = reader["c_img"].ToString(),
                                brand = reader["c_bname"].ToString()
                            };
                            products.Add(product);
                        }
                    }

                }
                return products;
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

        public List<BrandClass> getbrand()
        {

            try
            {
                List<BrandClass> brands = new List<BrandClass>();
                conn.Open();
                string query = "select * from brand";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    var reader = cmd.ExecuteReader();

                    if (reader != null)
                    {

                        while (reader.Read())
                        {
                            var brand = new BrandClass
                            {
                                c_bid = Convert.ToInt32(reader["c_bid"]),
                                c_bname = reader["c_bname"].ToString()
                            };
                            brands.Add(brand);
                        }
                    }
                }
                return brands;

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

        public async Task<int> addshoesdata(AddShoesClass model)
        {

            try
            {
                string filepath = "";
                int flg = 0;
                int row = 0;

                if (model.img != null)
                {

                    Directory.CreateDirectory(@"D:\core_exam\Mvc\Mvc\wwwroot\img");
                    Console.WriteLine("this is if imge");
                    string chekfile = "D:/core_exam/Mvc/Mvc/wwwroot/img" + model.img.FileName;
                    string extension = Path.GetExtension(model.img.FileName);
                    Console.WriteLine("==" + extension);

                    ArrayList arr = new ArrayList { ".jpg", ".png" };

                    bool chek = arr.Contains(extension);
                    if (chek)
                    {
                        Console.WriteLine(chek);

                        if (File.Exists(chekfile))
                        {
                            Console.WriteLine("file exists");
                            flg = 0;
                        }
                        else
                        {

                            flg = 1;
                            filepath = Path.Combine("D:/core_exam/Mvc/Mvc/wwwroot/img", model.img.FileName);
                            Console.WriteLine(filepath);

                            using (var stream = File.Create(filepath))
                            {
                                await model.img.CopyToAsync(stream);
                            }



                        }
                    }
                }

                if (flg == 1)
                {


                    conn.Open();
                    string query = "insert into t_shoes(c_name,c_size,c_color,c_qty,c_price,c_date,c_gender,c_img,c_bid)values(@c_name,@c_size,@c_color,@c_qty,@c_price,@c_date,@c_gender,@c_img,@c_bid)";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@c_name", model.c_name);
                        cmd.Parameters.AddWithValue("@c_size", model.c_size);
                        cmd.Parameters.AddWithValue("@c_color", model.c_color);
                        cmd.Parameters.AddWithValue("@c_qty", model.c_qty);
                        cmd.Parameters.AddWithValue("@c_price", model.c_price);
                        cmd.Parameters.AddWithValue("@c_date", DateTime.Parse(model.c_date.ToString()));
                        cmd.Parameters.AddWithValue("@c_gender", model.c_gender);
                        cmd.Parameters.AddWithValue("@c_img", "/img/" + model.img.FileName);
                        cmd.Parameters.AddWithValue("@c_bid", model.c_bid);
                        row = cmd.ExecuteNonQuery();
                    }
                    return row;

                }
                else
                {
                    return 0;
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

        public int deletedata(int id)
        {

            try
            {
                int row = 0;
                conn.Open();
                string query = "delete from t_shoes where c_sid=@id";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    row = cmd.ExecuteNonQuery();

                }

                return row;
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

        public List<AddShoesClass> fatch_update_data(int id)
        {

            try
            {
                List<AddShoesClass> addShoes = new List<AddShoesClass>();
                conn.Open();
                string query = "select * from t_shoes where c_sid=@id";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("=="+reader["c_sid"].ToString());
                            AddShoesClass addshoes = new AddShoesClass
                            {
                                c_sid = Convert.ToInt32(reader["c_sid"].ToString()),
                                c_name = reader["c_name"].ToString(),
                                c_price = Convert.ToInt32(reader["c_price"]),
                                c_gender = reader["c_gender"].ToString(),
                                c_size = Convert.ToInt32(reader["c_size"]),
                                c_qty = Convert.ToInt32(reader["c_qty"]),
                                c_date = DateTime.Parse(reader["c_date"].ToString()),
                                color = reader["c_color"].ToString().Split(',').ToList(),
                                c_bid = Convert.ToInt32(reader["c_bid"]),
                                path = reader["c_img"].ToString(),


                            };
                            oldimgpath = reader["c_img"].ToString();
                            addShoes.Add(addshoes);

                        }
                    }
                    return addShoes;
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

        public async Task<int> update_data(AddShoesClass model,int id)
        {

            try
            {


                string filepath = "";
                int flg = 0;
                int row = 0;

                if (model.img != null)
                {

                    Directory.CreateDirectory(@"D:\core_exam\Mvc\Mvc\wwwroot\img");
                    Console.WriteLine("this is if imge");
                    string chekfile = "D:/core_exam/Mvc/Mvc/wwwroot/img" + model.img.FileName;
                    string extension = Path.GetExtension(model.img.FileName);
                    Console.WriteLine("==" + extension);

                    ArrayList arr = new ArrayList { ".jpg", ".png" };

                    bool chek = arr.Contains(extension);
                    if (chek)
                    {
                        Console.WriteLine(chek);

                        if (File.Exists(chekfile))
                        {
                            Console.WriteLine("file exists");
                            flg = 0;
                        }
                        else
                        {

                            flg = 1;
                            filepath = Path.Combine("D:/core_exam/Mvc/Mvc/wwwroot/img", model.img.FileName);
                            Console.WriteLine(filepath);

                            using (var stream = File.Create(filepath))
                            {
                                await model.img.CopyToAsync(stream);
                            }
                            oldimgpath = "/img/" + model.img.FileName;



                        }
                    }
                }

                model.c_color = string.Join(',', model.color);
                Console.WriteLine(model.c_color);
                Console.WriteLine(model.c_sid);

                conn.Open();
                string query = "update t_shoes set c_name=@c_name,c_size=@c_size,c_color=@c_color,c_qty=@c_qty,c_price=@c_price,c_date=@c_date,c_gender=@c_gender,c_img=@c_img,c_bid=@c_bid where c_sid=@id ";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id",id);
                    cmd.Parameters.AddWithValue("@c_name", model.c_name);
                    cmd.Parameters.AddWithValue("@c_size", model.c_size);
                    cmd.Parameters.AddWithValue("@c_color", model.c_color);
                    cmd.Parameters.AddWithValue("@c_qty", model.c_qty);
                    cmd.Parameters.AddWithValue("@c_price", model.c_price);
                    cmd.Parameters.AddWithValue("@c_date", DateTime.Parse(model.c_date.ToString()));
                    cmd.Parameters.AddWithValue("@c_gender", model.c_gender);
                    cmd.Parameters.AddWithValue("@c_img", oldimgpath);
                    cmd.Parameters.AddWithValue("@c_bid", model.c_bid);
                    row = cmd.ExecuteNonQuery();
                }
                return row;


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
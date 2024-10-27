using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC_Practical.Models;
using Npgsql;

namespace MVC_Practical.BAL
{
    public class internHelper
    {
        public readonly NpgsqlConnection _conn;
        public readonly string _imageFilePath;
        public internHelper(NpgsqlConnection conn)
        {
            _conn = conn;
            _imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        }

        public List<internDemo> GetInterns()
        {
            try
            {
                var internList = new List<internDemo>();
                _conn.Open();
                using (var command = new NpgsqlCommand(@"SELECT i.c_internId, i.c_internName, i.c_gender, i.c_topicId, t.c_topicname,
                i.c_date_Of_presentation, i.c_status, i.c_topic_image FROM t_interns_demo AS i JOIN t_assinged_task AS t ON i.c_topicId = t.c_topicId; ", _conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            internList.Add(new internDemo
                            {
                                c_internName = reader["c_internName"].ToString(),
                                c_internId = Convert.ToInt32(reader["c_internId"].ToString()),
                                c_gender = Convert.ToChar(reader["c_gender"]),
                                c_date_Of_presentation = Convert.ToDateTime(reader["c_date_Of_presentation"].ToString()),
                                c_status = Convert.ToBoolean(reader["c_status"].ToString()),
                                c_topicImage = reader["c_topic_image"].ToString(),
                                assignTopic = new topics
                                {
                                    c_topicId = Convert.ToInt32(reader["c_topicId"]),
                                    c_topicName = reader["c_topicname"].ToString()
                                }
                            });
                        }
                    }
                }

                return internList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                _conn.Close();
            }
        }

        public internDemo GetSelectedInterns(int id)
        {
            try
            {
                var intern = new internDemo();
                _conn.Open();
                using (var command = new NpgsqlCommand(@"SELECT i.c_internId, i.c_internName, i.c_gender, i.c_topicId, t.c_topicname,
                i.c_date_Of_presentation, i.c_status, i.c_topic_image FROM t_interns_demo AS i JOIN t_assinged_task AS t ON i.c_topicId = t.c_topicId WHERE c_internId = @c_internId", _conn))
                {
                    command.Parameters.AddWithValue("c_internId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            intern.c_internName = reader["c_internName"].ToString();
                            intern.c_internId = Convert.ToInt32(reader["c_internId"]);
                            intern.c_topicId = Convert.ToInt32(reader["c_topicId"]);
                            intern.c_gender = Convert.ToChar(reader["c_gender"]);
                            intern.c_date_Of_presentation = Convert.ToDateTime(reader["c_date_Of_presentation"].ToString());
                            intern.c_status = Convert.ToBoolean(reader["c_status"].ToString());
                            intern.c_topicImage = reader["c_topic_image"].ToString();
                            intern.assignTopic = new topics
                            {
                                c_topicId = Convert.ToInt32(reader["c_topicId"]),
                                c_topicName = reader["c_topicname"].ToString()
                            };
                        }
                    }
                }

                return intern;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                _conn.Close();
            }
        }

        public bool AddIntern(internDemo intern)
        {
            try
            {
                _conn.Open();
                intern.c_topicImage = SaveFile(intern.imagePath!, intern.c_internName!);
                if (intern.c_topicImage != null && intern.c_topicImage != "")
                {

                    using (var command = new NpgsqlCommand(@"INSERT INTO t_interns_demo(
	                        c_internname, c_gender, c_topicid, c_date_of_presentation, c_status, c_topic_image)
	                        VALUES (@c_internname, @c_gender,@c_topicid, @c_date_of_presentation, @c_status, @c_topic_image);", _conn))
                    {
                        command.Parameters.AddWithValue("c_internname", intern.c_internName!);
                        command.Parameters.AddWithValue("c_gender", intern.c_gender);
                        command.Parameters.AddWithValue("c_topicid", intern.c_topicId);
                        command.Parameters.AddWithValue("c_date_of_presentation", intern.c_date_Of_presentation);
                        command.Parameters.AddWithValue("c_status", intern.c_status);
                        command.Parameters.AddWithValue("c_topic_image", intern.c_topicImage);

                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File Save massage : {ex}");
                return false;
            }
            finally
            {
                _conn.Close();
            }
        }

        public bool UpdateIntern(internDemo intern)
        {
            try
            {
                _conn.Open();
                if (intern.imagePath != null)
                {
                    intern.c_topicImage = SaveFile(intern.imagePath!, intern.c_internName!);
                    if (intern.c_topicImage != null && intern.c_topicImage != "")
                    {

                        using (var command = new NpgsqlCommand(@"UPDATE t_interns_demo
	                SET  c_internname=@c_internname, c_gender=@c_gender, c_topicid=@c_topicid, c_date_of_presentation=@c_date_of_presentation, c_status=@c_status, c_topic_image=@c_topic_image
	                WHERE c_internid=@c_internid", _conn))
                        {
                            command.Parameters.AddWithValue("c_internname", intern.c_internName!);
                            command.Parameters.AddWithValue("c_gender", intern.c_gender);
                            command.Parameters.AddWithValue("c_topicid", intern.c_topicId);
                            command.Parameters.AddWithValue("c_date_of_presentation", intern.c_date_Of_presentation);
                            command.Parameters.AddWithValue("c_status", intern.c_status);
                            command.Parameters.AddWithValue("c_topic_image", intern.c_topicImage);
                            command.Parameters.AddWithValue("c_internid", intern.c_internId);

                            int result = command.ExecuteNonQuery();
                            if (result > 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    using (var command = new NpgsqlCommand(@"UPDATE t_interns_demo
	                SET  c_internname=@c_internname, c_gender=@c_gender, c_topicid=@c_topicid, c_date_of_presentation=@c_date_of_presentation, c_status=@c_status
	                WHERE c_internid=@c_internid", _conn))
                    {
                        command.Parameters.AddWithValue("c_internname", intern.c_internName!);
                        command.Parameters.AddWithValue("c_gender", intern.c_gender);
                        command.Parameters.AddWithValue("c_topicid", intern.c_topicId);
                        command.Parameters.AddWithValue("c_date_of_presentation", intern.c_date_Of_presentation);
                        command.Parameters.AddWithValue("c_status", intern.c_status);
                        command.Parameters.AddWithValue("c_internid", intern.c_internId);

                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File Save massage : {ex}");
                return false;
            }
            finally
            {
                _conn.Close();
            }
        }

        public bool DeleteIntern(int id)
        {
            try
            {
                _conn.Open();

                using (var command = new NpgsqlCommand(@"DELETE FROM t_interns_demo WHERE c_internid=@c_internid", _conn))
                {
                    command.Parameters.AddWithValue("c_internid", id);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File Save massage : {ex}");
                return false;
            }
            finally
            {
                _conn.Close();
            }
        }

        public string SaveFile(IFormFile formFile, string internName)
        {
            try
            {
                if (formFile.Length == 0 && formFile == null)
                {
                    return null!;
                }

                if (!Directory.Exists(_imageFilePath))
                {
                    Directory.CreateDirectory(_imageFilePath);
                }

                var fileName = Path.GetFileName(formFile.FileName);
                var filePath = Path.Combine(_imageFilePath, internName +  fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(stream);
                }

                return internName + fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File Save massage : {ex}");
                return null!;
            }
        }
    }
}
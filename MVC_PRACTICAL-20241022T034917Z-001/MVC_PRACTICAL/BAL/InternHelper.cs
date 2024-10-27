using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC_PRACTICAL.Models;
using Npgsql;

namespace MVC_PRACTICAL.BAL
{
    public class InternHelper
    {
        private readonly NpgsqlConnection _conn;
        private readonly string _imagePath;
        public InternHelper(NpgsqlConnection connection)
        {
            _conn = connection;
            _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","images");

            if(!Directory.Exists(_imagePath))
            {
                Directory.CreateDirectory(_imagePath);
            }
        }

        public List<InternClass> FetchAllInterns()
        {
            try
            {
                var interns = new List<InternClass>(); 
                _conn.Open();

                using (var command = new NpgsqlCommand(@"SELECT i.c_internid,i.c_internname,i.c_gender,i.c_topicid,i.c_date_of_presentation,i.c_status,i.c_topic_image,t.c_topicname FROM public.t_internsdemo i LEFT JOIN public.t_assignedtask t ON i.c_topicid = t.c_topicid;",_conn))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            interns.Add(new InternClass
                            {
                                    InternId = Convert.ToInt32(reader["c_internid"]),
                                    InternName = reader["c_internname"] as string,
                                    Gender = reader["c_gender"] as string,
                                    TopicId = Convert.ToInt32(reader["c_topicid"]),
                                    DateOfPresentation = Convert.ToDateTime(reader["c_date_of_presentation"]),
                                    Status = Convert.ToBoolean(reader["c_status"]),
                                    TopicImage = reader["c_topic_image"] as string,
                                    AssignedTopic = new topic
                                    {
                                        TopicId = Convert.ToInt32(reader["c_topicid"]),

                                        TopicName = reader["c_topicname"] as string,
                                    }
                            });
                        }
                    }
                }
            return interns;

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
        public List<topic> FetchAllTopics()
        {
            var topics = new List<topic>();
            _conn.Open();


            using var command = new NpgsqlCommand("SELECT c_TopicId, c_TopicName FROM public.t_AssignedTask", _conn);
            using var reader = command.ExecuteReader();


            while (reader.Read())
            {
                topics.Add(new topic
                {
                    TopicId = Convert.ToInt32(reader["c_TopicId"]),
                    TopicName = reader["c_TopicName"].ToString()
                });
            }


            _conn.Close();
            return topics;
        }

public InternClass FetchInternDetails(int id)
{
    try
    {
            InternClass intern = null!;
            _conn.Open();


            // Query with a join to fetch the topic name along with the intern details
            using (var command = new NpgsqlCommand(@"SELECT i.*, t.c_topicname FROM public.t_internsdemo i LEFT JOIN public.t_assignedtask t ON i.c_topicid = t.c_topicid WHERE i.c_internid = @id;", _conn))
            {

                command.Parameters.AddWithValue("@id", id);


                using (var reader = command.ExecuteReader())
                {

                        while (reader.Read())
                        {
                            intern = new InternClass
                            {
                                InternId = Convert.ToInt32(reader["c_internid"]),
                                InternName = reader["c_internname"].ToString(),
                                Gender = reader["c_gender"].ToString(),
                                TopicId = Convert.ToInt32(reader["c_topicid"]),
                                AssignedTopic = new topic 
                                {
                                    TopicId = Convert.ToInt32(reader["c_topicid"]),
                                    TopicName = reader["c_topicname"].ToString()
                                },
                                DateOfPresentation = Convert.ToDateTime(reader["c_date_of_presentation"]),
                                Status = Convert.ToBoolean(reader["c_status"]),
                                TopicImage = reader["c_topic_image"] as string,
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



public InternClass EditSelectedInterns(int id)
        {
            try
            {
                var intern = new InternClass();
                _conn.Open();
                using (var command = new NpgsqlCommand(@"SELECT i.c_internid, i.c_internname, i.c_gender, i.c_topicid, t.c_topicname,
                i.c_date_of_presentation, i.c_status, i.c_topic_image FROM t_internsdemo AS i JOIN t_assignedtask AS t ON i.c_topicid = t.c_topicid WHERE c_internid = @c_internid", _conn))
                {
                    command.Parameters.AddWithValue("c_internId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            intern.InternName = reader["c_internName"].ToString();
                            intern.InternId = Convert.ToInt32(reader["c_internId"]);
                            intern.TopicId = Convert.ToInt32(reader["c_topicId"]);
                            intern.Gender = reader["c_gender"] as string;
                            intern.DateOfPresentation = Convert.ToDateTime(reader["c_date_Of_presentation"].ToString());
                            intern.Status = Convert.ToBoolean(reader["c_status"].ToString());
                            intern.TopicImage = reader["c_topic_image"].ToString();
                            intern.AssignedTopic = new topic
                            {
                                TopicId = Convert.ToInt32(reader["c_topicId"]),
                                TopicName = reader["c_topicname"].ToString()
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


        public bool AddNewIntern(InternClass intern)
        {
            try
            {
                _conn.Open();
                intern.TopicImage = SaveImage(intern.TopicImageFile!,intern.InternName!);
                if(intern.TopicImageFile != null && intern.TopicImage != "")
                { 
                    using (var command = new NpgsqlCommand(@"INSERT INTO t_internsdemo (c_internname, c_gender, c_topicid, c_date_of_presentation, c_status, c_topic_image) VALUES (@InternName,@gender,@topicid,@dateofpresentation,@status,@topicimage);", _conn))
                    {
                            command.Parameters.AddWithValue("@internname", intern.InternName!);
                            command.Parameters.AddWithValue("@gender",intern.Gender!);
                            command.Parameters.AddWithValue("@topicid",intern.TopicId);
                            command.Parameters.AddWithValue("@dateofpresentation",intern.DateOfPresentation);
                            command.Parameters.AddWithValue("@status",intern.Status);
                            command.Parameters.AddWithValue("@topicimage",(object)intern.TopicImage ?? DBNull.Value);

                    
                            int result = command.ExecuteNonQuery();
                            if(result > 0)
                            {
                                return true;
                            }
                    }
                }
                return false;
            
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File save Massage : {ex}");
                return false;
            }
            finally{
                _conn.Close();
            }
        }    
        
public bool UpdateIntern(InternClass intern)
{
            try
            {
                _conn.Open();
                if (intern.TopicImageFile != null)
                {
                    intern.TopicImage = SaveImage(intern.TopicImageFile!, intern.InternName!);
                    if (intern.TopicImage != null && intern.TopicImage != "")
                    {

                        using (var command = new NpgsqlCommand(@"UPDATE t_internsdemo SET  c_internname=@c_internname, c_gender=@c_gender, c_topicid=@c_topicid, c_date_of_presentation=@c_date_of_presentation, c_status=@c_status, c_topic_image=@c_topic_image WHERE c_internid=@c_internid", _conn))
                        {                                                           
                            command.Parameters.AddWithValue("c_internname", intern.InternName!);
                            command.Parameters.AddWithValue("c_gender", intern.Gender!);
                            command.Parameters.AddWithValue("c_topicid", intern.TopicId);
                            command.Parameters.AddWithValue("c_date_of_presentation", intern.DateOfPresentation);
                            command.Parameters.AddWithValue("c_status", intern.Status);
                            command.Parameters.AddWithValue("c_topic_image", intern.TopicImage);
                            command.Parameters.AddWithValue("c_internid", intern.InternId);

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
                    using (var command = new NpgsqlCommand(@"UPDATE t_internsdemo
	                SET  c_internname=@c_internname, c_gender=@c_gender, c_topicid=@c_topicid, c_date_of_presentation=@c_date_of_presentation, c_status=@c_status
	                WHERE c_internid=@c_internid", _conn))
                    {
                        command.Parameters.AddWithValue("c_internname", intern.InternName!);
                        command.Parameters.AddWithValue("c_gender", intern.Gender!);
                        command.Parameters.AddWithValue("c_topicid", intern.TopicId);
                        command.Parameters.AddWithValue("c_date_of_presentation", intern.DateOfPresentation);
                        command.Parameters.AddWithValue("c_status", intern.Status);
                        command.Parameters.AddWithValue("c_internid", intern.InternId);

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

                using (var command = new NpgsqlCommand(@"DELETE FROM t_internsdemo WHERE c_internid=@c_internid", _conn))
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



        private string SaveImage(IFormFile imageFile, string internname)
        {
             try
            {
            if (imageFile.Length == 0 &&  imageFile == null)
            {
                return null!;
            }
           
                // var fileName = Path.GetFileName(imageFile.FileName);
                // var filePath = Path.Combine(_imagePath, fileName);

                // using (var stream = new FileStream(filePath, FileMode.Create))
                // {
                //     imageFile.CopyTo(stream);
                // }
                // Console.WriteLine($"Image Saved successfully at {filePath}");
                // return $"/images/{fileName}";
                if (!Directory.Exists(_imagePath))
                {
                    Directory.CreateDirectory(_imagePath);
                }

                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(_imagePath, internname +  fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                return internname + fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image: {ex.Message}");
                return null!;
            }
            
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC_Practical.Models;
using Npgsql;

namespace MVC_Practical.BAL
{
    public class topicHelper
    {
        public readonly NpgsqlConnection _conn;

        public topicHelper(NpgsqlConnection conn)
        {
            _conn = conn;
        }

        public List<topics> GetTopics()
        {
            try
            {
                var topicList = new List<topics>();
                _conn.Open();
                using (var command = new NpgsqlCommand(@"SELECT c_topicId,c_topicName FROM t_assinged_task", _conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            topicList.Add(new topics
                            {
                                c_topicId = Convert.ToInt32(reader["c_topicId"].ToString()),
                                c_topicName = reader["c_topicName"].ToString()
                            });
                        }
                    }
                }

                return topicList;
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

        public topics GetSelectedTopics(int id)
        {
            try
            {
                var topic = new topics();
                _conn.Open();
                using (var command = new NpgsqlCommand(@"SELECT c_topicId,c_topicName FROM t_assinged_task WHERE c_topicId = @c_topicId", _conn))
                {
                    command.Parameters.AddWithValue("c_topicId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                                topic.c_topicId = Convert.ToInt32(reader["c_topicId"].ToString());
                                topic.c_topicName = reader["c_topicName"].ToString();
                        }
                    }
                }

                return topic;
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

        public bool AddTopics(topics topic)
        {
            try
            {
                _conn.Open();
                using (var command = new NpgsqlCommand(@"INSERT INTO t_assinged_task (c_topicName) VALUES (@c_topicName)", _conn))
                {
                    command.Parameters.AddWithValue("c_topicName", topic.c_topicName!);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }
                }
                return false;
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

        public bool UpdateTopics(topics topic)
        {
            try
            {
                _conn.Open();
                using (var command = new NpgsqlCommand(@"UPDATE t_assinged_task SET c_topicName=@C_topicName WHERE c_topicId = @c_topicId", _conn))
                {
                    command.Parameters.AddWithValue("c_topicName", topic.c_topicName!);
                    command.Parameters.AddWithValue("c_topicId", topic.c_topicId);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }
                }
                return false;
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

        public bool DeleteTopics(int id)
        {
            try
            {
                _conn.Open();
                using (var command = new NpgsqlCommand(@"DELETE FROM t_assinged_task WHERE c_topicId = @c_topicId", _conn))
                {
                    command.Parameters.AddWithValue("c_topicId", id);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }
                }
                return false;
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
    }
}